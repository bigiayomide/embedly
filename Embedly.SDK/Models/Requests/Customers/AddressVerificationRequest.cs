using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Embedly.SDK.Models.Requests.Customers;

/// <summary>
///     Request model for customer address verification.
/// </summary>
public sealed class AddressVerificationRequest
{
    /// <summary>
    ///     Gets or sets the customer identifier.
    /// </summary>
    [Required(ErrorMessage = "Customer ID is required")]
    [JsonPropertyName("customerId")]
    public string CustomerId { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the customers electricity meter number.
    /// </summary>
    [Required(ErrorMessage = "The meter number is required")]
    [JsonPropertyName("meterNumber")]
    public string MeterNumber { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the customer's house address.
    /// </summary>
    [Required(ErrorMessage = "The house address is required")]
    [JsonPropertyName("houseAddress")]
    public string HouseAddress { get; set; } = string.Empty;
}