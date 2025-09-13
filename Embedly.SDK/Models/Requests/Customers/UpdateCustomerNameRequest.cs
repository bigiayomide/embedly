using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Embedly.SDK.Models.Requests.Customers;

/// <summary>
/// Request model for updating a customer's name.
/// </summary>
public sealed class UpdateCustomerNameRequest
{
    /// <summary>
    /// Gets or sets the customer ID (used for routing, not serialized).
    /// </summary>
    [JsonIgnore]
    public string CustomerId { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the customer's new first name.
    /// </summary>
    [Required(ErrorMessage = "First name is required")]
    [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters")]
    [JsonPropertyName("firstName")]
    public string FirstName { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the customer's new last name.
    /// </summary>
    [Required(ErrorMessage = "Last name is required")]
    [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters")]
    [JsonPropertyName("lastName")]
    public string LastName { get; set; } = string.Empty;
}