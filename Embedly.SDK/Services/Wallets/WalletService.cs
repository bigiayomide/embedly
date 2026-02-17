using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Embedly.SDK.Configuration;
using Embedly.SDK.Helpers;
using Embedly.SDK.Http;
using Embedly.SDK.Models.Requests.Wallets;
using Embedly.SDK.Models.Responses.Common;
using Embedly.SDK.Models.Responses.Wallets;
using Microsoft.Extensions.Options;

namespace Embedly.SDK.Services.Wallets;

/// <summary>
///     Implementation of wallet management operations based on actual Base API endpoints.
/// </summary>
internal sealed class WalletService : BaseService, IWalletService
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="WalletService" /> class.
    /// </summary>
    /// <param name="httpClient">The HTTP client.</param>
    /// <param name="options">The configuration options.</param>
    public WalletService(IEmbedlyHttpClient httpClient, IOptions<EmbedlyOptions> options)
        : base(httpClient, options)
    {
    }

    // ===== WALLET CREATION =====

    /// <inheritdoc />
    public async Task<ApiResponse<CreateWalletResponse>> CreateWalletAsync(CreateWalletRequest request,
        CancellationToken cancellationToken = default)
    {
        Guard.ThrowIfNull(request, nameof(request));

        var url = BuildUrl(ServiceUrls.Base, "api/v1/wallets/add");
        return await HttpClient.PostAsync<CreateWalletRequest, CreateWalletResponse>(url, request, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<Wallet>> CreateCorporateWalletAsync(string customerId,
        CreateCorporateWalletRequest request, CancellationToken cancellationToken = default)
    {
        Guard.ThrowIfNullOrWhiteSpace(customerId, nameof(customerId));
        Guard.ThrowIfNull(request, nameof(request));

        var url = BuildUrl(ServiceUrls.Base, $"api/v1/corporate/customers/{customerId}/wallets");
        return await HttpClient.PostAsync<CreateCorporateWalletRequest, Wallet>(url, request, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<object>> AddOrganizationWalletAsync(AddOrganizationWalletRequest request,
        CancellationToken cancellationToken = default)
    {
        Guard.ThrowIfNull(request, nameof(request));

        var url = BuildUrl(ServiceUrls.Base, "api/v1/organizations/add/wallet");
        return await HttpClient.PostAsync<AddOrganizationWalletRequest, object>(url, request, cancellationToken);
    }

    // ===== WALLET RETRIEVAL =====

    /// <inheritdoc />
    public async Task<ApiResponse<Wallet>> GetWalletAsync(string walletId,
        CancellationToken cancellationToken = default)
    {
        Guard.ThrowIfNullOrWhiteSpace(walletId, nameof(walletId));

        var url = BuildUrl(ServiceUrls.Base, $"api/v1/wallets/get/wallet/{walletId}");
        return await HttpClient.GetAsync<Wallet>(url, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<Wallet>> GetWalletByAccountNumberAsync(string accountNumber,
        CancellationToken cancellationToken = default)
    {
        Guard.ThrowIfNullOrWhiteSpace(accountNumber, nameof(accountNumber));

        var url = BuildUrl(ServiceUrls.Base, $"api/v1/wallets/get/wallet/account/{accountNumber}");
        return await HttpClient.GetAsync<Wallet>(url, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<List<Wallet>>> GetCustomerWalletsAsync(string customerId,
        CancellationToken cancellationToken = default)
    {
        Guard.ThrowIfNullOrWhiteSpace(customerId, nameof(customerId));

        var url = BuildUrl(ServiceUrls.Base, $"api/v1/wallets/get/wallet/customer/{customerId}");
        return await HttpClient.GetAsync<List<Wallet>>(url, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<Wallet>> GetCustomerWalletAsync(string customerId, string walletId,
        CancellationToken cancellationToken = default)
    {
        Guard.ThrowIfNullOrWhiteSpace(customerId, nameof(customerId));
        Guard.ThrowIfNullOrWhiteSpace(walletId, nameof(walletId));

        var url = BuildUrl(ServiceUrls.Base, $"api/v1/wallets/get/customer/{customerId}/wallet/{walletId}");
        return await HttpClient.GetAsync<Wallet>(url, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<List<Wallet>>> GetOrganizationWalletsAsync(
        CancellationToken cancellationToken = default)
    {
        var url = BuildUrl(ServiceUrls.Base, "api/v1/wallets/get/organization-wallets");
        return await HttpClient.GetAsync<List<Wallet>>(url, cancellationToken);
    }

    // ===== WALLET SEARCH =====

    /// <inheritdoc />
    public async Task<ApiResponse<List<Wallet>>> SearchWalletsByEmailAsync(SearchWalletByEmailRequest request,
        CancellationToken cancellationToken = default)
    {
        Guard.ThrowIfNull(request, nameof(request));

        var url = BuildUrl(ServiceUrls.Base, "api/v1/wallets/search/email");
        return await HttpClient.PostAsync<SearchWalletByEmailRequest, List<Wallet>>(url, request, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<List<Wallet>>> SearchWalletsByMobileAsync(SearchWalletByMobileRequest request,
        CancellationToken cancellationToken = default)
    {
        Guard.ThrowIfNull(request, nameof(request));

        var url = BuildUrl(ServiceUrls.Base, "api/v1/wallets/search/mobile");
        return await HttpClient.PostAsync<SearchWalletByMobileRequest, List<Wallet>>(url, request, cancellationToken);
    }

    // ===== WALLET HISTORY & TRANSACTIONS =====

    /// <inheritdoc />
    public async Task<ApiResponse<WalletHistoryResponse>> GetWalletHistoryAsync(GetWalletHistoryRequest request,
        CancellationToken cancellationToken = default)
    {
        Guard.ThrowIfNull(request, nameof(request));

        var url = BuildUrl(ServiceUrls.Base, "api/v1/wallets/history");
        return await HttpClient.GetAsync<WalletHistoryResponse>(url, request.ToQueryParameters(), cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<WalletHistoryResponse>> GetWalletHistoryByAccountNumberAsync(
        GetWalletHistoryByAccountRequest request, CancellationToken cancellationToken = default)
    {
        Guard.ThrowIfNull(request, nameof(request));

        var url = BuildUrl(ServiceUrls.Base, "api/v1/wallets/account-number/history");
        return await HttpClient.GetAsync<WalletHistoryResponse>(url, request.ToQueryParameters(), cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<List<WalletTransaction>>> GetOrganizationTransactionHistoryAsync(Guid customerId,
        CancellationToken cancellationToken = default)
    {
        var queryParams = new Dictionary<string, object?> { { "CustomerId", customerId } };
        var url = BuildUrl(ServiceUrls.Base, "api/v1/wallets/history/organization/transactions");
        return await HttpClient.GetAsync<List<WalletTransaction>>(url, queryParams, cancellationToken);
    }

    // ===== WALLET TRANSACTIONS =====

    /// <inheritdoc />
    public async Task<ApiResponse<object>> PostWalletTransactionAsync(PendingTransactionRequest request,
        CancellationToken cancellationToken = default)
    {
        Guard.ThrowIfNull(request, nameof(request));

        var url = BuildUrl(ServiceUrls.Base, "api/v1/operations/wallet/transaction/operations/post");
        return await HttpClient.PutAsync<PendingTransactionRequest, object>(url, request, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<object>> ReverseTransactionAsync(ReverseTransactionRequest request,
        CancellationToken cancellationToken = default)
    {
        Guard.ThrowIfNull(request, nameof(request));

        var url = BuildUrl(ServiceUrls.Base, "api/v1/operations/wallet/transaction/reverse");
        return await HttpClient.PutAsync<ReverseTransactionRequest, object>(url, request, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<object>> PostTransactionByAccountNumberAsync(PendingTransactionRequest request,
        CancellationToken cancellationToken = default)
    {
        Guard.ThrowIfNull(request, nameof(request));

        var url = BuildUrl(ServiceUrls.Base, "api/v1/operations/wallet/transaction/account-number/post");
        return await HttpClient.PutAsync<PendingTransactionRequest, object>(url, request, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<object>> CompleteWalletTransactionAsync(CompleteTransactionRequest request,
        CancellationToken cancellationToken = default)
    {
        Guard.ThrowIfNull(request, nameof(request));

        var url = BuildUrl(ServiceUrls.Base, "api/v1/wallets/wallet/transaction/post/complete");
        return await HttpClient.PutAsync<CompleteTransactionRequest, object>(url, request, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<WalletTransferResult>> WalletToWalletTransferAsync(
        WalletToWalletTransferRequest request, CancellationToken cancellationToken = default)
    {
        Guard.ThrowIfNull(request, nameof(request));

        var url = BuildUrl(ServiceUrls.Base, "api/v1/wallets/wallet/transaction/v2/wallet-to-wallet");
        return await HttpClient.PostAsync<WalletToWalletTransferRequest, WalletTransferResult>(url, request,
            cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<WalletTransferStatus>> GetWalletTransferStatusAsync(
        GetWalletTransferStatusRequest request, CancellationToken cancellationToken = default)
    {
        Guard.ThrowIfNull(request, nameof(request));

        var url = BuildUrl(ServiceUrls.Base, "api/v1/wallets/wallet/transaction/wallet-to-wallet/status");
        return await HttpClient.PostAsync<GetWalletTransferStatusRequest, WalletTransferStatus>(url, request,
            cancellationToken);
    }

    // ===== WALLET RESTRICTIONS =====

    /// <inheritdoc />
    public async Task<ApiResponse<List<WalletRestrictionType>>> GetWalletRestrictionTypesAsync(
        CancellationToken cancellationToken = default)
    {
        var url = BuildUrl(ServiceUrls.Base, "api/v1/wallets/get/restriction/types");
        return await HttpClient.GetAsync<List<WalletRestrictionType>>(url, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<object>> RestrictWalletAsync(RestrictWalletRequest request,
        CancellationToken cancellationToken = default)
    {
        Guard.ThrowIfNull(request, nameof(request));

        var url = BuildUrl(ServiceUrls.Base, "api/v1/wallets/wallet/restrict");
        return await HttpClient.PatchAsync<RestrictWalletRequest, object>(url, request, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<object>> RestrictWalletByAccountNumberAsync(string accountNumber,
        RestrictWalletRequest request, CancellationToken cancellationToken = default)
    {
        Guard.ThrowIfNullOrWhiteSpace(accountNumber, nameof(accountNumber));
        Guard.ThrowIfNull(request, nameof(request));

        var url = BuildUrl(ServiceUrls.Base, $"api/v1/wallets/wallet/restrict/account/{accountNumber}");
        return await HttpClient.PatchAsync<RestrictWalletRequest, object>(url, request, cancellationToken);
    }

    // ===== WALLET TYPES & METADATA =====

    /// <inheritdoc />
    public async Task<ApiResponse<List<WalletType>>> GetWalletTypesAsync(CancellationToken cancellationToken = default)
    {
        var url = BuildUrl(ServiceUrls.Base, "api/v1/wallets/wallet/types/get");
        return await HttpClient.GetAsync<List<WalletType>>(url, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<List<VirtualAccountType>>> GetVirtualAccountTypesAsync(
        CancellationToken cancellationToken = default)
    {
        var url = BuildUrl(ServiceUrls.Base, "api/v1/wallets/wallet/virtual/account/types/get");
        return await HttpClient.GetAsync<List<VirtualAccountType>>(url, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<WalletInterest>> GetMonthlyWalletInterestAsync(string walletId,
        CancellationToken cancellationToken = default)
    {
        Guard.ThrowIfNullOrWhiteSpace(walletId, nameof(walletId));

        var url = BuildUrl(ServiceUrls.Base, $"api/v1/interests/get/month/wallet/{walletId}");
        return await HttpClient.GetAsync<WalletInterest>(url, cancellationToken);
    }

    // ===== STAGING/TESTING OPERATIONS =====

    /// <inheritdoc />
    public async Task<ApiResponse<object>> SimulateInflowAsync(SimulateInflowRequest request,
        CancellationToken cancellationToken = default)
    {
        Guard.ThrowIfNull(request, nameof(request));

        var url = BuildUrl(ServiceUrls.Base, "api/v1/wallets/simulate/inflow");
        return await HttpClient.PostAsync<SimulateInflowRequest, object>(url, request, cancellationToken);
    }
}