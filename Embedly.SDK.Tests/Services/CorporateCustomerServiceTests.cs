using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Embedly.SDK.Models.Requests.CorporateCustomers;
using Embedly.SDK.Models.Requests.Wallets;
using Embedly.SDK.Models.Responses.CorporateCustomers;
using Embedly.SDK.Models.Responses.Wallets;
using Embedly.SDK.Services.CorporateCustomers;
using Embedly.SDK.Tests.Testing;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Embedly.SDK.Tests.Services;

/// <summary>
///     Comprehensive unit tests for CorporateCustomerService following SDK patterns.
///     Tests all corporate customer management operations including CRUD, directors, documents, and wallets.
/// </summary>
[TestFixture]
public class CorporateCustomerServiceTests : ServiceTestBase
{
    private CorporateCustomerService _corporateCustomerService = null!;

    protected override void OnSetUp()
    {
        _corporateCustomerService = new CorporateCustomerService(MockHttpClient.Object, MockOptions.Object);
    }

    #region Corporate Customer Tests

    [Test]
    public async Task CreateAsync_WithValidRequest_ReturnsSuccessfulResponse()
    {
        // Arrange
        var request = CreateValidCreateCorporateCustomerRequest();
        var expectedCorporateCustomer = CreateTestCorporateCustomer();
        var apiResponse = CreateSuccessfulApiResponse(expectedCorporateCustomer);

        MockHttpClient
            .Setup(x => x.PostAsync<CreateCorporateCustomerRequest, CorporateCustomer>(
                It.Is<string>(url => url.Contains("api/v1/corporate/customers")),
                request,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _corporateCustomerService.CreateAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().BeEquivalentTo(expectedCorporateCustomer);
        MockHttpClient.Verify(x => x.PostAsync<CreateCorporateCustomerRequest, CorporateCustomer>(
            It.Is<string>(url => url.Contains("api/v1/corporate/customers")),
            request,
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public void CreateAsync_WithNullRequest_ThrowsArgumentNullException()
    {
        // Act & Assert
        var act = () => _corporateCustomerService.CreateAsync(null!);
        act.Should().ThrowAsync<ArgumentNullException>()
            .WithParameterName("request");
    }

    [Test]
    public async Task GetByIdAsync_WithValidId_ReturnsSuccessfulResponse()
    {
        // Arrange
        var corporateCustomerId = Guid.NewGuid().ToString();
        var expectedCorporateCustomer = CreateTestCorporateCustomer();
        var apiResponse = CreateSuccessfulApiResponse(expectedCorporateCustomer);

        MockHttpClient
            .Setup(x => x.GetAsync<CorporateCustomer>(
                It.Is<string>(url => url.Contains($"api/v1/corporate/customers/{corporateCustomerId}")),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _corporateCustomerService.GetByIdAsync(corporateCustomerId);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().BeEquivalentTo(expectedCorporateCustomer);
    }

    [Test]
    public void GetByIdAsync_WithNullOrEmptyId_ThrowsArgumentException()
    {
        // Act & Assert
        var act1 = () => _corporateCustomerService.GetByIdAsync(null!);
        var act2 = () => _corporateCustomerService.GetByIdAsync(string.Empty);
        var act3 = () => _corporateCustomerService.GetByIdAsync("   ");

        act1.Should().ThrowAsync<ArgumentException>();
        act2.Should().ThrowAsync<ArgumentException>();
        act3.Should().ThrowAsync<ArgumentException>();
    }

    [Test]
    public async Task UpdateAsync_WithValidRequest_ReturnsSuccessfulResponse()
    {
        // Arrange
        var corporateCustomerId = Guid.NewGuid().ToString();
        var request = CreateValidUpdateCorporateCustomerRequest();
        var expectedCorporateCustomer = CreateTestCorporateCustomer();
        var apiResponse = CreateSuccessfulApiResponse(expectedCorporateCustomer);

        MockHttpClient
            .Setup(x => x.PutAsync<UpdateCorporateCustomerRequest, CorporateCustomer>(
                It.Is<string>(url => url.Contains($"api/v1/corporate/customers/{corporateCustomerId}")),
                request,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _corporateCustomerService.UpdateAsync(corporateCustomerId, request);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().BeEquivalentTo(expectedCorporateCustomer);
    }

    [Test]
    public void UpdateAsync_WithInvalidParameters_ThrowsArgumentException()
    {
        // Arrange
        var request = CreateValidUpdateCorporateCustomerRequest();

        // Act & Assert
        var act1 = () => _corporateCustomerService.UpdateAsync(null!, request);
        var act2 = () => _corporateCustomerService.UpdateAsync(Guid.NewGuid().ToString(), null!);

        act1.Should().ThrowAsync<ArgumentException>();
        act2.Should().ThrowAsync<ArgumentNullException>();
    }

    #endregion

    #region Director Management Tests

    [Test]
    public async Task AddDirectorAsync_WithValidRequest_ReturnsSuccessfulResponse()
    {
        // Arrange
        var corporateCustomerId = Guid.NewGuid().ToString();
        var request = CreateValidAddDirectorRequest();
        var expectedDirectorResponse = CreateTestDirectorResponse();
        var apiResponse = CreateSuccessfulApiResponse(expectedDirectorResponse);

        MockHttpClient
            .Setup(x => x.PostAsync<AddDirectorRequest, AddDirectorResponse>(
                It.Is<string>(url => url.Contains($"api/v1/corporate/customers/{corporateCustomerId}/directors")),
                request,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _corporateCustomerService.AddDirectorAsync(corporateCustomerId, request);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().BeEquivalentTo(expectedDirectorResponse);
    }

    [Test]
    public async Task GetDirectorsAsync_WithValidId_ReturnsSuccessfulResponse()
    {
        // Arrange
        var corporateCustomerId = Guid.NewGuid().ToString();
        var expectedDirectors = new List<Director> { CreateTestDirector(), CreateTestDirector() };
        var apiResponse = CreateSuccessfulApiResponse(expectedDirectors);

        MockHttpClient
            .Setup(x => x.GetAsync<List<Director>>(
                It.Is<string>(url => url.Contains($"api/v1/corporate/customers/{corporateCustomerId}/directors")),
                It.Is<Dictionary<string, object?>>(dict =>
                    dict.ContainsKey("page") && dict.ContainsKey("pageSize")),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _corporateCustomerService.GetDirectorsAsync(corporateCustomerId);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().BeEquivalentTo(expectedDirectors);
    }

    [Test]
    public async Task GetDirectorByIdAsync_WithValidIds_ReturnsSuccessfulResponse()
    {
        // Arrange
        var corporateCustomerId = Guid.NewGuid().ToString();
        var directorId = Guid.NewGuid().ToString();
        var expectedDirector = CreateTestDirector();
        var apiResponse = CreateSuccessfulApiResponse(expectedDirector);

        MockHttpClient
            .Setup(x => x.GetAsync<Director>(
                It.Is<string>(url => url.Contains($"api/v1/corporate/customers/{corporateCustomerId}/directors/{directorId}")),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _corporateCustomerService.GetDirectorByIdAsync(corporateCustomerId, directorId);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().BeEquivalentTo(expectedDirector);
    }

    [Test]
    public async Task UpdateDirectorAsync_WithValidRequest_ReturnsSuccessfulResponse()
    {
        // Arrange
        var corporateCustomerId = Guid.NewGuid().ToString();
        var directorId = Guid.NewGuid().ToString();
        var request = CreateValidAddDirectorRequest();
        var expectedDirector = CreateTestDirector();
        var apiResponse = CreateSuccessfulApiResponse(expectedDirector);

        MockHttpClient
            .Setup(x => x.PutAsync<AddDirectorRequest, Director>(
                It.Is<string>(url => url.Contains($"api/v1/corporate/customers/{corporateCustomerId}/directors/{directorId}")),
                request,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _corporateCustomerService.UpdateDirectorAsync(corporateCustomerId, directorId, request);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().BeEquivalentTo(expectedDirector);
    }

    #endregion

    #region Document Management Tests

    [Test]
    public async Task AddDocumentAsync_WithValidRequest_ReturnsSuccessfulResponse()
    {
        // Arrange
        var corporateCustomerId = Guid.NewGuid().ToString();
        var request = CreateValidAddCorporateDocumentRequest();
        var expectedDocument = CreateTestCorporateDocument();
        var apiResponse = CreateSuccessfulApiResponse(expectedDocument);

        MockHttpClient
            .Setup(x => x.PostAsync<AddCorporateDocumentRequest, CorporateDocument>(
                It.Is<string>(url => url.Contains($"api/v1/corporate/customers/{corporateCustomerId}/documents")),
                request,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _corporateCustomerService.AddDocumentAsync(corporateCustomerId, request);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().BeEquivalentTo(expectedDocument);
    }

    [Test]
    public async Task GetDocumentsAsync_WithValidId_ReturnsSuccessfulResponse()
    {
        // Arrange
        var corporateCustomerId = Guid.NewGuid().ToString();
        var expectedDocuments = new List<CorporateDocument> { CreateTestCorporateDocument() };
        var apiResponse = CreateSuccessfulApiResponse(expectedDocuments.AsEnumerable());

        MockHttpClient
            .Setup(x => x.GetAsync<IEnumerable<CorporateDocument>>(
                It.Is<string>(url => url.Contains($"api/v1/corporate/customers/{corporateCustomerId}/documents")),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _corporateCustomerService.GetDocumentsAsync(corporateCustomerId);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().BeEquivalentTo(expectedDocuments);
    }

    [Test]
    public async Task UpdateDocumentAsync_WithValidRequest_ReturnsSuccessfulResponse()
    {
        // Arrange
        var corporateCustomerId = Guid.NewGuid().ToString();
        var request = CreateValidUpdateCorporateDocumentRequest();
        var expectedDocument = CreateTestCorporateDocument();
        var apiResponse = CreateSuccessfulApiResponse(expectedDocument);

        MockHttpClient
            .Setup(x => x.PutAsync<UpdateCorporateDocumentRequest, CorporateDocument>(
                It.Is<string>(url => url.Contains($"api/v1/corporate/customers/{corporateCustomerId}/documents")),
                request,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _corporateCustomerService.UpdateDocumentAsync(corporateCustomerId, request);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().BeEquivalentTo(expectedDocument);
    }

    #endregion

    #region Wallet Management Tests

    [Test]
    public async Task CreateWalletAsync_WithValidRequest_ReturnsSuccessfulResponse()
    {
        // Arrange
        var corporateCustomerId = Guid.NewGuid().ToString();
        var request = CreateValidCreateCorporateWalletRequest();
        var expectedWallet = CreateTestWallet();
        var apiResponse = CreateSuccessfulApiResponse(expectedWallet);

        MockHttpClient
            .Setup(x => x.PostAsync<CreateCorporateWalletRequest, Wallet>(
                It.Is<string>(url => url.Contains($"api/v1/corporate/customers/{corporateCustomerId}/wallets")),
                request,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _corporateCustomerService.CreateWalletAsync(corporateCustomerId, request);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().BeEquivalentTo(expectedWallet);
    }

    #endregion

    #region Test Data Helper Methods

    private static CreateCorporateCustomerRequest CreateValidCreateCorporateCustomerRequest()
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

    private static UpdateCorporateCustomerRequest CreateValidUpdateCorporateCustomerRequest()
    {
        return new UpdateCorporateCustomerRequest
        {
            FullBusinessName = "Updated Corporation Ltd",
            Email = "updated@testcorp.com",
            City = "Abuja"
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
            MeterNumber = "MTR123456",
            DiscoCode = "OYO"
        };
    }

    private static AddCorporateDocumentRequest CreateValidAddCorporateDocumentRequest()
    {
        return new AddCorporateDocumentRequest
        {
            Cac = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("CAC Document Content")),
            Tin = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("TIN Document Content"))
        };
    }

    private static UpdateCorporateDocumentRequest CreateValidUpdateCorporateDocumentRequest()
    {
        return new UpdateCorporateDocumentRequest
        {
            Cac = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("Updated CAC Document Content"))
        };
    }

    private static CreateCorporateWalletRequest CreateValidCreateCorporateWalletRequest()
    {
        return new CreateCorporateWalletRequest
        {
            CurrencyId = Guid.NewGuid(),
            Name = "Test Corporate Wallet"
        };
    }

    private static CorporateCustomer CreateTestCorporateCustomer()
    {
        return new CorporateCustomer
        {
            Id = Guid.NewGuid(),
            OrganizationId = Guid.NewGuid(),
            RcNumber = "RC123456",
            Tin = "TIN123456789",
            FullBusinessName = "Test Corporation Ltd",
            BusinessAddress = "123 Business District, Lagos",
            CountryId = Guid.NewGuid(),
            City = "Lagos",
            Email = "test@testcorp.com",
            WalletPreferredName = "TestCorp Wallet",
            CreatedAt = DateTimeOffset.UtcNow
        };
    }

    private static Director CreateTestDirector()
    {
        return new Director
        {
            Id = Guid.NewGuid(),
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@testcorp.com",
            PhoneNumber = "+2348012345678",
            DateOfBirth = new DateTime(1980, 1, 1),
            Address = "123 Director Street, Lagos",
            Bvn = "12345678901",
            Nin = "12345678901",
            MeterNumber = "MTR123456",
            CreatedAt = DateTimeOffset.UtcNow
        };
    }

    private static AddDirectorResponse CreateTestDirectorResponse()
    {
        return new AddDirectorResponse()
        {
            CustomerId = Guid.NewGuid(),
            DirectorId = Guid.NewGuid()
        };
    }

    private static CorporateDocument CreateTestCorporateDocument()
    {
        return new CorporateDocument
        {
            Id = Guid.NewGuid(),
            Cac = "https://storage.example.com/cac-document.pdf",
            Tin = "https://storage.example.com/tin-document.pdf",
            CreatedAt = DateTimeOffset.UtcNow
        };
    }

    private static Wallet CreateTestWallet()
    {
        return new Wallet
        {
            Id = Guid.NewGuid(),
            Name = "Test Corporate Wallet",
            CurrencyId = Guid.NewGuid(),
            AvailableBalance = 0,
            LedgerBalance = 0,
            IsDefault = true,
            WalletClassificationId = Guid.NewGuid(),
            CustomerTypeId = Guid.NewGuid()
        };
    }

    #endregion
}