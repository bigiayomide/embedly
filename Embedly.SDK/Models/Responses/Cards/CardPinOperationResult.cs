using System;
using System.Text.Json.Serialization;

namespace Embedly.SDK.Models.Responses.Cards;

/// <summary>
/// Represents the result of a card PIN operation (reset, change, or verification).
/// </summary>
public sealed class CardPinOperationResult
{
    /// <summary>
    /// Gets or sets the operation ID.
    /// </summary>
    [JsonPropertyName("operationId")]
    public string OperationId { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets whether the operation was successful.
    /// </summary>
    [JsonPropertyName("success")]
    public bool Success { get; set; }
    
    /// <summary>
    /// Gets or sets the operation status.
    /// </summary>
    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the operation message.
    /// </summary>
    [JsonPropertyName("message")]
    public string? Message { get; set; }
    
    /// <summary>
    /// Gets or sets the card number (masked).
    /// </summary>
    [JsonPropertyName("maskedCardNumber")]
    public string? MaskedCardNumber { get; set; }
    
    /// <summary>
    /// Gets or sets the timestamp of the operation.
    /// </summary>
    [JsonPropertyName("timestamp")]
    public DateTime Timestamp { get; set; }
}