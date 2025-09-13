using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Embedly.SDK.Webhooks;

/// <summary>
/// Base webhook handler for processing Embedly webhook events.
/// </summary>
public abstract class WebhookHandler : IWebhookHandler
{
    private readonly ILogger<WebhookHandler>? _logger;
    private readonly Dictionary<string, Func<WebhookEvent, CancellationToken, Task>> _handlers;
    
    /// <summary>
    /// Initializes a new instance of the WebhookHandler class.
    /// </summary>
    /// <param name="logger">Optional logger instance.</param>
    protected WebhookHandler(ILogger<WebhookHandler>? logger = null)
    {
        _logger = logger;
        _handlers = new Dictionary<string, Func<WebhookEvent, CancellationToken, Task>>();
        RegisterHandlers();
    }
    
    /// <summary>
    /// Registers event handlers. Override this method to register custom handlers.
    /// </summary>
    protected virtual void RegisterHandlers()
    {
        // Default handlers can be registered here
    }
    
    /// <summary>
    /// Registers a handler for a specific event type.
    /// </summary>
    /// <param name="eventType">The event type to handle.</param>
    /// <param name="handler">The handler function.</param>
    protected void RegisterHandler(string eventType, Func<WebhookEvent, CancellationToken, Task> handler)
    {
        _handlers[eventType] = handler;
    }
    
    /// <inheritdoc />
    public virtual async Task HandleEventAsync(WebhookEvent webhookEvent, CancellationToken cancellationToken = default)
    {
        if (webhookEvent == null)
        {
            _logger?.LogWarning("Received null webhook event");
            return;
        }
        
        _logger?.LogInformation("Processing webhook event: {EventType} with ID: {EventId}", 
            webhookEvent.Event, webhookEvent.Id);
        
        try
        {
            if (_handlers.TryGetValue(webhookEvent.Event, out var handler))
            {
                await handler(webhookEvent, cancellationToken);
                _logger?.LogInformation("Successfully processed webhook event: {EventType}", webhookEvent.Event);
            }
            else
            {
                await HandleUnknownEventAsync(webhookEvent, cancellationToken);
            }
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Error processing webhook event: {EventType}", webhookEvent.Event);
            throw;
        }
    }
    
    /// <summary>
    /// Handles unknown event types. Override to provide custom behavior.
    /// </summary>
    /// <param name="webhookEvent">The webhook event.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    protected virtual Task HandleUnknownEventAsync(WebhookEvent webhookEvent, CancellationToken cancellationToken)
    {
        _logger?.LogWarning("Received unknown webhook event type: {EventType}", webhookEvent.Event);
        return Task.CompletedTask;
    }
}

/// <summary>
/// Interface for webhook event handlers.
/// </summary>
public interface IWebhookHandler
{
    /// <summary>
    /// Handles a webhook event.
    /// </summary>
    /// <param name="webhookEvent">The webhook event to handle.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task HandleEventAsync(WebhookEvent webhookEvent, CancellationToken cancellationToken = default);
}