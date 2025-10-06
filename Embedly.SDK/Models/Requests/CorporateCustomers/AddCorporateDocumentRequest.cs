using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Embedly.SDK.Models.Requests.CorporateCustomers;

/// <summary>
///     Request model for adding a document to a corporate customer.
/// </summary>
public sealed class AddCorporateDocumentRequest
{
    /// <summary>
    ///     Gets or sets the CAC document as base64-encoded string.
    /// </summary>
    [JsonPropertyName("cac")]
    [Base64String(ErrorMessage = "CAC document must be a valid base64-encoded string")]
    public string? Cac { get; set; }

    /// <summary>
    ///     Gets or sets the TIN document as base64-encoded string.
    /// </summary>
    [JsonPropertyName("tin")]
    [Base64String(ErrorMessage = "TIN document must be a valid base64-encoded string")]
    public string? Tin { get; set; }

    /// <summary>
    ///     Gets or sets the board resolution document as base64-encoded string.
    /// </summary>
    [JsonPropertyName("boardResolution")]
    [Base64String(ErrorMessage = "Board resolution document must be a valid base64-encoded string")]
    public string? BoardResolution { get; set; }

    /// <summary>
    ///     Gets or sets the utility bill document as base64-encoded string.
    /// </summary>
    [JsonPropertyName("utilityBill")]
    [Base64String(ErrorMessage = "Utility bill document must be a valid base64-encoded string")]
    public string? UtilityBill { get; set; }

    /// <summary>
    ///     Gets or sets the MEMART document as base64-encoded string.
    /// </summary>
    [JsonPropertyName("memart")]
    [Base64String(ErrorMessage = "MEMART document must be a valid base64-encoded string")]
    public string? Memart { get; set; }

    /// <summary>
    ///     Gets or sets the SCUML document as base64-encoded string.
    /// </summary>
    [JsonPropertyName("scuml")]
    [Base64String(ErrorMessage = "SCUML document must be a valid base64-encoded string")]
    public string? Scuml { get; set; }
}

/// <summary>
///     Validation attribute for base64-encoded strings.
/// </summary>
public sealed class Base64StringAttribute : ValidationAttribute
{
    /// <summary>
    ///     Determines whether the specified value is a valid base64-encoded string.
    /// </summary>
    /// <param name="value">The value to validate.</param>
    /// <returns>true if the value is a valid base64 string or null; otherwise, false.</returns>
    public override bool IsValid(object? value)
    {
        if (value is null)
            return true; // Allow null values

        if (value is not string stringValue)
            return false;

        if (string.IsNullOrWhiteSpace(stringValue))
            return true; // Allow empty strings

        try
        {
            // Try to convert from base64 to validate format
            Convert.FromBase64String(stringValue);
            return true;
        }
        catch (FormatException)
        {
            return false;
        }
    }
}