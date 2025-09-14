using System;
using System.Threading;
using System.Threading.Tasks;
using Embedly.SDK.Configuration;
using Embedly.SDK.Helpers;
using Embedly.SDK.Http;
using Embedly.SDK.Models.Requests.Cards;
using Embedly.SDK.Models.Responses.Cards;
using Embedly.SDK.Models.Responses.Common;
using Microsoft.Extensions.Options;

namespace Embedly.SDK.Services.Cards;

/// <summary>
///     Service for card management operations with automatic PIN encryption.
/// </summary>
internal sealed class CardService : BaseService, ICardService
{
    private readonly IPinEncryptionService _pinEncryptionService;

    /// <summary>
    ///     Initializes a new instance of the <see cref="CardService" /> class.
    /// </summary>
    /// <param name="httpClient">The HTTP client.</param>
    /// <param name="options">The configuration options.</param>
    /// <param name="pinEncryptionService">The PIN encryption service.</param>
    public CardService(IEmbedlyHttpClient httpClient, IOptions<EmbedlyOptions> options,
        IPinEncryptionService pinEncryptionService)
        : base(httpClient, options)
    {
        _pinEncryptionService = pinEncryptionService ?? throw new ArgumentNullException(nameof(pinEncryptionService));
    }

    /// <inheritdoc />
    public async Task<ApiResponse<AfrigoCard>> IssueAfrigoCardAsync(IssueAfrigoCardRequest request,
        CancellationToken cancellationToken = default)
    {
        Guard.ThrowIfNull(request, nameof(request));

        var url = BuildUrl(ServiceUrls.Cards, "api/v1/operations/cards/afrigo/issue-card");
        return await HttpClient.PostAsync<IssueAfrigoCardRequest, AfrigoCard>(url, request, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<AfrigoCard>> ActivateAfrigoCardAsync(ActivateAfrigoCardRequest request,
        CancellationToken cancellationToken = default)
    {
        Guard.ThrowIfNull(request, nameof(request));

        // Encrypt PIN if provided
        var processedRequest = request;
        if (!string.IsNullOrEmpty(request.Pin))
        {
            var encryptedPin = _pinEncryptionService.EncryptPin(request.Pin);
            processedRequest = request with { Pin = encryptedPin };
        }

        var url = BuildUrl(ServiceUrls.Cards, "api/v1/operations/cards/afrigo/activate");
        return await HttpClient.PostAsync<ActivateAfrigoCardRequest, AfrigoCard>(url, processedRequest,
            cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<AfrigoCard>> UpdateAfrigoCardAsync(UpdateAfrigoCardRequest request,
        CancellationToken cancellationToken = default)
    {
        Guard.ThrowIfNull(request, nameof(request));

        var url = BuildUrl(ServiceUrls.Cards, "api/v1/operations/cards/afrigo/update-card-info");
        return await HttpClient.PostAsync<UpdateAfrigoCardRequest, AfrigoCard>(url, request, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<CardPinOperationResult>> ResetCardPinAsync(ResetCardPinRequest request,
        CancellationToken cancellationToken = default)
    {
        Guard.ThrowIfNull(request, nameof(request));

        // Encrypt PIN if provided
        var processedRequest = request;
        if (!string.IsNullOrEmpty(request.Pin))
        {
            var encryptedPin = _pinEncryptionService.EncryptPin(request.Pin);
            processedRequest = request with { Pin = encryptedPin };
        }

        var url = BuildUrl(ServiceUrls.Cards, "api/v1/operations/cards/afrigo/reset-card-pin");
        return await HttpClient.PostAsync<ResetCardPinRequest, CardPinOperationResult>(url, processedRequest,
            cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<CardPinOperationResult>> ChangeCardPinAsync(ChangeCardPinRequest request,
        CancellationToken cancellationToken = default)
    {
        Guard.ThrowIfNull(request, nameof(request));

        // Encrypt both old and new PINs if provided
        var processedRequest = request;

        if (!string.IsNullOrEmpty(request.OldPin))
        {
            var encryptedOldPin = _pinEncryptionService.EncryptPin(request.OldPin);
            processedRequest = processedRequest with { OldPin = encryptedOldPin };
        }

        if (!string.IsNullOrEmpty(request.NewPin))
        {
            var encryptedNewPin = _pinEncryptionService.EncryptPin(request.NewPin);
            processedRequest = processedRequest with { NewPin = encryptedNewPin };
        }

        var url = BuildUrl(ServiceUrls.Cards, "api/v1/operations/cards/afrigo/change-card-pin");
        return await HttpClient.PostAsync<ChangeCardPinRequest, CardPinOperationResult>(url, processedRequest,
            cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<CardPinOperationResult>> CheckCardPinAsync(CheckCardPinRequest request,
        CancellationToken cancellationToken = default)
    {
        Guard.ThrowIfNull(request, nameof(request));

        // Encrypt PIN if provided
        var processedRequest = request;
        if (!string.IsNullOrEmpty(request.Pin))
        {
            var encryptedPin = _pinEncryptionService.EncryptPin(request.Pin);
            processedRequest = request with { Pin = encryptedPin };
        }

        var url = BuildUrl(ServiceUrls.Cards, "api/v1/operations/cards/afrigo/check-card-pin");
        return await HttpClient.PostAsync<CheckCardPinRequest, CardPinOperationResult>(url, processedRequest,
            cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<AfrigoCard>> BlockCardAsync(BlockCardRequest request,
        CancellationToken cancellationToken = default)
    {
        Guard.ThrowIfNull(request, nameof(request));

        var url = BuildUrl(ServiceUrls.Cards, "api/v1/operations/cards/afrigo/block-card");
        return await HttpClient.PostAsync<BlockCardRequest, AfrigoCard>(url, request, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<AfrigoCard>> UnblockCardAsync(UnblockCardRequest request,
        CancellationToken cancellationToken = default)
    {
        Guard.ThrowIfNull(request, nameof(request));

        var url = BuildUrl(ServiceUrls.Cards, "api/v1/operations/cards/afrigo/unblock-card");
        return await HttpClient.PostAsync<UnblockCardRequest, AfrigoCard>(url, request, cancellationToken);
    }
}