using System;
using System.Linq;
using System.Threading.Tasks;
using Embedly.SDK.Models.Requests.CorporateCustomers;
using Embedly.SDK.Services.CorporateCustomers;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Embedly.SDK.Tests.Integration;

/// <summary>
///     Integration tests for complete corporate customer workflows following SDK patterns.
///     Tests real API interactions for corporate customer lifecycle management.
/// </summary>
[TestFixture]
[Category("Integration")]
[Category("CorporateCustomer")]
public class CorporateCustomerWorkflowIntegrationTests : IntegrationTestBase
{
    private ICorporateCustomerService _corporateCustomerService = null!;

    [SetUp]
    public void SetUp()
    {
        _corporateCustomerService = ServiceProvider.GetRequiredService<ICorporateCustomerService>();
    }

    /// <summary>
    ///     Complete corporate customer lifecycle: Create → Retrieve → Update → Add Director → Add Documents → Create Wallet
    /// </summary>
    [Test]
    [Order(1)]
    public async Task CompleteCorporateCustomerLifecycleWorkflow_ShouldSucceedWithAllSteps()
    {
        LogStep("Starting Complete Corporate Customer Lifecycle Workflow");

        // Step 1: Create a new corporate customer
        LogStep("Creating new corporate customer");
        var createRequest = CreateUniqueCorporateCustomerRequest();
        LogApiCall("Create Corporate Customer", createRequest);

        var createResponse = await _corporateCustomerService.CreateAsync(createRequest);
        LogApiResponse("Create Corporate Customer Response", createResponse);

        createResponse.Should().NotBeNull();
        createResponse.Success.Should().BeTrue("Corporate customer creation should succeed");
        createResponse.Data.Should().NotBeNull();

        var corporateCustomerId = createResponse.Data!.Id.ToString();
        LogSuccess($"Created corporate customer with ID: {corporateCustomerId}");

        // Step 2: Retrieve the created corporate customer
        LogStep("Retrieving corporate customer by ID");
        var getResponse = await _corporateCustomerService.GetByIdAsync(corporateCustomerId);
        LogApiResponse("Get Corporate Customer Response", getResponse);

        getResponse.Should().NotBeNull();
        getResponse.Success.Should().BeTrue("Corporate customer retrieval should succeed");
        getResponse.Data.Should().NotBeNull();
        getResponse.Data!.Id.ToString().Should().Be(corporateCustomerId);
        getResponse.Data.FullBusinessName.Should().Be(createRequest.FullBusinessName);

        // Step 3: Update the corporate customer
        LogStep("Updating corporate customer information");
        var updateRequest = new UpdateCorporateCustomerRequest
        {
            FullBusinessName = $"Updated {createRequest.FullBusinessName}",
            Email = "updated.email@testcorp.com",
            City = "Updated City"
        };
        LogApiCall("Update Corporate Customer", updateRequest);

        var updateResponse = await _corporateCustomerService.UpdateAsync(corporateCustomerId, updateRequest);
        LogApiResponse("Update Corporate Customer Response", updateResponse);

        updateResponse.Should().NotBeNull();
        updateResponse.Success.Should().BeTrue("Corporate customer update should succeed");
        updateResponse.Data.Should().NotBeNull();
        updateResponse.Data!.FullBusinessName.Should().Be(updateRequest.FullBusinessName);

        // Step 4: Add a director to the corporate customer
        LogStep("Adding director to corporate customer");
        var addDirectorRequest = CreateUniqueDirectorRequest();
        LogApiCall("Add Director", addDirectorRequest);

        var addDirectorResponse = await _corporateCustomerService.AddDirectorAsync(corporateCustomerId, addDirectorRequest);
        LogApiResponse("Add Director Response", addDirectorResponse);

        addDirectorResponse.Should().NotBeNull();
        addDirectorResponse.Success.Should().BeTrue("Director addition should succeed");
        addDirectorResponse.Data.Should().NotBeNull();

        var directorId = addDirectorResponse.Data!.DirectorId.ToString();
        LogSuccess($"Added director with ID: {directorId}");

        // Step 5: Retrieve directors
        LogStep("Retrieving directors for corporate customer");
        var getDirectorsResponse = await _corporateCustomerService.GetDirectorsAsync(corporateCustomerId);
        LogApiResponse("Get Directors Response", getDirectorsResponse);

        getDirectorsResponse.Should().NotBeNull();
        getDirectorsResponse.Success.Should().BeTrue("Directors retrieval should succeed");
        getDirectorsResponse.Data.Should().NotBeNull();
        getDirectorsResponse.Data!.Should().HaveCountGreaterOrEqualTo(1);

        // Step 6: Retrieve specific director
        LogStep("Retrieving specific director by ID");
        var getDirectorResponse = await _corporateCustomerService.GetDirectorByIdAsync(corporateCustomerId, directorId);
        LogApiResponse("Get Director By ID Response", getDirectorResponse);

        getDirectorResponse.Should().NotBeNull();
        getDirectorResponse.Success.Should().BeTrue("Director retrieval should succeed");
        getDirectorResponse.Data.Should().NotBeNull();
        getDirectorResponse.Data!.FirstName.Should().Be(addDirectorRequest.FirstName);

        // Step 7: Update director information
        LogStep("Updating director information");
        var updateDirectorRequest = new AddDirectorRequest
        {
            FirstName = $"Updated {addDirectorRequest.FirstName}",
            LastName = addDirectorRequest.LastName,
            Email = "updated.director@testcorp.com",
            PhoneNumber = addDirectorRequest.PhoneNumber,
            DateOfBirth = addDirectorRequest.DateOfBirth,
            Address = addDirectorRequest.Address,
            Bvn = addDirectorRequest.Bvn,
            Nin = addDirectorRequest.Nin,
            MeterNumber = addDirectorRequest.MeterNumber
        };
        LogApiCall("Update Director", updateDirectorRequest);

        var updateDirectorResponse = await _corporateCustomerService.UpdateDirectorAsync(corporateCustomerId, directorId, updateDirectorRequest);
        LogApiResponse("Update Director Response", updateDirectorResponse);

        updateDirectorResponse.Should().NotBeNull();
        updateDirectorResponse.Success.Should().BeTrue("Director update should succeed");
        updateDirectorResponse.Data.Should().NotBeNull();
        updateDirectorResponse.Data!.FirstName.Should().Be(updateDirectorRequest.FirstName);

        // Step 8: Add documents to corporate customer
        LogStep("Adding documents to corporate customer");
        var addDocumentRequest = CreateUniqueDocumentRequest();
        LogApiCall("Add Documents", addDocumentRequest);

        var addDocumentResponse = await _corporateCustomerService.AddDocumentAsync(corporateCustomerId, addDocumentRequest);
        LogApiResponse("Add Documents Response", addDocumentResponse);

        addDocumentResponse.Should().NotBeNull();
        addDocumentResponse.Success.Should().BeTrue("Document addition should succeed");
        addDocumentResponse.Data.Should().NotBeNull();

        // Step 9: Retrieve documents
        LogStep("Retrieving documents for corporate customer");
        var getDocumentsResponse = await _corporateCustomerService.GetDocumentsAsync(corporateCustomerId);
        LogApiResponse("Get Documents Response", getDocumentsResponse);

        getDocumentsResponse.Should().NotBeNull();
        getDocumentsResponse.Success.Should().BeTrue("Documents retrieval should succeed");
        getDocumentsResponse.Data.Should().NotBeNull();

        // Step 10: Update documents
        LogStep("Updating documents for corporate customer");
        var updateDocumentRequest = new UpdateCorporateDocumentRequest
        {
            Cac = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("Updated CAC Document Content"))
        };
        LogApiCall("Update Documents", updateDocumentRequest);

        var updateDocumentResponse = await _corporateCustomerService.UpdateDocumentAsync(corporateCustomerId, updateDocumentRequest);
        LogApiResponse("Update Documents Response", updateDocumentResponse);

        updateDocumentResponse.Should().NotBeNull();
        updateDocumentResponse.Success.Should().BeTrue("Document update should succeed");
        updateDocumentResponse.Data.Should().NotBeNull();

        LogStep("Complete Corporate Customer Lifecycle Workflow completed successfully");
    }

    /// <summary>
    ///     Test error handling for invalid corporate customer operations
    /// </summary>
    [Test]
    [Order(2)]
    public async Task CorporateCustomerErrorHandling_ShouldHandleInvalidOperations()
    {
        LogStep("Testing Corporate Customer Error Handling");

        // Test 1: Try to get non-existent corporate customer
        LogStep("Testing retrieval of non-existent corporate customer");
        var nonExistentId = Guid.NewGuid().ToString();
        var getResponse = await _corporateCustomerService.GetByIdAsync(nonExistentId);
        LogApiResponse("Get Non-Existent Corporate Customer Response", getResponse);

        getResponse.Should().NotBeNull();
        getResponse.Success.Should().BeFalse("Retrieval of non-existent corporate customer should fail");

        // Test 2: Try to update non-existent corporate customer
        LogStep("Testing update of non-existent corporate customer");
        var updateRequest = new UpdateCorporateCustomerRequest
        {
            FullBusinessName = "Non-existent Corporation"
        };

        var updateResponse = await _corporateCustomerService.UpdateAsync(nonExistentId, updateRequest);
        LogApiResponse("Update Non-Existent Corporate Customer Response", updateResponse);

        updateResponse.Should().NotBeNull();
        updateResponse.Success.Should().BeFalse("Update of non-existent corporate customer should fail");

        // Test 3: Try to add director to non-existent corporate customer
        LogStep("Testing add director to non-existent corporate customer");
        var addDirectorRequest = CreateUniqueDirectorRequest();

        var addDirectorResponse = await _corporateCustomerService.AddDirectorAsync(nonExistentId, addDirectorRequest);
        LogApiResponse("Add Director to Non-Existent Corporate Customer Response", addDirectorResponse);

        addDirectorResponse.Should().NotBeNull();
        addDirectorResponse.Success.Should().BeFalse("Adding director to non-existent corporate customer should fail");

        LogStep("Corporate Customer Error Handling tests completed");
    }

    #region Helper Methods

    private CreateCorporateCustomerRequest CreateUniqueCorporateCustomerRequest()
    {
        var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        return new CreateCorporateCustomerRequest
        {
            RcNumber = $"RC{timestamp}",
            Tin = $"TIN{timestamp}",
            FullBusinessName = $"Test Corporation {timestamp} Ltd",
            BusinessAddress = $"123 Business District {timestamp}, Lagos",
            CountryId = Guid.Parse("550e8400-e29b-41d4-a716-446655440000"), // Use a fixed test country ID
            City = "Lagos",
            Email = $"test{timestamp}@testcorp.com",
            WalletPreferredName = $"TestCorp{timestamp} Wallet"
        };
    }

    private AddDirectorRequest CreateUniqueDirectorRequest()
    {
        var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        return new AddDirectorRequest
        {
            FirstName = $"John{timestamp}",
            LastName = $"Doe{timestamp}",
            Email = $"john.doe{timestamp}@testcorp.com",
            PhoneNumber = $"+23480123456{timestamp.ToString()[^2..]}",
            DateOfBirth = new DateTime(1980, 1, 1),
            Address = $"123 Director Street {timestamp}, Lagos",
            Bvn = $"{timestamp}"[^11..].PadLeft(11, '0'), // Last 11 digits
            Nin = $"{timestamp}"[^11..].PadLeft(11, '1'), // Last 11 digits
            MeterNumber = $"MTR{timestamp}"
        };
    }

    private AddCorporateDocumentRequest CreateUniqueDocumentRequest()
    {
        var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        return new AddCorporateDocumentRequest
        {
            Cac = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"CAC Document Content {timestamp}")),
            Tin = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"TIN Document Content {timestamp}")),
            BoardResolution = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes($"Board Resolution Content {timestamp}"))
        };
    }

    #endregion
}