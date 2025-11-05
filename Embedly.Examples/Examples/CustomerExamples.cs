using Embedly.Examples.Infrastructure.Services;
using Embedly.SDK;
using Embedly.SDK.Exceptions;
using Embedly.SDK.Models.Requests.Customers;

namespace Embedly.Examples.Examples;

/// <summary>
///     Real working examples for customer management operations.
/// </summary>
public class CustomerExamples(
    IEmbedlyClient embedlyClient,
    ILogger<CustomerExamples> logger,
    ICorrelationService correlationService,
    IRetryService retryService)
{
    /// <summary>
    ///     Example: Create a customer with proper error handling.
    /// </summary>
    public async Task<Result<string>> CreateCustomerAsync()
    {
        var correlationId = correlationService.GenerateNew();

        return await retryService.ExecuteWithRetryAsync(async () =>
        {
            // You'll need to get these from your system configuration or lookup services
            var organizationId = Guid.Parse("12345678-1234-1234-1234-123456789012"); // Replace with actual
            var customerTypeId = Guid.Parse("12345678-1234-1234-1234-123456789012"); // Replace with actual
            var countryId = Guid.Parse("12345678-1234-1234-1234-123456789012"); // Replace with actual Nigeria ID

            var request = new CreateCustomerRequest
            {
                OrganizationId = organizationId,
                FirstName = "John",
                LastName = "Doe",
                MiddleName = "Michael",
                EmailAddress = $"john.doe.{DateTime.Now:yyyyMMddHHmmss}@example.com",
                MobileNumber = "+2348012345678",
                DateOfBirth = new DateTime(1990, 5, 15),
                CustomerTypeId = customerTypeId,
                CountryId = countryId,
                City = "Lagos",
                Address = "123 Victoria Island, Lagos"
            };

            logger.LogInformation("Creating customer: {FirstName} {LastName}. Correlation: {CorrelationId}",
                request.FirstName, request.LastName, correlationId);

            var response = await embedlyClient.Customers.CreateAsync(request);

            if (!response.Success || response.Data == null)
                throw new InvalidOperationException($"Customer creation failed: {response.Error?.Message}");

            logger.LogInformation("Customer created successfully: {CustomerId}. Correlation: {CorrelationId}",
                response.Data.Id, correlationId);

            return response.Data.Id;
        }, "CreateCustomer");
    }

    /// <summary>
    ///     Example: Get a customer by ID.
    /// </summary>
    public async Task<Result<string>> GetCustomerAsync(string customerId)
    {
        var correlationId = correlationService.GenerateNew();

        return await retryService.ExecuteWithRetryAsync(async () =>
        {
            logger.LogInformation("Retrieving customer: {CustomerId}. Correlation: {CorrelationId}",
                customerId, correlationId);

            var response = await embedlyClient.Customers.GetByIdAsync(customerId);

            if (!response.Success || response.Data == null)
                throw new InvalidOperationException($"Customer retrieval failed: {response.Error?.Message}");

            logger.LogInformation("Customer retrieved: {CustomerName}. Correlation: {CorrelationId}",
                $"{response.Data.FirstName} {response.Data.LastName}", correlationId);

            return $"Customer: {response.Data.FirstName} {response.Data.LastName} ({response.Data.EmailAddress})";
        }, "GetCustomer");
    }

    /// <summary>
    ///     Example: List customers with pagination.
    /// </summary>
    public async Task<Result<List<string>>> ListCustomersAsync(int page = 1, int pageSize = 10)
    {
        var correlationId = correlationService.GenerateNew();

        return await retryService.ExecuteWithRetryAsync(async () =>
        {
            logger.LogInformation("Listing customers, page {Page}, size {PageSize}. Correlation: {CorrelationId}",
                page, pageSize, correlationId);

            var customers = await embedlyClient.Customers.GetAllAsync(page, pageSize);

            if (!customers.Success || customers.Data == null)
                throw new InvalidOperationException($"Customer listing failed: {customers.Error?.Message}");

            var customerList = customers.Data
                .Select(c => $"{c.FirstName} {c.LastName} - {c.EmailAddress}")
                .ToList();

            logger.LogInformation("Retrieved {Count} customers. Correlation: {CorrelationId}",
                customerList.Count, correlationId);

            return customerList;
        }, "ListCustomers");
    }

    /// <summary>
    ///     Example: Demonstrate complete customer workflow.
    /// </summary>
    public async Task<Result<string>> DemonstrateCustomerWorkflowAsync()
    {
        var correlationId = correlationService.GenerateNew();
        logger.LogInformation("Starting customer workflow demonstration. Correlation: {CorrelationId}", correlationId);

        try
        {
            // Step 1: Create customer
            Console.WriteLine("Step 1: Creating customer...");
            var createResult = await CreateCustomerAsync();
            if (!createResult.IsSuccess)
                return Result<string>.Failure($"Failed to create customer: {createResult.Error}");

            var customerId = createResult.Value!;
            Console.WriteLine($"✅ Customer created: {customerId}");

            // Step 2: Retrieve customer
            Console.WriteLine("\nStep 2: Retrieving customer details...");
            var getResult = await GetCustomerAsync(customerId);
            if (!getResult.IsSuccess) return Result<string>.Failure($"Failed to retrieve customer: {getResult.Error}");

            Console.WriteLine($"✅ {getResult.Value}");

            // Step 3: List customers
            Console.WriteLine("\nStep 3: Listing recent customers...");
            var listResult = await ListCustomersAsync(1, 5);
            if (listResult.IsSuccess && listResult.Value!.Any())
            {
                Console.WriteLine("✅ Recent customers:");
                foreach (var customer in listResult.Value!.Take(3)) Console.WriteLine($"   - {customer}");
            }

            Console.WriteLine("\n✅ Customer workflow completed successfully!");
            return Result<string>.Success($"Workflow completed for customer {customerId}");
        }
        catch (EmbedlyApiException ex)
        {
            var error = $"API Error: {ex.StatusCode} - {ex.Message}";
            logger.LogError(ex, "API error in customer workflow. Correlation: {CorrelationId}", correlationId);
            Console.WriteLine($"❌ {error}");
            return Result<string>.Failure(error);
        }
        catch (Exception ex)
        {
            var error = $"Unexpected error: {ex.Message}";
            logger.LogError(ex, "Unexpected error in customer workflow. Correlation: {CorrelationId}", correlationId);
            Console.WriteLine($"❌ {error}");
            return Result<string>.Failure(error);
        }
    }
}