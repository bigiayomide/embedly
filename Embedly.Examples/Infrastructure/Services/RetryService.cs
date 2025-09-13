using Polly;
using Polly.Extensions.Http;
using System.Net;

namespace Embedly.Examples.Infrastructure.Services;

/// <summary>
/// Service for handling retry logic with exponential backoff.
/// </summary>
public interface IRetryService
{
    /// <summary>
    /// Executes an operation with retry logic.
    /// </summary>
    Task<Result<T>> ExecuteWithRetryAsync<T>(Func<Task<T>> operation, string operationName, CancellationToken cancellationToken = default);

    /// <summary>
    /// Executes an operation with retry logic that returns no value.
    /// </summary>
    Task<Result> ExecuteWithRetryAsync(Func<Task> operation, string operationName, CancellationToken cancellationToken = default);
}

/// <summary>
/// Implementation of retry service using Polly.
/// </summary>
public class RetryService : IRetryService
{
    private readonly ILogger<RetryService> _logger;
    private readonly ICorrelationService _correlationService;

    public RetryService(ILogger<RetryService> logger, ICorrelationService correlationService)
    {
        _logger = logger;
        _correlationService = correlationService;
    }

    public async Task<Result<T>> ExecuteWithRetryAsync<T>(Func<Task<T>> operation, string operationName, CancellationToken cancellationToken = default)
    {
        var policy = Policy
            .Handle<HttpRequestException>()
            .Or<TaskCanceledException>()
            .Or<OperationCanceledException>()
            .OrResult<T>(result => result == null)
            .WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                onRetry: (outcome, timespan, retryCount, context) =>
                {
                    var exception = outcome.Exception;
                    _logger.LogWarning(
                        "Operation {OperationName} failed on attempt {RetryCount}. Retrying in {Delay}ms. Correlation: {CorrelationId}. Exception: {Exception}",
                        operationName, retryCount, timespan.TotalMilliseconds, _correlationService.CorrelationId, exception?.Message);
                });

        try
        {
            var result = await policy.ExecuteAsync(async () =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                return await operation();
            });

            _logger.LogInformation(
                "Operation {OperationName} completed successfully. Correlation: {CorrelationId}",
                operationName, _correlationService.CorrelationId);

            return Result<T>.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Operation {OperationName} failed after all retry attempts. Correlation: {CorrelationId}",
                operationName, _correlationService.CorrelationId);

            return Result<T>.Failure(ex);
        }
    }

    public async Task<Result> ExecuteWithRetryAsync(Func<Task> operation, string operationName, CancellationToken cancellationToken = default)
    {
        var policy = Policy
            .Handle<HttpRequestException>()
            .Or<TaskCanceledException>()
            .Or<OperationCanceledException>()
            .WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                onRetry: (exception, timespan, retryCount, context) =>
                {
                    _logger.LogWarning(
                        "Operation {OperationName} failed on attempt {RetryCount}. Retrying in {Delay}ms. Correlation: {CorrelationId}. Exception: {Exception}",
                        operationName, retryCount, timespan.TotalMilliseconds, _correlationService.CorrelationId, exception.Message);
                });

        try
        {
            await policy.ExecuteAsync(async () =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                await operation();
            });

            _logger.LogInformation(
                "Operation {OperationName} completed successfully. Correlation: {CorrelationId}",
                operationName, _correlationService.CorrelationId);

            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Operation {OperationName} failed after all retry attempts. Correlation: {CorrelationId}",
                operationName, _correlationService.CorrelationId);

            return Result.Failure(ex);
        }
    }
}