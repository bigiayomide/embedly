using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Embedly.SDK.Models.Requests.CorporateCustomers;

namespace Embedly.SDK.Models.Requests.Checkout;

/// <summary>
///     Request model for getting an organization's checkout prefix mappings with pagination and search.
/// </summary>
public sealed class GetOrganizationPrefixMappingsRequest
{
    /// <summary>
    ///     Gets or sets the organization ID.
    /// </summary>
    [NonEmptyGuid(ErrorMessage = "Organization ID is required")]
    public Guid OrganizationId { get; set; }

    /// <summary>
    ///     Gets or sets the page number (1-based).
    /// </summary>
    [Range(1, int.MaxValue, ErrorMessage = "Page must be greater than 0")]
    public int Page { get; set; } = 1;

    /// <summary>
    ///     Gets or sets the page size.
    /// </summary>
    [Range(1, int.MaxValue, ErrorMessage = "Page size must be greater than 0")]
    public int PageSize { get; set; } = 10;

    /// <summary>
    ///     Gets or sets the search filter.
    /// </summary>
    public string? Search { get; set; }

    /// <summary>
    ///     Converts to a query parameters dictionary.
    /// </summary>
    public Dictionary<string, object?> ToQueryParameters()
    {
        var parameters = new Dictionary<string, object?>
        {
            ["organizationId"] = OrganizationId,
            ["page"] = Page,
            ["pageSize"] = PageSize
        };

        if (!string.IsNullOrWhiteSpace(Search))
            parameters["search"] = Search;

        return parameters;
    }
}
