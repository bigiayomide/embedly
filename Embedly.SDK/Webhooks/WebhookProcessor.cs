using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Embedly.SDK.Helpers;

namespace Embedly.SDK.Webhooks;

/// <summary>
/// Processes webhook requests from Embedly.
/// </summary>
public sealed class WebhookProcessor : IWebhookProcessor
{
    private readonly IWebhookValidator _validator;
    private readonly IWebhookHandler _handler;
    private readonly ILogger<WebhookProcessor>? _logger;
    
    /// <summary>
    /// Initializes a new instance of the WebhookProcessor class.
    /// </summary>
    /// <param name="validator">The webhook validator.</param>
    /// <param name="handler">The webhook handler.</param>
    /// <param name="logger">Optional logger instance.</param>
    public WebhookProcessor(
        IWebhookValidator validator,
        IWebhookHandler handler,
        ILogger<WebhookProcessor>? logger = null)
    {
        _validator = Guard.ThrowIfNull(validator);
        _handler = Guard.ThrowIfNull(handler);
        _logger = logger;
    }
    
    /// <inheritdoc />
    public async Task<WebhookProcessResult> ProcessWebhookAsync(
        string payload,
        string signature,
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger?.LogDebug("Processing webhook with signature: {Signature}", signature);
            
            // Validate and parse the webhook
            var webhookEvent = _validator.ParseEvent(payload, signature);
            
            if (webhookEvent == null)
            {
                _logger?.LogWarning("Failed to parse webhook event");
                return new WebhookProcessResult
                {
                    Success = false,
                    Error = "Failed to parse webhook event"
                };
            }
            
            // Handle the event
            await _handler.HandleEventAsync(webhookEvent, cancellationToken);
            
            _logger?.LogInformation("Successfully processed webhook event: {EventId}", webhookEvent.Id);

            return new WebhookProcessResult
            {
                Success = true,
                EventId = webhookEvent.Id,
                EventType = webhookEvent.Event
            };
        }
        catch (InvalidOperationException ex)
        {
            _logger?.LogError(ex, "Invalid webhook signature or format");
            return new WebhookProcessResult
            {
                Success = false,
                Error = ex.Message
            };
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Unexpected error processing webhook");
            return new WebhookProcessResult
            {
                Success = false,
                Error = "An unexpected error occurred while processing the webhook"
            };
        }
    }
    
    /// <inheritdoc />
    public bool ValidateWebhook(string payload, string signature)
    {
        try
        {
            return _validator.ValidateSignature(payload, signature);
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Error validating webhook signature");
            return false;
        }
    }
}

/// <summary>
/// Interface for webhook processing.
/// </summary>
public interface IWebhookProcessor
{
    /// <summary>
    /// Processes a webhook request.
    /// </summary>
    /// <param name="payload">The webhook payload.</param>
    /// <param name="signature">The webhook signature.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The processing result.</returns>
    Task<WebhookProcessResult> ProcessWebhookAsync(
        string payload,
        string signature,
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Validates a webhook signature without processing.
    /// </summary>
    /// <param name="payload">The webhook payload.</param>
    /// <param name="signature">The webhook signature.</param>
    /// <returns>True if valid, false otherwise.</returns>
    bool ValidateWebhook(string payload, string signature);
}

/// <summary>
/// Result of webhook processing.
/// </summary>
public sealed class WebhookProcessResult
{
    /// <summary>
    /// Gets or sets whether the webhook was processed successfully.
    /// </summary>
    public bool Success { get; set; }
    
    /// <summary>
    /// Gets or sets the event ID if processing was successful.
    /// </summary>
    public string? EventId { get; set; }
    
    /// <summary>
    /// Gets or sets the event type if processing was successful.
    /// </summary>
    public string? EventType { get; set; }
    
    /// <summary>
    /// Gets or sets the error message if processing failed.
    /// </summary>
    public string? Error { get; set; }
}