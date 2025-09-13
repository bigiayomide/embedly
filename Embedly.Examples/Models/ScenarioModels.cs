namespace Embedly.Examples.Models;

/// <summary>
/// Base class for scenario requests.
/// </summary>
public abstract class ScenarioRequestBase
{
    public string CorrelationId { get; set; } = Guid.NewGuid().ToString("N")[..12].ToUpperInvariant();
    public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
    public Dictionary<string, object> Metadata { get; set; } = new();
}

/// <summary>
/// Base class for scenario results.
/// </summary>
public abstract class ScenarioResultBase
{
    public string CorrelationId { get; set; } = string.Empty;
    public DateTime StartedAt { get; set; }
    public DateTime CompletedAt { get; set; }
    public TimeSpan Duration => CompletedAt - StartedAt;
    public Dictionary<string, object> Metadata { get; set; } = new();
    public List<string> Steps { get; set; } = new();
}

/// <summary>
/// Customer information for scenarios.
/// </summary>
public class CustomerInfo
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string MiddleName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public string CustomerType { get; set; } = "INDIVIDUAL";
    public string? Bvn { get; set; }
    public string? Nin { get; set; }
    public AddressInfo? Address { get; set; }
}

/// <summary>
/// Address information.
/// </summary>
public class AddressInfo
{
    public string Street { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string Country { get; set; } = "NG";
    public string PostalCode { get; set; } = string.Empty;
}

/// <summary>
/// Wallet configuration for scenarios.
/// </summary>
public class WalletConfig
{
    public string Currency { get; set; } = "NGN";
    public string WalletType { get; set; } = "PRIMARY";
    public decimal? InitialFunding { get; set; }
    public Dictionary<string, object> Metadata { get; set; } = new();
}

/// <summary>
/// Card configuration for scenarios.
/// </summary>
public class CardConfig
{
    public string CardType { get; set; } = "VIRTUAL";
    public string PickupMethod { get; set; } = "DELIVERY";
    public string IdType { get; set; } = "NIN";
    public string? IdNumber { get; set; }
    public bool SetPin { get; set; } = true;
    public decimal? SpendingLimit { get; set; }
}

/// <summary>
/// Payout configuration for scenarios.
/// </summary>
public class PayoutConfig
{
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "NGN";
    public string PayoutType { get; set; } = "BANK_TRANSFER";
    public string AccountNumber { get; set; } = string.Empty;
    public string BankCode { get; set; } = string.Empty;
    public string AccountName { get; set; } = string.Empty;
    public string? Description { get; set; }
}

/// <summary>
/// Result of a completed scenario step.
/// </summary>
public class ScenarioStep
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime StartedAt { get; set; }
    public DateTime CompletedAt { get; set; }
    public TimeSpan Duration => CompletedAt - StartedAt;
    public bool IsSuccess { get; set; }
    public string? Error { get; set; }
    public Dictionary<string, object> Data { get; set; } = new();
}

/// <summary>
/// Created resource information.
/// </summary>
public class CreatedResource
{
    public string Type { get; set; } = string.Empty;
    public string Id { get; set; } = string.Empty;
    public string? Name { get; set; }
    public Dictionary<string, object> Properties { get; set; } = new();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

/// <summary>
/// Transaction information.
/// </summary>
public class TransactionInfo
{
    public string Id { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Currency { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string? Reference { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public Dictionary<string, object> Metadata { get; set; } = new();
}