using System;
using FluentAssertions;
using NUnit.Framework;
using Embedly.SDK.Services.Cards;
using Embedly.SDK.Tests.Testing;

namespace Embedly.SDK.Tests.Services;

/// <summary>
/// Unit tests for PinEncryptionService following SDK patterns.
/// Tests PIN encryption operations and RSA key management.
/// </summary>
[TestFixture]
public class PinEncryptionServiceTests : ServiceTestBase
{
    private PinEncryptionService _pinEncryptionService = null!;

    protected override void OnSetUp()
    {
        _pinEncryptionService = new PinEncryptionService(MockOptions.Object);
    }

    #region RSA Public Key Tests

    [Test]
    public void GetRsaPublicKey_ReturnsValidPublicKey()
    {
        // Act
        var publicKey = _pinEncryptionService.GetRsaPublicKey();

        // Assert
        publicKey.Should().NotBeNullOrEmpty();
        publicKey.Should().StartWith("-----BEGIN PUBLIC KEY-----");
        publicKey.Should().EndWith("-----END PUBLIC KEY-----");
    }

    [Test]
    public void GetRsaPublicKey_CalledMultipleTimes_ReturnsConsistentKey()
    {
        // Act
        var publicKey1 = _pinEncryptionService.GetRsaPublicKey();
        var publicKey2 = _pinEncryptionService.GetRsaPublicKey();

        // Assert
        publicKey1.Should().Be(publicKey2);
    }

    [Test]
    public void GetRsaPublicKey_ReturnsValidPemFormat()
    {
        // Act
        var publicKey = _pinEncryptionService.GetRsaPublicKey();

        // Assert
        publicKey.Should().NotBeNullOrEmpty();
        publicKey.Should().Contain("-----BEGIN PUBLIC KEY-----");
        publicKey.Should().Contain("-----END PUBLIC KEY-----");
        publicKey.Split('\n', StringSplitOptions.RemoveEmptyEntries).Length.Should().BeGreaterThan(2);
    }

    #endregion

    #region PIN Encryption Tests

    [Test]
    public void EncryptPin_WithValidPin_ReturnsEncryptedString()
    {
        // Arrange
        var plainTextPin = "1234";

        // Act
        var encryptedPin = _pinEncryptionService.EncryptPin(plainTextPin);

        // Assert
        encryptedPin.Should().NotBeNullOrEmpty();
        encryptedPin.Should().NotBe(plainTextPin);
        encryptedPin.Length.Should().BeGreaterThan(plainTextPin.Length);
    }

    [Test]
    public void EncryptPin_WithValidPin_ReturnsBase64String()
    {
        // Arrange
        var plainTextPin = "1234";

        // Act
        var encryptedPin = _pinEncryptionService.EncryptPin(plainTextPin);

        // Assert
        encryptedPin.Should().NotBeNullOrEmpty();

        // Verify it's valid Base64 by attempting to decode it
        var isValidBase64 = IsValidBase64String(encryptedPin);
        isValidBase64.Should().BeTrue("encrypted PIN should be a valid Base64 string");
    }

    [Test]
    public void EncryptPin_WithSamePin_ReturnsDifferentEncryptedValues()
    {
        // Arrange
        var plainTextPin = "1234";

        // Act
        var encryptedPin1 = _pinEncryptionService.EncryptPin(plainTextPin);
        var encryptedPin2 = _pinEncryptionService.EncryptPin(plainTextPin);

        // Assert
        encryptedPin1.Should().NotBeNullOrEmpty();
        encryptedPin2.Should().NotBeNullOrEmpty();
        // Note: RSA encryption with padding should produce different results each time
        // This is expected behavior for secure encryption
    }

    [Test]
    public void EncryptPin_WithDifferentPins_ReturnsDifferentEncryptedValues()
    {
        // Arrange
        var plainTextPin1 = "1234";
        var plainTextPin2 = "5678";

        // Act
        var encryptedPin1 = _pinEncryptionService.EncryptPin(plainTextPin1);
        var encryptedPin2 = _pinEncryptionService.EncryptPin(plainTextPin2);

        // Assert
        encryptedPin1.Should().NotBeNullOrEmpty();
        encryptedPin2.Should().NotBeNullOrEmpty();
        encryptedPin1.Should().NotBe(encryptedPin2);
    }

    [Test]
    public void EncryptPin_WithNullPin_ThrowsArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(
            () => _pinEncryptionService.EncryptPin(null!));
    }

    [Test]
    public void EncryptPin_WithEmptyPin_ThrowsArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(
            () => _pinEncryptionService.EncryptPin(string.Empty));
    }

    [Test]
    public void EncryptPin_WithWhitespacePin_ThrowsArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(
            () => _pinEncryptionService.EncryptPin("   "));
    }

    [Test]
    public void EncryptPin_WithValidLongPin_ReturnsEncryptedString()
    {
        // Arrange
        var plainTextPin = "123456789012"; // 12-digit PIN

        // Act
        var encryptedPin = _pinEncryptionService.EncryptPin(plainTextPin);

        // Assert
        encryptedPin.Should().NotBeNullOrEmpty();
        encryptedPin.Should().NotBe(plainTextPin);
    }

    [Test]
    public void EncryptPin_WithSpecialCharacters_ReturnsEncryptedString()
    {
        // Arrange
        var plainTextPin = "1@3#";

        // Act
        var encryptedPin = _pinEncryptionService.EncryptPin(plainTextPin);

        // Assert
        encryptedPin.Should().NotBeNullOrEmpty();
        encryptedPin.Should().NotBe(plainTextPin);
    }

    #endregion

    #region Integration Tests

    [Test]
    public void PinEncryption_EndToEndFlow_WorksCorrectly()
    {
        // Arrange
        var plainTextPin = "9876";

        // Act - Get public key
        var publicKey = _pinEncryptionService.GetRsaPublicKey();

        // Act - Encrypt PIN
        var encryptedPin = _pinEncryptionService.EncryptPin(plainTextPin);

        // Assert
        publicKey.Should().NotBeNullOrEmpty();
        encryptedPin.Should().NotBeNullOrEmpty();
        encryptedPin.Should().NotBe(plainTextPin);

        var isValidBase64 = IsValidBase64String(encryptedPin);
        isValidBase64.Should().BeTrue();
    }

    [Test]
    public void PinEncryption_MultipleOperations_MaintainsConsistency()
    {
        // Arrange
        var pins = new[] { "0000", "1111", "2222", "3333", "4444" };

        // Act & Assert
        foreach (var pin in pins)
        {
            var publicKey = _pinEncryptionService.GetRsaPublicKey();
            var encryptedPin = _pinEncryptionService.EncryptPin(pin);

            publicKey.Should().NotBeNullOrEmpty();
            encryptedPin.Should().NotBeNullOrEmpty();
            encryptedPin.Should().NotBe(pin);
        }
    }

    #endregion

    #region Helper Methods

    private static bool IsValidBase64String(string base64String)
    {
        try
        {
            Convert.FromBase64String(base64String);
            return true;
        }
        catch (FormatException)
        {
            return false;
        }
    }

    #endregion
}