using System;
using System.Text.Json.Serialization;

namespace Embedly.SDK.Models.Responses.Wallets;

/// <summary>
///     Represents a wallet transaction as returned by the API.
/// </summary>
public sealed class WalletTransaction
{
    /// <summary>
    ///     Gets or sets the unique transaction identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the wallet ID.
    /// </summary>
    [JsonPropertyName("walletId")]
    public string WalletId { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the product ID.
    /// </summary>
    [JsonPropertyName("productId")]
    public string? ProductId { get; set; }

    /// <summary>
    ///     Gets or sets the transaction remarks.
    /// </summary>
    [JsonPropertyName("remarks")]
    public string? Remarks { get; set; }

    /// <summary>
    ///     Gets or sets the transaction amount.
    /// </summary>
    [JsonPropertyName("amount")]
    public decimal Amount { get; set; }

    /// <summary>
    ///     Gets or sets the debit/credit indicator ("D" for debit, "C" for credit).
    /// </summary>
    [JsonPropertyName("debitCreditIndicator")]
    public string? DebitCreditIndicator { get; set; }

    /// <summary>
    ///     Gets or sets the balance after this transaction.
    /// </summary>
    [JsonPropertyName("balance")]
    public decimal Balance { get; set; }

    /// <summary>
    ///     Gets or sets the transaction reference.
    /// </summary>
    [JsonPropertyName("transactionReference")]
    public string? TransactionReference { get; set; }

    /// <summary>
    ///     Gets or sets the transaction ID.
    /// </summary>
    [JsonPropertyName("transactionId")]
    public string? TransactionId { get; set; }

    /// <summary>
    ///     Gets or sets whether this transaction is active.
    /// </summary>
    [JsonPropertyName("isActive")]
    public bool IsActive { get; set; }

    /// <summary>
    ///     Gets or sets the date when the transaction was created.
    /// </summary>
    [JsonPropertyName("dateCreated")]
    public DateTime? DateCreated { get; set; }

    /// <summary>
    ///     Gets or sets the mobile number associated with the transaction.
    /// </summary>
    [JsonPropertyName("mobileNumber")]
    public string? MobileNumber { get; set; }

    /// <summary>
    ///     Gets or sets the account number associated with the transaction.
    /// </summary>
    [JsonPropertyName("accountNumber")]
    public string? AccountNumber { get; set; }

    /// <summary>
    ///     Gets or sets the name associated with the transaction.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }
}

/// <summary>
///     Transaction type enumeration.
/// </summary>
public enum TransactionType
{
    /// <summary>
    ///     Wallet-to-wallet transfer.
    /// </summary>
    [JsonPropertyName("transfer")] Transfer,

    /// <summary>
    ///     Deposit into wallet.
    /// </summary>
    [JsonPropertyName("deposit")] Deposit,

    /// <summary>
    ///     Withdrawal from wallet.
    /// </summary>
    [JsonPropertyName("withdrawal")] Withdrawal,

    /// <summary>
    ///     Payment from wallet.
    /// </summary>
    [JsonPropertyName("payment")] Payment,

    /// <summary>
    ///     Refund to wallet.
    /// </summary>
    [JsonPropertyName("refund")] Refund,

    /// <summary>
    ///     Fee charge.
    /// </summary>
    [JsonPropertyName("fee")] Fee,

    /// <summary>
    ///     Reversal transaction.
    /// </summary>
    [JsonPropertyName("reversal")] Reversal
}

/// <summary>
///     Transaction direction enumeration.
/// </summary>
public enum TransactionDirection
{
    /// <summary>
    ///     Money leaving the wallet (debit).
    /// </summary>
    [JsonPropertyName("debit")] Debit,

    /// <summary>
    ///     Money entering the wallet (credit).
    /// </summary>
    [JsonPropertyName("credit")] Credit
}

/// <summary>
///     Transaction status enumeration.
/// </summary>
public enum TransactionStatus
{
    /// <summary>
    ///     Transaction is pending.
    /// </summary>
    [JsonPropertyName("pending")] Pending,

    /// <summary>
    ///     Transaction completed successfully.
    /// </summary>
    [JsonPropertyName("completed")] Completed,

    /// <summary>
    ///     Transaction failed.
    /// </summary>
    [JsonPropertyName("failed")] Failed,

    /// <summary>
    ///     Transaction was reversed.
    /// </summary>
    [JsonPropertyName("reversed")] Reversed,

    /// <summary>
    ///     Transaction was cancelled.
    /// </summary>
    [JsonPropertyName("cancelled")] Cancelled
}
