using Embedly.SDK.Webhooks;
using Microsoft.Extensions.Logging;

namespace Embedly.Examples.Infrastructure.Services;

/// <summary>
/// Example webhook handler using the Embedly SDK's built-in webhook system.
/// Developers only need to inherit from WebhookHandler and register their event handlers.
/// </summary>
public class EmbedlyWebhookHandler : WebhookHandler
{
    private readonly ILogger<EmbedlyWebhookHandler> _logger;

    public EmbedlyWebhookHandler(ILogger<EmbedlyWebhookHandler> logger) : base(logger)
    {
        _logger = logger;
    }

    protected override void RegisterHandlers()
    {
        // Customer events
        RegisterHandler(WebhookEventTypes.CustomerCreated, HandleCustomerCreatedAsync);
        RegisterHandler(WebhookEventTypes.CustomerUpdated, HandleCustomerUpdatedAsync);
        RegisterHandler(WebhookEventTypes.CustomerVerified, HandleCustomerVerifiedAsync);

        // Wallet events
        RegisterHandler(WebhookEventTypes.WalletCreated, HandleWalletCreatedAsync);
        RegisterHandler(WebhookEventTypes.WalletActivated, HandleWalletActivatedAsync);

        // Transaction events
        RegisterHandler(WebhookEventTypes.TransactionCreated, HandleTransactionCreatedAsync);
        RegisterHandler(WebhookEventTypes.TransactionCompleted, HandleTransactionCompletedAsync);
        RegisterHandler(WebhookEventTypes.TransactionFailed, HandleTransactionFailedAsync);

        // Payment events
        RegisterHandler(WebhookEventTypes.PaymentInitiated, HandlePaymentInitiatedAsync);
        RegisterHandler(WebhookEventTypes.PaymentCompleted, HandlePaymentCompletedAsync);
        RegisterHandler(WebhookEventTypes.PaymentFailed, HandlePaymentFailedAsync);

        // Card events
        RegisterHandler(WebhookEventTypes.CardCreated, HandleCardCreatedAsync);
        RegisterHandler(WebhookEventTypes.CardActivated, HandleCardActivatedAsync);
        RegisterHandler(WebhookEventTypes.CardBlocked, HandleCardBlockedAsync);

        // KYC events
        RegisterHandler(WebhookEventTypes.KycSubmitted, HandleKycSubmittedAsync);
        RegisterHandler(WebhookEventTypes.KycApproved, HandleKycApprovedAsync);
        RegisterHandler(WebhookEventTypes.KycRejected, HandleKycRejectedAsync);

        // Payout events
        RegisterHandler(WebhookEventTypes.PayoutInitiated, HandlePayoutInitiatedAsync);
        RegisterHandler(WebhookEventTypes.PayoutCompleted, HandlePayoutCompletedAsync);
        RegisterHandler(WebhookEventTypes.PayoutFailed, HandlePayoutFailedAsync);
    }

    // Customer Event Handlers
    private async Task HandleCustomerCreatedAsync(WebhookEvent webhookEvent, CancellationToken cancellationToken)
    {
        var customerData = webhookEvent.GetData<CustomerCreatedData>();

        _logger.LogInformation("Customer created: {CustomerId}, Email: {Email}",
            customerData?.CustomerId, customerData?.Email);

        // Add your business logic here:
        // - Send welcome email
        // - Update your database
        // - Trigger other business processes
        // - Update analytics/metrics

        await Task.CompletedTask;
    }

    private async Task HandleCustomerUpdatedAsync(WebhookEvent webhookEvent, CancellationToken cancellationToken)
    {
        var customerData = webhookEvent.GetData<CustomerUpdatedData>();

        _logger.LogInformation("Customer updated: {CustomerId}", customerData?.CustomerId);

        // Add your business logic here:
        // - Update customer data in your system
        // - Sync with CRM systems
        // - Trigger verification workflows

        await Task.CompletedTask;
    }

    private async Task HandleCustomerVerifiedAsync(WebhookEvent webhookEvent, CancellationToken cancellationToken)
    {
        var customerData = webhookEvent.GetData<CustomerVerifiedData>();

        _logger.LogInformation("Customer verified: {CustomerId}, Level: {VerificationLevel}",
            customerData?.CustomerId, customerData?.VerificationLevel);

        // Add your business logic here:
        // - Unlock features based on verification level
        // - Send verification success notification
        // - Update account limits

        await Task.CompletedTask;
    }

    // Wallet Event Handlers
    private async Task HandleWalletCreatedAsync(WebhookEvent webhookEvent, CancellationToken cancellationToken)
    {
        var walletData = webhookEvent.GetData<WalletCreatedData>();

        _logger.LogInformation("Wallet created: {WalletId}, Customer: {CustomerId}",
            walletData?.WalletId, walletData?.CustomerId);

        // Add your business logic here:
        // - Set up wallet in your system
        // - Configure initial settings
        // - Send wallet setup confirmation

        await Task.CompletedTask;
    }

    private async Task HandleWalletActivatedAsync(WebhookEvent webhookEvent, CancellationToken cancellationToken)
    {
        var walletData = webhookEvent.GetData<WalletActivatedData>();

        _logger.LogInformation("Wallet activated: {WalletId}", walletData?.WalletId);

        // Add your business logic here:
        // - Enable wallet features
        // - Send activation notification
        // - Update UI status

        await Task.CompletedTask;
    }

    // Transaction Event Handlers
    private async Task HandleTransactionCreatedAsync(WebhookEvent webhookEvent, CancellationToken cancellationToken)
    {
        var transactionData = webhookEvent.GetData<TransactionCreatedData>();

        _logger.LogInformation("Transaction created: {TransactionId}, Amount: {Amount}",
            transactionData?.TransactionId, transactionData?.Amount);

        // Add your business logic here:
        // - Track transaction in your system
        // - Update order status
        // - Send transaction receipt

        await Task.CompletedTask;
    }

    private async Task HandleTransactionCompletedAsync(WebhookEvent webhookEvent, CancellationToken cancellationToken)
    {
        var transactionData = webhookEvent.GetData<TransactionCompletedData>();

        _logger.LogInformation("Transaction completed: {TransactionId}, Status: {Status}",
            transactionData?.TransactionId, transactionData?.Status);

        // Add your business logic here:
        // - Mark order as paid
        // - Release goods/services
        // - Update accounting system
        // - Send completion notification

        await Task.CompletedTask;
    }

    private async Task HandleTransactionFailedAsync(WebhookEvent webhookEvent, CancellationToken cancellationToken)
    {
        var transactionData = webhookEvent.GetData<TransactionFailedData>();

        _logger.LogInformation("Transaction failed: {TransactionId}, Reason: {FailureReason}",
            transactionData?.TransactionId, transactionData?.FailureReason);

        // Add your business logic here:
        // - Handle failed payment
        // - Retry payment if appropriate
        // - Send failure notification
        // - Update order status

        await Task.CompletedTask;
    }

    // Payment Event Handlers
    private async Task HandlePaymentInitiatedAsync(WebhookEvent webhookEvent, CancellationToken cancellationToken)
    {
        var paymentData = webhookEvent.GetData<PaymentInitiatedData>();

        _logger.LogInformation("Payment initiated: {PaymentId}, Amount: {Amount}",
            paymentData?.PaymentId, paymentData?.Amount);

        await Task.CompletedTask;
    }

    private async Task HandlePaymentCompletedAsync(WebhookEvent webhookEvent, CancellationToken cancellationToken)
    {
        var paymentData = webhookEvent.GetData<PaymentCompletedData>();

        _logger.LogInformation("Payment completed: {PaymentId}", paymentData?.PaymentId);

        await Task.CompletedTask;
    }

    private async Task HandlePaymentFailedAsync(WebhookEvent webhookEvent, CancellationToken cancellationToken)
    {
        var paymentData = webhookEvent.GetData<PaymentFailedData>();

        _logger.LogInformation("Payment failed: {PaymentId}, Reason: {Reason}",
            paymentData?.PaymentId, paymentData?.FailureReason);

        await Task.CompletedTask;
    }

    // Card Event Handlers
    private async Task HandleCardCreatedAsync(WebhookEvent webhookEvent, CancellationToken cancellationToken)
    {
        var cardData = webhookEvent.GetData<CardCreatedData>();

        _logger.LogInformation("Card created: {CardId}, Customer: {CustomerId}",
            cardData?.CardId, cardData?.CustomerId);

        await Task.CompletedTask;
    }

    private async Task HandleCardActivatedAsync(WebhookEvent webhookEvent, CancellationToken cancellationToken)
    {
        var cardData = webhookEvent.GetData<CardActivatedData>();

        _logger.LogInformation("Card activated: {CardId}", cardData?.CardId);

        await Task.CompletedTask;
    }

    private async Task HandleCardBlockedAsync(WebhookEvent webhookEvent, CancellationToken cancellationToken)
    {
        var cardData = webhookEvent.GetData<CardBlockedData>();

        _logger.LogInformation("Card blocked: {CardId}, Reason: {Reason}",
            cardData?.CardId, cardData?.Reason);

        await Task.CompletedTask;
    }

    // KYC Event Handlers
    private async Task HandleKycSubmittedAsync(WebhookEvent webhookEvent, CancellationToken cancellationToken)
    {
        var kycData = webhookEvent.GetData<KycSubmittedData>();

        _logger.LogInformation("KYC submitted: {CustomerId}, Type: {DocumentType}",
            kycData?.CustomerId, kycData?.DocumentType);

        await Task.CompletedTask;
    }

    private async Task HandleKycApprovedAsync(WebhookEvent webhookEvent, CancellationToken cancellationToken)
    {
        var kycData = webhookEvent.GetData<KycApprovedData>();

        _logger.LogInformation("KYC approved: {CustomerId}, Level: {Level}",
            kycData?.CustomerId, kycData?.ApprovedLevel);

        await Task.CompletedTask;
    }

    private async Task HandleKycRejectedAsync(WebhookEvent webhookEvent, CancellationToken cancellationToken)
    {
        var kycData = webhookEvent.GetData<KycRejectedData>();

        _logger.LogInformation("KYC rejected: {CustomerId}, Reason: {Reason}",
            kycData?.CustomerId, kycData?.RejectionReason);

        await Task.CompletedTask;
    }

    // Payout Event Handlers
    private async Task HandlePayoutInitiatedAsync(WebhookEvent webhookEvent, CancellationToken cancellationToken)
    {
        var payoutData = webhookEvent.GetData<PayoutInitiatedData>();

        _logger.LogInformation("Payout initiated: {PayoutId}, Amount: {Amount}",
            payoutData?.PayoutId, payoutData?.Amount);

        await Task.CompletedTask;
    }

    private async Task HandlePayoutCompletedAsync(WebhookEvent webhookEvent, CancellationToken cancellationToken)
    {
        var payoutData = webhookEvent.GetData<PayoutCompletedData>();

        _logger.LogInformation("Payout completed: {PayoutId}", payoutData?.PayoutId);

        await Task.CompletedTask;
    }

    private async Task HandlePayoutFailedAsync(WebhookEvent webhookEvent, CancellationToken cancellationToken)
    {
        var payoutData = webhookEvent.GetData<PayoutFailedData>();

        _logger.LogInformation("Payout failed: {PayoutId}, Reason: {Reason}",
            payoutData?.PayoutId, payoutData?.FailureReason);

        await Task.CompletedTask;
    }
}

// Example data models for webhook events
// In a real application, these would be defined based on actual Embedly webhook payloads

public class CustomerCreatedData
{
    public string? CustomerId { get; set; }
    public string? Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}

public class CustomerUpdatedData
{
    public string? CustomerId { get; set; }
    public string? Email { get; set; }
}

public class CustomerVerifiedData
{
    public string? CustomerId { get; set; }
    public string? VerificationLevel { get; set; }
}

public class WalletCreatedData
{
    public string? WalletId { get; set; }
    public string? CustomerId { get; set; }
    public string? Currency { get; set; }
}

public class WalletActivatedData
{
    public string? WalletId { get; set; }
}

public class TransactionCreatedData
{
    public string? TransactionId { get; set; }
    public decimal Amount { get; set; }
    public string? Currency { get; set; }
}

public class TransactionCompletedData
{
    public string? TransactionId { get; set; }
    public string? Status { get; set; }
}

public class TransactionFailedData
{
    public string? TransactionId { get; set; }
    public string? FailureReason { get; set; }
}

public class PaymentInitiatedData
{
    public string? PaymentId { get; set; }
    public decimal Amount { get; set; }
}

public class PaymentCompletedData
{
    public string? PaymentId { get; set; }
}

public class PaymentFailedData
{
    public string? PaymentId { get; set; }
    public string? FailureReason { get; set; }
}

public class CardCreatedData
{
    public string? CardId { get; set; }
    public string? CustomerId { get; set; }
}

public class CardActivatedData
{
    public string? CardId { get; set; }
}

public class CardBlockedData
{
    public string? CardId { get; set; }
    public string? Reason { get; set; }
}

public class KycSubmittedData
{
    public string? CustomerId { get; set; }
    public string? DocumentType { get; set; }
}

public class KycApprovedData
{
    public string? CustomerId { get; set; }
    public string? ApprovedLevel { get; set; }
}

public class KycRejectedData
{
    public string? CustomerId { get; set; }
    public string? RejectionReason { get; set; }
}

public class PayoutInitiatedData
{
    public string? PayoutId { get; set; }
    public decimal Amount { get; set; }
}

public class PayoutCompletedData
{
    public string? PayoutId { get; set; }
}

public class PayoutFailedData
{
    public string? PayoutId { get; set; }
    public string? FailureReason { get; set; }
}