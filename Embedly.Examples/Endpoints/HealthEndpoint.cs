using Embedly.Examples.Infrastructure.Services;
using FastEndpoints;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Embedly.Examples.Endpoints;

/// <summary>
///     FastEndpoints health check endpoint.
/// </summary>
public class HealthEndpoint : EndpointWithoutRequest
{
    private readonly IHealthService _healthService;
    private readonly ILogger<HealthEndpoint> _logger;

    public HealthEndpoint(IHealthService healthService, ILogger<HealthEndpoint> logger)
    {
        _healthService = healthService;
        _logger = logger;
    }

    public override void Configure()
    {
        Get("/api/health");
        AllowAnonymous();
        Description(d => d
            .WithTags("Health")
            .WithSummary("Application health check")
            .WithDescription("Returns the health status of the application and its dependencies")
            .Produces<HealthResponse>(200, "application/json")
            .ProducesProblem(503));
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        try
        {
            var healthResult = await _healthService.CheckHealthAsync(ct);

            var response = new HealthResponse
            {
                Service = "Embedly Examples API",
                Status = healthResult.Status.ToString().ToLowerInvariant(),
                Timestamp = DateTime.UtcNow,
                Version = "1.0.0",
                Dependencies = healthResult.Data?.ToDictionary(x => x.Key, x => x.Value) ??
                               new Dictionary<string, object>()
            };

            if (healthResult.Status == HealthStatus.Healthy)
                await Send.ResponseAsync(response, cancellation: ct);
            else
                await Send.ResponseAsync(TypedResults.Json(response, statusCode: 503), cancellation: ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking application health");
            await Send.ResponseAsync(new HealthResponse
            {
                Service = "Embedly Examples API",
                Status = "error",
                Timestamp = DateTime.UtcNow,
                Version = "1.0.0",
                Error = "Health check failed"
            }, 500, ct);
        }
    }
}

/// <summary>
///     Response model for health check.
/// </summary>
public class HealthResponse
{
    public string Service { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public string Version { get; set; } = string.Empty;
    public string? Error { get; set; }
    public Dictionary<string, object>? Dependencies { get; set; }
}