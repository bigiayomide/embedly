namespace Embedly.Examples.Infrastructure.Services;

/// <summary>
/// Service for managing correlation IDs across requests and operations.
/// </summary>
public interface ICorrelationService
{
    /// <summary>
    /// Gets the current correlation ID.
    /// </summary>
    string CorrelationId { get; }

    /// <summary>
    /// Sets a new correlation ID.
    /// </summary>
    void SetCorrelationId(string correlationId);

    /// <summary>
    /// Generates a new correlation ID and sets it as current.
    /// </summary>
    string GenerateNew();
}

/// <summary>
/// Implementation of correlation service using AsyncLocal for thread safety.
/// </summary>
public class CorrelationService : ICorrelationService
{
    private static readonly AsyncLocal<string?> _correlationId = new();

    public string CorrelationId => _correlationId.Value ?? GenerateNew();

    public void SetCorrelationId(string correlationId)
    {
        if (string.IsNullOrWhiteSpace(correlationId))
            throw new ArgumentException("Correlation ID cannot be null or empty", nameof(correlationId));

        _correlationId.Value = correlationId;
    }

    public string GenerateNew()
    {
        var newId = Guid.NewGuid().ToString("N")[..12].ToUpperInvariant();
        _correlationId.Value = newId;
        return newId;
    }
}

/// <summary>
/// Extension methods for logging with correlation IDs.
/// </summary>
public static class LoggerExtensions
{
    public static IDisposable BeginCorrelatedScope(this ILogger logger, ICorrelationService correlationService)
    {
        return logger.BeginScope(new Dictionary<string, object>
        {
            ["CorrelationId"] = correlationService.CorrelationId
        });
    }

    public static void LogInformationCorrelated(this ILogger logger, ICorrelationService correlationService,
        string message, params object[] args)
    {
        using (logger.BeginCorrelatedScope(correlationService))
        {
            logger.LogInformation(message, args);
        }
    }

    public static void LogErrorCorrelated(this ILogger logger, ICorrelationService correlationService,
        Exception exception, string message, params object[] args)
    {
        using (logger.BeginCorrelatedScope(correlationService))
        {
            logger.LogError(exception, message, args);
        }
    }
}