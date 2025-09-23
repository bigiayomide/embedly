using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Embedly.SDK.Configuration;
using Embedly.SDK.Exceptions;
using Embedly.SDK.Helpers;
using Embedly.SDK.Models.Common;
using Embedly.SDK.Models.Responses.Common;
using Microsoft.Extensions.Options;

namespace Embedly.SDK.Http;

/// <summary>
///     HTTP client for making requests to the Embedly API.
/// </summary>
internal sealed class EmbedlyHttpClient : IEmbedlyHttpClient, IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;
    private readonly EmbedlyOptions _options;

    /// <summary>
    ///     Initializes a new instance of the <see cref="EmbedlyHttpClient" /> class.
    /// </summary>
    /// <param name="httpClient">The HTTP client.</param>
    /// <param name="options">The Embedly options.</param>
    public EmbedlyHttpClient(HttpClient httpClient, IOptions<EmbedlyOptions> options)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));

        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNameCaseInsensitive = true
        };

        ConfigureHttpClient();
    }

    /// <inheritdoc />
    public void Dispose()
    {
        _httpClient?.Dispose();
    }

    /// <inheritdoc />
    public async Task<ApiResponse<T>> GetAsync<T>(string endpoint, CancellationToken cancellationToken = default)
    {
        return await GetAsync<T>(endpoint, new Dictionary<string, object?>(), cancellationToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<T>> GetAsync<T>(
        string endpoint,
        Dictionary<string, object?> queryParams,
        CancellationToken cancellationToken = default)
    {
        Guard.ThrowIfNullOrWhiteSpace(endpoint, nameof(endpoint));
        Guard.ThrowIfNull(queryParams, nameof(queryParams));

        var uri = BuildUri(endpoint, queryParams);
        var response = await SendRequestAsync(HttpMethod.Get, uri, null, cancellationToken)
            .ConfigureAwait(false);

        return await DeserializeResponseAsync<ApiResponse<T>>(response, endpoint, "GET")
            .ConfigureAwait(false);
    }


    /// <inheritdoc />
    public async Task<ApiResponse<T>> PostAsync<T>(string endpoint, object? content = null,
        CancellationToken cancellationToken = default)
    {
        Guard.ThrowIfNullOrWhiteSpace(endpoint, nameof(endpoint));

        var response = await SendRequestAsync(HttpMethod.Post, endpoint, content, cancellationToken)
            .ConfigureAwait(false);

        return await DeserializeResponseAsync<ApiResponse<T>>(response, endpoint, "POST")
            .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<TResponse>> PostAsync<TRequest, TResponse>(string endpoint, TRequest request,
        CancellationToken cancellationToken = default)
    {
        Guard.ThrowIfNullOrWhiteSpace(endpoint, nameof(endpoint));

        var response = await SendRequestAsync(HttpMethod.Post, endpoint, request, cancellationToken)
            .ConfigureAwait(false);

        return await DeserializeResponseAsync<ApiResponse<TResponse>>(response, endpoint, "POST")
            .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<TResponse>> PostAsync<TRequest, TResponse>(string endpoint, TRequest request,
        Dictionary<string, object?> queryParams, CancellationToken cancellationToken = default)
    {
        Guard.ThrowIfNullOrWhiteSpace(endpoint, nameof(endpoint));
        Guard.ThrowIfNull(queryParams, nameof(queryParams));

        var uri = BuildUri(endpoint, queryParams);
        var response = await SendRequestAsync(HttpMethod.Post, uri, request, cancellationToken)
            .ConfigureAwait(false);

        return await DeserializeResponseAsync<ApiResponse<TResponse>>(response, endpoint, "POST")
            .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<T>> PutAsync<T>(string endpoint, object? content = null,
        CancellationToken cancellationToken = default)
    {
        Guard.ThrowIfNullOrWhiteSpace(endpoint, nameof(endpoint));

        var response = await SendRequestAsync(HttpMethod.Put, endpoint, content, cancellationToken)
            .ConfigureAwait(false);

        return await DeserializeResponseAsync<ApiResponse<T>>(response, endpoint, "PUT")
            .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<TResponse>> PutAsync<TRequest, TResponse>(string endpoint, TRequest request,
        CancellationToken cancellationToken = default)
    {
        Guard.ThrowIfNullOrWhiteSpace(endpoint, nameof(endpoint));

        var response = await SendRequestAsync(HttpMethod.Put, endpoint, request, cancellationToken)
            .ConfigureAwait(false);

        return await DeserializeResponseAsync<ApiResponse<TResponse>>(response, endpoint, "PUT")
            .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<T>> PatchAsync<T>(string endpoint, object? content = null,
        CancellationToken cancellationToken = default)
    {
        Guard.ThrowIfNullOrWhiteSpace(endpoint, nameof(endpoint));

        var response = await SendRequestAsync(HttpMethod.Patch, endpoint, content, cancellationToken)
            .ConfigureAwait(false);

        return await DeserializeResponseAsync<ApiResponse<T>>(response, endpoint, "PATCH")
            .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<TResponse>> PatchAsync<TRequest, TResponse>(string endpoint, TRequest request,
        CancellationToken cancellationToken = default)
    {
        Guard.ThrowIfNullOrWhiteSpace(endpoint, nameof(endpoint));

        var response = await SendRequestAsync(HttpMethod.Patch, endpoint, request, cancellationToken)
            .ConfigureAwait(false);

        return await DeserializeResponseAsync<ApiResponse<TResponse>>(response, endpoint, "PATCH")
            .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task DeleteAsync(string endpoint, CancellationToken cancellationToken = default)
    {
        Guard.ThrowIfNullOrWhiteSpace(endpoint, nameof(endpoint));

        await SendRequestAsync(HttpMethod.Delete, endpoint, null, cancellationToken)
            .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<T>> DeleteAsync<T>(string endpoint, CancellationToken cancellationToken = default)
    {
        Guard.ThrowIfNullOrWhiteSpace(endpoint, nameof(endpoint));

        var response = await SendRequestAsync(HttpMethod.Delete, endpoint, null, cancellationToken)
            .ConfigureAwait(false);

        return await DeserializeResponseAsync<ApiResponse<T>>(response, endpoint, "DELETE")
            .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<T>> PostMultipartAsync<T>(
        string endpoint,
        Stream fileStream,
        string fileName,
        string contentType = "application/octet-stream",
        CancellationToken cancellationToken = default)
    {
        Guard.ThrowIfNullOrWhiteSpace(endpoint, nameof(endpoint));
        Guard.ThrowIfNull(fileStream, nameof(fileStream));
        Guard.ThrowIfNullOrWhiteSpace(fileName, nameof(fileName));

        using var request = new HttpRequestMessage(HttpMethod.Post, endpoint);

        // Add authentication header
        request.Headers.Add("Authorization", $"Bearer {_options.ApiKey}");

        // Create multipart content
        using var multipartContent = new MultipartFormDataContent();
        using var streamContent = new StreamContent(fileStream);
        streamContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);
        multipartContent.Add(streamContent, "file", fileName);

        request.Content = multipartContent;

        try
        {
            var response = await _httpClient.SendAsync(request, cancellationToken)
                .ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
                await HandleErrorResponseAsync(response, endpoint, "POST")
                    .ConfigureAwait(false);

            return await DeserializeResponseAsync<ApiResponse<T>>(response, endpoint, "POST")
                .ConfigureAwait(false);
        }
        catch (HttpRequestException ex)
        {
            throw new EmbedlyException($"Network error occurred while uploading file to {endpoint}: {ex.Message}", ex);
        }
        catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
        {
            throw new EmbedlyException($"File upload to {endpoint} timed out after {_options.Timeout}", ex);
        }
        catch (TaskCanceledException ex) when (cancellationToken.IsCancellationRequested)
        {
            throw new OperationCanceledException($"File upload to {endpoint} was cancelled", ex, cancellationToken);
        }
    }

    private void ConfigureHttpClient()
    {
        _httpClient.Timeout = _options.Timeout;

        if (!_httpClient.DefaultRequestHeaders.Contains("Accept"))
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
    }

    private async Task<HttpResponseMessage> SendRequestAsync(
        HttpMethod method,
        string endpoint,
        object? content,
        CancellationToken cancellationToken)
    {
        using var request = new HttpRequestMessage(method, endpoint);

        if (content != null)
        {
            var json = JsonSerializer.Serialize(content, _jsonOptions);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");
        }

        try
        {
            var response = await _httpClient.SendAsync(request, cancellationToken)
                .ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
                await HandleErrorResponseAsync(response, endpoint, method.Method)
                    .ConfigureAwait(false);

            return response;
        }
        catch (HttpRequestException ex)
        {
            throw new EmbedlyException($"Network error occurred while calling {method} {endpoint}: {ex.Message}", ex);
        }
        catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
        {
            throw new EmbedlyException($"Request to {method} {endpoint} timed out after {_options.Timeout}", ex);
        }
        catch (TaskCanceledException ex) when (cancellationToken.IsCancellationRequested)
        {
            throw new OperationCanceledException($"Request to {method} {endpoint} was cancelled", ex,
                cancellationToken);
        }
    }

    private async Task<T> DeserializeResponseAsync<T>(
        HttpResponseMessage response,
        string endpoint,
        string method)
    {
        var responseBody = await response.Content.ReadAsStringAsync()
            .ConfigureAwait(false);

        if (string.IsNullOrWhiteSpace(responseBody))
        {
            if (typeof(T) == typeof(string)) return (T)(object)string.Empty;

            throw new EmbedlyException($"Empty response received from {method} {endpoint}");
        }

        try
        {
            var result = JsonSerializer.Deserialize<T>(responseBody, _jsonOptions);
            return result ?? throw new EmbedlyException($"Failed to deserialize response from {method} {endpoint}");
        }
        catch (JsonException ex)
        {
            throw new EmbedlyException(
                $"Failed to deserialize response from {method} {endpoint}: {ex.Message}. Response: {responseBody}",
                ex);
        }
    }

    private async Task HandleErrorResponseAsync(
        HttpResponseMessage response,
        string endpoint,
        string method)
    {
        var responseBody = await response.Content.ReadAsStringAsync()
            .ConfigureAwait(false);

        var requestId = GetRequestId(response);

        switch (response.StatusCode)
        {
            case HttpStatusCode.Unauthorized:
                throw EmbedlyApiException.Unauthorized("Invalid API key or authentication failed");

            case HttpStatusCode.Forbidden:
                throw EmbedlyApiException.Forbidden(endpoint);

            case HttpStatusCode.NotFound:
                throw EmbedlyApiException.NotFound("Resource", endpoint);

            case HttpStatusCode.TooManyRequests:
                var retryAfter = response.Headers.RetryAfter?.ToString();
                throw EmbedlyApiException.RateLimitExceeded(retryAfter);

            case HttpStatusCode.BadRequest:
                var message = await TryParseErrorMessage(responseBody)
                    .ConfigureAwait(false) ?? "Bad request";
                throw new EmbedlyApiException(
                    message,
                    response.StatusCode,
                    responseBody,
                    "BAD_REQUEST",
                    requestId,
                    endpoint,
                    method);

            default:
                var errorMessage = await TryParseErrorMessage(responseBody)
                        .ConfigureAwait(false) ?? $"API request failed with status {response.StatusCode}";
                throw new EmbedlyApiException(
                    errorMessage,
                    response.StatusCode,
                    responseBody,
                    response.StatusCode.ToString(),
                    requestId,
                    endpoint,
                    method);
        }
    }

    private static Task<string?> TryParseErrorMessage(string responseBody)
    {
        if (string.IsNullOrWhiteSpace(responseBody)) return Task.FromResult<string?>(null);

        try
        {
            var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(responseBody, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            return Task.FromResult(errorResponse?.GetErrorMessage());
        }
        catch
        {
            return Task.FromResult<string?>(null);
        }
    }

    private static string? GetRequestId(HttpResponseMessage response)
    {
        response.Headers.TryGetValues("x-request-id", out var values);
        return values?.FirstOrDefault();
    }

    private static string BuildUri(string endpoint, Dictionary<string, object?> queryParams)
    {
        if (!queryParams.Any()) return endpoint;

        var queryString = string.Join("&",
            queryParams
                .Where(kvp => kvp.Value != null)
                .Select(kvp => $"{Uri.EscapeDataString(kvp.Key)}={Uri.EscapeDataString(kvp.Value!.ToString()!)}"));

        var separator = endpoint.Contains('?') ? "&" : "?";
        return $"{endpoint}{separator}{queryString}";
    }
}