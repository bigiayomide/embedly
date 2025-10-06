using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using Embedly.SDK.Models.Requests.CorporateCustomers;
using FluentAssertions;
using NUnit.Framework;

namespace Embedly.SDK.Tests.Models;

/// <summary>
///     Unit tests for corporate customer request model validations.
/// </summary>
[TestFixture]
public class CorporateCustomerRequestValidationTests
{
    private ValidationContext _validationContext = null!;

    [SetUp]
    public void SetUp()
    {
        _validationContext = new ValidationContext(new object());
    }

    #region CreateCorporateCustomerRequest Tests

    [Test]
    public void CreateCorporateCustomerRequest_WithValidData_PassesValidation()
    {
        // Arrange
        var request = new CreateCorporateCustomerRequest
        {
            RcNumber = "RC123456",
            Tin = "TIN123456789",
            FullBusinessName = "Test Corporation Ltd",
            BusinessAddress = "123 Business District, Lagos",
            CountryId = Guid.NewGuid(),
            City = "Lagos",
            Email = "test@testcorp.com",
            WalletPreferredName = "TestCorp Wallet"
        };

        // Act
        var validationResults = ValidateModel(request);

        // Assert
        validationResults.Should().BeEmpty();
    }

    [Test]
    [TestCase("", "RC Number is required")]
    [TestCase(null, "RC Number is required")]
    public void CreateCorporateCustomerRequest_WithInvalidRcNumber_FailsValidation(string rcNumber, string expectedError)
    {
        // Arrange
        var request = CreateValidCreateRequest();
        request.RcNumber = rcNumber;

        // Act
        var validationResults = ValidateModel(request);

        // Assert
        validationResults.Should().ContainSingle()
            .Which.ErrorMessage.Should().Be(expectedError);
    }

    [Test]
    public void CreateCorporateCustomerRequest_WithTooLongRcNumber_FailsValidation()
    {
        // Arrange
        var request = CreateValidCreateRequest();
        request.RcNumber = new string('A', 16); // Max is 15

        // Act
        var validationResults = ValidateModel(request);

        // Assert
        validationResults.Should().ContainSingle()
            .Which.ErrorMessage.Should().Be("RC Number cannot exceed 15 characters");
    }

    [Test]
    [TestCase("", "TIN is required")]
    [TestCase(null, "TIN is required")]
    public void CreateCorporateCustomerRequest_WithInvalidTin_FailsValidation(string tin, string expectedError)
    {
        // Arrange
        var request = CreateValidCreateRequest();
        request.Tin = tin;

        // Act
        var validationResults = ValidateModel(request);

        // Assert
        validationResults.Should().ContainSingle()
            .Which.ErrorMessage.Should().Be(expectedError);
    }

    [Test]
    public void CreateCorporateCustomerRequest_WithTooLongTin_FailsValidation()
    {
        // Arrange
        var request = CreateValidCreateRequest();
        request.Tin = new string('A', 16); // Max is 15

        // Act
        var validationResults = ValidateModel(request);

        // Assert
        validationResults.Should().ContainSingle()
            .Which.ErrorMessage.Should().Be("TIN cannot exceed 15 characters");
    }

    [Test]
    [TestCase("", "Full business name is required")]
    [TestCase(null, "Full business name is required")]
    public void CreateCorporateCustomerRequest_WithInvalidBusinessName_FailsValidation(string businessName, string expectedError)
    {
        // Arrange
        var request = CreateValidCreateRequest();
        request.FullBusinessName = businessName;

        // Act
        var validationResults = ValidateModel(request);

        // Assert
        validationResults.Should().ContainSingle()
            .Which.ErrorMessage.Should().Be(expectedError);
    }

    [Test]
    public void CreateCorporateCustomerRequest_WithTooLongBusinessName_FailsValidation()
    {
        // Arrange
        var request = CreateValidCreateRequest();
        request.FullBusinessName = new string('A', 151); // Max is 150

        // Act
        var validationResults = ValidateModel(request);

        // Assert
        validationResults.Should().ContainSingle()
            .Which.ErrorMessage.Should().Be("Full business name cannot exceed 150 characters");
    }

    [Test]
    [TestCase("invalid-email")]
    [TestCase("@invalid.com")]
    [TestCase("invalid@")]
    [TestCase("")]
    [TestCase(null)]
    public void CreateCorporateCustomerRequest_WithInvalidEmail_FailsValidation(string email)
    {
        // Arrange
        var request = CreateValidCreateRequest();
        request.Email = email;

        // Act
        var validationResults = ValidateModel(request);

        // Assert
        validationResults.Should().NotBeEmpty();
        validationResults.Should().Contain(r =>
            r.ErrorMessage!.Contains("Email") || r.ErrorMessage.Contains("email"));
    }

    [Test]
    public void CreateCorporateCustomerRequest_WithEmptyCountryId_FailsValidation()
    {
        // Arrange
        var request = CreateValidCreateRequest();
        request.CountryId = Guid.Empty;

        // Act
        var validationResults = ValidateModel(request);

        // Assert
        validationResults.Should().ContainSingle()
            .Which.ErrorMessage.Should().Be("Country ID is required");
    }

    #endregion

    #region UpdateCorporateCustomerRequest Tests

    [Test]
    public void UpdateCorporateCustomerRequest_WithValidData_PassesValidation()
    {
        // Arrange
        var request = new UpdateCorporateCustomerRequest
        {
            FullBusinessName = "Updated Corporation Ltd",
            Email = "updated@testcorp.com",
            City = "Abuja"
        };

        // Act
        var validationResults = ValidateModel(request);

        // Assert
        validationResults.Should().BeEmpty();
    }

    [Test]
    public void UpdateCorporateCustomerRequest_WithAllNullValues_PassesValidation()
    {
        // Arrange - All fields are optional in update request
        var request = new UpdateCorporateCustomerRequest();

        // Act
        var validationResults = ValidateModel(request);

        // Assert
        validationResults.Should().BeEmpty();
    }

    [Test]
    public void UpdateCorporateCustomerRequest_WithTooLongBusinessName_FailsValidation()
    {
        // Arrange
        var request = new UpdateCorporateCustomerRequest
        {
            FullBusinessName = new string('A', 151) // Max is 150
        };

        // Act
        var validationResults = ValidateModel(request);

        // Assert
        validationResults.Should().ContainSingle()
            .Which.ErrorMessage.Should().Be("Full business name cannot exceed 150 characters");
    }

    [Test]
    [TestCase("invalid-email")]
    [TestCase("@invalid.com")]
    [TestCase("invalid@")]
    public void UpdateCorporateCustomerRequest_WithInvalidEmail_FailsValidation(string email)
    {
        // Arrange
        var request = new UpdateCorporateCustomerRequest
        {
            Email = email
        };

        // Act
        var validationResults = ValidateModel(request);

        // Assert
        validationResults.Should().NotBeEmpty();
        validationResults.Should().Contain(r =>
            r.ErrorMessage!.Contains("email"));
    }

    #endregion

    #region AddDirectorRequest Tests

    [Test]
    public void AddDirectorRequest_WithValidData_PassesValidation()
    {
        // Arrange
        var request = new AddDirectorRequest
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@testcorp.com",
            PhoneNumber = "+2348012345678",
            DateOfBirth = new DateTime(1980, 1, 1),
            Address = "123 Director Street, Lagos",
            Bvn = "12345678901",
            Nin = "12345678901",
            MeterNumber = "MTR123456"
        };

        // Act
        var validationResults = ValidateModel(request);

        // Assert
        validationResults.Should().BeEmpty();
    }

    [Test]
    [TestCase("", "First name is required")]
    [TestCase(null, "First name is required")]
    public void AddDirectorRequest_WithInvalidFirstName_FailsValidation(string firstName, string expectedError)
    {
        // Arrange
        var request = CreateValidAddDirectorRequest();
        request.FirstName = firstName;

        // Act
        var validationResults = ValidateModel(request);

        // Assert
        validationResults.Should().ContainSingle()
            .Which.ErrorMessage.Should().Be(expectedError);
    }

    [Test]
    public void AddDirectorRequest_WithTooLongBvn_FailsValidation()
    {
        // Arrange
        var request = CreateValidAddDirectorRequest();
        request.Bvn = "123456789012"; // Max is 11

        // Act
        var validationResults = ValidateModel(request);

        // Assert
        validationResults.Should().ContainSingle()
            .Which.ErrorMessage.Should().Be("BVN cannot exceed 11 characters");
    }

    [Test]
    public void AddDirectorRequest_WithTooLongNin_FailsValidation()
    {
        // Arrange
        var request = CreateValidAddDirectorRequest();
        request.Nin = "123456789012"; // Max is 11

        // Act
        var validationResults = ValidateModel(request);

        // Assert
        validationResults.Should().ContainSingle()
            .Which.ErrorMessage.Should().Be("NIN cannot exceed 11 characters");
    }

    #endregion

    #region AddCorporateDocumentRequest Tests

    [Test]
    public void AddCorporateDocumentRequest_WithValidBase64_PassesValidation()
    {
        // Arrange
        var request = new AddCorporateDocumentRequest
        {
            Cac = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("CAC Document Content")),
            Tin = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("TIN Document Content"))
        };

        // Act
        var validationResults = ValidateModel(request);

        // Assert
        validationResults.Should().BeEmpty();
    }

    [Test]
    public void AddCorporateDocumentRequest_WithNullValues_PassesValidation()
    {
        // Arrange - All fields are optional
        var request = new AddCorporateDocumentRequest();

        // Act
        var validationResults = ValidateModel(request);

        // Assert
        validationResults.Should().BeEmpty();
    }

    [Test]
    [TestCase("Invalid Base64!")]
    [TestCase("Not base64 at all")]
    [TestCase("123ABC")]
    public void AddCorporateDocumentRequest_WithInvalidBase64_FailsValidation(string invalidBase64)
    {
        // Arrange
        var request = new AddCorporateDocumentRequest
        {
            Cac = invalidBase64
        };

        // Act
        var validationResults = ValidateModel(request);

        // Assert
        validationResults.Should().ContainSingle()
            .Which.ErrorMessage.Should().Be("CAC document must be a valid base64-encoded string");
    }

    [Test]
    public void AddCorporateDocumentRequest_WithMultipleInvalidBase64_FailsValidation()
    {
        // Arrange
        var request = new AddCorporateDocumentRequest
        {
            Cac = "Invalid CAC",
            Tin = "Invalid TIN",
            BoardResolution = "Invalid Board Resolution"
        };

        // Act
        var validationResults = ValidateModel(request);

        // Assert
        validationResults.Should().HaveCount(3);
        validationResults.Should().Contain(r => r.ErrorMessage!.Contains("CAC"));
        validationResults.Should().Contain(r => r.ErrorMessage!.Contains("TIN"));
        validationResults.Should().Contain(r => r.ErrorMessage!.Contains("Board resolution"));
    }

    #endregion

    #region Helper Methods

    private static CreateCorporateCustomerRequest CreateValidCreateRequest()
    {
        return new CreateCorporateCustomerRequest
        {
            RcNumber = "RC123456",
            Tin = "TIN123456789",
            FullBusinessName = "Test Corporation Ltd",
            BusinessAddress = "123 Business District, Lagos",
            CountryId = Guid.NewGuid(),
            City = "Lagos",
            Email = "test@testcorp.com",
            WalletPreferredName = "TestCorp Wallet"
        };
    }

    private static AddDirectorRequest CreateValidAddDirectorRequest()
    {
        return new AddDirectorRequest
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@testcorp.com",
            PhoneNumber = "+2348012345678",
            DateOfBirth = new DateTime(1980, 1, 1),
            Address = "123 Director Street, Lagos",
            Bvn = "12345678901",
            Nin = "12345678901",
            MeterNumber = "MTR123456"
        };
    }

    private List<ValidationResult> ValidateModel(object model)
    {
        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(model);
        Validator.TryValidateObject(model, validationContext, validationResults, true);
        return validationResults;
    }

    #endregion
}