using System.Text;
using Embedly.SDK.Webhooks;
using Microsoft.AspNetCore.Mvc;

namespace Embedly.Examples.Controllers;

/// <summary>
///     Simple webhook controller using the Embedly SDK's built-in webhook system.
///     Developers just need to inject IWebhookProcessor and call ProcessWebhookAsync.
/// </summary>
[ApiController]
[Route("api/webhooks")]
[Produces("application/json")]
public class WebhookController(
    IWebhookProcessor webhookProcessor,
    ILogger<WebhookController> logger)
    : ControllerBase
{
    /// <summary>
    ///     Primary webhook endpoint for receiving Embedly events.
    ///     Uses the SDK's built-in WebhookProcessor for validation and processing.
    /// </summary>
    [HttpPost("embedly")]
    public async Task<IActionResult> HandleEmbedlyWebhook()
    {
        try
        {
            // Read the payload
            var payload = await ReadPayloadAsync();
            if (string.IsNullOrEmpty(payload))
            {
                logger.LogWarning("Received empty webhook payload");
                return BadRequest(new { error = "Empty payload" });
            }

            // Get the signature
            var signature = Request.Headers["X-Auth-Signature"].FirstOrDefault();
            if (string.IsNullOrEmpty(signature))
            {
                logger.LogWarning("Missing webhook signature");
                return BadRequest(new { error = "Missing signature" });
            }

            // Use the SDK's built-in processor - it handles validation and processing
            var result = await webhookProcessor.ProcessWebhookAsync(payload, signature);

            if (result.Success)
            {
                logger.LogInformation("Webhook processed successfully: {EventType} {EventId}",
                    result.EventType, result.EventId);

                return Ok(new
                {
                    status = "success",
                    eventType = result.EventType,
                    eventId = result.EventId,
                    processedAt = DateTime.UtcNow
                });
            }

            logger.LogWarning("Webhook processing failed: {Error}", result.Error);
            return BadRequest(new
            {
                status = "failed",
                error = result.Error,
                processedAt = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error processing webhook");
            return StatusCode(500, new
            {
                status = "error",
                error = "Internal server error",
                processedAt = DateTime.UtcNow
            });
        }
    }

    /// <summary>
    ///     Health check endpoint.
    /// </summary>
    [HttpGet("health")]
    public IActionResult HealthCheck()
    {
        return Ok(new
        {
            service = "Embedly Webhook Handler",
            status = "healthy",
            timestamp = DateTime.UtcNow,
            version = "1.0.0"
        });
    }

    /// <summary>
    ///     Test webhook signature validation.
    /// </summary>
    [HttpPost("test-signature")]
    public IActionResult TestSignature([FromBody] TestSignatureRequest request)
    {
        if (string.IsNullOrEmpty(request.Payload) || string.IsNullOrEmpty(request.Signature))
            return BadRequest("Payload and signature are required");

        // Use the SDK's built-in validation
        var isValid = webhookProcessor.ValidateWebhook(request.Payload, request.Signature);

        return Ok(new
        {
            isValid,
            reason = isValid ? "Valid signature" : "Invalid signature",
            timestamp = DateTime.UtcNow
        });
    }

    /// <summary>
    ///     Get supported webhook event types.
    /// </summary>
    [HttpGet("event-types")]
    public IActionResult GetSupportedEventTypes()
    {
        // Using the SDK's built-in event types
        var eventTypes = new[]
        {
            new { Type = WebhookEventTypes.CustomerCreated, Description = "Customer created" },
            new { Type = WebhookEventTypes.CustomerUpdated, Description = "Customer updated" },
            new { Type = WebhookEventTypes.CustomerVerified, Description = "Customer verified" },
            new { Type = WebhookEventTypes.WalletCreated, Description = "Wallet created" },
            new { Type = WebhookEventTypes.WalletActivated, Description = "Wallet activated" },
            new { Type = WebhookEventTypes.TransactionCreated, Description = "Transaction created" },
            new { Type = WebhookEventTypes.TransactionCompleted, Description = "Transaction completed" },
            new { Type = WebhookEventTypes.TransactionFailed, Description = "Transaction failed" },
            new { Type = WebhookEventTypes.PaymentInitiated, Description = "Payment initiated" },
            new { Type = WebhookEventTypes.PaymentCompleted, Description = "Payment completed" },
            new { Type = WebhookEventTypes.PaymentFailed, Description = "Payment failed" },
            new { Type = WebhookEventTypes.CardCreated, Description = "Card created" },
            new { Type = WebhookEventTypes.CardActivated, Description = "Card activated" },
            new { Type = WebhookEventTypes.CardBlocked, Description = "Card blocked" },
            new { Type = WebhookEventTypes.KycSubmitted, Description = "KYC submitted" },
            new { Type = WebhookEventTypes.KycApproved, Description = "KYC approved" },
            new { Type = WebhookEventTypes.KycRejected, Description = "KYC rejected" },
            new { Type = WebhookEventTypes.PayoutInitiated, Description = "Payout initiated" },
            new { Type = WebhookEventTypes.PayoutCompleted, Description = "Payout completed" },
            new { Type = WebhookEventTypes.PayoutFailed, Description = "Payout failed" }
        };

        return Ok(new
        {
            supportedEvents = eventTypes,
            count = eventTypes.Length,
            timestamp = DateTime.UtcNow
        });
    }

    private async Task<string> ReadPayloadAsync()
    {
        using var reader = new StreamReader(Request.Body, Encoding.UTF8);
        return await reader.ReadToEndAsync();
    }
}

/// <summary>
///     Request model for testing webhook signatures.
/// </summary>
public class TestSignatureRequest
{
    public string Payload { get; set; } = string.Empty;
    public string Signature { get; set; } = string.Empty;
}