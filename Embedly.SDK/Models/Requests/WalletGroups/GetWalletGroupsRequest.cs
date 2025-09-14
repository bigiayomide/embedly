using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Embedly.SDK.Models.Requests.WalletGroups;

/// <summary>
///     Request model for getting wallet groups.
/// </summary>
public sealed class GetWalletGroupsRequest
{
    /// <summary>
    ///     Gets or sets the customer ID to filter wallet groups.
    /// </summary>
    public string? CustomerId { get; set; }

    /// <summary>
    ///     Gets or sets the page number (1-based).
    /// </summary>
    [Range(1, int.MaxValue, ErrorMessage = "Page number must be greater than 0")]
    public int Page { get; set; } = 1;

    /// <summary>
    ///     Gets or sets the number of items per page.
    /// </summary>
    [Range(1, 100, ErrorMessage = "Page size must be between 1 and 100")]
    public int PageSize { get; set; } = 50;

    /// <summary>
    ///     Converts to query parameters.
    /// </summary>
    public Dictionary<string, object?> ToQueryParameters()
    {
        var parameters = new Dictionary<string, object?>
        {
            ["page"] = Page,
            ["pageSize"] = PageSize
        };

        if (!string.IsNullOrWhiteSpace(CustomerId))
            parameters["customerId"] = CustomerId;

        return parameters;
    }
}