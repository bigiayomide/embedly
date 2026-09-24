using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Embedly.SDK.Configuration;
using Embedly.SDK.Helpers;
using Embedly.SDK.Http;
using Embedly.SDK.Models.Requests.Checkout;
using Embedly.SDK.Models.Responses.Checkout;
using Embedly.SDK.Models.Responses.Common;
using Microsoft.Extensions.Options;

namespace Embedly.SDK.Services.Checkout;

/// <summary>
///     Service for checkout wallet operations based on actual checkout API.
/// </summary>
internal sealed class CheckoutService : BaseService, ICheckoutService
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="CheckoutService" /> class.
    /// </summary>
    /// <param name="httpClient">The HTTP client.</param>
    /// <param name="options">The configuration options.</param>
    public CheckoutService(IEmbedlyHttpClient httpClient, IOptions<EmbedlyOptions> options)
        : base(httpClient, options)
    {
    }

    /// <inheritdoc />
    public async Task<ApiResponse<CheckoutWallet>> GenerateCheckoutWalletAsync(GenerateCheckoutWalletRequest request,
        CancellationToken cancellationToken = default)
    {
        Guard.ThrowIfNull(request, nameof(request));
        Guard.ThrowIfEmpty(request.OrganizationId, "organizationId");

        var url = BuildUrl(ServiceUrls.Checkout, "api/v1/checkout-wallet");
        return await HttpClient.PostAsync<GenerateCheckoutWalletRequest, CheckoutWallet>(url, request,
            cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<List<CheckoutWallet>>> GetCheckoutWalletsAsync(GetCheckoutWalletsRequest request,
        CancellationToken cancellationToken = default)
    {
        Guard.ThrowIfNull(request, nameof(request));
        Guard.ThrowIfEmpty(request.OrganizationId, "organizationId");

        var url = BuildUrl(ServiceUrls.Checkout, "api/v1/checkout-wallet");
        var queryParams = request.ToQueryParameters();
        return await HttpClient.GetAsync<List<CheckoutWallet>>(url, queryParams, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<CheckoutWallet>> GetCheckoutWalletWithTransactionsAsync(Guid walletId,
        Guid organizationId, CancellationToken cancellationToken = default)
    {
        Guard.ThrowIfEmpty(organizationId, nameof(organizationId));

        var queryParams = new Dictionary<string, object?>
        {
            ["organizationId"] = organizationId
        };

        var url = BuildUrl(ServiceUrls.Checkout, $"api/v1/checkout-wallet/{walletId}/transactions");
        return await HttpClient.GetAsync<CheckoutWallet>(url, queryParams, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<List<OrganizationPrefixMapping>>> GetOrganizationPrefixMappingsAsync(
        Guid organizationId, int page = 1, int pageSize = 10, string? search = null,
        CancellationToken cancellationToken = default)
    {
        var request = new GetOrganizationPrefixMappingsRequest
        {
            OrganizationId = organizationId,
            Page = page,
            PageSize = pageSize,
            Search = search
        };

        return await GetOrganizationPrefixMappingsAsync(request, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<List<OrganizationPrefixMapping>>> GetOrganizationPrefixMappingsAsync(
        GetOrganizationPrefixMappingsRequest request, CancellationToken cancellationToken = default)
    {
        Guard.ThrowIfNull(request, nameof(request));
        Guard.ThrowIfEmpty(request.OrganizationId, "organizationId");

        var url = BuildUrl(ServiceUrls.Checkout, "api/v1/prefix-map/me");
        var queryParams = request.ToQueryParameters();
        return await HttpClient.GetAsync<List<OrganizationPrefixMapping>>(url, queryParams, cancellationToken);
    }

    // ===== SPLIT BENEFICIARIES =====

    /// <inheritdoc />
    public async Task<ApiResponse<SplitBeneficiary>> CreateSplitBeneficiaryAsync(
        CreateSplitBeneficiaryRequest request, CancellationToken cancellationToken = default)
    {
        Guard.ThrowIfNull(request, nameof(request));
        Guard.ThrowIfEmpty(request.OrganizationId, "organizationId");
        Guard.ThrowIfNullOrWhiteSpace(request.BeneficiaryName, "beneficiaryName");
        Guard.ThrowIfNullOrWhiteSpace(request.AccountNumber, "accountNumber");

        var url = BuildUrl(ServiceUrls.Checkout, "api/v1/split-beneficiaries");
        return await HttpClient.PostAsync<CreateSplitBeneficiaryRequest, SplitBeneficiary>(url, request,
            cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<SplitBeneficiaryListResponse>> GetSplitBeneficiariesAsync(
        GetSplitBeneficiariesRequest request, CancellationToken cancellationToken = default)
    {
        Guard.ThrowIfNull(request, nameof(request));
        Guard.ThrowIfEmpty(request.OrganizationId, "organizationId");

        var url = BuildUrl(ServiceUrls.Checkout, "api/v1/split-beneficiaries");
        var queryParams = request.ToQueryParameters();
        return await HttpClient.GetAsync<SplitBeneficiaryListResponse>(url, queryParams, cancellationToken);
    }

    /// <inheritdoc />
    public Task<ApiResponse<object>> ActivateSplitBeneficiaryAsync(SplitBeneficiaryStatusRequest request,
        CancellationToken cancellationToken = default)
    {
        return SetSplitBeneficiaryStatusAsync(request, "activate", cancellationToken);
    }

    /// <inheritdoc />
    public Task<ApiResponse<object>> DeactivateSplitBeneficiaryAsync(SplitBeneficiaryStatusRequest request,
        CancellationToken cancellationToken = default)
    {
        return SetSplitBeneficiaryStatusAsync(request, "deactivate", cancellationToken);
    }

    private async Task<ApiResponse<object>> SetSplitBeneficiaryStatusAsync(SplitBeneficiaryStatusRequest request,
        string action, CancellationToken cancellationToken)
    {
        Guard.ThrowIfNull(request, nameof(request));
        Guard.ThrowIfEmpty(request.BeneficiaryId, "beneficiaryId");
        Guard.ThrowIfEmpty(request.OrganizationId, "organizationId");

        var url = BuildUrl(ServiceUrls.Checkout, $"api/v1/split-beneficiaries/{request.BeneficiaryId}/{action}");
        return await HttpClient.PatchAsync<SplitBeneficiaryStatusRequest, object>(url, request, cancellationToken);
    }
}