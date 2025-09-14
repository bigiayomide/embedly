using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Embedly.SDK.Models.Requests.ProductLimits;

/// <summary>
///     Request model for getting product limits.
/// </summary>
public sealed class GetProductLimitsRequest
{
    /// <summary>
    ///     Gets or sets the product ID filter.
    /// </summary>
    public string? ProductId { get; set; }

    /// <summary>
    ///     Gets or sets the limit type filter.
    /// </summary>
    public string? Type { get; set; }

    /// <summary>
    ///     Gets or sets the active status filter.
    /// </summary>
    public bool? IsActive { get; set; }

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

        if (!string.IsNullOrWhiteSpace(ProductId))
            parameters["productId"] = ProductId;

        if (!string.IsNullOrWhiteSpace(Type))
            parameters["type"] = Type;

        if (IsActive.HasValue)
            parameters["isActive"] = IsActive.Value;

        return parameters;
    }
}