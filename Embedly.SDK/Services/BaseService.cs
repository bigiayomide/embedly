using System;
using Embedly.SDK.Configuration;
using Embedly.SDK.Helpers;
using Embedly.SDK.Http;
using Microsoft.Extensions.Options;

namespace Embedly.SDK.Services;

/// <summary>
///     Base class for all Embedly API services.
/// </summary>
public abstract class BaseService
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="BaseService" /> class.
    /// </summary>
    /// <param name="httpClient">The HTTP client.</param>
    /// <param name="options">The configuration options.</param>
    protected BaseService(IEmbedlyHttpClient httpClient, IOptions<EmbedlyOptions> options)
    {
        HttpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        Options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        ServiceUrls = Options.GetServiceUrls();
    }

    /// <summary>
    ///     Gets the HTTP client for making API requests.
    /// </summary>
    protected IEmbedlyHttpClient HttpClient { get; }

    /// <summary>
    ///     Gets the Embedly configuration options.
    /// </summary>
    protected EmbedlyOptions Options { get; }

    /// <summary>
    ///     Gets the service URLs for the current environment.
    /// </summary>
    protected ServiceUrls ServiceUrls { get; }

    /// <summary>
    ///     Builds a complete URL for the base API service.
    /// </summary>
    /// <param name="endpoint">The API endpoint.</param>
    /// <returns>The complete URL.</returns>
    protected string BuildUrl(string endpoint)
    {
        return BuildUrl(ServiceUrls.Base, endpoint);
    }

    /// <summary>
    ///     Builds a complete URL for a specific service.
    /// </summary>
    /// <param name="serviceUrl">The base service URL.</param>
    /// <param name="endpoint">The API endpoint.</param>
    /// <returns>The complete URL.</returns>
    protected string BuildUrl(string serviceUrl, string endpoint)
    {
        Guard.ThrowIfNullOrWhiteSpace(serviceUrl, nameof(serviceUrl));
        Guard.ThrowIfNullOrWhiteSpace(endpoint, nameof(endpoint));

        var baseUrl = serviceUrl.TrimEnd('/');
        var cleanEndpoint = endpoint.TrimStart('/');

        return $"{baseUrl}/{cleanEndpoint}";
    }

    /// <summary>
    ///     Validates required string parameters.
    /// </summary>
    /// <param name="value">The value to validate.</param>
    /// <param name="paramName">The parameter name for exception messages.</param>
    /// <exception cref="ArgumentException">Thrown when the value is null, empty, or whitespace.</exception>
    protected static void ValidateRequired(string? value, string paramName)
    {
        Guard.ThrowIfNullOrWhiteSpace(value, paramName);
    }

    /// <summary>
    ///     Validates required object parameters.
    /// </summary>
    /// <param name="value">The value to validate.</param>
    /// <param name="paramName">The parameter name for exception messages.</param>
    /// <exception cref="ArgumentNullException">Thrown when the value is null.</exception>
    protected static void ValidateRequired<T>(T? value, string paramName) where T : class
    {
        ArgumentNullException.ThrowIfNull(value, paramName);
    }
}