# Embedly SDK Examples

A professional reference implementation showing how to use the Embedly SDK with clean architecture patterns, proper
error handling, and the SDK's built-in webhook system.

## 🏗️ Project Structure

```
Embedly.Examples/
├── Controllers/
│   ├── CustomerController.cs         # Customer management API endpoints
│   ├── WalletController.cs          # Wallet management API endpoints
│   └── WebhookController.cs         # Webhook endpoint using SDK's built-in system
├── Examples/
│   ├── CustomerExamples.cs          # Working customer management examples
│   └── WalletExamples.cs           # Working wallet management examples
├── Infrastructure/
│   ├── Configuration/
│   │   ├── ApplicationSettings.cs   # Application configuration
│   │   ├── EmbedlySettings.cs      # Embedly SDK configuration
│   │   └── LoggingSettings.cs      # Logging configuration
│   ├── Extensions/
│   │   └── ServiceCollectionExtensions.cs # DI container setup
│   └── Services/
│       ├── CorrelationService.cs    # Request correlation tracking
│       ├── EmbedlyWebhookHandler.cs # Webhook event handlers
│       ├── HealthService.cs         # Health check implementation
│       ├── IResult.cs              # Result pattern interfaces
│       └── RetryService.cs         # Retry logic with exponential backoff
├── Program.cs                       # Application entry point
├── appsettings.json                # Configuration settings
└── WEBHOOK_GUIDE.md                # Webhook implementation guide
```

## 🔗 Webhook Handler Mapping System

### How Event Processing Works

The Embedly SDK uses a **one-handler-per-event** pattern. Each webhook event type gets its own dedicated handler method
that you write.

#### 1. Each Event Gets Its Own Handler Method

```csharp
public class EmbedlyWebhookHandler : WebhookHandler
{
    protected override void RegisterHandlers()
    {
        // Each event type maps to a specific handler method
        RegisterHandler(WebhookEventTypes.CustomerCreated, HandleCustomerCreatedAsync);
        RegisterHandler(WebhookEventTypes.CustomerUpdated, HandleCustomerUpdatedAsync);
        RegisterHandler(WebhookEventTypes.TransactionCompleted, HandleTransactionCompletedAsync);
        RegisterHandler(WebhookEventTypes.PaymentFailed, HandlePaymentFailedAsync);
        // ... one registration per event type
    }

    // Separate handler method for each event
    private async Task HandleCustomerCreatedAsync(WebhookEvent webhookEvent, CancellationToken cancellationToken)
    {
        var customerData = webhookEvent.GetData<CustomerCreatedData>();

        // Business logic specific to customer creation
        await SendWelcomeEmail(customerData.Email);
        await UpdateCrmSystem(customerData);
        await CreateUserAccount(customerData);
    }

    private async Task HandleTransactionCompletedAsync(WebhookEvent webhookEvent, CancellationToken cancellationToken)
    {
        var transactionData = webhookEvent.GetData<TransactionCompletedData>();

        // Business logic specific to completed transactions
        await MarkOrderAsPaid(transactionData.TransactionId);
        await SendReceipt(transactionData);
        await UpdateInventory(transactionData);
    }
}
```

#### 2. The Routing Process

```
Webhook arrives → SDK routes by event type → Calls your specific handler
```

**Example Flow:**

1. Webhook with `"event": "customer.created"` arrives
2. SDK looks up `WebhookEventTypes.CustomerCreated` in registered handlers
3. SDK calls your `HandleCustomerCreatedAsync` method
4. Your method processes the customer creation logic

#### 3. You Choose Which Events to Handle

You don't have to handle every event - only register handlers for events you care about:

```csharp
protected override void RegisterHandlers()
{
    // Only handle the events your app cares about
    RegisterHandler(WebhookEventTypes.CustomerCreated, HandleCustomerCreatedAsync);
    RegisterHandler(WebhookEventTypes.TransactionCompleted, HandleTransactionCompletedAsync);

    // Skip events you don't need:
    // - CustomerUpdated (not registered = ignored)
    // - PaymentFailed (not registered = ignored)
}
```

#### 4. Strongly-Typed Data for Each Event

Each handler gets strongly-typed data specific to that event:

```csharp
private async Task HandleCustomerCreatedAsync(WebhookEvent webhookEvent, CancellationToken cancellationToken)
{
    // Get customer-specific data
    var customerData = webhookEvent.GetData<CustomerCreatedData>();
    // customerData has: CustomerId, Email, FirstName, LastName, etc.
}

private async Task HandleTransactionCompletedAsync(WebhookEvent webhookEvent, CancellationToken cancellationToken)
{
    // Get transaction-specific data
    var transactionData = webhookEvent.GetData<TransactionCompletedData>();
    // transactionData has: TransactionId, Amount, Status, etc.
}
```

#### 5. Internal Mapping (How the SDK Works)

The SDK maintains an internal dictionary that routes events:

```csharp
// Inside the SDK's WebhookHandler base class
private readonly Dictionary<string, Func<WebhookEvent, CancellationToken, Task>> _handlers;

// When you call RegisterHandler(), it adds to this dictionary
protected void RegisterHandler(string eventType, Func<WebhookEvent, CancellationToken, Task> handler)
{
    _handlers[eventType] = handler;  // "customer.created" → HandleCustomerCreatedAsync
}

// When webhook arrives, SDK looks up and calls your handler
public async Task HandleEventAsync(WebhookEvent webhookEvent, CancellationToken cancellationToken)
{
    if (_handlers.TryGetValue(webhookEvent.Event, out var handler))
    {
        await handler(webhookEvent, cancellationToken);  // Calls your specific method
    }
}
```

### Event Type Constants

The SDK provides these constants in `WebhookEventTypes`:

```csharp
public static class WebhookEventTypes
{
    public const string CustomerCreated = "customer.created";
    public const string CustomerUpdated = "customer.updated";
    public const string TransactionCompleted = "transaction.completed";
    // ... and many more
}
```

### Your Handler Methods

When you create a handler method like:

```csharp
private async Task HandleCustomerCreatedAsync(WebhookEvent webhookEvent, CancellationToken cancellationToken)
{
    var customerData = webhookEvent.GetData<CustomerCreatedData>();
    // Your business logic here
}
```

This method signature **must match** the delegate expected by `RegisterHandler()`:

- `Func<WebhookEvent, CancellationToken, Task>`

### 📝 Summary for Developers

✅ **One handler method per event type you want to process**
✅ **Register only the events you care about - ignore the rest**
✅ **SDK automatically routes incoming webhooks to your handlers**
✅ **Each handler gets strongly-typed data for that specific event**
✅ **Write your business logic in each handler method**

**Example: If you only care about customers and transactions:**

```csharp
protected override void RegisterHandlers()
{
    // Only register what you need
    RegisterHandler(WebhookEventTypes.CustomerCreated, HandleCustomerCreatedAsync);
    RegisterHandler(WebhookEventTypes.TransactionCompleted, HandleTransactionCompletedAsync);

    // All other events (payments, cards, etc.) will be ignored
}
```

## 🚀 Quick Start

### 1. Configuration

```json
// appsettings.json
{
  "Embedly": {
    "ApiKey": "your-api-key",
    "OrganizationId": "your-org-id",
    "Environment": "Sandbox",
    "WebhookSecret": "your-webhook-secret"
  }
}
```

### 2. Service Registration

```csharp
// Program.cs
services.AddApplicationServices(configuration);
services.AddExampleServices();
services.AddWebhookServices(configuration); // Registers SDK's webhook system
```

### 3. Create Your Webhook Handler

```csharp
public class MyWebhookHandler : WebhookHandler
{
    protected override void RegisterHandlers()
    {
        // Map event types to your handler methods
        RegisterHandler(WebhookEventTypes.CustomerCreated, HandleCustomerCreatedAsync);
        RegisterHandler(WebhookEventTypes.TransactionCompleted, HandleTransactionCompletedAsync);
    }

    private async Task HandleCustomerCreatedAsync(WebhookEvent webhookEvent, CancellationToken cancellationToken)
    {
        var data = webhookEvent.GetData<CustomerCreatedData>();
        // Your business logic
        await Task.CompletedTask;
    }
}
```

### 4. Register Your Handler

```csharp
// In ServiceCollectionExtensions.cs
services.AddWebhookHandler<MyWebhookHandler>();
```

## 🎣 Complete Webhook Setup Guide

### Step-by-Step Webhook Implementation

#### 1. Add Webhook Configuration

```json
// appsettings.json
{
  "Embedly": {
    "WebhookSecret": "your-webhook-secret-from-embedly-dashboard"
  }
}
```

#### 2. Create Your Webhook Handler Class

```csharp
// Infrastructure/Services/MyWebhookHandler.cs
using Embedly.SDK.Webhooks;

public class MyWebhookHandler : WebhookHandler
{
    private readonly ILogger<MyWebhookHandler> _logger;
    private readonly IEmailService _emailService;
    private readonly IOrderService _orderService;

    public MyWebhookHandler(
        ILogger<MyWebhookHandler> logger,
        IEmailService emailService,
        IOrderService orderService) : base(logger)
    {
        _logger = logger;
        _emailService = emailService;
        _orderService = orderService;
    }

    protected override void RegisterHandlers()
    {
        // Register only the events you care about
        RegisterHandler(WebhookEventTypes.CustomerCreated, HandleCustomerCreatedAsync);
        RegisterHandler(WebhookEventTypes.TransactionCompleted, HandleTransactionCompletedAsync);
        RegisterHandler(WebhookEventTypes.PaymentFailed, HandlePaymentFailedAsync);
    }

    private async Task HandleCustomerCreatedAsync(WebhookEvent webhookEvent, CancellationToken cancellationToken)
    {
        var customerData = webhookEvent.GetData<CustomerCreatedData>();

        _logger.LogInformation("New customer created: {CustomerId}", customerData?.CustomerId);

        // Your business logic
        if (customerData?.Email != null)
        {
            await _emailService.SendWelcomeEmailAsync(customerData.Email);
        }
    }

    private async Task HandleTransactionCompletedAsync(WebhookEvent webhookEvent, CancellationToken cancellationToken)
    {
        var transactionData = webhookEvent.GetData<TransactionCompletedData>();

        _logger.LogInformation("Transaction completed: {TransactionId}", transactionData?.TransactionId);

        // Your business logic
        if (transactionData?.TransactionId != null)
        {
            await _orderService.MarkOrderAsPaidAsync(transactionData.TransactionId);
            await _emailService.SendReceiptAsync(transactionData.TransactionId);
        }
    }

    private async Task HandlePaymentFailedAsync(WebhookEvent webhookEvent, CancellationToken cancellationToken)
    {
        var paymentData = webhookEvent.GetData<PaymentFailedData>();

        _logger.LogWarning("Payment failed: {PaymentId}, Reason: {Reason}",
            paymentData?.PaymentId, paymentData?.FailureReason);

        // Your business logic
        await _orderService.HandleFailedPaymentAsync(paymentData?.PaymentId);
    }
}
```

#### 3. Register Services in Program.cs

```csharp
// Program.cs
var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();

// Register Embedly SDK with webhook support
builder.Services.AddEmbedlyWebhooks(builder.Configuration.GetValue<string>("Embedly:WebhookSecret"));

// Register your webhook handler
builder.Services.AddWebhookHandler<MyWebhookHandler>();

// Register your business services
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IOrderService, OrderService>();

var app = builder.Build();

// Configure pipeline
app.UseRouting();
app.MapControllers();

app.Run();
```

#### 4. Create Webhook Controller (ASP.NET Core)

```csharp
// Controllers/WebhookController.cs
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
    public async Task<IActionResult> HandleEmbedlyWebhook()
    {
        try
        {
            // Read the payload
            var payload = await ReadPayloadAsync();
            var signature = Request.Headers["X-Embedly-Signature"].FirstOrDefault();

            if (string.IsNullOrEmpty(payload) || string.IsNullOrEmpty(signature))
            {
                return BadRequest("Missing payload or signature");
            }

            // SDK handles validation and processing
            var result = await _webhookProcessor.ProcessWebhookAsync(payload, signature);

            if (result.Success)
            {
                _logger.LogInformation("Webhook processed: {EventType}", result.EventType);
                return Ok(new { status = "success", eventType = result.EventType });
            }
            else
            {
                _logger.LogWarning("Webhook failed: {Error}", result.Error);
                return BadRequest(new { status = "failed", error = result.Error });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Webhook processing error");
            return StatusCode(500, new { status = "error", error = "Internal server error" });
        }
    }

    private async Task<string> ReadPayloadAsync()
    {
        using var reader = new StreamReader(Request.Body, Encoding.UTF8);
        return await reader.ReadToEndAsync();
    }
}
```

## 🚀 FastEndpoints Example

If you're using **FastEndpoints** instead of controllers:

#### 1. Install FastEndpoints

```bash
dotnet add package FastEndpoints
```

#### 2. Create FastEndpoints Webhook Endpoint

```csharp
// Endpoints/WebhookEndpoint.cs
using FastEndpoints;
using Embedly.SDK.Webhooks;

public class WebhookEndpoint : EndpointWithoutRequest
{
    private readonly IWebhookProcessor _webhookProcessor;
    private readonly ILogger<WebhookEndpoint> _logger;

    public WebhookEndpoint(IWebhookProcessor webhookProcessor, ILogger<WebhookEndpoint> logger)
    {
        _webhookProcessor = webhookProcessor;
        _logger = logger;
    }

    public override void Configure()
    {
        Post("/api/webhooks/embedly");
        AllowAnonymous();
        Description(d => d
            .WithTags("Webhooks")
            .WithSummary("Handle Embedly webhook events")
            .WithDescription("Receives and processes webhook events from Embedly"));
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        try
        {
            // Read the payload
            var payload = await ReadPayloadAsync();
            var signature = HttpContext.Request.Headers["X-Embedly-Signature"].FirstOrDefault();

            if (string.IsNullOrEmpty(payload) || string.IsNullOrEmpty(signature))
            {
                await SendAsync(new { error = "Missing payload or signature" }, 400, ct);
                return;
            }

            // SDK handles validation and processing
            var result = await _webhookProcessor.ProcessWebhookAsync(payload, signature);

            if (result.Success)
            {
                _logger.LogInformation("Webhook processed: {EventType}", result.EventType);
                await SendOkAsync(new
                {
                    status = "success",
                    eventType = result.EventType,
                    eventId = result.EventId,
                    processedAt = DateTime.UtcNow
                }, ct);
            }
            else
            {
                _logger.LogWarning("Webhook failed: {Error}", result.Error);
                await SendAsync(new
                {
                    status = "failed",
                    error = result.Error,
                    processedAt = DateTime.UtcNow
                }, 400, ct);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Webhook processing error");
            await SendAsync(new
            {
                status = "error",
                error = "Internal server error",
                processedAt = DateTime.UtcNow
            }, 500, ct);
        }
    }

    private async Task<string> ReadPayloadAsync()
    {
        using var reader = new StreamReader(HttpContext.Request.Body, Encoding.UTF8);
        return await reader.ReadToEndAsync();
    }
}
```

#### 3. Configure FastEndpoints in Program.cs

```csharp
// Program.cs
using FastEndpoints;
using FastEndpoints.Swagger;

var builder = WebApplication.CreateBuilder(args);

// Add FastEndpoints
builder.Services.AddFastEndpoints();
builder.Services.AddSwaggerDoc();

// Register Embedly SDK with webhook support
builder.Services.AddEmbedlyWebhooks(builder.Configuration.GetValue<string>("Embedly:WebhookSecret"));

// Register your webhook handler
builder.Services.AddWebhookHandler<MyWebhookHandler>();

// Register your business services
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IOrderService, OrderService>();

var app = builder.Build();

// Configure FastEndpoints
app.UseFastEndpoints(c =>
{
    c.Endpoints.RoutePrefix = "api";
    c.Serializer.Options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
});

app.UseSwaggerGen();

app.Run();
```

## 🎯 Testing Your Webhook

### 1. Local Testing with ngrok

```bash
# Install ngrok
npm install -g ngrok

# Run your app
dotnet run

# In another terminal, expose your local app
ngrok http 5000

# Use the ngrok URL in Embedly dashboard: https://abc123.ngrok.io/api/webhooks/embedly
```

### 2. Test Webhook Signature Validation

```csharp
// Add a test endpoint to verify your setup
[HttpPost("test-signature")]
public IActionResult TestSignature([FromBody] TestSignatureRequest request)
{
    var isValid = _webhookProcessor.ValidateWebhook(request.Payload, request.Signature);
    return Ok(new { isValid, message = isValid ? "Valid signature" : "Invalid signature" });
}

public class TestSignatureRequest
{
    public string Payload { get; set; } = string.Empty;
    public string Signature { get; set; } = string.Empty;
}
```

## 📊 Architecture Patterns Used

### Result Pattern

```csharp
public async Task<Result<Customer>> CreateCustomerAsync()
{
    try
    {
        var response = await _embedlyClient.Customers.CreateAsync(request);
        return Result<Customer>.Success(response.Data);
    }
    catch (Exception ex)
    {
        return Result<Customer>.Failure($"Failed to create customer: {ex.Message}");
    }
}
```

### Correlation Tracking

```csharp
public async Task ProcessRequest()
{
    var correlationId = _correlationService.GetOrCreateCorrelationId();
    _logger.LogInformation("Processing request {CorrelationId}", correlationId);
}
```

### Retry with Exponential Backoff

```csharp
var result = await _retryService.ExecuteWithRetryAsync(async () =>
{
    return await _embedlyClient.SomeOperation();
}, "OperationName");
```

## 🔧 Running the Examples

### Interactive Mode

```bash
dotnet run -- --mode=interactive
```

### Web API Mode

```bash
dotnet run -- --mode=webapi
```

### Console Examples

```bash
dotnet run -- --mode=console
```

### Specific Scenario

```bash
dotnet run -- --mode=scenario --scenario=customer-management
```

## 🔒 Security Features

- **Webhook Signature Validation**: HMAC-SHA256 with constant-time comparison
- **Request Correlation**: Track requests across services
- **Structured Logging**: Security-conscious logging without sensitive data
- **Error Handling**: Proper exception handling without information leakage

## 📋 Available Examples

### Customer Management

- Create customer with validation
- Update customer information
- Verify customer identity
- Handle customer lifecycle events

### Wallet Management

- Create customer wallets
- Activate wallets
- Handle wallet events
- Balance operations

### Webhook Processing

- Automatic signature validation
- Event type routing
- Strongly-typed event data
- Error handling and retry logic

## 🎯 Why This Architecture?

1. **Separation of Concerns**: Each layer has a single responsibility
2. **Testability**: Dependency injection makes unit testing straightforward
3. **Maintainability**: Clear patterns make the code easy to understand and modify
4. **Scalability**: Architecture supports adding new features without major changes
5. **Production Ready**: Includes logging, monitoring, error handling, and security

## 📝 Key Benefits of Using SDK's Webhook System

✅ **No Custom Code**: SDK handles validation, parsing, and routing
✅ **Type Safety**: Use `GetData<T>()` for strongly-typed event data
✅ **Security Built-in**: HMAC-SHA256 validation with timing attack prevention
✅ **Error Handling**: Comprehensive error handling and logging
✅ **Easy Testing**: Clean separation makes testing straightforward
✅ **Maintainable**: Updates come with SDK versions

## 🔍 Troubleshooting

### Common Issues

1. **Webhook signature validation fails**
    - Verify webhook secret in configuration
    - Check that payload hasn't been modified in transit

2. **Handler not called**
    - Ensure handler is registered with correct event type constant
    - Verify handler method signature matches expected delegate

3. **Deserialization errors**
    - Check that your data model matches the actual webhook payload structure
    - Use nullable properties for optional fields

### Debug Mode

Set logging level to Debug to see detailed webhook processing:

```json
{
  "Logging": {
    "LogLevel": {
      "Embedly": "Debug"
    }
  }
}
```

This will show:

- Incoming webhook payloads
- Signature validation details
- Event routing decisions
- Handler execution results