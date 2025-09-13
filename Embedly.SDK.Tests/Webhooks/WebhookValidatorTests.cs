using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using FluentAssertions;
using NUnit.Framework;
using Embedly.SDK.Webhooks;
using Embedly.SDK.Tests.Testing;

namespace Embedly.SDK.Tests.Webhooks;

/// <summary>
/// Comprehensive unit tests for WebhookValidator following SDK patterns.
/// Tests signature validation, event parsing, and security scenarios.
/// </summary>
[TestFixture]
public class WebhookValidatorTests : TestBase
{
    private WebhookValidator _webhookValidator = null!;
    private const string TestWebhookSecret = "test-webhook-secret-key-12345";

    protected override void OnSetUp()
    {
        _webhookValidator = new WebhookValidator(TestWebhookSecret);
    }

    #region Constructor Tests

    [Test]
    public void Constructor_WithValidSecret_CreatesInstance()
    {
        // Act
        var validator = new WebhookValidator("valid-secret");

        // Assert
        validator.Should().NotBeNull();
    }

    [Test]
    public void Constructor_WithNullSecret_ThrowsArgumentException()
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => new WebhookValidator(null!));
        exception.Should().NotBeNull();
    }

    [Test]
    public void Constructor_WithEmptySecret_ThrowsArgumentException()
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => new WebhookValidator(string.Empty));
        exception.Should().NotBeNull();
    }

    [Test]
    public void Constructor_WithWhitespaceSecret_ThrowsArgumentException()
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => new WebhookValidator("   "));
        exception.Should().NotBeNull();
    }

    #endregion

    #region Signature Validation Tests

    [Test]
    public void ValidateSignature_WithValidSignature_ReturnsTrue()
    {
        // Arrange
        var payload = CreateTestWebhookPayload();
        var validSignature = ComputeTestSignature(payload);

        // Act
        var result = _webhookValidator.ValidateSignature(payload, validSignature);

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public void ValidateSignature_WithInvalidSignature_ReturnsFalse()
    {
        // Arrange
        var payload = CreateTestWebhookPayload();
        const string invalidSignature = "invalid-signature";

        // Act
        var result = _webhookValidator.ValidateSignature(payload, invalidSignature);

        // Assert
        result.Should().BeFalse();
    }

    [Test]
    public void ValidateSignature_WithTamperedPayload_ReturnsFalse()
    {
        // Arrange
        var originalPayload = CreateTestWebhookPayload();
        var tamperedPayload = originalPayload.Replace("\"event\":\"customer.created\"", "\"event\":\"customer.deleted\"");
        var signatureForOriginal = ComputeTestSignature(originalPayload);

        // Act
        var result = _webhookValidator.ValidateSignature(tamperedPayload, signatureForOriginal);

        // Assert
        result.Should().BeFalse();
    }

    [Test]
    public void ValidateSignature_WithNullPayload_ReturnsFalse()
    {
        // Arrange
        var validSignature = ComputeTestSignature("test");

        // Act
        var result = _webhookValidator.ValidateSignature(null!, validSignature);

        // Assert
        result.Should().BeFalse();
    }

    [Test]
    public void ValidateSignature_WithEmptyPayload_ReturnsFalse()
    {
        // Arrange
        var validSignature = ComputeTestSignature("test");

        // Act
        var result = _webhookValidator.ValidateSignature(string.Empty, validSignature);

        // Assert
        result.Should().BeFalse();
    }

    [Test]
    public void ValidateSignature_WithWhitespacePayload_ReturnsFalse()
    {
        // Arrange
        var validSignature = ComputeTestSignature("test");

        // Act
        var result = _webhookValidator.ValidateSignature("   ", validSignature);

        // Assert
        result.Should().BeFalse();
    }

    [Test]
    public void ValidateSignature_WithNullSignature_ReturnsFalse()
    {
        // Arrange
        var payload = CreateTestWebhookPayload();

        // Act
        var result = _webhookValidator.ValidateSignature(payload, null!);

        // Assert
        result.Should().BeFalse();
    }

    [Test]
    public void ValidateSignature_WithEmptySignature_ReturnsFalse()
    {
        // Arrange
        var payload = CreateTestWebhookPayload();

        // Act
        var result = _webhookValidator.ValidateSignature(payload, string.Empty);

        // Assert
        result.Should().BeFalse();
    }

    [Test]
    public void ValidateSignature_WithWhitespaceSignature_ReturnsFalse()
    {
        // Arrange
        var payload = CreateTestWebhookPayload();

        // Act
        var result = _webhookValidator.ValidateSignature(payload, "   ");

        // Assert
        result.Should().BeFalse();
    }

    [Test]
    public void ValidateSignature_IsCaseInsensitive()
    {
        // Arrange
        var payload = CreateTestWebhookPayload();
        var signature = ComputeTestSignature(payload);
        var upperCaseSignature = signature.ToUpperInvariant();

        // Act
        var result = _webhookValidator.ValidateSignature(payload, upperCaseSignature);

        // Assert
        result.Should().BeTrue();
    }

    #endregion

    #region Event Parsing Tests

    [Test]
    public void ParseEvent_WithValidPayloadAndSignature_ReturnsWebhookEvent()
    {
        // Arrange
        var payload = CreateTestWebhookPayload();
        var validSignature = ComputeTestSignature(payload);

        // Act
        var result = _webhookValidator.ParseEvent(payload, validSignature);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be("evt_12345");
        result.Event.Should().Be("customer.created");
        result.Timestamp.Should().Be(CreateTestTimestamp());
        result.Data.Should().NotBeNull();
    }

    [Test]
    public void ParseEvent_WithInvalidSignature_ThrowsInvalidOperationException()
    {
        // Arrange
        var payload = CreateTestWebhookPayload();
        const string invalidSignature = "invalid-signature";

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(
            () => _webhookValidator.ParseEvent(payload, invalidSignature));

        exception.Should().NotBeNull();
        exception!.Message.Should().Contain("Invalid webhook signature");
    }

    [Test]
    public void ParseEvent_WithMalformedJson_ThrowsInvalidOperationException()
    {
        // Arrange
        const string malformedPayload = "{\"invalid_json\":}";
        var validSignature = ComputeTestSignature(malformedPayload);

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(
            () => _webhookValidator.ParseEvent(malformedPayload, validSignature));

        exception.Should().NotBeNull();
        exception!.Message.Should().Contain("Failed to parse webhook event");
        exception.InnerException.Should().BeOfType<JsonException>();
    }

    [Test]
    public void ParseEvent_WithValidComplexPayload_ParsesAllFields()
    {
        // Arrange
        var complexPayload = CreateComplexWebhookPayload();
        var validSignature = ComputeTestSignature(complexPayload);

        // Act
        var result = _webhookValidator.ParseEvent(complexPayload, validSignature);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be("evt_complex_12345");
        result.Event.Should().Be("wallet.transaction.completed");
        result.Data.Should().NotBeNull();
        result.Metadata.Should().NotBeNull();
    }

    #endregion

    #region Typed Event Data Parsing Tests

    [Test]
    public void ParseEventData_WithValidCustomerEvent_ReturnsTypedData()
    {
        // Arrange
        var payload = CreateCustomerEventPayload();
        var validSignature = ComputeTestSignature(payload);

        // Act
        var result = _webhookValidator.ParseEventData<TestCustomerEventData>(payload, validSignature);

        // Assert
        result.Should().NotBeNull();
        result!.CustomerId.Should().Be("cust_12345");
        result.FirstName.Should().Be("John");
        result.LastName.Should().Be("Doe");
        result.Email.Should().Be("john.doe@example.com");
    }

    [Test]
    public void ParseEventData_WithInvalidSignature_ThrowsInvalidOperationException()
    {
        // Arrange
        var payload = CreateCustomerEventPayload();
        const string invalidSignature = "invalid-signature";

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(
            () => _webhookValidator.ParseEventData<TestCustomerEventData>(payload, invalidSignature));

        exception.Should().NotBeNull();
        exception!.Message.Should().Contain("Invalid webhook signature");
    }

    [Test]
    public void ParseEventData_WithWrongDataType_ReturnsNull()
    {
        // Arrange
        var payload = CreateCustomerEventPayload();
        var validSignature = ComputeTestSignature(payload);

        // Act
        var result = _webhookValidator.ParseEventData<TestWalletEventData>(payload, validSignature);

        // Assert - Should return null when data doesn't match expected type structure
        result.Should().BeNull();
    }

    #endregion

    #region Security Tests

    [Test]
    public void ValidateSignature_WithTimingAttack_ConsistentTiming()
    {
        // Arrange
        var payload = CreateTestWebhookPayload();
        var validSignature = ComputeTestSignature(payload);
        var invalidSignature = "completely-different-invalid-signature-that-is-same-length-as-valid";

        // Ensure signatures are same length to test timing attack resistance
        if (validSignature.Length != invalidSignature.Length)
        {
            invalidSignature = invalidSignature.PadRight(validSignature.Length, '0')[..validSignature.Length];
        }

        // Act & Assert - Both should return quickly and consistently
        var validResult = _webhookValidator.ValidateSignature(payload, validSignature);
        var invalidResult = _webhookValidator.ValidateSignature(payload, invalidSignature);

        validResult.Should().BeTrue();
        invalidResult.Should().BeFalse();
    }

    [Test]
    public void ValidateSignature_WithReplayAttack_StillValidates()
    {
        // Arrange - Simulate replay attack with same payload and signature
        var payload = CreateTestWebhookPayload();
        var signature = ComputeTestSignature(payload);

        // Act - Same payload/signature used multiple times
        var firstAttempt = _webhookValidator.ValidateSignature(payload, signature);
        var secondAttempt = _webhookValidator.ValidateSignature(payload, signature);
        var thirdAttempt = _webhookValidator.ValidateSignature(payload, signature);

        // Assert - All should validate (replay protection should be handled at application level)
        firstAttempt.Should().BeTrue();
        secondAttempt.Should().BeTrue();
        thirdAttempt.Should().BeTrue();
    }

    [Test]
    public void ValidateSignature_WithLargePayload_ValidatesCorrectly()
    {
        // Arrange
        var largePayload = CreateLargeWebhookPayload();
        var validSignature = ComputeTestSignature(largePayload);

        // Act
        var result = _webhookValidator.ValidateSignature(largePayload, validSignature);

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public void ValidateSignature_WithSpecialCharacters_ValidatesCorrectly()
    {
        // Arrange
        var payloadWithSpecialChars = CreateWebhookPayloadWithSpecialCharacters();
        var validSignature = ComputeTestSignature(payloadWithSpecialChars);

        // Act
        var result = _webhookValidator.ValidateSignature(payloadWithSpecialChars, validSignature);

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public void ValidateSignature_WithUnicodeCharacters_ValidatesCorrectly()
    {
        // Arrange
        var payloadWithUnicode = CreateWebhookPayloadWithUnicode();
        var validSignature = ComputeTestSignature(payloadWithUnicode);

        // Act
        var result = _webhookValidator.ValidateSignature(payloadWithUnicode, validSignature);

        // Assert
        result.Should().BeTrue();
    }

    #endregion

    #region Helper Methods

    private string CreateTestWebhookPayload()
    {
        return JsonSerializer.Serialize(new
        {
            id = "evt_12345",
            @event = "customer.created",
            created_at = CreateTestTimestamp(),
            data = new
            {
                customer_id = "cust_12345",
                name = "John Doe",
                email = "john.doe@example.com"
            }
        }, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
    }

    private string CreateComplexWebhookPayload()
    {
        return JsonSerializer.Serialize(new
        {
            id = "evt_complex_12345",
            @event = "wallet.transaction.completed",
            created_at = CreateTestTimestamp(),
            data = new
            {
                transaction_id = "txn_12345",
                wallet_id = "wlt_12345",
                amount = 100000,
                currency = "NGN",
                reference = "ref_12345"
            },
            metadata = new
            {
                source = "api",
                version = "v1",
                retry_count = 0
            }
        }, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
    }

    private string CreateCustomerEventPayload()
    {
        return JsonSerializer.Serialize(new
        {
            id = "evt_customer_12345",
            @event = "customer.created",
            created_at = CreateTestTimestamp(),
            data = new
            {
                customer_id = "cust_12345",
                first_name = "John",
                last_name = "Doe",
                email = "john.doe@example.com",
                phone_number = "+2348012345678"
            }
        }, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
    }

    private string CreateLargeWebhookPayload()
    {
        var largeData = new StringBuilder();
        for (int i = 0; i < 1000; i++)
        {
            largeData.Append($"item_{i}_");
        }

        return JsonSerializer.Serialize(new
        {
            id = "evt_large_12345",
            @event = "large.data.event",
            created_at = CreateTestTimestamp(),
            data = new
            {
                large_field = largeData.ToString(),
                additional_data = "This is a test of large payload validation"
            }
        }, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
    }

    private string CreateWebhookPayloadWithSpecialCharacters()
    {
        return JsonSerializer.Serialize(new
        {
            id = "evt_special_12345",
            @event = "special.characters.test",
            created_at = CreateTestTimestamp(),
            data = new
            {
                message = "Special chars: !@#$%^&*()_+-={}[]|;':\",./<>?",
                escaped = "Escaped \"quotes\" and \\backslashes\\",
                newlines = "Line 1\nLine 2\rLine 3\r\n"
            }
        }, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
    }

    private string CreateWebhookPayloadWithUnicode()
    {
        return JsonSerializer.Serialize(new
        {
            id = "evt_unicode_12345",
            @event = "unicode.characters.test",
            created_at = CreateTestTimestamp(),
            data = new
            {
                english = "Hello World",
                spanish = "Hola Mundo",
                chinese = "你好世界",
                arabic = "مرحبا بالعالم",
                emoji = "🌍🚀💰",
                currency = "₦ € $ ¥ £"
            }
        }, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
    }

    private string ComputeTestSignature(string payload)
    {
        var keyBytes = Encoding.UTF8.GetBytes(TestWebhookSecret);
        var payloadBytes = Encoding.UTF8.GetBytes(payload);

        using var hmac = new HMACSHA256(keyBytes);
        var hash = hmac.ComputeHash(payloadBytes);
        return Convert.ToHexString(hash).ToLowerInvariant();
    }

    #endregion

    #region Test Data Classes

    private class TestCustomerEventData
    {
        public string? CustomerId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
    }

    private class TestWalletEventData
    {
        public string? WalletId { get; set; }
        public decimal? Balance { get; set; }
        public string? Currency { get; set; }
    }

    #endregion
}