# Embedly.SDK - Official .NET SDK for Embedly.ng

[![NuGet](https://img.shields.io/nuget/v/Embedly.SDK.svg)](https://www.nuget.org/packages/Embedly.SDK/)
[![Downloads](https://img.shields.io/nuget/dt/Embedly.SDK.svg)](https://www.nuget.org/packages/Embedly.SDK/)
[![License](https://img.shields.io/github/license/embedly/embedly-dotnet-sdk.svg)](https://github.com/embedly/embedly-dotnet-sdk/blob/main/LICENSE)

The official .NET SDK for [Embedly.ng](https://embedly.ng) - Nigeria's leading embedded finance platform. This SDK provides comprehensive access to wallet management, payments, cards, and financial services APIs.

## Features

- 🏦 **Complete API Coverage** - Full access to all Embedly.ng services
- 🚀 **Async/Await Support** - Modern asynchronous programming patterns
- 🔒 **Type Safety** - Strongly typed models and responses
- 🔧 **Dependency Injection** - Native DI container integration
- 📊 **Logging & Monitoring** - Built-in request/response logging
- 🔄 **Retry Policies** - Automatic retry with exponential backoff
- 🌍 **Multi-Environment** - Staging and production environment support
- 💳 **Nigerian Market Focus** - NIN/BVN KYC, Naira currency support

## Installation

Install the SDK via NuGet Package Manager:

```bash
dotnet add package Embedly.SDK
```

Or via Package Manager Console:

```powershell
Install-Package Embedly.SDK
```

## Quick Start

### 1. Configure Services (ASP.NET Core)

```csharp
using Embedly.SDK.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add Embedly SDK services
builder.Services.AddEmbedly(options =>
{
    options.ApiKey = "your-api-key";
    options.Environment = EmbedlyEnvironment.Staging; // or Production
});

// Or configure from appsettings.json
builder.Services.AddEmbedly(builder.Configuration.GetSection("Embedly"));

var app = builder.Build();
```

### 2. Configuration (appsettings.json)

```json
{
  "Embedly": {
    "ApiKey": "your-api-key-here",
    "Environment": "Staging",
    "Timeout": "00:00:30",
    "RetryCount": 3,
    "EnableLogging": true
  }
}
```

### 3. Use in Your Application

```csharp
using Embedly.SDK;
using Embedly.SDK.Models.Requests.Customers;

public class CustomerController : ControllerBase
{
    private readonly IEmbedlyClient _embedlyClient;

    public CustomerController(IEmbedlyClient embedlyClient)
    {
        _embedlyClient = embedlyClient;
    }

    [HttpPost("customers")]
    public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerRequest request)
    {
        try
        {
            var customer = await _embedlyClient.Customers.CreateAsync(request);
            return Ok(customer);
        }
        catch (EmbedlyApiException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}
```

## Available Services

### Customer Management
```csharp
// Create a customer
var customer = await embedlyClient.Customers.CreateAsync(new CreateCustomerRequest
{
    FirstName = "John",
    LastName = "Doe",
    Email = "john.doe@example.com",
    PhoneNumber = "+2348012345678",
    DateOfBirth = new DateTime(1990, 1, 15)
});

// Get customer by ID
var customer = await embedlyClient.Customers.GetByIdAsync("customer-id");

// Update customer name
var updatedCustomer = await embedlyClient.Customers.UpdateNameAsync(
    "customer-id", "Jane", "Smith");

// KYC upgrade using NIN
var kycResult = await embedlyClient.Customers.UpgradeKycWithNinAsync(new NinKycUpgradeRequest
{
    CustomerId = "customer-id",
    Nin = "12345678901",
    DateOfBirth = new DateTime(1990, 1, 15)
});
```

### Wallet Operations (Coming Soon)
```csharp
// Create wallet
var wallet = await embedlyClient.Wallets.CreateAsync(new CreateWalletRequest
{
    CustomerId = "customer-id",
    Currency = "NGN"
});

// Transfer between wallets
var transfer = await embedlyClient.Wallets.TransferAsync(new WalletTransferRequest
{
    FromWalletId = "wallet-1",
    ToWalletId = "wallet-2",
    Amount = Money.FromNaira(1000.00m)
});
```

## Error Handling

The SDK provides comprehensive error handling with specific exception types:

```csharp
try
{
    var customer = await embedlyClient.Customers.GetByIdAsync("invalid-id");
}
catch (EmbedlyApiException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
{
    // Handle not found
    Console.WriteLine($"Customer not found: {ex.Message}");
}
catch (EmbedlyApiException ex) when (ex.StatusCode == HttpStatusCode.Unauthorized)
{
    // Handle authentication error
    Console.WriteLine("Invalid API key");
}
catch (EmbedlyValidationException ex)
{
    // Handle validation errors
    foreach (var error in ex.ValidationErrors)
    {
        Console.WriteLine($"{error.PropertyName}: {error.ErrorMessage}");
    }
}
catch (EmbedlyException ex)
{
    // Handle general SDK errors
    Console.WriteLine($"SDK Error: {ex.Message}");
}
```

## Money and Currency

The SDK includes a robust `Money` type for handling Nigerian Naira:

```csharp
// Create money amounts
var amount1 = Money.FromNaira(1000.50m);      // ₦1,000.50
var amount2 = Money.FromKobo(150000);         // ₦1,500.00 (150000 kobo)

// Arithmetic operations
var total = amount1 + amount2;                // ₦2,500.50
var half = total / 2;                         // ₦1,250.25

// Display formatting
Console.WriteLine(total.ToString());          // ₦2,500.50
```

## Webhook Handling (Coming Soon)

```csharp
// In your webhook controller
[HttpPost("webhooks/embedly")]
public async Task<IActionResult> HandleWebhook()
{
    var signature = Request.Headers["x-embedly-signature"];
    var payload = await ReadBodyAsync();
    
    var result = await _webhookProcessor.ProcessWebhookAsync(payload, signature);
    return result.Success ? Ok() : BadRequest();
}
```

## Advanced Configuration

### Custom HTTP Client Configuration

```csharp
services.AddEmbedly(options =>
{
    options.ApiKey = "your-api-key";
    options.Environment = EmbedlyEnvironment.Production;
    options.Timeout = TimeSpan.FromSeconds(60);
    options.RetryCount = 5;
    options.EnableLogging = true;
    options.LogRequestBodies = false; // For security
});
```

### Custom Service URLs

```csharp
services.AddEmbedly(options =>
{
    options.ApiKey = "your-api-key";
    options.CustomServiceUrls = new ServiceUrls
    {
        Base = "https://custom-api.yourdomain.com/v1",
        Payout = "https://custom-payout.yourdomain.com",
        // ... other URLs
    };
});
```

## Requirements

- .NET 6.0, 7.0, 8.0, or 9.0
- An Embedly.ng API key ([Get one here](https://developer.embedly.ng))

## Contributing

Contributions are welcome! Please read our [Contributing Guide](CONTRIBUTING.md) for details.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Support

- 📖 [API Documentation](https://developer.embedly.ng)
- 💬 [Community Forum](https://community.embedly.ng)
- 📧 [Support Email](mailto:support@embedly.ng)
- 🐛 [Report Issues](https://github.com/embedly/embedly-dotnet-sdk/issues)

---

Built with ❤️ for the Nigerian fintech ecosystem.