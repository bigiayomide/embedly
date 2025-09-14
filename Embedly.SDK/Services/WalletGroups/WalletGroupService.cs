using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Embedly.SDK.Configuration;
using Embedly.SDK.Helpers;
using Embedly.SDK.Http;
using Embedly.SDK.Models.Requests.WalletGroups;
using Embedly.SDK.Models.Responses.Common;
using Embedly.SDK.Models.Responses.WalletGroups;
using Microsoft.Extensions.Options;

namespace Embedly.SDK.Services.WalletGroups;

/// <summary>
///     Service for wallet group management operations based on actual Base API.
/// </summary>
internal sealed class WalletGroupService : BaseService, IWalletGroupService
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="WalletGroupService" /> class.
    /// </summary>
    /// <param name="httpClient">The HTTP client.</param>
    /// <param name="options">The configuration options.</param>
    public WalletGroupService(IEmbedlyHttpClient httpClient, IOptions<EmbedlyOptions> options)
        : base(httpClient, options)
    {
    }

    /// <inheritdoc />
    public async Task<ApiResponse<WalletGroup>> CreateWalletGroupAsync(CreateWalletGroupRequest request,
        CancellationToken cancellationToken = default)
    {
        Guard.ThrowIfNull(request, nameof(request));

        var url = BuildUrl(ServiceUrls.Base, "api/v1/walletgroups/add");
        return await HttpClient.PostAsync<CreateWalletGroupRequest, WalletGroup>(url, request, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<WalletGroup>> GetWalletGroupAsync(string id,
        CancellationToken cancellationToken = default)
    {
        Guard.ThrowIfNullOrWhiteSpace(id, nameof(id));

        var url = BuildUrl(ServiceUrls.Base, $"api/v1/walletgroups/get/{id}");
        return await HttpClient.GetAsync<WalletGroup>(url, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<List<WalletGroup>>> GetWalletGroupsAsync(
        CancellationToken cancellationToken = default)
    {
        var url = BuildUrl(ServiceUrls.Base, "api/v1/walletgroups/get");
        return await HttpClient.GetAsync<List<WalletGroup>>(url, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<object>> AddWalletGroupFeatureAsync(AddWalletGroupFeatureRequest request,
        CancellationToken cancellationToken = default)
    {
        Guard.ThrowIfNull(request, nameof(request));

        var url = BuildUrl(ServiceUrls.Base, "api/v1/walletgroups/feature/add");
        return await HttpClient.PostAsync<AddWalletGroupFeatureRequest, object>(url, request, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<object>> AddWalletToGroupAsync(string id, string walletId,
        CancellationToken cancellationToken = default)
    {
        Guard.ThrowIfNullOrWhiteSpace(id, nameof(id));
        Guard.ThrowIfNullOrWhiteSpace(walletId, nameof(walletId));

        var url = BuildUrl(ServiceUrls.Base, $"api/v1/walletgroups/{id}/wallet/{walletId}/add");
        return await HttpClient.PostAsync<object, object>(url, new { }, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<List<WalletGroupFeature>>> GetWalletGroupFeaturesAsync(
        CancellationToken cancellationToken = default)
    {
        var url = BuildUrl(ServiceUrls.Base, "api/v1/walletgroups/walletfeatures/get");
        return await HttpClient.GetAsync<List<WalletGroupFeature>>(url, cancellationToken);
    }
}