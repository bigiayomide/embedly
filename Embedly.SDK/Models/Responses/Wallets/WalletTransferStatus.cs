using System;
using System.Text.Json.Serialization;

namespace Embedly.SDK.Models.Responses.Wallets;

/// <summary>
///     Status of a wallet-to-wallet transfer.
/// </summary>
public sealed class WalletTransferStatus
{
    /// <summary>
    ///     Gets or sets the transaction reference.
    /// </summary>
    [JsonPropertyName("reference")]
    public string? Reference { get; set; }

    /// <summary>
    ///     Gets or sets the transfer status (e.g. "Success", "Failed").
    /// </summary>
    [JsonPropertyName("status")]
    public string? Status { get; set; }

    /// <summary>
    ///     Gets or sets the timestamp of the transaction.
    /// </summary>
    [JsonPropertyName("timestamp")]
    public DateTime? Timestamp { get; set; }
}