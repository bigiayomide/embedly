using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Embedly.SDK.Models.Responses.Checkout;

/// <summary>
///     Paginated list of split beneficiaries.
/// </summary>
public sealed class SplitBeneficiaryListResponse
{
    /// <summary>
    ///     Gets or sets the split beneficiaries on the current page.
    /// </summary>
    [JsonPropertyName("beneficiaries")]
    public List<SplitBeneficiary> Beneficiaries { get; set; } = new();

    /// <summary>
    ///     Gets or sets the current page number.
    /// </summary>
    [JsonPropertyName("page")]
    public int Page { get; set; }

    /// <summary>
    ///     Gets or sets the page size.
    /// </summary>
    [JsonPropertyName("pageSize")]
    public int PageSize { get; set; }

    /// <summary>
    ///     Gets or sets the total number of beneficiaries across all pages.
    /// </summary>
    [JsonPropertyName("totalCount")]
    public int TotalCount { get; set; }

    /// <summary>
    ///     Gets or sets the total number of pages.
    /// </summary>
    [JsonPropertyName("totalPages")]
    public int TotalPages { get; set; }
}
