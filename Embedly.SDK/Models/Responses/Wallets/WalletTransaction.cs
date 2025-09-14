using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Embedly.SDK.Models.Common;

namespace Embedly.SDK.Models.Responses.Wallets;

/// <summary>
///     Represents a wallet transaction.
/// </summary>
public sealed class WalletTransaction
{
    /// <summary>
    ///     Gets or sets the unique transaction identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the transaction reference.
    /// </summary>
    [JsonPropertyName("reference")]
    public string Reference { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the wallet ID.
    /// </summary>
    [JsonPropertyName("walletId")]
    public string WalletId { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the transaction amount in the smallest currency unit.
    /// </summary>
    [JsonPropertyName("amount")]
    public long Amount { get; set; }

    /// <summary>
    ///     Gets or sets the currency code.
    /// </summary>
    [JsonPropertyName("currency")]
    public string Currency { get; set; } = "NGN";

    /// <summary>
    ///     Gets or sets the transaction type.
    /// </summary>
    [JsonPropertyName("transactionType")]
    public TransactionType TransactionType { get; set; }

    /// <summary>
    ///     Gets or sets the transaction direction (debit/credit).
    /// </summary>
    [JsonPropertyName("direction")]
    public TransactionDirection Direction { get; set; }

    /// <summary>
    ///     Gets or sets the transaction status.
    /// </summary>
    [JsonPropertyName("status")]
    public TransactionStatus Status { get; set; }

    /// <summary>
    ///     Gets or sets the transaction description.
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    /// <summary>
    ///     Gets or sets the counterpart wallet ID (for transfers).
    /// </summary>
    [JsonPropertyName("counterpartWalletId")]
    public string? CounterpartWalletId { get; set; }

    /// <summary>
    ///     Gets or sets the balance before this transaction.
    /// </summary>
    [JsonPropertyName("balanceBefore")]
    public long BalanceBefore { get; set; }

    /// <summary>
    ///     Gets or sets the balance after this transaction.
    /// </summary>
    [JsonPropertyName("balanceAfter")]
    public long BalanceAfter { get; set; }

    /// <summary>
    ///     Gets or sets the date when the transaction was created.
    /// </summary>
    [JsonPropertyName("createdAt")]
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    ///     Gets or sets the date when the transaction was processed.
    /// </summary>
    [JsonPropertyName("processedAt")]
    public DateTimeOffset? ProcessedAt { get; set; }

    /// <summary>
    ///     Gets or sets additional metadata for the transaction.
    /// </summary>
    [JsonPropertyName("metadata")]
    public Dictionary<string, object?>? Metadata { get; set; }

    /// <summary>
    ///     Gets the transaction amount as a Money object.
    /// </summary>
    public Money GetAmount()
    {
        return new Money(Amount, Currency);
    }

    /// <summary>
    ///     Gets the balance before as a Money object.
    /// </summary>
    public Money GetBalanceBefore()
    {
        return new Money(BalanceBefore, Currency);
    }

    /// <summary>
    ///     Gets the balance after as a Money object.
    /// </summary>
    public Money GetBalanceAfter()
    {
        return new Money(BalanceAfter, Currency);
    }
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