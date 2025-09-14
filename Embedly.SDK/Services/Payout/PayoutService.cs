using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Embedly.SDK.Configuration;
using Embedly.SDK.Helpers;
using Embedly.SDK.Http;
using Embedly.SDK.Models.Requests.Payout;
using Embedly.SDK.Models.Responses.Common;
using Embedly.SDK.Models.Responses.Payout;
using Microsoft.Extensions.Options;

namespace Embedly.SDK.Services.Payout;

/// <summary>
///     Service for payout operations.
/// </summary>
internal sealed class PayoutService : BaseService, IPayoutService
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="PayoutService" /> class.
    /// </summary>
    /// <param name="httpClient">The HTTP client.</param>
    /// <param name="options">The configuration options.</param>
    public PayoutService(IEmbedlyHttpClient httpClient, IOptions<EmbedlyOptions> options)
        : base(httpClient, options)
    {
    }

    /// <inheritdoc />
    public async Task<ApiResponse<List<Bank>>> GetBanksAsync(string? search = null,
        CancellationToken cancellationToken = default)
    {
        var url = BuildUrl(ServiceUrls.Payout, "api/Payout/banks");
        if (!string.IsNullOrEmpty(search)) url = $"{url}?search={Uri.EscapeDataString(search)}";
        return await HttpClient.GetAsync<List<Bank>>(url, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<NameEnquiryResponse>> NameEnquiryAsync(NameEnquiryRequest request,
        CancellationToken cancellationToken = default)
    {
        Guard.ThrowIfNull(request, nameof(request));

        var url = BuildUrl(ServiceUrls.Payout, "api/Payout/name-enquiry");
        return await HttpClient.PostAsync<NameEnquiryRequest, NameEnquiryResponse>(url, request, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<PayoutTransaction>> InterBankTransferAsync(BankTransferRequest request,
        CancellationToken cancellationToken = default)
    {
        Guard.ThrowIfNull(request, nameof(request));

        var url = BuildUrl(ServiceUrls.Payout, "api/Payout/inter-bank-transfer");
        return await HttpClient.PostAsync<BankTransferRequest, PayoutTransaction>(url, request, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<PayoutTransaction>> GetTransactionStatusAsync(string transactionReference,
        CancellationToken cancellationToken = default)
    {
        Guard.ThrowIfNullOrWhiteSpace(transactionReference, nameof(transactionReference));

        var url = BuildUrl(ServiceUrls.Payout, $"api/Payout/status/{transactionReference}");
        return await HttpClient.GetAsync<PayoutTransaction>(url, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<OrganizationPayoutData>> GetOrganizationPayoutDataAsync(Guid organizationId,
        CancellationToken cancellationToken = default)
    {
        var url = BuildUrl(ServiceUrls.Payout, $"api/Payout/organization/{organizationId}");
        return await HttpClient.GetAsync<OrganizationPayoutData>(url, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<PayoutProfileResponse>> CreatePayoutProfileAsync(PayoutRequestDto request,
        CancellationToken cancellationToken = default)
    {
        Guard.ThrowIfNull(request, nameof(request));

        var url = BuildUrl(ServiceUrls.Payout, "api/Payout/profile/organization");
        return await HttpClient.PostAsync<PayoutRequestDto, PayoutProfileResponse>(url, request, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<List<PayoutTransaction>>> GetAllPayoutsAsync(
        CancellationToken cancellationToken = default)
    {
        var url = BuildUrl(ServiceUrls.Payout, "api/Payout");
        return await HttpClient.GetAsync<List<PayoutTransaction>>(url, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<WalletDetails>> GetWalletByAccountNumberAsync(string accountNumber,
        CancellationToken cancellationToken = default)
    {
        Guard.ThrowIfNullOrWhiteSpace(accountNumber, nameof(accountNumber));

        var url = BuildUrl(ServiceUrls.Payout, $"wallet/{Uri.EscapeDataString(accountNumber)}");
        return await HttpClient.GetAsync<WalletDetails>(url, cancellationToken);
    }

    // ===== PAYOUT LIMITS OPERATIONS (7 endpoints) =====

    /// <inheritdoc />
    public async Task<ApiResponse<GlobalPayoutLimit>> AddGlobalPayoutLimitAsync(AddGlobalPayoutLimitRequest request,
        CancellationToken cancellationToken = default)
    {
        Guard.ThrowIfNull(request, nameof(request));

        var url = BuildUrl(ServiceUrls.Payout, "api/operations/payout-limits/global-limit");
        return await HttpClient.PostAsync<AddGlobalPayoutLimitRequest, GlobalPayoutLimit>(url, request,
            cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<PaginatedResponse<GlobalPayoutLimit>>> GetGlobalPayoutLimitsAsync(
        GetPayoutLimitsRequest request, CancellationToken cancellationToken = default)
    {
        Guard.ThrowIfNull(request, nameof(request));

        var url = BuildUrl(ServiceUrls.Payout,
            $"api/operations/payout-limits/global-limit?pageNumber={request.PageNumber}&pageSize={request.PageSize}");
        return await HttpClient.GetAsync<PaginatedResponse<GlobalPayoutLimit>>(url, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<GlobalPayoutLimit>> UpdateGlobalPayoutLimitAsync(Guid globalLimitId,
        AddGlobalPayoutLimitRequest request, CancellationToken cancellationToken = default)
    {
        Guard.ThrowIfNull(request, nameof(request));

        var url = BuildUrl(ServiceUrls.Payout, $"api/operations/payout-limits/global-limit/{globalLimitId}");
        return await HttpClient.PutAsync<AddGlobalPayoutLimitRequest, GlobalPayoutLimit>(url, request,
            cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<GlobalPayoutLimit>> UpdateOrganizationCurrencyLimitAsync(Guid organizationId,
        Guid currencyId, AddGlobalPayoutLimitRequest request, CancellationToken cancellationToken = default)
    {
        Guard.ThrowIfNull(request, nameof(request));

        var url = BuildUrl(ServiceUrls.Payout,
            $"api/operations/payout-limits/global-limit/organization/{organizationId}/currency/{currencyId}");
        return await HttpClient.PutAsync<AddGlobalPayoutLimitRequest, GlobalPayoutLimit>(url, request,
            cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<GlobalPayoutLimit>> CreateDefaultGlobalPayoutLimitAsync(
        AddGlobalPayoutLimitRequest request, CancellationToken cancellationToken = default)
    {
        Guard.ThrowIfNull(request, nameof(request));

        var url = BuildUrl(ServiceUrls.Payout, "api/operations/payout-limits/global-limit/default");
        return await HttpClient.PostAsync<AddGlobalPayoutLimitRequest, GlobalPayoutLimit>(url, request,
            cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<GlobalPayoutLimit>> UpdateDefaultGlobalPayoutLimitAsync(Guid defaultLimitId,
        AddGlobalPayoutLimitRequest request, CancellationToken cancellationToken = default)
    {
        Guard.ThrowIfNull(request, nameof(request));

        var url = BuildUrl(ServiceUrls.Payout, $"api/operations/payout-limits/global-limit/default/{defaultLimitId}");
        return await HttpClient.PutAsync<AddGlobalPayoutLimitRequest, GlobalPayoutLimit>(url, request,
            cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<GlobalPayoutLimit>> EnableOrDisableGlobalPayoutLimitAsync(Guid limitId, bool enabled,
        CancellationToken cancellationToken = default)
    {
        var url = BuildUrl(ServiceUrls.Payout,
            $"api/operations/payout-limits/global-limit/enable-or-disable?enable={enabled}&limitId={limitId}");
        return await HttpClient.PutAsync<object, GlobalPayoutLimit>(url, new object(), cancellationToken);
    }
}