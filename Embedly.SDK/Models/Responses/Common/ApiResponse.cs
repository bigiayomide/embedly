using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Embedly.SDK.Models.Responses.Common;

/// <summary>
///     Base response wrapper for Embedly API responses.
/// </summary>
/// <typeparam name="T">The type of the response data.</typeparam>
public sealed class ApiResponse<T>
{
    private bool? _success;

    /// <summary>
    ///     Gets or sets a value indicating whether the request was successful.
    ///     Maps to "success" (WaaS API) — also set by <see cref="Succeeded"/> for Payout API compatibility.
    ///     When neither is present (e.g. Checkout API), falls back to a 2xx <see cref="StatusCode"/>.
    /// </summary>
    [JsonPropertyName("success")]
    public bool Success
    {
        get => _success ?? StatusCode is >= 200 and < 300;
        set => _success = value;
    }

    /// <summary>
    ///     Payout API uses "succeeded" instead of "success". This property ensures
    ///     both response formats are handled correctly.
    /// </summary>
    [JsonPropertyName("succeeded")]
    public bool Succeeded
    {
        get => Success;
        set => Success = value;
    }

    /// <summary>
    ///     Gets or sets the response message.
    /// </summary>
    [JsonPropertyName("message")]
    public string? Message { get; set; }

    /// <summary>
    ///     Gets or sets the response data.
    /// </summary>
    [JsonPropertyName("data")]
    public T? Data { get; set; }

    /// <summary>
    ///     Gets or sets error details if the request failed.
    /// </summary>
    [JsonPropertyName("error")]
    public ErrorDetails? Error { get; set; }

    /// <summary>
    ///     Gets or sets pagination information if applicable.
    /// </summary>
    [JsonPropertyName("pagination")]
    public PaginationInfo? Pagination { get; set; }

    /// <summary>
    ///     Gets or sets the request timestamp.
    /// </summary>
    [JsonPropertyName("timestamp")]
    public DateTimeOffset? Timestamp { get; set; }

    /// <summary>
    ///     Gets or sets the request ID for tracking.
    /// </summary>
    [JsonPropertyName("requestId")]
    public string? RequestId { get; set; }

    /// <summary>
    ///     Gets or sets the HTTP status code associated with the error.
    /// </summary>
    [JsonPropertyName("statusCode")]
    public int StatusCode { get; set; }

    /// <summary>
    ///     Gets or sets the specific error code, if provided.
    /// </summary>
    [JsonPropertyName("code")]
    public string? Code { get; set; }
    
}

/// <summary>
///     Error details for failed API responses.
/// </summary>
public sealed class ErrorDetails
{
    /// <summary>
    ///     Gets or sets the error code.
    /// </summary>
    [JsonPropertyName("code")]
    public string? Code { get; set; }

    /// <summary>
    ///     Gets or sets the error message.
    /// </summary>
    [JsonPropertyName("message")]
    public string? Message { get; set; }

    /// <summary>
    ///     Gets or sets additional error details.
    /// </summary>
    [JsonPropertyName("details")]
    public Dictionary<string, object?>? Details { get; set; }

    /// <summary>
    ///     Gets or sets validation errors if applicable.
    /// </summary>
    [JsonPropertyName("validationErrors")]
    public List<ValidationError>? ValidationErrors { get; set; }
}

/// <summary>
///     Validation error information.
/// </summary>
public sealed class ValidationError
{
    /// <summary>
    ///     Gets or sets the field name that failed validation.
    /// </summary>
    [JsonPropertyName("field")]
    public string? Field { get; set; }

    /// <summary>
    ///     Gets or sets the validation error message.
    /// </summary>
    [JsonPropertyName("message")]
    public string? Message { get; set; }

    /// <summary>
    ///     Gets or sets the attempted value.
    /// </summary>
    [JsonPropertyName("attemptedValue")]
    public object? AttemptedValue { get; set; }
}

/// <summary>
///     Pagination information for paginated responses.
/// </summary>
public sealed class PaginationInfo
{
    /// <summary>
    ///     Gets or sets the current page number.
    /// </summary>
    [JsonPropertyName("page")]
    public int Page { get; set; }

    /// <summary>
    ///     Gets or sets the page size.
    /// </summary>
    [JsonPropertyName("pageSize")]
    public int PageSize { get; set; }

    /// <summary>
    ///     Gets or sets the total number of items.
    /// </summary>
    [JsonPropertyName("totalItems")]
    public long TotalItems { get; set; }

    /// <summary>
    ///     Gets or sets the total number of pages.
    /// </summary>
    [JsonPropertyName("totalPages")]
    public int TotalPages { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether there is a next page.
    /// </summary>
    [JsonPropertyName("hasNext")]
    public bool HasNext { get; set; }

    /// <summary>
    ///     Gets or sets a value indicating whether there is a previous page.
    /// </summary>
    [JsonPropertyName("hasPrevious")]
    public bool HasPrevious { get; set; }

    /// <summary>
    ///     Checkout API uses "currentPage" instead of "page".
    /// </summary>
    [JsonPropertyName("currentPage")]
    public int CurrentPage
    {
        get => Page;
        set => Page = value;
    }

    /// <summary>
    ///     Checkout API uses "totalCount" instead of "totalItems".
    /// </summary>
    [JsonPropertyName("totalCount")]
    public long TotalCount
    {
        get => TotalItems;
        set => TotalItems = value;
    }

    /// <summary>
    ///     Checkout API uses "hasNextPage" instead of "hasNext".
    /// </summary>
    [JsonPropertyName("hasNextPage")]
    public bool HasNextPage
    {
        get => HasNext;
        set => HasNext = value;
    }

    /// <summary>
    ///     Checkout API uses "hasPreviousPage" instead of "hasPrevious".
    /// </summary>
    [JsonPropertyName("hasPreviousPage")]
    public bool HasPreviousPage
    {
        get => HasPrevious;
        set => HasPrevious = value;
    }
}

/// <summary>
///     Represents a paginated response from the Embedly API.
/// </summary>
/// <typeparam name="T">The type of items in the paginated response.</typeparam>
public sealed class PaginatedResponse<T>
{
    /// <summary>
    ///     Gets or sets the items for the current page.
    /// </summary>
    [JsonPropertyName("items")]
    public List<T> Items { get; set; } = new();

    /// <summary>
    ///     Gets or sets the pagination information.
    /// </summary>
    [JsonPropertyName("pagination")]
    public PaginationInfo? Pagination { get; set; }
}