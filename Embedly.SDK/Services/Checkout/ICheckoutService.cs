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
    ///     GET /api/v1/checkout-wallet
    /// </summary>
    /// <param name="request">The get checkout wallets request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of organization checkout wallets.</returns>
    Task<ApiResponse<List<CheckoutWallet>>> GetCheckoutWalletsAsync(GetCheckoutWalletsRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Retrieves a checkout wallet with its checkout history and received transactions.
    ///     GET /api/v1/checkout-wallet/{walletId}/transactions
    /// </summary>
    /// <param name="walletId">The wallet ID.</param>
    /// <param name="organizationId">The organization ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The checkout wallet with transactions.</returns>
    Task<ApiResponse<CheckoutWallet>> GetCheckoutWalletWithTransactionsAsync(Guid walletId, Guid organizationId,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Gets organization prefix mappings. These mappings are required to create checkout wallets.
    ///     GET /api/v1/prefix-map/me
    /// </summary>
    /// <param name="organizationId">The organization ID.</param>
    /// <param name="page">The page number (1-based).</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <param name="search">Optional search filter.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of organization prefix mappings.</returns>
    Task<ApiResponse<List<OrganizationPrefixMapping>>> GetOrganizationPrefixMappingsAsync(Guid organizationId,
        int page = 1, int pageSize = 10, string? search = null, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Gets organization prefix mappings using a request object.
    ///     GET /api/v1/prefix-map/me
    /// </summary>
    /// <param name="request">The get organization prefix mappings request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of organization prefix mappings.</returns>
    Task<ApiResponse<List<OrganizationPrefixMapping>>> GetOrganizationPrefixMappingsAsync(
        GetOrganizationPrefixMappingsRequest request, CancellationToken cancellationToken = default);

    // ===== SPLIT BENEFICIARIES =====

    /// <summary>
    ///     Creates a split payment beneficiary that checkout wallet payments can be split to.
    ///     POST /api/v1/split-beneficiaries
    /// </summary>
    /// <param name="request">The create split beneficiary request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The created split beneficiary.</returns>
    Task<ApiResponse<SplitBeneficiary>> CreateSplitBeneficiaryAsync(CreateSplitBeneficiaryRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Gets an organization's split beneficiaries with filtering and pagination.
    ///     GET /api/v1/split-beneficiaries
    /// </summary>
    /// <param name="request">The get split beneficiaries request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A page of split beneficiaries.</returns>
    Task<ApiResponse<SplitBeneficiaryListResponse>> GetSplitBeneficiariesAsync(GetSplitBeneficiariesRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Activates a split beneficiary.
    ///     PATCH /api/v1/split-beneficiaries/{beneficiaryId}/activate
    /// </summary>
    /// <param name="request">The split beneficiary to activate.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The API response; <c>Data</c> is always null.</returns>
    Task<ApiResponse<object>> ActivateSplitBeneficiaryAsync(SplitBeneficiaryStatusRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Deactivates a split beneficiary.
    ///     PATCH /api/v1/split-beneficiaries/{beneficiaryId}/deactivate
    /// </summary>
    /// <param name="request">The split beneficiary to deactivate.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The API response; <c>Data</c> is always null.</returns>
    Task<ApiResponse<object>> DeactivateSplitBeneficiaryAsync(SplitBeneficiaryStatusRequest request,
        CancellationToken cancellationToken = default);
}