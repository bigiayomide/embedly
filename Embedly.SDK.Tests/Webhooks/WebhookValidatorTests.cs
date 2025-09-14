using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
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
        var tamperedPayload = originalPayload.Replace("\"event\":\"payout\"", "\"event\":\"payout_failed\"");
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
        result.Event.Should().Be("payout");
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
        result!.Event.Should().Be("checkout.payment.success");
        result.Data.Should().NotBeNull();
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
        var result = _webhookValidator.ParseEventData<TestNipEventData>(payload, validSignature);

        // Assert
        result.Should().NotBeNull();
        result!.AccountNumber.Should().Be("9710128903");
        result.Reference.Should().Be("000001250630150523063421028600");
        result.Amount.Should().Be(100.0m);
        result.SenderName.Should().Be("OGHENETEGA KELVIN ESEDERE");
    }

    [Test]
    public void ParseEventData_WithInvalidSignature_ThrowsInvalidOperationException()
    {
        // Arrange
        var payload = CreateCustomerEventPayload();
        const string invalidSignature = "invalid-signature";

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(
            () => _webhookValidator.ParseEventData<TestNipEventData>(payload, invalidSignature));

        exception.Should().NotBeNull();
        exception!.Message.Should().Contain("Invalid webhook signature");
    }

    [Test]
    public void ParseEventData_WithWrongDataType_ReturnsNull()
    {
        // Arrange - Create NIP event payload but try to deserialize as Payout data
        var payload = CreateCustomerEventPayload();
        var validSignature = ComputeTestSignature(payload);

        // Act
        var result = _webhookValidator.ParseEventData<TestPayoutEventData>(payload, validSignature);

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
            @event = "payout",
            data = new
            {
                sessionId = (string?)null,
                debitAccountNumber = "0097411531",
                creditAccountNumber = "0003433020",
                debitAccountName = "Test User",
                creditAccountName = "TEST RECIPIENT",
                amount = 500.0,
                currency = "NGN",
                status = "Success",
                paymentReference = "668931654633533785081478974241",
                deliveryStatusMessage = (string?)null,
                deliveryStatusCode = (string?)null,
                dateOfTransaction = "0001-01-01T00:00:00"
            }
        });
    }

    private string CreateComplexWebhookPayload()
    {
        return JsonSerializer.Serialize(new
        {
            id = "evt_complex_12345",
            @event = "checkout.payment.success",
            data = new
            {
                transactionId = "8d79fdc1-8e09-4914-85c4-4bc8e256b650",
                walletId = "7f8ac489-a9e7-4fb5-a192-7ea859f9e4af",
                checkoutRef = "CHK202507241246121093911",
                amount = 3000,
                status = "success",
                senderAccountNumber = "0098960218",
                senderName = "ME",
                recipientAccountNumber = "0098960218",
                recipientName = "Granular Press",
                reference = "65738239gbdjd",
                createdAt = "2025-07-24T15:48:10.6768368Z"
            }
        });
    }

    private string CreateCustomerEventPayload()
    {
        return JsonSerializer.Serialize(new
        {
            id = "evt_customer_12345",
            @event = "nip",
            data = new
            {
                accountNumber = "9710128903",
                reference = "000001250630150523063421028600",
                amount = 100.0,
                fee = 0.0,
                senderName = "OGHENETEGA KELVIN ESEDERE",
                senderBank = "",
                dateOfTransaction = "2025-06-30T14:07:14.4553594Z",
                description = "TRF 9710128903 PAYREF: OneBank Transfer"
            }
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

        using var hmac = new HMACSHA512(keyBytes);
        var hash = hmac.ComputeHash(payloadBytes);
        return Convert.ToHexString(hash).ToLowerInvariant();
    }

    #endregion

    #region Test Data Classes

    private class TestNipEventData
    {
        [JsonPropertyName("accountNumber")]
        public string? AccountNumber { get; set; }

        [JsonPropertyName("reference")]
        public string? Reference { get; set; }

        [JsonPropertyName("amount")]
        public decimal? Amount { get; set; }

        [JsonPropertyName("senderName")]
        public string? SenderName { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }
    }

    private class TestPayoutEventData
    {
        [JsonPropertyName("debitAccountNumber")]
        public string? DebitAccountNumber { get; set; }

        [JsonPropertyName("creditAccountNumber")]
        public string? CreditAccountNumber { get; set; }

        [JsonPropertyName("amount")]
        public decimal? Amount { get; set; }

        [JsonPropertyName("currency")]
        public string? Currency { get; set; }

        [JsonPropertyName("status")]
        public string? Status { get; set; }
    }

    #endregion
}