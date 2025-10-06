using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Embedly.SDK.Models.Requests.Customers;

/// <summary>
///     Request model for upgrading customer KYC using NIN (National Identification Number).
/// </summary>
public sealed class NinKycUpgradeRequest
{
    /// <summary>
    ///     Gets or sets the customer identifier.
    /// </summary>
    [Required(ErrorMessage = "Customer ID is required")]
    [JsonPropertyName("customerId")]
    public string CustomerId { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the National Identification Number (NIN).
    /// </summary>
    [Required(ErrorMessage = "NIN is required")]
    [RegularExpression(@"^\d{11}$", ErrorMessage = "NIN must be 11 digits")]
    [JsonPropertyName("nin")]
    public string Nin { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the customer's date of birth for verification.
    /// </summary>
    [Required(ErrorMessage = "Date of birth is required")]
    [JsonPropertyName("dob")]
    public DateTime DateOfBirth { get; set; }

    /// <summary>
    ///     Gets or sets the last name of the individual.
    /// </summary>
    [Required(ErrorMessage = "The lastname field is required.")]
    [JsonPropertyName("lastname")]
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the first name of the individual.
    /// </summary>
    [Required(ErrorMessage = "The firstname field is required.")]
    [JsonPropertyName("firstname")]
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    ///     The number verification attempts to be made.
    /// </summary>
    [JsonPropertyName("verify")]
    public int? Verify { get; set; }

    /// <summary>
    ///     Adds query parameters to the request endpoint.
    /// </summary>
    /// <returns>A simple query parameters dictionary.</returns>
    public Dictionary<string, object?> ToQueryParameters()
    {
        var parameters = new Dictionary<string, object?>
        {
            ["customerId"] = CustomerId,
            ["nin"] = Nin,
            ["verify"] = Verify
        };

        return parameters;
    }
}