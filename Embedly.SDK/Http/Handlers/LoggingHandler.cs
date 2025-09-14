using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Embedly.SDK.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Embedly.SDK.Http.Handlers;

/// <summary>
///     HTTP message handler that logs request and response details.
/// </summary>
internal sealed class LoggingHandler : DelegatingHandler
{
    private readonly ILogger<LoggingHandler> _logger;
    private readonly EmbedlyOptions _options;

    /// <summary>
    ///     Initializes a new instance of the <see cref="LoggingHandler" /> class.
    /// </summary>
    /// <param name="logger">The logger instance.</param>
    /// <param name="options">The Embedly options.</param>
    public LoggingHandler(ILogger<LoggingHandler> logger, IOptions<EmbedlyOptions> options)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
    }

    /// <inheritdoc />
    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (!_options.EnableLogging) return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

        var requestId = Guid.NewGuid().ToString("N")[..8];

        await LogRequestAsync(request, requestId, cancellationToken).ConfigureAwait(false);

        var stopwatch = Stopwatch.StartNew();
        HttpResponseMessage? response = null;
        Exception? exception = null;

        try
        {
            response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
            return response;
        }
        catch (Exception ex)
        {
            exception = ex;
            throw;
        }
        finally
        {
            stopwatch.Stop();
            await LogResponseAsync(response, exception, requestId, stopwatch.ElapsedMilliseconds, cancellationToken)
                .ConfigureAwait(false);
        }
    }

    private async Task LogRequestAsync(
        HttpRequestMessage request,
        string requestId,
        CancellationToken cancellationToken)
    {
        var logLevel = LogLevel.Debug;

        if (!_logger.IsEnabled(logLevel)) return;

        var sb = new StringBuilder();
        sb.AppendLine($"[{requestId}] HTTP Request:");
        sb.AppendLine($"  Method: {request.Method}");
        sb.AppendLine($"  URI: {request.RequestUri}");

        // Log headers (excluding sensitive ones)
        if (request.Headers.Any())
        {
            sb.AppendLine("  Headers:");
            foreach (var header in request.Headers)
            {
                var value = IsSensitiveHeader(header.Key) ? "[REDACTED]" : string.Join(", ", header.Value);
                sb.AppendLine($"    {header.Key}: {value}");
            }
        }

        // Log request body if enabled and present
        if (_options.LogRequestBodies && request.Content != null)
            try
            {
                var content = await request.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
                if (!string.IsNullOrEmpty(content)) sb.AppendLine($"  Body: {SanitizeContent(content)}");
            }
            catch (Exception ex)
            {
                sb.AppendLine($"  Body: [Error reading content: {ex.Message}]");
            }

        _logger.Log(logLevel, "{HttpRequest}", sb.ToString().TrimEnd());
    }

    private async Task LogResponseAsync(
        HttpResponseMessage? response,
        Exception? exception,
        string requestId,
        long elapsedMs,
        CancellationToken cancellationToken)
    {
        var logLevel = exception != null || response?.IsSuccessStatusCode == false
            ? LogLevel.Warning
            : LogLevel.Debug;

        if (!_logger.IsEnabled(logLevel)) return;

        var sb = new StringBuilder();
        sb.AppendLine($"[{requestId}] HTTP Response ({elapsedMs}ms):");

        if (exception != null)
        {
            sb.AppendLine($"  Exception: {exception.GetType().Name}: {exception.Message}");
        }
        else if (response != null)
        {
            sb.AppendLine($"  Status: {(int)response.StatusCode} {response.StatusCode}");

            // Log response headers
            if (response.Headers.Any())
            {
                sb.AppendLine("  Headers:");
                foreach (var header in response.Headers)
                    sb.AppendLine($"    {header.Key}: {string.Join(", ", header.Value)}");
            }

            // Log response body if enabled
            if (_options.LogRequestBodies && response.Content != null)
                try
                {
                    var content = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
                    if (!string.IsNullOrEmpty(content)) sb.AppendLine($"  Body: {SanitizeContent(content)}");
                }
                catch (Exception ex)
                {
                    sb.AppendLine($"  Body: [Error reading content: {ex.Message}]");
                }
        }

        _logger.Log(logLevel, "{HttpResponse}", sb.ToString().TrimEnd());
    }

    private static bool IsSensitiveHeader(string headerName)
    {
        return headerName.Equals("x-api-key", StringComparison.OrdinalIgnoreCase) ||
               headerName.Equals("authorization", StringComparison.OrdinalIgnoreCase) ||
               headerName.Contains("token", StringComparison.OrdinalIgnoreCase) ||
               headerName.Contains("key", StringComparison.OrdinalIgnoreCase);
    }

    private static string SanitizeContent(string content)
    {
        // Truncate very long content for logging
        if (content.Length > 1000) return $"{content[..1000]}... [truncated {content.Length - 1000} characters]";

        return content;
    }
}