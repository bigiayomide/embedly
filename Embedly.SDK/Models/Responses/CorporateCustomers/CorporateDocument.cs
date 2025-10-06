using System;
using System.Text.Json.Serialization;

namespace Embedly.SDK.Models.Responses.CorporateCustomers;

/// <summary>
///     Represents a document associated with a corporate customer.
/// </summary>
public sealed class CorporateDocument
{
    /// <summary>
    ///     Gets or sets the unique document identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    /// <summary>
    ///     Gets or sets the CAC document URL.
    /// </summary>
    [JsonPropertyName("cac")]
    public string? Cac { get; set; }

    /// <summary>
    ///     Gets or sets the TIN document URL.
    /// </summary>
    [JsonPropertyName("tin")]
    public string? Tin { get; set; }

    /// <summary>
    ///     Gets or sets the board resolution document URL.
    /// </summary>
    [JsonPropertyName("boardResolution")]
    public string? BoardResolution { get; set; }

    /// <summary>
    ///     Gets or sets the utility bill document URL.
    /// </summary>
    [JsonPropertyName("utilityBill")]
    public string? UtilityBill { get; set; }

    /// <summary>
    ///     Gets or sets the MEMART document URL.
    /// </summary>
    [JsonPropertyName("memart")]
    public string? Memart { get; set; }

    /// <summary>
    ///     Gets or sets the SCUML document URL.
    /// </summary>
    [JsonPropertyName("scuml")]
    public string? Scuml { get; set; }

    /// <summary>
    ///     Gets or sets the date when the document was uploaded.
    /// </summary>
    [JsonPropertyName("dateCreated")]
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    ///     Gets or sets the date when the document was last updated.
    /// </summary>
    [JsonPropertyName("dateModified")]
    public DateTimeOffset? UpdatedAt { get; set; }
}

/// <summary>
///     Document verification status.
/// </summary>
public enum DocumentVerificationStatus
{
    /// <summary>
    ///     Document is not verified.
    /// </summary>
    [JsonPropertyName("unverified")]
    Unverified,

    /// <summary>
    ///     Document verification is pending.
    /// </summary>
    [JsonPropertyName("pending")]
    Pending,

    /// <summary>
    ///     Document is verified.
    /// </summary>
    [JsonPropertyName("verified")]
    Verified,

    /// <summary>
    ///     Document verification was rejected.
    /// </summary>
    [JsonPropertyName("rejected")]
    Rejected,

    /// <summary>
    ///     Document has expired.
    /// </summary>
    [JsonPropertyName("expired")]
    Expired
}

/// <summary>
///     Document status.
/// </summary>
public enum DocumentStatus
{
    /// <summary>
    ///     Document is active.
    /// </summary>
    [JsonPropertyName("active")]
    Active,

    /// <summary>
    ///     Document is archived.
    /// </summary>
    [JsonPropertyName("archived")]
    Archived,

    /// <summary>
    ///     Document was deleted.
    /// </summary>
    [JsonPropertyName("deleted")]
    Deleted
}