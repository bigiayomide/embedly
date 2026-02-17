using System.Text.Json.Serialization;

namespace Embedly.SDK.Models.Responses.Cards;

/// <summary>
///     Response from simulating a debit card transaction (staging environment only).
/// </summary>
public sealed class SimulateDebitCardTransactionResponse
{
    /// <summary>
    ///     Gets or sets whether the simulation was successful.
    /// </summary>
    [JsonPropertyName("isSuccessful")]
    public bool IsSuccessful { get; set; }

    /// <summary>
    ///     Gets or sets the response code.
    /// </summary>
    [JsonPropertyName("responseCode")]
    public string? ResponseCode { get; set; }

    /// <summary>
    ///     Gets or sets the response message.
    /// </summary>
    [JsonPropertyName("responseMessage")]
    public string? ResponseMessage { get; set; }
}
