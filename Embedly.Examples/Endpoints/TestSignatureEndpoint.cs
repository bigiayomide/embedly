using FastEndpoints;
using Embedly.SDK.Webhooks;

namespace Embedly.Examples.Endpoints;

/// <summary>
/// FastEndpoints implementation for testing webhook signature validation.
/// </summary>
public class TestSignatureEndpoint(IWebhookProcessor webhookProcessor, ILogger<TestSignatureEndpoint> logger)
    : Endpoint<TestSignatureRequest, TestSignatureResponse>
{
    public override void Configure()
    {
        Post("/api/webhooks/test-signature");
        AllowAnonymous();
        Description(d => d
            .WithTags("Webhooks")
            .WithSummary("Test webhook signature validation")
            .WithDescription("Tests webhook signature validation without processing the event")
            .Produces<TestSignatureResponse>(200, "application/json")
            .ProducesProblem(400));
    }

    public override async Task HandleAsync(TestSignatureRequest req, CancellationToken ct)
    {
        try
        {
            if (string.IsNullOrEmpty(req.Payload) || string.IsNullOrEmpty(req.Signature))
            {
                await Send.ResponseAsync(new TestSignatureResponse
                {
                    IsValid = false,
                    Message = "Payload and signature are required",
                    Timestamp = DateTime.UtcNow
                }, 400, ct);
                return;
            }

            var isValid = webhookProcessor.ValidateWebhook(req.Payload, req.Signature);

            logger.LogInformation("Signature validation test: {IsValid}", isValid);

            await Send.ResponseAsync(new TestSignatureResponse
            {
                IsValid = isValid,
                Message = isValid ? "Valid signature" : "Invalid signature",
                Timestamp = DateTime.UtcNow
            }, cancellation:ct);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error testing webhook signature");
            await Send.ResponseAsync(new TestSignatureResponse
            {
                IsValid = false,
                Message = "Error validating signature",
                Timestamp = DateTime.UtcNow
            }, cancellation:ct);
        }
    }
}

/// <summary>
/// Request model for testing webhook signatures.
/// </summary>
public class TestSignatureRequest
{
    public string Payload { get; set; } = string.Empty;
    public string Signature { get; set; } = string.Empty;
}

/// <summary>
/// Response model for signature validation test.
/// </summary>
public class TestSignatureResponse
{
    public bool IsValid { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
}