using Embedly.Examples.Infrastructure.Services;
using Embedly.SDK;
using Embedly.SDK.Exceptions;
using Embedly.SDK.Models.Requests.Wallets;

namespace Embedly.Examples.Examples;

/// <summary>
///     Real working examples for wallet operations.
/// </summary>
public class WalletExamples(
    IEmbedlyClient embedlyClient,
    ILogger<WalletExamples> logger,
    ICorrelationService correlationService,
    IRetryService retryService)
{
    /// <summary>
    ///     Example: Create a wallet for a customer.
    /// </summary>
    public async Task<Result<string>> CreateWalletAsync(string customerId)
    {
        var correlationId = correlationService.GenerateNew();

        return await retryService.ExecuteWithRetryAsync(async () =>
        {
            // You'll need to get these from your system configuration
            var customerGuid = Guid.Parse(customerId);
            var currencyId = Guid.Parse("12345678-1234-1234-1234-123456789012"); // Replace with actual NGN currency ID
            var walletClassificationId = Guid.Parse("12345678-1234-1234-1234-123456789012"); // Replace with actual
            var customerTypeId = Guid.Parse("12345678-1234-1234-1234-123456789012"); // Replace with actual

            var request = new CreateWalletRequest
            {
                CustomerId = customerGuid.ToString(),
                CurrencyId = currencyId.ToString(),
                WalletClassificationId = walletClassificationId.ToString(),
                CustomerTypeId = customerTypeId.ToString(),
                IsDefault = true,
                Name = "Primary Wallet"
            };

            logger.LogInformation("Creating wallet for customer: {CustomerId}. Correlation: {CorrelationId}",
                customerId, correlationId);

            var response = await embedlyClient.Wallets.CreateWalletAsync(request);

            if (!response.Success || response.Data == null)
                throw new InvalidOperationException($"Wallet creation failed: {response.Error?.Message}");

            logger.LogInformation("Wallet created successfully: {WalletId}. Correlation: {CorrelationId}",
                response.Data.Id, correlationId);

            return response.Data.Id.ToString();
        }, "CreateWallet");
    }

    /// <summary>
    ///     Example: Get wallet details.
    /// </summary>
    public async Task<Result<string>> GetWalletAsync(string walletId)
    {
        var correlationId = correlationService.GenerateNew();

        return await retryService.ExecuteWithRetryAsync(async () =>
        {
            logger.LogInformation("Retrieving wallet: {WalletId}. Correlation: {CorrelationId}",
                walletId, correlationId);

            var response = await embedlyClient.Wallets.GetWalletAsync(walletId);

            if (!response.Success || response.Data == null)
                throw new InvalidOperationException($"Wallet retrieval failed: {response.Error?.Message}");

            var wallet = response.Data;
            var balanceInfo = $"Balance: {wallet.AvailableBalance:C}";

            logger.LogInformation("Wallet retrieved: {WalletId}, {Balance}. Correlation: {CorrelationId}",
                walletId, balanceInfo, correlationId);

            return $"Wallet {walletId}: {balanceInfo}";
        }, "GetWallet");
    }

    /// <summary>
    ///     Example: Demonstrate complete wallet workflow.
    /// </summary>
    public async Task<Result<string>> DemonstrateWalletWorkflowAsync()
    {
        var correlationId = correlationService.GenerateNew();
        logger.LogInformation("Starting wallet workflow demonstration. Correlation: {CorrelationId}", correlationId);

        try
        {
            // For this demo, we'll assume we have a customer ID
            // In a real scenario, you'd create a customer first
            var customerId = "12345678-1234-1234-1234-123456789012"; // Replace with actual customer ID

            // Step 1: Create wallet
            Console.WriteLine("Step 1: Creating wallet...");
            var createResult = await CreateWalletAsync(customerId);
            if (!createResult.IsSuccess)
                return Result<string>.Failure($"Failed to create wallet: {createResult.Error}");

            var walletId = createResult.Value!;
            Console.WriteLine($"✅ Wallet created: {walletId}");

            // Step 2: Get wallet details
            Console.WriteLine("\nStep 2: Retrieving wallet details...");
            var getResult = await GetWalletAsync(walletId);
            if (!getResult.IsSuccess) return Result<string>.Failure($"Failed to retrieve wallet: {getResult.Error}");

            Console.WriteLine($"✅ {getResult.Value}");

            Console.WriteLine("\n✅ Wallet workflow completed successfully!");
            return Result<string>.Success($"Workflow completed for wallet {walletId}");
        }
        catch (EmbedlyApiException ex)
        {
            var error = $"API Error: {ex.StatusCode} - {ex.Message}";
            logger.LogError(ex, "API error in wallet workflow. Correlation: {CorrelationId}", correlationId);
            Console.WriteLine($"❌ {error}");
            return Result<string>.Failure(error);
        }
        catch (Exception ex)
        {
            var error = $"Unexpected error: {ex.Message}";
            logger.LogError(ex, "Unexpected error in wallet workflow. Correlation: {CorrelationId}", correlationId);
            Console.WriteLine($"❌ {error}");
            return Result<string>.Failure(error);
        }
    }
}