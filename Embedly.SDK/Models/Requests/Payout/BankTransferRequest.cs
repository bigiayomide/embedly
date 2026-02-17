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
    [Required(ErrorMessage = "Destination bank code is required")]
    [JsonPropertyName("destinationBankCode")]
    public string DestinationBankCode { get; set; } = string.Empty;

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
    [Required(ErrorMessage = "Destination account name is required")]
    [JsonPropertyName("destinationAccountName")]
    public string DestinationAccountName { get; set; } = string.Empty;

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
    [Required(ErrorMessage = "Source account name is required")]
    [JsonPropertyName("sourceAccountName")]
    public string SourceAccountName { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the transfer remarks.
    /// </summary>
    [Required(ErrorMessage = "Remarks is required")]
    [JsonPropertyName("remarks")]
    public string Remarks { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the amount to transfer (in smallest currency unit, e.g., kobo).
    /// </summary>
    [Required(ErrorMessage = "Amount is required")]
    [JsonPropertyName("amount")]
    public int Amount { get; set; }

    /// <summary>
    ///     Gets or sets the currency ID.
    /// </summary>
    [Required(ErrorMessage = "Currency ID is required")]
    [JsonPropertyName("currencyId")]
    public Guid CurrencyId { get; set; }

    /// <summary>
    ///     Gets or sets the customer transaction reference (optional).
    /// </summary>
    [JsonPropertyName("customerTransactionReference")]
    public string? CustomerTransactionReference { get; set; }

    /// <summary>
    ///     Gets or sets the staging status for testing. Only used in staging environment.
    ///     Valid values: "success", "failed".
    /// </summary>
    [JsonPropertyName("stagingStatus")]
    public string? StagingStatus { get; set; }
}
