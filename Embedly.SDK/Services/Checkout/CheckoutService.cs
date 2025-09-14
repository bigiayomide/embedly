using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Embedly.SDK.Configuration;
using Embedly.SDK.Helpers;
using Embedly.SDK.Http;
using Embedly.SDK.Models.Requests.Checkout;
using Embedly.SDK.Models.Responses.Checkout;
using Embedly.SDK.Models.Responses.Common;

namespace Embedly.SDK.Services.Checkout;

/// <summary>
/// Service for checkout wallet operations based on actual checkout API.
/// </summary>
internal sealed class CheckoutService : BaseService, ICheckoutService
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CheckoutService"/> class.
    /// </summary>
    /// <param name="httpClient">The HTTP client.</param>
    /// <param name="options">The configuration options.</param>
    public CheckoutService(IEmbedlyHttpClient httpClient, IOptions<EmbedlyOptions> options)
        : base(httpClient, options)
    {
    }

    /// <inheritdoc />
    public async Task<ApiResponse<CheckoutWallet>> GenerateCheckoutWalletAsync(GenerateCheckoutWalletRequest request, CancellationToken cancellationToken = default)
    {
        Guard.ThrowIfNull(request, nameof(request));
        
        var url = BuildUrl(ServiceUrls.Checkout, "api/v1/checkout-wallet");
        return await HttpClient.PostAsync<GenerateCheckoutWalletRequest, CheckoutWallet>(url, request, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<List<CheckoutWallet>>> GetCheckoutWalletsAsync(GetCheckoutWalletsRequest request, CancellationToken cancellationToken = default)
    {
        Guard.ThrowIfNull(request, nameof(request));
        
        var url = BuildUrl(ServiceUrls.Checkout, "api/v1/checkout-wallet");
        var queryParams = request.ToQueryParameters();
        return await HttpClient.GetAsync<List<CheckoutWallet>>(url, queryParams, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<CheckoutWallet>> GetCheckoutWalletWithTransactionsAsync(Guid walletId, Guid organizationId, CancellationToken cancellationToken = default)
    {
        var queryParams = new Dictionary<string, object?>
        {
            ["organizationId"] = organizationId
        };
        
        var url = BuildUrl(ServiceUrls.Checkout, $"api/v1/checkout-wallet/{walletId}/transactions");
        return await HttpClient.GetAsync<CheckoutWallet>(url, queryParams, cancellationToken);
    }
}