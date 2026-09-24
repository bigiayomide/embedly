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
- 🔔 **Webhook Support** - Built-in webhook validation and processing
- 💰 **Checkout Integration** - Dynamic account generation for payments

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

### Wallet Operations
```csharp
// Create wallet
var wallet = await embedlyClient.Wallets.CreateWalletAsync(new CreateWalletRequest
{
    CustomerId = "customer-id",
    CurrencyId = "currency-id"
});

// Get wallet by ID
var wallet = await embedlyClient.Wallets.GetWalletAsync("wallet-id");

// Get wallet by account number
var wallet = await embedlyClient.Wallets.GetWalletByAccountNumberAsync("1234567890");

// Wallet to wallet transfer
var transfer = await embedlyClient.Wallets.WalletToWalletTransferAsync(new WalletToWalletTransferRequest
{
    FromAccount = "source-account-number",
    ToAccount = "destination-account-number",
    Amount = 5000.00m,  // Uses decimal for precision
    TransactionReference = "TXN-123456",
    Remarks = "Payment for services"
});

// Check transfer status
var status = await embedlyClient.Wallets.GetWalletTransferStatusAsync("TXN-123456");

// Get wallet transaction history
var history = await embedlyClient.Wallets.GetWalletHistoryAsync(walletId);

// Simulate inflow (Staging only - for testing)
var inflow = await embedlyClient.Wallets.SimulateInflowAsync(new SimulateInflowRequest
{
    AccountNumber = "1234567890",
    Amount = 10000.00m,
    Narration = "Test deposit"
});
```

### Checkout (Dynamic Account Generation)
```csharp
var organizationId = Guid.Parse("your-organization-id");

// Get organization prefix mappings (required to create checkout wallets)
var prefixes = await embedlyClient.Checkout.GetOrganizationPrefixMappingsAsync(
    organizationId, page: 1, pageSize: 10, search: null);  // page, pageSize and search are optional

// Create a checkout wallet (generates a temporary account for a one-time payment)
var checkout = await embedlyClient.Checkout.GenerateCheckoutWalletAsync(new GenerateCheckoutWalletRequest
{
    OrganizationId = organizationId,
    ExpectedAmount = 15000.00m,  // Uses decimal for precision
    OrganizationPrefixMappingId = prefixes.Data![0].Id,
    ExpiryDurationMinutes = 30,  // Optional, defaults to 30
    // Optional details
    InvoiceReference = "INV-001",
    Description = "Order #1234",
    CurrencyCode = "NGN",
    CustomerEmail = "customer@example.com",
    CustomerName = "Jane Doe"
});
Console.WriteLine($"Pay into {checkout.Data!.WalletNumber} (ref: {checkout.Data.CheckoutRef})");

// List checkout wallets (paginated and filterable)
var wallets = await embedlyClient.Checkout.GetCheckoutWalletsAsync(new GetCheckoutWalletsRequest
{
    OrganizationId = organizationId,
    Page = 1,          // Optional, defaults to 1
    PageSize = 10,     // Optional, defaults to 10
    Status = "Used"    // Optional: Used, Failed, Reversed, Completed or Expired
    // Also: StartDate, EndDate, WalletNumber, OrganizationPrefixMappingId
});
Console.WriteLine($"{wallets.Pagination?.TotalItems} wallets in total");

// Get a checkout wallet with its checkout history and received payments
var details = await embedlyClient.Checkout.GetCheckoutWalletWithTransactionsAsync(
    checkout.Data.Id, organizationId);
foreach (var payment in details.Data!.Transactions ?? new())
    Console.WriteLine($"{payment.SenderName} paid {payment.Amount} ({payment.Status})");
```

#### Split Payments
A checkout wallet's payment can be split across split beneficiaries.
```csharp
// Create a split beneficiary
var beneficiary = await embedlyClient.Checkout.CreateSplitBeneficiaryAsync(new CreateSplitBeneficiaryRequest
{
    OrganizationId = organizationId,
    BeneficiaryName = "John Doe",
    AccountNumber = "0123456789",
    // Optional details
    BankCode = "000013",
    BankName = "GTBank Plc",
    BeneficiaryAlias = "Supplier"
});

// Create a checkout wallet whose payment is split
var splitCheckout = await embedlyClient.Checkout.GenerateCheckoutWalletAsync(new GenerateCheckoutWalletRequest
{
    OrganizationId = organizationId,
    ExpectedAmount = 20000.00m,
    OrganizationPrefixMappingId = prefixes.Data[0].Id,
    SplitType = "Fixed",  // "Fixed" or "Percentage"
    IncomeSplitConfig = new List<IncomeSplitConfig>
    {
        new() { BeneficiaryId = beneficiary.Data!.Id, SplitValue = 5000.00m, FeeValue = 0m, FeeBearer = false }
    }
});

// List split beneficiaries (paginated and filterable)
var beneficiaries = await embedlyClient.Checkout.GetSplitBeneficiariesAsync(new GetSplitBeneficiariesRequest
{
    OrganizationId = organizationId,
    Page = 1,           // Optional, defaults to 1
    PageSize = 10,      // Optional, defaults to 10
    IsActive = true,    // Optional
    SearchTerm = "John" // Optional
});
Console.WriteLine($"{beneficiaries.Data!.TotalCount} beneficiaries in total");

// Deactivate or reactivate a split beneficiary
var statusRequest = new SplitBeneficiaryStatusRequest
{
    BeneficiaryId = beneficiary.Data.Id,
    OrganizationId = organizationId
};
await embedlyClient.Checkout.DeactivateSplitBeneficiaryAsync(statusRequest);
await embedlyClient.Checkout.ActivateSplitBeneficiaryAsync(statusRequest);
```

### Payout (Bank Transfers)
```csharp
// Get list of banks
var banks = await embedlyClient.Payout.GetBanksAsync();

// Verify account name
var nameEnquiry = await embedlyClient.Payout.NameEnquiryAsync(new NameEnquiryRequest
{
    AccountNumber = "1234567890",
    BankCode = "058"  // GTBank
});

// Initiate bank transfer
var transfer = await embedlyClient.Payout.InterBankTransferAsync(new BankTransferRequest
{
    SourceAccountNumber = "source-account",
    SourceAccountName = "Source Name",
    DestinationAccountNumber = "1234567890",
    DestinationAccountName = nameEnquiry.Data.AccountName,
    DestinationBankCode = "058",
    Amount = 5000.00m,  // Uses decimal for precision
    Remarks = "Payment"
});

// Check transaction status
var status = await embedlyClient.Payout.GetTransactionStatusAsync("transaction-reference");
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

## Webhook Handling

The SDK provides built-in webhook validation and processing with HMAC-SHA512 signature verification.

```csharp
// Register webhook services
builder.Services.AddEmbedlyWebhooks(options =>
{
    options.WebhookSecret = "your-webhook-secret";
});

// In your webhook controller
[HttpPost("webhooks/embedly")]
public async Task<IActionResult> HandleWebhook(
    [FromServices] IWebhookProcessor webhookProcessor)
{
    var signature = Request.Headers["x-embedly-signature"].ToString();
    using var reader = new StreamReader(Request.Body);
    var payload = await reader.ReadToEndAsync();

    var result = await webhookProcessor.ProcessWebhookAsync(payload, signature);
    return result.Success ? Ok() : BadRequest(result.Error);
}

// Or use the validator directly
[HttpPost("webhooks/embedly")]
public IActionResult HandleWebhook(
    [FromServices] IWebhookValidator validator)
{
    var signature = Request.Headers["x-embedly-signature"].ToString();
    var payload = await ReadBodyAsync();

    // Validate signature
    if (!validator.ValidateSignature(payload, signature))
        return Unauthorized("Invalid signature");

    // Parse event
    var webhookEvent = validator.ParseEvent(payload, signature);

    // Handle based on event type
    switch (webhookEvent.Event)
    {
        case WebhookEventTypes.CheckoutPaymentSuccess:
            // Handle checkout payment
            break;
        case WebhookEventTypes.Payout:
            // Handle payout notification
            break;
        case WebhookEventTypes.Nip:
            // Handle NIP transfer notification
            break;
    }

    return Ok();
}
```

### Supported Webhook Event Types

| Event Type | Description |
|------------|-------------|
| `checkout.payment.success` | Checkout payment completed successfully |
| `payout` | Payout transaction notification |
| `nip` | NIP (NIBSS Instant Payment) notification |
| `card.transaction.atm` | ATM card transaction |
| `card.transaction.pos` | POS card transaction |
| `card.management.updateInfo` | Card information updated |
| `card.management.relink` | Card relinked |

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

## Migration Guide

### Breaking Changes in v2.x

#### Money Fields Changed from `double` to `decimal`

All monetary amount fields have been changed from `double` to `decimal` for better precision in financial calculations. Update your code to use the `m` suffix for decimal literals:

```csharp
// Before (v1.x)
var request = new BankTransferRequest { Amount = 5000.00 };

// After (v2.x)
var request = new BankTransferRequest { Amount = 5000.00m };
```

**Affected properties:**
- `WalletToWalletTransferRequest.Amount`
- `BankTransferRequest.Amount`
- `GenerateCheckoutWalletRequest.ExpectedAmount`
- `FundWalletRequest.Amount`
- `PendingTransactionRequest.Amount`
- `SimulateInflowRequest.Amount`
- Various response models (`WalletDetails`, `CheckoutWallet`, etc.)

#### KYC Verify Parameter Type Change

The `Verify` parameter in KYC requests changed from `int?` to `string?`:

```csharp
// Before (v1.x)
var request = new BvnKycUpgradeRequest { Verify = 1 };

// After (v2.x)
var request = new BvnKycUpgradeRequest { Verify = "1" };
```

#### ExpiryDurationMinutes Now Optional

`GenerateCheckoutWalletRequest.ExpiryDurationMinutes` is now optional and defaults to 30 minutes:

```csharp
// Before - was required
var request = new GenerateCheckoutWalletRequest { ExpiryDurationMinutes = 30 };

// After - optional, defaults to 30
var request = new GenerateCheckoutWalletRequest { /* uses default */ };
```

### Breaking Changes in the Checkout API Update

The Checkout service was updated to match the current [Checkout API documentation](https://developer.embedly.ng/api-reference/checkout). See the [Checkout section](#checkout-dynamic-account-generation) for full examples, including the new split payment methods.

#### Organization Prefix Mappings Use a New Endpoint and Response

`GetOrganizationPrefixMappingsAsync` now calls `GET /api/v1/prefix-map/me` and supports pagination and search. The optional `page`, `pageSize` and `search` parameters come before the cancellation token, so a token passed positionally must now be named:

```csharp
// Before
var prefixes = await embedlyClient.Checkout.GetOrganizationPrefixMappingsAsync(organizationId, cancellationToken);

// After
var prefixes = await embedlyClient.Checkout.GetOrganizationPrefixMappingsAsync(
    organizationId, cancellationToken: cancellationToken);

// Or with the new request object
var prefixes = await embedlyClient.Checkout.GetOrganizationPrefixMappingsAsync(
    new GetOrganizationPrefixMappingsRequest { OrganizationId = organizationId, Page = 1, PageSize = 10 });
```

`OrganizationPrefixMapping` now matches the new response:

- **Removed:** `PrimaryPrefix`, `BankName`, `BankCode`, `IsActive`, `CreatedAt`, `UpdatedAt`
- **Added:** `PrimaryPrefixId`, `Alias`, `OrganizationName`, `OrganizationIsActive` (a string such as `"active"`, not a `bool`)

#### `GetCheckoutWalletsRequest` Changes

- `PageSize` now defaults to `10` (was `20`).
- `StartDate` and `EndDate` changed from `DateTime?` to `string?` and are sent to the API as given, rather than being formatted by the SDK.
- New optional `OrganizationPrefixMappingId` (`Guid?`) filter.

```csharp
// Before
var request = new GetCheckoutWalletsRequest { OrganizationId = organizationId, StartDate = new DateTime(2025, 11, 1) };

// After
var request = new GetCheckoutWalletsRequest { OrganizationId = organizationId, StartDate = "2025-11-01" };
```

#### `CheckoutWallet` Properties Removed

`OrganizationPrefixMappingId`, `PrimaryPrefix`, `SecondaryPrefix` and `AutoGeneratedSuffix` were removed. The Checkout API no longer returns them.

`CheckoutWallet` also gained new properties: `CheckoutRef`, `InvoiceReference`, `Description`, `CurrencyCode`, `CustomerEmail`, `CustomerName`, `Metadata`, `SplitType`, `SplitConfigurations`, `WalletHistories` and `Transactions` (the last is populated by `GetCheckoutWalletWithTransactionsAsync`).

#### Empty Organization IDs Are Rejected

All Checkout methods now throw `ArgumentException` (parameter name `organizationId`) when the organization ID is `Guid.Empty`, instead of sending it to the API. The split beneficiary activate/deactivate methods do the same for an empty `BeneficiaryId`.

#### Behavior Changes

- **`ApiResponse<T>.Success` for Checkout responses.** The Checkout API returns `statusCode` but no `success` field, so these responses previously always reported `Success == false`. When a response has neither `success` nor `succeeded`, `Success` is now `true` for a 2xx `statusCode`. Responses that include `success` or `succeeded` are unaffected.
- **Checkout pagination.** `ApiResponse<T>.Pagination` is now populated from the Checkout API's `currentPage`, `totalCount`, `hasNextPage` and `hasPreviousPage` fields (exposed through the existing `Page`, `TotalItems`, `HasNext` and `HasPrevious` properties). When a `PaginationInfo` is serialized, both sets of names are written.

## Support

- 📖 [API Documentation](https://developer.embedly.ng)
- 💬 [Community Forum](https://community.embedly.ng)
- 📧 [Support Email](mailto:support@embedly.ng)
- 🐛 [Report Issues](https://github.com/embedly/embedly-dotnet-sdk/issues)

---

Built with ❤️ for the Nigerian fintech ecosystem.