using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Embedly.SDK.Webhooks;

/// <summary>
/// Represents a webhook event from Embedly.
/// </summary>
public sealed class WebhookEvent
{
    /// <summary>
    /// Gets or sets the event ID.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the event type.
    /// </summary>
    [JsonPropertyName("event")]
    public string Event { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the event timestamp.
    /// </summary>
    [JsonPropertyName("timestamp")]
    public DateTimeOffset Timestamp { get; set; }
    
    /// <summary>
    /// Gets or sets the event data as a JSON element.
    /// </summary>
    [JsonPropertyName("data")]
    public JsonElement Data { get; set; }
    
    /// <summary>
    /// Gets or sets additional event metadata.
    /// </summary>
    [JsonPropertyName("metadata")]
    public Dictionary<string, object?>? Metadata { get; set; }
    
    /// <summary>
    /// Deserializes the event data to a specific type.
    /// </summary>
    /// <typeparam name="T">The type to deserialize to.</typeparam>
    /// <returns>The deserialized data.</returns>
    public T? GetData<T>() where T : class
    {
        if (Data.ValueKind == JsonValueKind.Null || Data.ValueKind == JsonValueKind.Undefined)
            return null;
            
        return JsonSerializer.Deserialize<T>(Data.GetRawText(), new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
    }
}

/// <summary>
/// Common webhook event types.
/// </summary>
public static class WebhookEventTypes
{
    // Customer events
    /// <summary>
    /// Fired when a customer is created.
    /// </summary>
    public const string CustomerCreated = "customer.created";
    /// <summary>
    /// Fired when a customer is updated.
    /// </summary>
    public const string CustomerUpdated = "customer.updated";
    /// <summary>
    /// Fired when a customer is deleted.
    /// </summary>
    public const string CustomerDeleted = "customer.deleted";
    /// <summary>
    /// Fired when a customer is verified.
    /// </summary>
    public const string CustomerVerified = "customer.verified";
    
    // Wallet events
    /// <summary>
    /// Fired when a wallet is created.
    /// </summary>
    public const string WalletCreated = "wallet.created";
    /// <summary>
    /// Fired when a wallet is activated.
    /// </summary>
    public const string WalletActivated = "wallet.activated";
    /// <summary>
    /// Fired when a wallet is deactivated.
    /// </summary>
    public const string WalletDeactivated = "wallet.deactivated";
    /// <summary>
    /// Fired when a wallet is suspended.
    /// </summary>
    public const string WalletSuspended = "wallet.suspended";
    /// <summary>
    /// Fired when a wallet is closed.
    /// </summary>
    public const string WalletClosed = "wallet.closed";
    
    // Transaction events
    /// <summary>
    /// Fired when a transaction is created.
    /// </summary>
    public const string TransactionCreated = "transaction.created";
    /// <summary>
    /// Fired when a transaction is completed.
    /// </summary>
    public const string TransactionCompleted = "transaction.completed";
    /// <summary>
    /// Fired when a transaction fails.
    /// </summary>
    public const string TransactionFailed = "transaction.failed";
    /// <summary>
    /// Fired when a transaction is reversed.
    /// </summary>
    public const string TransactionReversed = "transaction.reversed";
    
    // Transfer events
    /// <summary>
    /// Fired when a transfer is initiated.
    /// </summary>
    public const string TransferInitiated = "transfer.initiated";
    /// <summary>
    /// Fired when a transfer is completed.
    /// </summary>
    public const string TransferCompleted = "transfer.completed";
    /// <summary>
    /// Fired when a transfer fails.
    /// </summary>
    public const string TransferFailed = "transfer.failed";
    
    // Payment events
    /// <summary>
    /// Fired when a payment is initiated.
    /// </summary>
    public const string PaymentInitiated = "payment.initiated";
    /// <summary>
    /// Fired when a payment is completed.
    /// </summary>
    public const string PaymentCompleted = "payment.completed";
    /// <summary>
    /// Fired when a payment fails.
    /// </summary>
    public const string PaymentFailed = "payment.failed";
    /// <summary>
    /// Fired when a payment is refunded.
    /// </summary>
    public const string PaymentRefunded = "payment.refunded";
    
    // Checkout events
    /// <summary>
    /// Fired when a checkout is created.
    /// </summary>
    public const string CheckoutCreated = "checkout.created";
    /// <summary>
    /// Fired when a checkout is completed.
    /// </summary>
    public const string CheckoutCompleted = "checkout.completed";
    /// <summary>
    /// Fired when a checkout expires.
    /// </summary>
    public const string CheckoutExpired = "checkout.expired";
    /// <summary>
    /// Fired when a checkout is cancelled.
    /// </summary>
    public const string CheckoutCancelled = "checkout.cancelled";
    
    // Card events
    /// <summary>
    /// Fired when a card is created.
    /// </summary>
    public const string CardCreated = "card.created";
    /// <summary>
    /// Fired when a card is activated.
    /// </summary>
    public const string CardActivated = "card.activated";
    /// <summary>
    /// Fired when a card is blocked.
    /// </summary>
    public const string CardBlocked = "card.blocked";
    /// <summary>
    /// Fired when a card expires.
    /// </summary>
    public const string CardExpired = "card.expired";
    
    // KYC events
    /// <summary>
    /// Fired when KYC documentation is submitted.
    /// </summary>
    public const string KycSubmitted = "kyc.submitted";
    /// <summary>
    /// Fired when KYC is approved.
    /// </summary>
    public const string KycApproved = "kyc.approved";
    /// <summary>
    /// Fired when KYC is rejected.
    /// </summary>
    public const string KycRejected = "kyc.rejected";
    /// <summary>
    /// Fired when KYC is pending review.
    /// </summary>
    public const string KycPending = "kyc.pending";
    
    // Product Limit events
    /// <summary>
    /// Fired when a product limit is created.
    /// </summary>
    public const string ProductLimitCreated = "product_limit.created";
    /// <summary>
    /// Fired when a product limit is updated.
    /// </summary>
    public const string ProductLimitUpdated = "product_limit.updated";
    /// <summary>
    /// Fired when a product limit is activated.
    /// </summary>
    public const string ProductLimitActivated = "product_limit.activated";
    /// <summary>
    /// Fired when a product limit is deactivated.
    /// </summary>
    public const string ProductLimitDeactivated = "product_limit.deactivated";
    /// <summary>
    /// Fired when a product limit is exceeded.
    /// </summary>
    public const string ProductLimitExceeded = "product_limit.exceeded";
    /// <summary>
    /// Fired when a product limit is reset.
    /// </summary>
    public const string ProductLimitReset = "product_limit.reset";
    
    // Payout events
    /// <summary>
    /// Fired when a payout is initiated.
    /// </summary>
    public const string PayoutInitiated = "payout.initiated";
    /// <summary>
    /// Fired when a payout is completed.
    /// </summary>
    public const string PayoutCompleted = "payout.completed";
    /// <summary>
    /// Fired when a payout fails.
    /// </summary>
    public const string PayoutFailed = "payout.failed";
    /// <summary>
    /// Fired when a payout is reversed.
    /// </summary>
    public const string PayoutReversed = "payout.reversed";
}