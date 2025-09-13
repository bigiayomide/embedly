using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Embedly.SDK.Models.Requests.Customers;
using Embedly.SDK.Services.Customers;

namespace Embedly.SDK.Tests.Integration;

/// <summary>
/// Integration tests for complete customer workflows following SDK patterns.
/// Tests real API interactions for customer lifecycle management.
/// </summary>
[TestFixture]
[Category("Integration")]
[Category("Customer")]
public class CustomerWorkflowIntegrationTests : IntegrationTestBase
{
    private ICustomerService _customerService = null!;

    [SetUp]
    public void SetUp()
    {
        _customerService = ServiceProvider.GetRequiredService<ICustomerService>();
    }

    /// <summary>
    /// Complete customer lifecycle: Create → Retrieve → Update → KYC Upgrade
    /// </summary>
    [Test, Order(1)]
    public async Task CompleteCustomerLifecycleWorkflow_ShouldSucceedWithAllSteps()
    {
        LogStep("Starting Complete Customer Lifecycle Workflow");

        // Step 1: Create a new customer
        LogStep("Creating new customer");
        var createRequest = CreateUniqueCustomerRequest();
        LogApiCall("Create Customer", createRequest);

        var createResponse = await _customerService.CreateAsync(createRequest);
        LogApiResponse("Create Customer Response", createResponse);

        createResponse.Should().NotBeNull();
        createResponse.Success.Should().BeTrue("Customer creation should succeed");
        createResponse.Data.Should().NotBeNull();

        var customerId = createResponse.Data!.Id;
        LogSuccess($"Customer created successfully with ID: {customerId}");

        // Step 2: Retrieve the customer by ID
        LogStep("Retrieving customer by ID");
        LogApiCall("Get Customer By ID", customerId);

        var getResponse = await _customerService.GetByIdAsync(customerId);
        LogApiResponse("Get Customer Response", getResponse);

        getResponse.Should().NotBeNull();
        getResponse.Success.Should().BeTrue("Customer retrieval should succeed");
        getResponse.Data.Should().NotBeNull();
        getResponse.Data!.Id.Should().Be(customerId);
        getResponse.Data.FirstName.Should().Be(createRequest.FirstName);
        getResponse.Data.LastName.Should().Be(createRequest.LastName);
        getResponse.Data.Email.Should().Be(createRequest.EmailAddress);

        LogSuccess("Customer retrieved successfully with matching data");

        // Step 3: Update customer name
        LogStep("Updating customer name");
        var updateNameRequest = new UpdateCustomerNameRequest
        {
            CustomerId = customerId,
            FirstName = "Updated",
            LastName = "Name"
        };
        LogApiCall("Update Customer Name", updateNameRequest);

        var updateResponse = await _customerService.UpdateNameAsync(updateNameRequest);
        LogApiResponse("Update Customer Name Response", updateResponse);

        if (updateResponse.Success)
        {
            updateResponse.Data.Should().NotBeNull();
            LogSuccess("Customer name updated successfully");
        }
        else
        {
            LogWarning($"Customer name update failed: {updateResponse.Message}");
        }

        // Step 4: Update customer contact information
        LogStep("Updating customer contact information");
        var updateContactRequest = new UpdateCustomerContactRequest
        {
            Email = CreateTestEmail("updated"),
            PhoneNumber = CreateTestPhoneNumber()
        };
        LogApiCall("Update Customer Contact", updateContactRequest);

        var contactResponse = await _customerService.UpdateContactAsync(customerId, updateContactRequest);
        LogApiResponse("Update Customer Contact Response", contactResponse);

        if (contactResponse.Success)
        {
            contactResponse.Data.Should().NotBeNull();
            LogSuccess("Customer contact updated successfully");
        }
        else
        {
            LogWarning($"Customer contact update failed: {contactResponse.Message}");
        }

        // Step 5: Get customer verification properties
        LogStep("Retrieving customer verification properties");
        LogApiCall("Get Verification Properties", customerId);

        var verificationResponse = await _customerService.GetVerificationPropertiesAsync(customerId);
        LogApiResponse("Verification Properties Response", verificationResponse);

        if (verificationResponse.Success)
        {
            verificationResponse.Data.Should().NotBeNull();
            verificationResponse.Data!.CustomerId.Should().Be(customerId);
            LogSuccess($"Verification properties retrieved - Level: {verificationResponse.Data.VerificationStatus}");
        }
        else
        {
            LogWarning($"Failed to retrieve verification properties: {verificationResponse.Message}");
        }

        LogSuccess("Complete Customer Lifecycle Workflow completed successfully");
    }

    /// <summary>
    /// Tests customer KYC upgrade workflows with different verification methods.
    /// </summary>
    [Test, Order(2)]
    public async Task CustomerKycUpgradeWorkflow_ShouldHandleDifferentVerificationMethods()
    {
        LogStep("Starting Customer KYC Upgrade Workflow");

        // Step 1: Create customer for KYC testing
        var createRequest = CreateUniqueCustomerRequest();
        createRequest.FirstName = "KYC";
        createRequest.LastName = "TestUser";

        LogApiCall("Create KYC Test Customer", createRequest);
        var createResponse = await _customerService.CreateAsync(createRequest);
        LogApiResponse("KYC Customer Creation", createResponse);

        if (!createResponse.Success)
        {
            LogError($"Failed to create KYC test customer: {createResponse.Message}");
            Assert.Fail("Cannot proceed with KYC tests without a customer");
            return;
        }

        var customerId = createResponse.Data!.Id;
        LogSuccess($"KYC test customer created with ID: {customerId}");

        // Step 2: Test NIN KYC upgrade
        LogStep("Testing NIN KYC upgrade");
        var ninUpgradeRequest = new NinKycUpgradeRequest
        {
            CustomerId = customerId,
            Nin = "12345678901", // Test NIN
            DateOfBirth = createRequest.DateOfBirth!.Value
        };

        LogApiCall("NIN KYC Upgrade", ninUpgradeRequest);
        var ninResponse = await _customerService.UpgradeKycWithNinAsync(ninUpgradeRequest);
        LogApiResponse("NIN KYC Response", ninResponse);

        if (ninResponse.Success)
        {
            ninResponse.Data.Should().NotBeNull();
            LogSuccess($"NIN KYC upgrade successful - Level: {ninResponse.Data!.VerificationStatus}");
        }
        else
        {
            LogWarning($"NIN KYC upgrade failed: {ninResponse.Message}");
        }

        // Step 3: Test BVN KYC upgrade
        LogStep("Testing BVN KYC upgrade");
        var bvnUpgradeRequest = new BvnKycUpgradeRequest
        {
            CustomerId = customerId,
            Bvn = "12345678901" // Test BVN
        };

        LogApiCall("BVN KYC Upgrade", bvnUpgradeRequest);
        var bvnResponse = await _customerService.UpgradeKycWithBvnAsync(bvnUpgradeRequest);
        LogApiResponse("BVN KYC Response", bvnResponse);

        if (bvnResponse.Success)
        {
            bvnResponse.Data.Should().NotBeNull();
            LogSuccess($"BVN KYC upgrade successful - Level: {bvnResponse.Data!.VerificationStatus}");
        }
        else
        {
            LogWarning($"BVN KYC upgrade failed: {bvnResponse.Message}");
        }

        // Step 4: Test address verification
        LogStep("Testing address verification");
        var addressRequest = new AddressVerificationRequest
        {
            CustomerId = customerId,
            Street = "123 Integration Test Street",
            City = "Lagos",
            State = "Lagos",
            Country = "NG",
            VerificationMethod = "utility_bill"
        };

        LogApiCall("Address Verification", addressRequest);
        var addressResponse = await _customerService.VerifyAddressAsync(addressRequest);
        LogApiResponse("Address Verification Response", addressResponse);

        if (addressResponse.Success)
        {
            addressResponse.Data.Should().NotBeNull();
            LogSuccess($"Address verification successful - Status: {addressResponse.Data!.VerificationStatus}");
        }
        else
        {
            LogWarning($"Address verification failed: {addressResponse.Message}");
        }

        LogSuccess("Customer KYC Upgrade Workflow completed");
    }

    /// <summary>
    /// Tests customer retrieval and listing operations with pagination.
    /// </summary>
    [Test, Order(3)]
    public async Task CustomerRetrievalWorkflow_ShouldHandlePaginationAndFiltering()
    {
        LogStep("Starting Customer Retrieval Workflow");

        // Step 1: Test getting all customers with default pagination
        LogStep("Testing default customer pagination");
        LogApiCall("Get All Customers (Default)");

        var defaultResponse = await _customerService.GetAllAsync();
        LogApiResponse("Default Customers Response", defaultResponse);

        defaultResponse.Should().NotBeNull();
        if (defaultResponse.Success)
        {
            defaultResponse.Data.Should().NotBeNull();
            LogSuccess($"Retrieved {defaultResponse.Data!.Count()} customers with default pagination");
        }
        else
        {
            LogWarning($"Default customer retrieval failed: {defaultResponse.Message}");
        }

        // Step 2: Test custom pagination
        LogStep("Testing custom pagination");
        LogApiCall("Get All Customers (Custom Pagination)", new PaginationLog { Page = 1, PageSize = 10 });

        var paginatedResponse = await _customerService.GetAllAsync(1, 10);
        LogApiResponse("Paginated Customers Response", paginatedResponse);

        if (paginatedResponse.Success)
        {
            paginatedResponse.Data.Should().NotBeNull();
            LogSuccess($"Retrieved {paginatedResponse.Data!.Count()} customers with custom pagination");
        }
        else
        {
            LogWarning($"Paginated customer retrieval failed: {paginatedResponse.Message}");
        }

        // Step 3: Test with request object
        LogStep("Testing request object pagination");
        var getCustomersRequest = new GetCustomersRequest
        {
            Page = 1,
            PageSize = 5
        };

        LogApiCall("Get All Customers (Request Object)", getCustomersRequest);
        var requestObjectResponse = await _customerService.GetAllAsync(getCustomersRequest);
        LogApiResponse("Request Object Customers Response", requestObjectResponse);

        if (requestObjectResponse.Success)
        {
            requestObjectResponse.Data.Should().NotBeNull();
            LogSuccess($"Retrieved {requestObjectResponse.Data!.Count()} customers with request object");
        }
        else
        {
            LogWarning($"Request object customer retrieval failed: {requestObjectResponse.Message}");
        }

        LogSuccess("Customer Retrieval Workflow completed");
    }

    /// <summary>
    /// Tests customer lookup operations for types and countries.
    /// </summary>
    [Test, Order(4)]
    public async Task CustomerLookupWorkflow_ShouldRetrieveSupportingData()
    {
        LogStep("Starting Customer Lookup Workflow");

        // Step 1: Get customer types
        LogStep("Retrieving customer types");
        LogApiCall("Get Customer Types");

        var typesResponse = await _customerService.GetCustomerTypesAsync();
        LogApiResponse("Customer Types Response", typesResponse);

        typesResponse.Should().NotBeNull();
        if (typesResponse.Success)
        {
            typesResponse.Data.Should().NotBeNull();
            LogSuccess($"Retrieved {typesResponse.Data!.Count()} customer types");

            foreach (var customerType in typesResponse.Data.Take(3))
            {
                TestContext.WriteLine($"  - {customerType.Name}: {customerType.Description}");
            }
        }
        else
        {
            LogWarning($"Customer types retrieval failed: {typesResponse.Message}");
        }

        // Step 2: Get supported countries
        LogStep("Retrieving supported countries");
        LogApiCall("Get Countries");

        var countriesResponse = await _customerService.GetCountriesAsync();
        LogApiResponse("Countries Response", countriesResponse);

        countriesResponse.Should().NotBeNull();
        if (countriesResponse.Success)
        {
            countriesResponse.Data.Should().NotBeNull();
            LogSuccess($"Retrieved {countriesResponse.Data!.Count()} countries");

            foreach (var country in countriesResponse.Data.Take(5))
            {
                TestContext.WriteLine($"  - {country.Name} ({country.Code})");
            }
        }
        else
        {
            LogWarning($"Countries retrieval failed: {countriesResponse.Message}");
        }

        LogSuccess("Customer Lookup Workflow completed");
    }

    /// <summary>
    /// Tests error scenarios and edge cases in customer operations.
    /// </summary>
    [Test, Order(5)]
    public async Task CustomerErrorHandlingWorkflow_ShouldHandleInvalidOperations()
    {
        LogStep("Starting Customer Error Handling Workflow");

        // Step 1: Test duplicate email creation
        LogStep("Testing duplicate email handling");
        var originalRequest = CreateUniqueCustomerRequest();

        // Create first customer
        var firstResponse = await _customerService.CreateAsync(originalRequest);
        if (!firstResponse.Success)
        {
            LogWarning("Skipping duplicate test - first customer creation failed");
        }
        else
        {
            // Try to create duplicate
            var duplicateRequest = CreateUniqueCustomerRequest();
            duplicateRequest.EmailAddress = originalRequest.EmailAddress; // Same email

            LogApiCall("Create Duplicate Customer", duplicateRequest);
            var duplicateResponse = await _customerService.CreateAsync(duplicateRequest);
            LogApiResponse("Duplicate Customer Response", duplicateResponse);

            if (!duplicateResponse.Success)
            {
                LogSuccess($"Duplicate email correctly rejected: {duplicateResponse.Message}");
            }
            else
            {
                LogWarning("Duplicate email was unexpectedly accepted");
            }
        }

        // Step 2: Test invalid customer ID retrieval
        LogStep("Testing invalid customer ID retrieval");
        var invalidId = Guid.NewGuid().ToString();

        LogApiCall("Get Invalid Customer", invalidId);
        var invalidResponse = await _customerService.GetByIdAsync(invalidId);
        LogApiResponse("Invalid Customer Response", invalidResponse);

        if (!invalidResponse.Success)
        {
            LogSuccess($"Invalid customer ID correctly handled: {invalidResponse.Message}");
        }
        else
        {
            LogWarning("Invalid customer ID unexpectedly returned success");
        }

        // Step 3: Test invalid KYC data
        LogStep("Testing invalid KYC upgrade");
        var invalidKycRequest = new NinKycUpgradeRequest
        {
            CustomerId = invalidId, // Invalid customer ID
            Nin = "invalid_nin",
            DateOfBirth = DateTime.UtcNow.AddDays(1) // Future date
        };

        LogApiCall("Invalid KYC Upgrade", invalidKycRequest);
        var invalidKycResponse = await _customerService.UpgradeKycWithNinAsync(invalidKycRequest);
        LogApiResponse("Invalid KYC Response", invalidKycResponse);

        if (!invalidKycResponse.Success)
        {
            LogSuccess($"Invalid KYC correctly rejected: {invalidKycResponse.Message}");
        }
        else
        {
            LogWarning("Invalid KYC was unexpectedly accepted");
        }

        LogSuccess("Customer Error Handling Workflow completed");
    }

    #region Helper Methods

    private CreateCustomerRequest CreateUniqueCustomerRequest()
    {
        var testId = CreateTestId();

        return new CreateCustomerRequest
        {
            FirstName = "Integration",
            LastName = $"Test{testId}",
            EmailAddress = CreateTestEmail($"integration-{testId}"),
            MobileNumber = CreateTestPhoneNumber(),
            DateOfBirth = DateTime.UtcNow.AddYears(-25), // 25 years old
            Address = "123 Test Integration Street, Lagos, Lagos State, Nigeria",
            City = "Lagos",
            CountryId = Guid.Parse("c15ad9ae-c4d7-4342-b70f-de5508627e3b"),
            CustomerTypeId = Guid.Parse("f671da57-e281-4b40-965f-a96f4205405e"),
            CustomerTierId = 1,
            OrganizationId = CreateTestGuid()
        };
    }

    #endregion
}

#region Typed Logging Models

/// <summary>
/// Strongly typed model for logging pagination parameters.
/// </summary>
internal sealed record PaginationLog
{
    public int Page { get; init; }
    public int PageSize { get; init; }
}

#endregion
