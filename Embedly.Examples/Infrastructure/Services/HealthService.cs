using Embedly.SDK;
using System.Diagnostics;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Embedly.Examples.Infrastructure.Services;

/// <summary>
/// Service for checking application and dependency health.
/// </summary>
public interface IHealthService
{
    /// <summary>
    /// Performs a comprehensive health check.
    /// </summary>
    Task<HealthCheckResult> CheckHealthAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if the Embedly API is accessible.
    /// </summary>
    Task<HealthCheckResult> CheckEmbedlyHealthAsync(CancellationToken cancellationToken = default);
}

/// <summary>
/// Implementation of health service.
/// </summary>
public class HealthService(
    IEmbedlyClient embedlyClient,
    ILogger<HealthService> logger,
    ICorrelationService correlationService)
    : IHealthService
{
    public async Task<HealthCheckResult> CheckHealthAsync(CancellationToken cancellationToken = default)
    {
        var stopwatch = Stopwatch.StartNew();
        var checks = new Dictionary<string, object>();

        try
        {
            // Check system resources
            var process = Process.GetCurrentProcess();
            checks["Memory"] = $"{process.WorkingSet64 / 1024 / 1024} MB";
            checks["Threads"] = process.Threads.Count;
            checks["Uptime"] = DateTime.Now - process.StartTime;

            // Check Embedly API
            var embedlyHealth = await CheckEmbedlyHealthAsync(cancellationToken);
            checks["Embedly"] = embedlyHealth.Status.ToString();

            foreach (var kvp in embedlyHealth.Data)
            {
                checks[$"Embedly_{kvp.Key}"] = kvp.Value;
            }

            stopwatch.Stop();
            checks["ResponseTime"] = $"{stopwatch.ElapsedMilliseconds}ms";

            var overallStatus = embedlyHealth.Status == HealthStatus.Healthy
                ? HealthStatus.Healthy
                : HealthStatus.Degraded;

            return new HealthCheckResult(
                status: overallStatus,
                description: "Application health check completed",
                data: checks);
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            checks["ResponseTime"] = $"{stopwatch.ElapsedMilliseconds}ms";
            checks["Error"] = ex.Message;

            logger.LogError(ex,
                "Health check failed. Correlation: {CorrelationId}",
                correlationService.CorrelationId);

            return new HealthCheckResult(
                status: HealthStatus.Unhealthy,
                description: "Health check failed",
                exception: ex,
                data: checks);
        }
    }

    public async Task<HealthCheckResult> CheckEmbedlyHealthAsync(CancellationToken cancellationToken = default)
    {
        var stopwatch = Stopwatch.StartNew();
        var data = new Dictionary<string, object>();

        try
        {
            // Try to fetch currencies as a health check (this is typically a lightweight operation)
            var response = await embedlyClient.Utilities.GetCurrenciesAsync(cancellationToken);

            stopwatch.Stop();
            data["ResponseTime"] = $"{stopwatch.ElapsedMilliseconds}ms";
            data["Status"] = "Connected";
            data["Timestamp"] = DateTime.UtcNow;

            if (response?.Data?.Any() != true)
                return new HealthCheckResult(
                    status: HealthStatus.Healthy,
                    description: "Embedly API is accessible",
                    data: data);
            data["CurrencyCount"] = response.Data.Count();
            data["SampleCurrency"] = response.Data.First().Code;

            return new HealthCheckResult(
                status: HealthStatus.Healthy,
                description: "Embedly API is accessible",
                data: data);
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            data["ResponseTime"] = $"{stopwatch.ElapsedMilliseconds}ms";
            data["Status"] = "Error";
            data["Error"] = ex.Message;
            data["Timestamp"] = DateTime.UtcNow;

            logger.LogError(ex,
                "Embedly health check failed. Correlation: {CorrelationId}",
                correlationService.CorrelationId);

            return new HealthCheckResult(
                status: HealthStatus.Unhealthy,
                description: "Embedly API is not accessible",
                exception: ex,
                data: data);
        }
    }
}

/// <summary>
/// Health check for use with ASP.NET Core health check middleware.
/// </summary>
public class EmbedlyHealthCheck : IHealthCheck
{
    private readonly IHealthService _healthService;

    public EmbedlyHealthCheck(IHealthService healthService)
    {
        _healthService = healthService;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        return await _healthService.CheckEmbedlyHealthAsync(cancellationToken);
    }
}