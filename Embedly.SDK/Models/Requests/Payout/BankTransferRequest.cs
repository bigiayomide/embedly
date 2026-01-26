using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Embedly.SDK.Models.Requests.Payout;

/// <summary>
///     Request model for inter-bank transfers based on InitiatePayout schema.
/// </summary>
public sealed class BankTransferRequest
{
    /// <summary>
    ///     Gets or sets the destination bank code.
    /// </summary>
    [JsonPropertyName("destinationBankCode")]
    public string? DestinationBankCode { get; set; }

    /// <summary>
    ///     Gets or sets the destination account number.
    /// </summary>
    [Required(ErrorMessage = "Destination account number is required")]
    [StringLength(10, MinimumLength = 10, ErrorMessage = "Account number must be exactly 10 digits")]
    [JsonPropertyName("destinationAccountNumber")]
    public string DestinationAccountNumber { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the destination account name.
    /// </summary>
    [JsonPropertyName("destinationAccountName")]
    public string? DestinationAccountName { get; set; }

    /// <summary>
    ///     Gets or sets the source account number.
    /// </summary>
    [Required(ErrorMessage = "Source account number is required")]
    [StringLength(10, MinimumLength = 10, ErrorMessage = "Account number must be exactly 10 digits")]
    [JsonPropertyName("sourceAccountNumber")]
    public string SourceAccountNumber { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the source account name.
    /// </summary>
    [JsonPropertyName("sourceAccountName")]
    public string? SourceAccountName { get; set; }

    /// <summary>
    ///     Gets or sets the transfer remarks.
    /// </summary>
    [JsonPropertyName("remarks")]
    public string? Remarks { get; set; }

    /// <summary>
    ///     Gets or sets the customer transaction reference.
    /// </summary>
    [JsonPropertyName("customerTransactionReference")]
    public string? CustomerTransactionReference { get; set; }

    /// <summary>
    ///     Gets or sets the amount to transfer.
    /// </summary>
    [Required(ErrorMessage = "Amount is required")]
    [Range(typeof(decimal), "1.5", "79228162514264337593543950335", ErrorMessage = "Amount must be at least 1.5")]
    [JsonPropertyName("amount")]
    public decimal Amount { get; set; }

    /// <summary>
    ///     Gets or sets the currency ID.
    /// </summary>
    [JsonPropertyName("currencyId")]
    public Guid? CurrencyId { get; set; }

    /// <summary>
    ///     Gets or sets the webhook URL.
    /// </summary>
    [JsonPropertyName("webhookUrl")]
    public string? WebhookUrl { get; set; }
}