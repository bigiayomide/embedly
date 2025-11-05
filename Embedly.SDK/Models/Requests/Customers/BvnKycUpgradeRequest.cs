using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Embedly.SDK.Models.Requests.Customers;

/// <summary>
///     Request model for upgrading customer KYC using BVN (Bank Verification Number).
/// </summary>
public sealed class BvnKycUpgradeRequest
{
    /// <summary>
    ///     Gets or sets the customer identifier.
    /// </summary>
    [Required(ErrorMessage = "Customer ID is required")]
    [JsonPropertyName("customerId")]
    public string CustomerId { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the Bank Verification Number (BVN).
    /// </summary>
    [Required(ErrorMessage = "BVN is required")]
    [RegularExpression(@"^\d{11}$", ErrorMessage = "BVN must be 11 digits")]
    [JsonPropertyName("bvn")]
    public string Bvn { get; set; } = string.Empty;

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
            ["verify"] = Verify
        };

        return parameters;
    }
}