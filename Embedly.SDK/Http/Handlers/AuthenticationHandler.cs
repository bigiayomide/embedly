using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Embedly.SDK.Configuration;
using Microsoft.Extensions.Options;

namespace Embedly.SDK.Http.Handlers;

/// <summary>
///     HTTP message handler that adds authentication headers to outbound requests.
/// </summary>
internal sealed class AuthenticationHandler : DelegatingHandler
{
    private const string ApiKeyHeaderName = "x-api-key";
    private readonly string _apiKey;

    /// <summary>
    ///     Initializes a new instance of the <see cref="AuthenticationHandler" /> class.
    /// </summary>
    /// <param name="options">The Embedly options containing the API key.</param>
    public AuthenticationHandler(IOptions<EmbedlyOptions> options)
    {
        ArgumentNullException.ThrowIfNull(options);
        _apiKey = options.Value.ApiKey ?? throw new ArgumentException("API key cannot be null", nameof(options));

        if (string.IsNullOrWhiteSpace(_apiKey))
            throw new ArgumentException("API key cannot be empty or whitespace", nameof(options));
    }

    /// <inheritdoc />
    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Add API key header if not already present
        if (!request.Headers.Contains(ApiKeyHeaderName)) request.Headers.Add(ApiKeyHeaderName, _apiKey);

        // Add User-Agent header for identification
        if (!request.Headers.Contains("User-Agent"))
        {
            var userAgent = $"Embedly-DotNet-SDK/{GetType().Assembly.GetName().Version}";
            request.Headers.Add("User-Agent", userAgent);
        }

        return base.SendAsync(request, cancellationToken);
    }
}