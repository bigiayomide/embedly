using System.Threading;
using System.Threading.Tasks;
using Embedly.SDK.Models.Requests.Cards;
using Embedly.SDK.Models.Responses.Cards;
using Embedly.SDK.Models.Responses.Common;

namespace Embedly.SDK.Services.Cards;

/// <summary>
///     Interface for card management operations.
/// </summary>
public interface ICardService
{
    /// <summary>
    ///     Issues an Afrigo card to a customer.
    /// </summary>
    /// <param name="request">The card issuance request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The issued card details.</returns>
    Task<ApiResponse<AfrigoCard>> IssueAfrigoCardAsync(IssueAfrigoCardRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Activates an issued Afrigo card.
    /// </summary>
    /// <param name="request">The card activation request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The activated card details.</returns>
    Task<ApiResponse<AfrigoCard>> ActivateAfrigoCardAsync(ActivateAfrigoCardRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Updates information for an Afrigo card.
    /// </summary>
    /// <param name="request">The card update request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The updated card details.</returns>
    Task<ApiResponse<AfrigoCard>> UpdateAfrigoCardAsync(UpdateAfrigoCardRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Resets the PIN for an Afrigo card.
    /// </summary>
    /// <param name="request">The PIN reset request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The operation result.</returns>
    Task<ApiResponse<CardPinOperationResult>> ResetCardPinAsync(ResetCardPinRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Changes the PIN for an Afrigo card.
    /// </summary>
    /// <param name="request">The PIN change request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The operation result.</returns>
    Task<ApiResponse<CardPinOperationResult>> ChangeCardPinAsync(ChangeCardPinRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Verifies the PIN for an Afrigo card.
    /// </summary>
    /// <param name="request">The PIN verification request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The verification result.</returns>
    Task<ApiResponse<CardPinOperationResult>> CheckCardPinAsync(CheckCardPinRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Blocks an Afrigo card.
    /// </summary>
    /// <param name="request">The card blocking request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The operation result.</returns>
    Task<ApiResponse<AfrigoCard>> BlockCardAsync(BlockCardRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Unblocks an Afrigo card.
    /// </summary>
    /// <param name="request">The card unblocking request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The operation result.</returns>
    Task<ApiResponse<AfrigoCard>> UnblockCardAsync(UnblockCardRequest request,
        CancellationToken cancellationToken = default);
}