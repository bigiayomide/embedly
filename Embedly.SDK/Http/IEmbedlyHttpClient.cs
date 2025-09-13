using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Embedly.SDK.Models.Responses.Common;

namespace Embedly.SDK.Http;

/// <summary>
/// Interface for making HTTP requests to the Embedly API.
/// </summary>
public interface IEmbedlyHttpClient
{
    /// <summary>
    /// Sends a GET request to the specified endpoint.
    /// </summary>
    /// <typeparam name="T">The type to deserialize the response to.</typeparam>
    /// <param name="endpoint">The API endpoint.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The API response wrapped in ApiResponse.</returns>
    Task<ApiResponse<T>> GetAsync<T>(string endpoint, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Sends a GET request to the specified endpoint with query parameters.
    /// </summary>
    /// <typeparam name="T">The type to deserialize the response to.</typeparam>
    /// <param name="endpoint">The API endpoint.</param>
    /// <param name="queryParams">The query parameters.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The API response wrapped in ApiResponse.</returns>
    Task<ApiResponse<T>> GetAsync<T>(string endpoint, Dictionary<string, object?> queryParams, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Sends a POST request to the specified endpoint.
    /// </summary>
    /// <typeparam name="T">The type to deserialize the response to.</typeparam>
    /// <param name="endpoint">The API endpoint.</param>
    /// <param name="content">The request content.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The API response wrapped in ApiResponse.</returns>
    Task<ApiResponse<T>> PostAsync<T>(string endpoint, object? content = null, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Sends a strongly-typed POST request to the specified endpoint.
    /// </summary>
    /// <typeparam name="TRequest">The type of the request.</typeparam>
    /// <typeparam name="TResponse">The type to deserialize the response to.</typeparam>
    /// <param name="endpoint">The API endpoint.</param>
    /// <param name="request">The request content.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The API response wrapped in ApiResponse.</returns>
    Task<ApiResponse<TResponse>> PostAsync<TRequest, TResponse>(string endpoint, TRequest request, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Sends a PUT request to the specified endpoint.
    /// </summary>
    /// <typeparam name="T">The type to deserialize the response to.</typeparam>
    /// <param name="endpoint">The API endpoint.</param>
    /// <param name="content">The request content.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The API response wrapped in ApiResponse.</returns>
    Task<ApiResponse<T>> PutAsync<T>(string endpoint, object? content = null, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Sends a strongly-typed PUT request to the specified endpoint.
    /// </summary>
    /// <typeparam name="TRequest">The type of the request.</typeparam>
    /// <typeparam name="TResponse">The type to deserialize the response to.</typeparam>
    /// <param name="endpoint">The API endpoint.</param>
    /// <param name="request">The request content.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The API response wrapped in ApiResponse.</returns>
    Task<ApiResponse<TResponse>> PutAsync<TRequest, TResponse>(string endpoint, TRequest request, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Sends a PATCH request to the specified endpoint.
    /// </summary>
    /// <typeparam name="T">The type to deserialize the response to.</typeparam>
    /// <param name="endpoint">The API endpoint.</param>
    /// <param name="content">The request content.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The API response wrapped in ApiResponse.</returns>
    Task<ApiResponse<T>> PatchAsync<T>(string endpoint, object? content = null, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Sends a strongly-typed PATCH request to the specified endpoint.
    /// </summary>
    /// <typeparam name="TRequest">The type of the request.</typeparam>
    /// <typeparam name="TResponse">The type to deserialize the response to.</typeparam>
    /// <param name="endpoint">The API endpoint.</param>
    /// <param name="request">The request content.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The API response wrapped in ApiResponse.</returns>
    Task<ApiResponse<TResponse>> PatchAsync<TRequest, TResponse>(string endpoint, TRequest request, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Sends a DELETE request to the specified endpoint.
    /// </summary>
    /// <param name="endpoint">The API endpoint.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task DeleteAsync(string endpoint, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Sends a DELETE request to the specified endpoint and returns a response.
    /// </summary>
    /// <typeparam name="T">The type to deserialize the response to.</typeparam>
    /// <param name="endpoint">The API endpoint.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The API response wrapped in ApiResponse.</returns>
    Task<ApiResponse<T>> DeleteAsync<T>(string endpoint, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Sends a POST request with multipart/form-data content (for file uploads).
    /// </summary>
    /// <typeparam name="T">The type to deserialize the response to.</typeparam>
    /// <param name="endpoint">The API endpoint.</param>
    /// <param name="fileStream">The file stream to upload.</param>
    /// <param name="fileName">The name of the file.</param>
    /// <param name="contentType">The content type of the file.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The API response wrapped in ApiResponse.</returns>
    Task<ApiResponse<T>> PostMultipartAsync<T>(string endpoint, Stream fileStream, string fileName, string contentType = "application/octet-stream", CancellationToken cancellationToken = default);
}