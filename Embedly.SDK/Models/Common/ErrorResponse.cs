using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace Embedly.SDK.Models.Common;

/// <summary>
///     Strongly typed error response from the API.
/// </summary>
public sealed class ErrorResponse
{
    /// <summary>
    ///     Gets or sets the error message.
    /// </summary>
    [JsonPropertyName("message")]
    public string? Message { get; set; }

    /// <summary>
    ///     Gets or sets the error field (handles both "error" and "Error" via case-insensitive deserialization).
    /// </summary>
    [JsonPropertyName("error")]
    public string? Error { get; set; }

    /// <summary>
    ///     Gets or sets the error code.
    /// </summary>
    [JsonPropertyName("code")]
    public string? Code { get; set; }

    /// <summary>
    ///     Gets or sets validation errors.
    /// </summary>
    [JsonPropertyName("errors")]
    public Dictionary<string, string[]>? Errors { get; set; }

    /// <summary>
    ///     Gets the first available error message in order of preference.
    ///     Includes validation errors from the <see cref="Errors"/> dictionary when present.
    /// </summary>
    public string? GetErrorMessage()
    {
        var baseMessage = Message ?? Error;

        if (Errors is not { Count: > 0 })
            return baseMessage;

        var validationDetails = string.Join("; ",
            Errors.Select(e => $"{e.Key}: {string.Join(", ", e.Value)}"));

        return string.IsNullOrEmpty(baseMessage)
            ? validationDetails
            : $"{baseMessage} — {validationDetails}";
    }
}