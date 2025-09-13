# Embedly SDK Webhook Integration Guide

This guide shows developers how to use the Embedly SDK's built-in webhook system instead of building custom webhook handling.

## ✅ The Right Way: Using SDK's Built-in System

### 1. Create Your Webhook Handler

Inherit from `WebhookHandler` and register your event handlers:

```csharp
using Embedly.SDK.Webhooks;

public class MyWebhookHandler : WebhookHandler
{
    private readonly ILogger<MyWebhookHandler> _logger;

    public MyWebhookHandler(ILogger<MyWebhookHandler> logger) : base(logger)
    {
        _logger = logger;
    }

    protected override void RegisterHandlers()
    {
        // Register handlers for events you care about
        RegisterHandler(WebhookEventTypes.CustomerCreated, HandleCustomerCreatedAsync);
        RegisterHandler(WebhookEventTypes.TransactionCompleted, HandleTransactionCompletedAsync);
        RegisterHandler(WebhookEventTypes.PaymentCompleted, HandlePaymentCompletedAsync);
    }

    private async Task HandleCustomerCreatedAsync(WebhookEvent webhookEvent, CancellationToken cancellationToken)
    {
        // Get strongly-typed data
        var customerData = webhookEvent.GetData<CustomerCreatedData>();

        _logger.LogInformation("New customer: {CustomerId}", customerData?.CustomerId);

        // Add your business logic:
        // - Send welcome email
        // - Update your database
        // - Trigger other processes

        await Task.CompletedTask;
    }

    private async Task HandleTransactionCompletedAsync(WebhookEvent webhookEvent, CancellationToken cancellationToken)
    {
        var transactionData = webhookEvent.GetData<TransactionCompletedData>();

        _logger.LogInformation("Transaction completed: {TransactionId}", transactionData?.TransactionId);

        // Add your business logic:
        // - Mark order as paid
        // - Release goods/services
        // - Send receipts

        await Task.CompletedTask;
    }
}
```

### 2. Register in DI Container

```csharp
// In Program.cs or Startup.cs
services.AddEmbedlyWebhooks("your-webhook-secret");
services.AddWebhookHandler<MyWebhookHandler>();
```

### 3. Simple Controller Implementation

```csharp
[ApiController]
[Route("api/webhooks")]
public class WebhookController : ControllerBase
{
    private readonly IWebhookProcessor _webhookProcessor;
    private readonly ILogger<WebhookController> _logger;

    public WebhookController(IWebhookProcessor webhookProcessor, ILogger<WebhookController> logger)
    {
        _webhookProcessor = webhookProcessor;
        _logger = logger;
    }

    [HttpPost("embedly")]
    public async Task<IActionResult> HandleWebhook()
    {
        var payload = await ReadPayloadAsync();
        var signature = Request.Headers["X-Embedly-Signature"].FirstOrDefault();

        // The SDK handles validation and processing
        var result = await _webhookProcessor.ProcessWebhookAsync(payload, signature);

        return result.Success
            ? Ok(new { status = "success", eventType = result.EventType })
            : BadRequest(new { status = "failed", error = result.Error });
    }

    private async Task<string> ReadPayloadAsync()
    {
        using var reader = new StreamReader(Request.Body, Encoding.UTF8);
        return await reader.ReadToEndAsync();
    }
}
```

## 🎯 Available Event Types

The SDK provides constants for all webhook events:

### Customer Events
- `WebhookEventTypes.CustomerCreated`
- `WebhookEventTypes.CustomerUpdated`
- `WebhookEventTypes.CustomerVerified`

### Transaction Events
- `WebhookEventTypes.TransactionCreated`
- `WebhookEventTypes.TransactionCompleted`
- `WebhookEventTypes.TransactionFailed`

### Payment Events
- `WebhookEventTypes.PaymentInitiated`
- `WebhookEventTypes.PaymentCompleted`
- `WebhookEventTypes.PaymentFailed`

### Card Events
- `WebhookEventTypes.CardCreated`
- `WebhookEventTypes.CardActivated`
- `WebhookEventTypes.CardBlocked`

### KYC Events
- `WebhookEventTypes.KycSubmitted`
- `WebhookEventTypes.KycApproved`
- `WebhookEventTypes.KycRejected`

[See complete list in `WebhookEventTypes` class]

## 🔒 Security Features Built-In

The SDK's webhook system automatically handles:
- ✅ Signature validation using HMAC-SHA256
- ✅ Constant-time comparison to prevent timing attacks
- ✅ Proper JSON parsing and deserialization
- ✅ Error handling and logging
- ✅ Structured event data with `GetData<T>()` method

## 📊 Benefits of Using SDK's System

### Instead of Building Custom:
```csharp
// ❌ DON'T: Custom implementation
public class CustomWebhookProcessor
{
    public async Task<Result> ProcessWebhookAsync(string payload, string signature)
    {
        // Custom validation logic
        // Custom parsing logic
        // Custom error handling
        // Custom logging
        // 100+ lines of boilerplate code
    }
}
```

### Use SDK's Built-in System:
```csharp
// ✅ DO: SDK's built-in system
public class MyWebhookHandler : WebhookHandler
{
    protected override void RegisterHandlers()
    {
        RegisterHandler(WebhookEventTypes.CustomerCreated, HandleCustomerCreatedAsync);
        // Just register your handlers - SDK does the rest!
    }
}
```

## 💡 Why This Approach is Better

1. **Less Code**: You write only your business logic, not infrastructure
2. **Tested & Secure**: The SDK handles all security and validation
3. **Strongly Typed**: Use `GetData<T>()` for type-safe data access
4. **Maintainable**: Updates to webhook handling come with SDK updates
5. **Standard Patterns**: Consistent with how other developers use the SDK

## 🚀 Getting Started

1. Look at `EmbedlyWebhookHandler.cs` for a complete example
2. Copy the pattern and add your business logic
3. Register your handler with `AddWebhookHandler<YourHandler>()`
4. Inject `IWebhookProcessor` in your controller
5. Call `ProcessWebhookAsync()` - that's it!

The SDK does all the heavy lifting, you just focus on your business logic!