using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Embedly.SDK.Models.Requests.WalletGroups;
using Embedly.SDK.Models.Responses.Common;
using Embedly.SDK.Models.Responses.WalletGroups;

namespace Embedly.SDK.Services.WalletGroups;

/// <summary>
///     Interface for wallet group management operations based on actual WaasCore API.
/// </summary>
public interface IWalletGroupService
{
    /// <summary>
    ///     Creates a new wallet group.
    ///     POST /api/v1/walletgroups/add
    /// </summary>
    /// <param name="request">The wallet group creation request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The created wallet group.</returns>
    Task<ApiResponse<WalletGroup>> CreateWalletGroupAsync(CreateWalletGroupRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Gets a wallet group by its ID.
    ///     GET /api/v1/walletgroups/get/{id}
    /// </summary>
    /// <param name="id">The wallet group ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The wallet group details.</returns>
    Task<ApiResponse<WalletGroup>> GetWalletGroupAsync(string id, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Gets all wallet groups.
    ///     GET /api/v1/walletgroups/get
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of wallet groups.</returns>
    Task<ApiResponse<List<WalletGroup>>> GetWalletGroupsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    ///     Adds a feature to a wallet group.
    ///     POST /api/v1/walletgroups/feature/add
    /// </summary>
    /// <param name="request">The add feature request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Success response.</returns>
    Task<ApiResponse<object>> AddWalletGroupFeatureAsync(AddWalletGroupFeatureRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Adds a wallet to a wallet group.
    ///     POST /api/v1/walletgroups/{id}/wallet/{walletId}/add
    /// </summary>
    /// <param name="id">The wallet group ID.</param>
    /// <param name="walletId">The wallet ID to add.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Success response.</returns>
    Task<ApiResponse<object>> AddWalletToGroupAsync(string id, string walletId,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Gets available wallet group features.
    ///     GET /api/v1/walletgroups/walletfeatures/get
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of available features.</returns>
    Task<ApiResponse<List<WalletGroupFeature>>> GetWalletGroupFeaturesAsync(
        CancellationToken cancellationToken = default);
}