using FastEndpoints;
using Embedly.SDK.Webhooks;
using System.Text;

namespace Embedly.Examples.Endpoints;

/// <summary>
/// FastEndpoints implementation for handling Embedly webhooks.
/// Alternative to the traditional WebhookController approach.
/// </summary>
public class WebhookEndpoint(IWebhookProcessor webhookProcessor, ILogger<WebhookEndpoint> logger)
    : EndpointWithoutRequest
{
    public override void Configure()
    {
        Post("/api/webhooks/embedly");
        AllowAnonymous();
        Description(d => d
            .WithTags("Webhooks")
            .WithSummary("Handle Embedly webhook events")
            .WithDescription("Receives and processes webhook events from Embedly using the SDK's built-in webhook system")
            .ProducesProblem(400)
            .ProducesProblem(500)
            .Produces<WebhookResponse>(200, "application/json"));
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        try
        {
            var payload = await ReadPayloadAsync();
            if (string.IsNullOrEmpty(payload))
            {
                logger.LogWarning("Received empty webhook payload");
                await Send.ResponseAsync(new WebhookErrorResponse
                {
                    Status = "failed",
                    Error = "Empty payload",
                    ProcessedAt = DateTime.UtcNow
                }, 400, ct);
                return;
            }

            var signature = HttpContext.Request.Headers["X-Embedly-Signature"].FirstOrDefault();
            if (string.IsNullOrEmpty(signature))
            {
                logger.LogWarning("Missing webhook signature");
                await Send.ResponseAsync(new WebhookErrorResponse
                {
                    Status = "failed",
                    Error = "Missing signature",
                    ProcessedAt = DateTime.UtcNow
                }, 400, ct);
                return;
            }

            var result = await webhookProcessor.ProcessWebhookAsync(payload, signature);

            if (result.Success)
            {
                logger.LogInformation("Webhook processed successfully: {EventType} {EventId}",
                    result.EventType, result.EventId);

                await Send.ResponseAsync(new WebhookResponse
                {
                    Status = "success",
                    EventType = result.EventType,
                    EventId = result.EventId,
                    ProcessedAt = DateTime.UtcNow
                }, cancellation: ct);
            }
            else
            {
                logger.LogWarning("Webhook processing failed: {Error}", result.Error);
                await Send.ResponseAsync(new WebhookErrorResponse
                {
                    Status = "failed",
                    Error = result.Error,
                    ProcessedAt = DateTime.UtcNow
                }, 400, cancellation: ct);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error processing webhook");
            await Send.ResponseAsync(new WebhookErrorResponse
            {
                Status = "error",
                Error = "Internal server error",
                ProcessedAt = DateTime.UtcNow
            }, statusCode: 500, cancellation: ct);
        }
    }

    private async Task<string> ReadPayloadAsync()
    {
        using var reader = new StreamReader(HttpContext.Request.Body, Encoding.UTF8);
        return await reader.ReadToEndAsync();
    }
}

/// <summary>
/// Response model for successful webhook processing.
/// </summary>
public class WebhookResponse
{
    public string Status { get; set; } = string.Empty;
    public string? EventType { get; set; }
    public string? EventId { get; set; }
    public DateTime ProcessedAt { get; set; }
}

/// <summary>
/// Response model for webhook processing errors.
/// </summary>
public class WebhookErrorResponse
{
    public string Status { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
    public DateTime ProcessedAt { get; set; }
}