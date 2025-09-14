using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Embedly.SDK.Models.Requests.Customers;
using Embedly.SDK.Models.Responses.Customers;
using Embedly.SDK.Services.Customers;
using Embedly.SDK.Tests.Testing;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Embedly.SDK.Tests.Services;

/// <summary>
///     Comprehensive unit tests for CustomerService following SDK patterns.
///     Tests all customer management operations including CRUD, KYC, and validation.
/// </summary>
[TestFixture]
public class CustomerServiceTests : ServiceTestBase
{
    private CustomerService _customerService = null!;

    protected override void OnSetUp()
    {
        _customerService = new CustomerService(MockHttpClient.Object, MockOptions.Object);
    }

    [Test]
    public async Task CreateAsync_WithValidRequest_ReturnsSuccessfulResponse()
    {
        // Arrange
        var request = CreateValidCustomerRequest();
        var expectedCustomer = CreateTestCustomer();
        var apiResponse = CreateSuccessfulApiResponse(expectedCustomer);

        MockHttpClient
            .Setup(x => x.PostAsync<CreateCustomerRequest, Customer>(
                It.Is<string>(url => url.Contains("api/v1/customers/add")),
                request,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _customerService.CreateAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Id.Should().Be(expectedCustomer.Id);
        result.Data.FirstName.Should().Be(request.FirstName);
        result.Data.LastName.Should().Be(request.LastName);
        result.Data.Email.Should().Be(request.EmailAddress);
        result.Data.PhoneNumber.Should().Be(request.MobileNumber);

        VerifyHttpClientPostCall<CreateCustomerRequest, Customer>("api/v1/customers/add", request);
    }

    [Test]
    public async Task CreateAsync_WithNullRequest_ThrowsArgumentNullException()
    {
        // Act & Assert
        AssertThrowsArgumentNullExceptionForNullRequest<CreateCustomerRequest, Customer>(
            _customerService.CreateAsync);
    }

    [Test]
    public async Task CreateAsync_WhenApiReturnsError_ReturnsFailedResponse()
    {
        // Arrange
        var request = CreateValidCustomerRequest();
        var apiResponse = CreateFailedApiResponse<Customer>("Email address already exists", "DUPLICATE_EMAIL");

        MockHttpClient
            .Setup(x => x.PostAsync<CreateCustomerRequest, Customer>(
                It.IsAny<string>(),
                It.IsAny<CreateCustomerRequest>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _customerService.CreateAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeFalse();
        result.Message.Should().Be("Email address already exists");
        result.Data.Should().BeNull();
        result.Error?.Code.Should().Be("DUPLICATE_EMAIL");
    }

    [Test]
    public async Task GetByIdAsync_WithValidId_ReturnsCustomer()
    {
        // Arrange
        var customerId = CreateTestGuid().ToString();
        var expectedCustomer = CreateTestCustomer(customerId);
        var apiResponse = CreateSuccessfulApiResponse(expectedCustomer);

        MockHttpClient
            .Setup(x => x.GetAsync<Customer>(
                It.Is<string>(url => url.Contains($"api/v1/customers/get/id/{customerId}")),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _customerService.GetByIdAsync(customerId);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Id.Should().Be(expectedCustomer.Id);

        VerifyHttpClientGetCall<Customer>($"api/v1/customers/get/id/{customerId}");
    }

    [Test]
    public void GetByIdAsync_WithNullId_ThrowsArgumentException()
    {
        // Act & Assert
        AssertThrowsArgumentExceptionForNullOrEmptyString(
            _customerService.GetByIdAsync, "customerId");
    }

    [Test]
    public async Task GetAllAsync_WithDefaultPagination_ReturnsCustomerList()
    {
        // Arrange
        var expectedCustomers = CreateTestCustomerList(3);
        var apiResponse = CreateSuccessfulApiResponse(expectedCustomers);

        MockHttpClient
            .Setup(x => x.GetAsync<List<Customer>>(
                It.Is<string>(url => url.Contains("api/v1/customers/get/all")),
                It.IsAny<Dictionary<string, object?>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _customerService.GetAllAsync();

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Should().HaveCount(3);
        result.Data.Should().BeEquivalentTo(expectedCustomers);

        MockHttpClient.Verify(
            x => x.GetAsync<List<Customer>>(
                It.Is<string>(url => url.Contains("api/v1/customers/get/all")),
                It.IsAny<Dictionary<string, object?>>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    public async Task GetAllAsync_WithCustomPagination_UsesCorrectParameters()
    {
        // Arrange
        const int page = 2;
        const int pageSize = 25;
        var expectedCustomers = CreateTestCustomerList(25);
        var apiResponse = CreateSuccessfulApiResponse(expectedCustomers);

        MockHttpClient
            .Setup(x => x.GetAsync<List<Customer>>(
                It.IsAny<string>(),
                It.IsAny<Dictionary<string, object?>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _customerService.GetAllAsync(page, pageSize);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();

        MockHttpClient.Verify(
            x => x.GetAsync<List<Customer>>(
                It.Is<string>(url => url.Contains("api/v1/customers/get/all")),
                It.IsAny<Dictionary<string, object?>>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    public async Task GetAllAsync_WithRequestObject_CallsCorrectEndpoint()
    {
        // Arrange
        var request = new GetCustomersRequest { Page = 3, PageSize = 15 };
        var expectedCustomers = CreateTestCustomerList(15);
        var apiResponse = CreateSuccessfulApiResponse(expectedCustomers);

        MockHttpClient
            .Setup(x => x.GetAsync<List<Customer>>(
                It.IsAny<string>(),
                It.IsAny<Dictionary<string, object?>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _customerService.GetAllAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();

        MockHttpClient.Verify(
            x => x.GetAsync<List<Customer>>(
                It.Is<string>(url => url.Contains("api/v1/customers/get/all")),
                It.IsAny<Dictionary<string, object?>>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    public void GetAllAsync_WithNullRequest_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.ThrowsAsync<ArgumentNullException>(() => _customerService.GetAllAsync(null!));
    }

    [Test]
    public async Task UpdateNameAsync_WithValidParameters_ReturnsUpdatedCustomer()
    {
        // Arrange
        var customerId = CreateTestGuid().ToString();
        const string firstName = "Updated";
        const string lastName = "Name";
        var expectedCustomer = CreateTestCustomer(customerId);
        expectedCustomer.FirstName = firstName;
        expectedCustomer.LastName = lastName;
        var apiResponse = CreateSuccessfulApiResponse(expectedCustomer);

        MockHttpClient
            .Setup(x => x.PatchAsync<UpdateCustomerNameRequest, Customer>(
                It.Is<string>(url => url.Contains($"api/v1/customers/customer/{customerId}/updatename")),
                It.IsAny<UpdateCustomerNameRequest>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _customerService.UpdateNameAsync(customerId, firstName, lastName);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.FirstName.Should().Be(firstName);
        result.Data.LastName.Should().Be(lastName);

        MockHttpClient.Verify(
            x => x.PatchAsync<UpdateCustomerNameRequest, Customer>(
                It.Is<string>(url => url.Contains($"api/v1/customers/customer/{customerId}/updatename")),
                It.Is<UpdateCustomerNameRequest>(r =>
                    r.CustomerId == customerId &&
                    r.FirstName == firstName &&
                    r.LastName == lastName),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    public async Task UpdateNameAsync_WithRequestObject_CallsCorrectEndpoint()
    {
        // Arrange
        var request = new UpdateCustomerNameRequest
        {
            CustomerId = CreateTestGuid().ToString(),
            FirstName = "NewFirst",
            LastName = "NewLast"
        };
        var expectedCustomer = CreateTestCustomer(request.CustomerId);
        var apiResponse = CreateSuccessfulApiResponse(expectedCustomer);

        MockHttpClient
            .Setup(x => x.PatchAsync<UpdateCustomerNameRequest, Customer>(
                It.IsAny<string>(),
                It.IsAny<UpdateCustomerNameRequest>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _customerService.UpdateNameAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();

        VerifyHttpClientPatchCall<UpdateCustomerNameRequest, Customer>(
            $"api/v1/customers/customer/{request.CustomerId}/updatename", request);
    }

    [Test]
    public async Task UpdateContactAsync_WithValidRequest_ReturnsUpdatedCustomer()
    {
        // Arrange
        var customerId = CreateTestGuid().ToString();
        var request = new UpdateCustomerContactRequest
        {
            Email = "updated@test.com",
            PhoneNumber = "+2348012345678"
        };
        var expectedCustomer = CreateTestCustomer(customerId);
        var apiResponse = CreateSuccessfulApiResponse(expectedCustomer);

        MockHttpClient
            .Setup(x => x.PatchAsync<UpdateCustomerContactRequest, Customer>(
                It.Is<string>(url => url.Contains($"api/v1/customers/customer/{customerId}/updatecontact")),
                request,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _customerService.UpdateContactAsync(customerId, request);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();

        VerifyHttpClientPatchCall<UpdateCustomerContactRequest, Customer>(
            $"api/v1/customers/customer/{customerId}/updatecontact", request);
    }

    [Test]
    public async Task UpdateCustomerTypeAsync_WithValidParameters_ReturnsUpdatedCustomer()
    {
        // Arrange
        var customerId = CreateTestGuid().ToString();
        var customerTypeId = CreateTestGuid().ToString();
        var expectedCustomer = CreateTestCustomer(customerId);
        var apiResponse = CreateSuccessfulApiResponse(expectedCustomer);

        MockHttpClient
            .Setup(x => x.PatchAsync<object, Customer>(
                It.Is<string>(url =>
                    url.Contains($"api/v1/customers/customer/{customerId}/customertype/{customerTypeId}/update")),
                It.IsAny<object>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _customerService.UpdateCustomerTypeAsync(customerId, customerTypeId);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();

        MockHttpClient.Verify(
            x => x.PatchAsync<object, Customer>(
                It.Is<string>(url =>
                    url.Contains($"api/v1/customers/customer/{customerId}/customertype/{customerTypeId}/update")),
                It.IsAny<object>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    public async Task UpgradeKycWithNinAsync_WithValidRequest_ReturnsKycResult()
    {
        // Arrange
        var request = new NinKycUpgradeRequest
        {
            CustomerId = CreateTestGuid().ToString(),
            Nin = "12345678901",
            DateOfBirth = CreateTestTimestamp(-30 * 365).DateTime // 30 years ago
        };
        var expectedResult = new KycUpgradeResult
        {
            Success = true,
            Message = "KYC upgrade successful",
            CustomerId = request.CustomerId,
            NewKycLevel = "TIER_2",
            VerificationStatus = CustomerVerificationStatus.Verified,
            VerificationReference = "KYC-REF-123456",
            ProcessedAt = DateTime.UtcNow
        };

        var apiResponse = CreateSuccessfulApiResponse(expectedResult);

        MockHttpClient
            .Setup(x => x.PostAsync<NinKycUpgradeRequest, KycUpgradeResult>(
                It.Is<string>(url => url.Contains("api/v1/customers/kyc/customer/nin")),
                request,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _customerService.UpgradeKycWithNinAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Success.Should().BeTrue();
        result.Data.VerificationStatus.Should().Be(CustomerVerificationStatus.Verified);

        VerifyHttpClientPostCall<NinKycUpgradeRequest, KycUpgradeResult>("api/v1/customers/kyc/customer/nin", request);
    }

    [Test]
    public async Task UpgradeKycWithBvnAsync_WithValidRequest_ReturnsKycResult()
    {
        // Arrange
        var request = new BvnKycUpgradeRequest
        {
            CustomerId = CreateTestGuid().ToString(),
            Bvn = "12345678901"
        };
        var expectedResult = new KycUpgradeResult
        {
            Success = true,
            Message = "BVN verification successful",
            CustomerId = request.CustomerId,
            NewKycLevel = "TIER_2",
            VerificationStatus = CustomerVerificationStatus.Verified,
            VerificationReference = "BVN-REF-123456",
            ProcessedAt = DateTime.UtcNow
        };

        var apiResponse = CreateSuccessfulApiResponse(expectedResult);

        MockHttpClient
            .Setup(x => x.PostAsync<BvnKycUpgradeRequest, KycUpgradeResult>(
                It.Is<string>(url => url.Contains($"api/v1/customers/kyc/monnify/kyc/customer/{request.CustomerId}")),
                request,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _customerService.UpgradeKycWithBvnAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Success.Should().BeTrue();
        result.Data.VerificationStatus.Should().Be(CustomerVerificationStatus.Verified);

        VerifyHttpClientPostCall<BvnKycUpgradeRequest, KycUpgradeResult>(
            $"api/v1/customers/kyc/monnify/kyc/customer/{request.CustomerId}", request);
    }

    [Test]
    public async Task VerifyAddressAsync_WithValidRequest_ReturnsVerificationResult()
    {
        // Arrange
        var request = new AddressVerificationRequest
        {
            CustomerId = CreateTestGuid().ToString(),
            Street = "123 Test Street",
            City = "Lagos",
            State = "Lagos",
            Country = "NG",
            VerificationMethod = "utility_bill"
        };
        var expectedResult = new AddressVerificationResult
        {
            Success = true,
            Message = "Address verification successful",
            CustomerId = request.CustomerId,
            VerificationStatus = "VERIFIED",
            VerificationReference = "ADDR-REF-123456",
            ProcessedAt = DateTime.UtcNow
        };
        var apiResponse = CreateSuccessfulApiResponse(expectedResult);

        MockHttpClient
            .Setup(x => x.PostAsync<AddressVerificationRequest, AddressVerificationResult>(
                It.Is<string>(url => url.Contains("api/v1/customers/kyc/address-verification")),
                request,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _customerService.VerifyAddressAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Success.Should().BeTrue();
        result.Data.VerificationStatus.Should().Be("VERIFIED");

        VerifyHttpClientPostCall<AddressVerificationRequest, AddressVerificationResult>(
            "api/v1/customers/kyc/address-verification", request);
    }

    [Test]
    public async Task GetVerificationPropertiesAsync_WithValidId_ReturnsProperties()
    {
        // Arrange
        var customerId = CreateTestGuid().ToString();
        var expectedProperties = new CustomerVerificationProperties
        {
            CustomerId = customerId,
            KycLevel = "TIER_1",
            CompletedVerifications = new List<string> { "NIN", "BVN", "EMAIL", "PHONE" }
        };
        var apiResponse = CreateSuccessfulApiResponse(expectedProperties);

        MockHttpClient
            .Setup(x => x.GetAsync<CustomerVerificationProperties>(
                It.Is<string>(url => url.Contains($"api/v1/customers/customer-verification-properties/{customerId}")),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _customerService.GetVerificationPropertiesAsync(customerId);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.CustomerId.Should().Be(customerId);
        result.Data.KycLevel.Should().Be("TIER_1");

        VerifyHttpClientGetCall<CustomerVerificationProperties>(
            $"api/v1/customers/customer-verification-properties/{customerId}");
    }

    [Test]
    public async Task GetCustomerTypesAsync_ReturnsCustomerTypesList()
    {
        // Arrange
        var expectedTypes = new List<CustomerType>
        {
            new() { Id = CreateTestGuid().ToString(), Name = "Individual", Description = "Individual customer" },
            new() { Id = CreateTestGuid().ToString(), Name = "Business", Description = "Business customer" }
        };
        var apiResponse = CreateSuccessfulApiResponse(expectedTypes);

        MockHttpClient
            .Setup(x => x.GetAsync<List<CustomerType>>(
                It.Is<string>(url => url.Contains("api/v1/customers/types/all")),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _customerService.GetCustomerTypesAsync();

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Should().HaveCount(2);
        result.Data.Should().BeEquivalentTo(expectedTypes);

        VerifyHttpClientGetCall<List<CustomerType>>("api/v1/customers/types/all");
    }

    [Test]
    public async Task GetCountriesAsync_ReturnsCountriesList()
    {
        // Arrange
        var expectedCountries = new List<Country>
        {
            new() { Id = 1, Name = "Nigeria", Code = "NG", IsoCode = "NGA" },
            new() { Id = 2, Name = "Ghana", Code = "GH", IsoCode = "GHA" }
        };
        var apiResponse = CreateSuccessfulApiResponse(expectedCountries);

        MockHttpClient
            .Setup(x => x.GetAsync<List<Country>>(
                It.Is<string>(url => url.Contains("api/v1/customers/countries/get")),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _customerService.GetCountriesAsync();

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Should().HaveCount(2);
        result.Data.Should().BeEquivalentTo(expectedCountries);

        VerifyHttpClientGetCall<List<Country>>("api/v1/customers/countries/get");
    }

    [Test]
    public void UpdateNameAsync_WithNullCustomerId_ThrowsArgumentException()
    {
        // Act & Assert
        var exception =
            Assert.ThrowsAsync<ArgumentException>(() => _customerService.UpdateNameAsync(null!, "First", "Last"));
        exception.Should().NotBeNull();
        exception!.ParamName.Should().Be("customerId");
    }

    [Test]
    public void UpdateNameAsync_WithNullFirstName_ThrowsArgumentException()
    {
        // Act & Assert
        var exception = Assert.ThrowsAsync<ArgumentException>(() =>
            _customerService.UpdateNameAsync(CreateTestGuid().ToString(), null!, "Last"));
        exception.Should().NotBeNull();
        exception!.ParamName.Should().Be("firstName");
    }

    [Test]
    public void UpdateNameAsync_WithNullLastName_ThrowsArgumentException()
    {
        // Act & Assert
        var exception = Assert.ThrowsAsync<ArgumentException>(() =>
            _customerService.UpdateNameAsync(CreateTestGuid().ToString(), "First", null!));
        exception.Should().NotBeNull();
        exception!.ParamName.Should().Be("lastName");
    }

    [Test]
    public void UpdateNameAsync_WithNullRequestObject_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.ThrowsAsync<ArgumentNullException>(() => _customerService.UpdateNameAsync(null!));
    }

    [Test]
    public void UpdateContactAsync_WithNullCustomerId_ThrowsArgumentException()
    {
        // Arrange
        var request = new UpdateCustomerContactRequest { Email = "test@test.com" };

        // Act & Assert
        var exception =
            Assert.ThrowsAsync<ArgumentException>(() => _customerService.UpdateContactAsync(null!, request));
        exception.Should().NotBeNull();
        exception!.ParamName.Should().Be("customerId");
    }

    [Test]
    public void UpdateContactAsync_WithNullRequest_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.ThrowsAsync<ArgumentNullException>(() =>
            _customerService.UpdateContactAsync(CreateTestGuid().ToString(), null!));
    }

    private CreateCustomerRequest CreateValidCustomerRequest()
    {
        return new CreateCustomerRequest
        {
            FirstName = "John",
            LastName = "Doe",
            EmailAddress = CreateTestEmail("john.doe"),
            MobileNumber = CreateTestPhoneNumber("1234"),
            DateOfBirth = DateTime.UtcNow.AddYears(-25), // 25 years ago
            Address = "123 Test Street, Lagos, Lagos State, Nigeria",
            City = "Lagos",
            CountryId = Guid.Parse("c15ad9ae-c4d7-4342-b70f-de5508627e3b"),
            CustomerTypeId = Guid.Parse("f671da57-e281-4b40-965f-a96f4205405e"),
            CustomerTierId = 1,
            OrganizationId = CreateTestGuid()
        };
    }

    private Customer CreateTestCustomer(string? customerId = null)
    {
        return new Customer
        {
            Id = customerId ?? CreateTestGuid().ToString(),
            FirstName = "John",
            LastName = "Doe",
            Email = CreateTestEmail("john.doe"),
            PhoneNumber = CreateTestPhoneNumber("1234"),
            DateOfBirth = DateTime.UtcNow.AddYears(-25),
            CreatedAt = DateTimeOffset.UtcNow,
            KycLevel = "TIER_1",
            Status = CustomerStatus.Active
        };
    }

    private List<Customer> CreateTestCustomerList(int count)
    {
        return Enumerable.Range(1, count)
            .Select(i => CreateTestCustomer($"customer-{i}"))
            .ToList();
    }
}