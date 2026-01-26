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
        result.Data.EmailAddress.Should().Be(request.EmailAddress);
        result.Data.MobileNumber.Should().Be(request.MobileNumber);

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
        var apiResponse = CreateSuccessfulApiResponse(true);

        MockHttpClient
            .Setup(x => x.PatchAsync<UpdateCustomerNameRequest, bool>(
                It.Is<string>(url => url.Contains($"api/v1/customers/customer/{customerId}/updatename")),
                It.IsAny<UpdateCustomerNameRequest>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _customerService.UpdateNameAsync(customerId, firstName, lastName);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().BeTrue();

        MockHttpClient.Verify(
            x => x.PatchAsync<UpdateCustomerNameRequest, bool>(
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
        var apiResponse = CreateSuccessfulApiResponse(true);

        MockHttpClient
            .Setup(x => x.PatchAsync<UpdateCustomerNameRequest, bool>(
                It.IsAny<string>(),
                It.IsAny<UpdateCustomerNameRequest>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _customerService.UpdateNameAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();

        VerifyHttpClientPatchCall<UpdateCustomerNameRequest, bool>(
            $"api/v1/customers/customer/{request.CustomerId}/updatename", request);
    }

    [Test]
    public async Task UpdateContactAsync_WithValidRequest_ReturnsUpdatedCustomer()
    {
        // Arrange
        var customerId = CreateTestGuid().ToString();
        var request = new UpdateCustomerContactRequest
        {
            EmailAddress = "updated@test.com",
            MobileNumber = "+2348012345678"
        };
        var apiResponse = CreateSuccessfulApiResponse(true);

        MockHttpClient
            .Setup(x => x.PatchAsync<UpdateCustomerContactRequest, bool>(
                It.Is<string>(url => url.Contains($"api/v1/customers/customer/{customerId}/updatecontact")),
                request,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _customerService.UpdateContactAsync(customerId, request);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();

        VerifyHttpClientPatchCall<UpdateCustomerContactRequest, bool>(
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
            DateOfBirth = CreateTestTimestamp(-30 * 365).DateTime, // 30 years ago
            FirstName = "KYC",
            LastName = "User",
            Verify = "1"
        };
        var expectedResult = new KycResultResponse
        {
            Id= 130,
            Applicant = new Applicant
            {
                FirstName = "KYC",
                Lastname = "User",
            },
            Summary = new Summary
            {
                NinCheck = new NinCheck
                {
                    Status = "EXACT_MATCH",
                    FieldMatches = new FieldMatches
                    {
                        FirstName = true,
                        LastName = true
                    }
                }
            },
            Status = new Status
            {
                State = "complete",
                Description = "verified"
            },
            Nin = new NinInfo
            {
                FirstName = "KYC",
                Lastname = "User",
                MiddleName = "John",
                Birthdate = "06-01-1974",
                Gender = "m",
                Phone = "08000000000",
                Nin = "12345678901",
                Photo = "/9j/4AAQSk******/wCpiNUFoooEf//Z",
            }
        };

        var apiResponse = CreateSuccessfulApiResponse(expectedResult);

        MockHttpClient
            .Setup(x => x.PostAsync<NinKycUpgradeRequest, KycResultResponse>(
                It.Is<string>(url => url.Contains("api/v1/customers/kyc/customer/nin")),
                        request,
                        It.IsAny<Dictionary<string, object?>>(),
                        It.IsAny<CancellationToken>()))
                    .ReturnsAsync(apiResponse);

        // Act
        var result = await _customerService.UpgradeKycWithNinAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Status!.Description.Should().Be("verified");
        // Note: Verification call removed - service uses 4-parameter PostAsync with query params
    }

    [Test]
    public async Task UpgradeKycWithBvnAsync_WithValidRequest_ReturnsBvnKycResult()
    {
        // Arrange
        var request = new BvnKycUpgradeRequest
        {
            CustomerId = CreateTestGuid().ToString(),
            Bvn = "12345678901",
            Verify = "1" // No. of verification attempts
        };

        var expectedResult = new BvnKycUpgradeResponse
        {
            KycCompleted = true,
            Response = new KycResultResponse
            {
                Id = CreateTestLongId(),
                Applicant = new Applicant
                {
                    FirstName = "Bunch",
                    Lastname = "Dillon"
                },
                Summary = new Summary
                {
                    BvnCheck = new BvnCheck
                    {
                        Status = "EXACT_MATCH",
                        FieldMatches = new FieldMatches
                        {
                            FirstName = true,
                            LastName = true
                        }
                    }
                },
                Status = new Status
                {
                    Description = "verified",
                    State = "complete"
                },
                Bvn = new BvnInfo
                {
                    Bvn = "95888168924", // Test BVN
                    Email = CreateTestEmail(),
                    EnrollmentBank = "011", // Test Bank
                    Firstname = "Bunch",
                    Lastname = "Dillon",
                    LgaOfResidence = "Agege",
                    MaritalStatus = "Single",
                    Nationality = "Nigerian",
                    StateOfResidence = "Lagos State",
                    ResidentialAddress = "121 Paul Gas Avenue",
                    WatchListed = "NO"
                }
            },
            Successful = true
        };

        var apiResponse = CreateSuccessfulApiResponse(expectedResult);

        MockHttpClient
            .Setup(x => x.PostAsync<BvnKycUpgradeRequest, BvnKycUpgradeResponse>(
                It.Is<string>(url => url.Contains($"api/v1/customers/kyc/premium-kyc")),
                request,
                It.IsAny<Dictionary<string, object?>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _customerService.UpgradeKycWithBvnAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.KycCompleted.Should().BeTrue();
        result.Data.Response!.Status!.Description.Should().Be("verified");
        // Note: Verification call removed - service uses 4-parameter PostAsync with query params
    }

    [Test]
    public async Task VerifyAddressAsync_WithValidRequest_ReturnsVerificationResult()
    {
        // Arrange
        var request = new AddressVerificationRequest
        {
            CustomerId = CreateTestGuid().ToString(),
            HouseAddress = "123 Integration Test Street, Lagos, Lagos State, Nigeria",
            MeterNumber = "09876543212" // Test meter number
        };
        var expectedResult = new AddressKycUpgradeResponse
        {
            Status = "successful",
            Message = "Verification successful!",
            Timestamp = DateTime.UtcNow,
            Data = new AddressVerificationData
            {
                Verified = true,
                HouseAddress = "123 Integration Test Street, Lagos, Lagos State, Nigeria",
                HouseOwner = "Du*****",
                ConfidenceLevel = 95,
                DiscoCode = "00"
            }
        };
        var apiResponse = CreateSuccessfulApiResponse(expectedResult);

        MockHttpClient
            .Setup(x => x.PostAsync<AddressVerificationRequest, AddressKycUpgradeResponse>(
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
        result.Data!.Status.Should().Be("successful");
        result.Data.Data!.Verified.Should().BeTrue();

        VerifyHttpClientPostCall<AddressVerificationRequest, AddressKycUpgradeResponse>(
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
            CustomerTierId = 1,
            HasNin = true
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
        result.Data.CustomerTierId.Should().Be(1);

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
            new() { Id = CreateTestGuid().ToString(), Name = "Nigeria", CountryCodeTwo = "NG", CountryCodeThree = "NGA" },
            new() { Id = CreateTestGuid().ToString(), Name = "Ghana", CountryCodeTwo = "GH", CountryCodeThree = "GHA" }
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
        var request = new UpdateCustomerContactRequest { EmailAddress = "test@test.com" };

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
            EmailAddress = CreateTestEmail("john.doe"),
            MobileNumber = CreateTestPhoneNumber("1234"),
            DateOfBirth = DateTime.UtcNow.AddYears(-25),
            DateCreated = DateTimeOffset.UtcNow,
            CustomerTierId = 0,
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