using System;
using Embedly.SDK.Models.Requests.CorporateCustomers;
using FluentAssertions;
using NUnit.Framework;

namespace Embedly.SDK.Tests.Validation;

/// <summary>
///     Unit tests for Base64StringAttribute validation.
/// </summary>
[TestFixture]
public class Base64StringAttributeTests
{
    private Base64StringAttribute _attribute = null!;

    [SetUp]
    public void SetUp()
    {
        _attribute = new Base64StringAttribute();
    }

    [Test]
    public void IsValid_WithNullValue_ReturnsTrue()
    {
        // Act
        var result = _attribute.IsValid(null);

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public void IsValid_WithEmptyString_ReturnsTrue()
    {
        // Act
        var result = _attribute.IsValid(string.Empty);

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public void IsValid_WithWhitespaceString_ReturnsTrue()
    {
        // Act
        var result = _attribute.IsValid("   ");

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public void IsValid_WithValidBase64String_ReturnsTrue()
    {
        // Arrange
        var validBase64 = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("Hello World"));

        // Act
        var result = _attribute.IsValid(validBase64);

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public void IsValid_WithValidPaddedBase64String_ReturnsTrue()
    {
        // Arrange - This creates a base64 string with padding
        var validBase64 = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("Test"));

        // Act
        var result = _attribute.IsValid(validBase64);

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public void IsValid_WithValidLongBase64String_ReturnsTrue()
    {
        // Arrange - Create a longer base64 string
        var longText = new string('A', 1000);
        var validBase64 = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(longText));

        // Act
        var result = _attribute.IsValid(validBase64);

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    [TestCase("Hello World")]
    [TestCase("Not Base64!")]
    [TestCase("123ABC")]
    [TestCase("SGVsbG8gV29ybGQ!")] // Invalid characters
    public void IsValid_WithInvalidBase64String_ReturnsFalse(string invalidBase64)
    {
        // Act
        var result = _attribute.IsValid(invalidBase64);

        // Assert
        result.Should().BeFalse();
    }

    [Test]
    public void IsValid_WithNonStringValue_ReturnsFalse()
    {
        // Act
        var result1 = _attribute.IsValid(123);
        var result2 = _attribute.IsValid(new object());
        var result3 = _attribute.IsValid(true);

        // Assert
        result1.Should().BeFalse();
        result2.Should().BeFalse();
        result3.Should().BeFalse();
    }

    [Test]
    public void IsValid_WithCorruptedBase64String_ReturnsFalse()
    {
        // Arrange - Start with valid base64 then corrupt it
        var validBase64 = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("Hello World"));
        var corruptedBase64 = validBase64.Substring(0, validBase64.Length - 2) + "!!";

        // Act
        var result = _attribute.IsValid(corruptedBase64);

        // Assert
        result.Should().BeFalse();
    }

    [Test]
    public void IsValid_WithPartialBase64String_ReturnsFalse()
    {
        // Arrange - Create an incomplete base64 string
        var validBase64 = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("Hello World"));
        var partialBase64 = validBase64.Substring(0, validBase64.Length / 2);

        // Act
        var result = _attribute.IsValid(partialBase64);

        // Assert
        result.Should().BeFalse();
    }

    [Test]
    public void ErrorMessage_CanBeCustomized()
    {
        // Arrange
        var customMessage = "Custom error message";
        var attribute = new Base64StringAttribute { ErrorMessage = customMessage };

        // Act & Assert
        attribute.ErrorMessage.Should().Be(customMessage);
    }

    [Test]
    public void IsValid_WithDocumentSizeBase64_ReturnsTrue()
    {
        // Arrange - Simulate a document-like base64 string
        var documentContent = new byte[1024]; // 1KB of data
        for (int i = 0; i < documentContent.Length; i++)
        {
            documentContent[i] = (byte)(i % 256);
        }
        var documentBase64 = Convert.ToBase64String(documentContent);

        // Act
        var result = _attribute.IsValid(documentBase64);

        // Assert
        result.Should().BeTrue();
    }
}