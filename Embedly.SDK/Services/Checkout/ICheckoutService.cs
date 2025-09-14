using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Embedly.SDK.Models.Requests.Checkout;
using Embedly.SDK.Models.Responses.Checkout;
using Embedly.SDK.Models.Responses.Common;

namespace Embedly.SDK.Services.Checkout;

/// <summary>
///     Interface for checkout wallet operations based on actual checkout API.
/// </summary>
public interface ICheckoutService
{
    /// <summary>
    ///     Generates a new checkout wallet.
    /// </summary>
    /// <param name="request">The checkout wallet generation request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The generated checkout wallet.</returns>
    Task<ApiResponse<CheckoutWallet>> GenerateCheckoutWalletAsync(GenerateCheckoutWalletRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Gets organization checkout wallets with pagination and filtering.
    /// </summary>
    /// <param name="request">The get checkout wallets request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of organization checkout wallets.</returns>
    Task<ApiResponse<List<CheckoutWallet>>> GetCheckoutWalletsAsync(GetCheckoutWalletsRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Retrieves wallet with transactions.
    /// </summary>
    /// <param name="walletId">The wallet ID.</param>
    /// <param name="organizationId">The organization ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The checkout wallet with transactions.</returns>
    Task<ApiResponse<CheckoutWallet>> GetCheckoutWalletWithTransactionsAsync(Guid walletId, Guid organizationId,
        CancellationToken cancellationToken = default);
}