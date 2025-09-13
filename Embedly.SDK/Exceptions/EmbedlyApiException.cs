using System;
using System.Collections.Generic;
using System.Net;

namespace Embedly.SDK.Exceptions;

/// <summary>
/// Exception thrown when an API call to Embedly fails.
/// </summary>
[Serializable]
public class EmbedlyApiException : EmbedlyException
{
    /// <summary>
    /// Gets the HTTP status code returned by the API.
    /// </summary>
    public HttpStatusCode StatusCode { get; }
    
    /// <summary>
    /// Gets the raw response body from the API.
    /// </summary>
    public string? ResponseBody { get; }
    
    /// <summary>
    /// Gets the request ID for tracking purposes.
    /// </summary>
    public string? RequestId { get; }
    
    /// <summary>
    /// Gets the endpoint that was called.
    /// </summary>
    public string? Endpoint { get; }
    
    /// <summary>
    /// Gets the HTTP method used.
    /// </summary>
    public string? HttpMethod { get; }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="EmbedlyApiException"/> class.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="statusCode">The HTTP status code.</param>
    public EmbedlyApiException(string message, HttpStatusCode statusCode) 
        : base(message)
    {
        StatusCode = statusCode;
    }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="EmbedlyApiException"/> class with full details.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="statusCode">The HTTP status code.</param>
    /// <param name="responseBody">The raw response body.</param>
    /// <param name="errorCode">The API error code.</param>
    /// <param name="requestId">The request ID for tracking.</param>
    /// <param name="endpoint">The endpoint that was called.</param>
    /// <param name="httpMethod">The HTTP method used.</param>
    /// <param name="innerException">The inner exception, if any.</param>
    public EmbedlyApiException(
        string message,
        HttpStatusCode statusCode,
        string? responseBody = null,
        string? errorCode = null,
        string? requestId = null,
        string? endpoint = null,
        string? httpMethod = null,
        Exception? innerException = null)
        : base(message, errorCode, CreateErrorContext(statusCode, responseBody, requestId, endpoint, httpMethod), innerException)
    {
        StatusCode = statusCode;
        ResponseBody = responseBody;
        RequestId = requestId;
        Endpoint = endpoint;
        HttpMethod = httpMethod;
    }
    
    private static Dictionary<string, object?> CreateErrorContext(
        HttpStatusCode statusCode,
        string? responseBody,
        string? requestId,
        string? endpoint,
        string? httpMethod)
    {
        return new Dictionary<string, object?>
        {
            ["StatusCode"] = statusCode,
            ["ResponseBody"] = responseBody,
            ["RequestId"] = requestId,
            ["Endpoint"] = endpoint,
            ["HttpMethod"] = httpMethod,
            ["Timestamp"] = DateTimeOffset.UtcNow
        };
    }
    
    /// <summary>
    /// Creates an exception for rate limiting scenarios.
    /// </summary>
    public static EmbedlyApiException RateLimitExceeded(string? retryAfter = null)
    {
        var message = "API rate limit exceeded.";
        if (!string.IsNullOrEmpty(retryAfter))
        {
            message += $" Retry after: {retryAfter}";
        }
        
        return new EmbedlyApiException(
            message,
            HttpStatusCode.TooManyRequests,
            errorCode: "RATE_LIMIT_EXCEEDED");
    }
    
    /// <summary>
    /// Creates an exception for authentication failures.
    /// </summary>
    public static EmbedlyApiException Unauthorized(string? message = null)
    {
        return new EmbedlyApiException(
            message ?? "Authentication failed. Please check your API key.",
            HttpStatusCode.Unauthorized,
            errorCode: "UNAUTHORIZED");
    }
    
    /// <summary>
    /// Creates an exception for forbidden access.
    /// </summary>
    public static EmbedlyApiException Forbidden(string? resource = null)
    {
        var message = resource != null 
            ? $"Access to resource '{resource}' is forbidden."
            : "Access to the requested resource is forbidden.";
            
        return new EmbedlyApiException(
            message,
            HttpStatusCode.Forbidden,
            errorCode: "FORBIDDEN");
    }
    
    /// <summary>
    /// Creates an exception for not found resources.
    /// </summary>
    public static EmbedlyApiException NotFound(string resourceType, string resourceId)
    {
        return new EmbedlyApiException(
            $"{resourceType} with ID '{resourceId}' was not found.",
            HttpStatusCode.NotFound,
            errorCode: "NOT_FOUND");
    }
}