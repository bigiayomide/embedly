using System;
using System.Text.Json.Serialization;

namespace Embedly.SDK.Models.Responses.Wallets;

/// <summary>
/// Response model for wallet closure operation.
/// </summary>
public sealed record WalletCloseResponse
{
    /// <summary>
    /// Gets or sets whether the closure was successful.
    /// </summary>
    [JsonPropertyName("success")]
    public bool Success { get; init; }
    
    /// <summary>
    /// Gets or sets the closure message.
    /// </summary>
    [JsonPropertyName("message")]
    public string Message { get; init; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the account number that was closed.
    /// </summary>
    [JsonPropertyName("accountNumber")]
    public string AccountNumber { get; init; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the closure reason.
    /// </summary>
    [JsonPropertyName("reason")]
    public string Reason { get; init; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the closure timestamp.
    /// </summary>
    [JsonPropertyName("closedAt")]
    public DateTimeOffset ClosedAt { get; init; }
}