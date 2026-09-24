using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Embedly.SDK.Models.Requests.CorporateCustomers;

namespace Embedly.SDK.Models.Requests.Checkout;

/// <summary>
///     Request model for getting organization checkout wallets with filtering and pagination.
/// </summary>
public sealed class GetCheckoutWalletsRequest
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
    [Range(1, 100, ErrorMessage = "Page size must be between 1 and 100")]
    public int PageSize { get; set; } = 10;

    /// <summary>
    ///     Gets or sets the wallet status filter.
    ///     Valid values: "Used", "Failed", "Reversed", "Completed", "Expired".
    /// </summary>
    public string? Status { get; set; }

    /// <summary>
    ///     Gets or sets the start date filter.
    /// </summary>
    public string? StartDate { get; set; }

    /// <summary>
    ///     Gets or sets the end date filter.
    /// </summary>
    public string? EndDate { get; set; }

    /// <summary>
    ///     Gets or sets the wallet number filter.
    /// </summary>
    public string? WalletNumber { get; set; }

    /// <summary>
    ///     Gets or sets the organization prefix mapping ID filter.
    /// </summary>
    public Guid? OrganizationPrefixMappingId { get; set; }

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

        if (!string.IsNullOrWhiteSpace(Status))
            parameters["status"] = Status;

        if (!string.IsNullOrWhiteSpace(StartDate))
            parameters["startDate"] = StartDate;

        if (!string.IsNullOrWhiteSpace(EndDate))
            parameters["endDate"] = EndDate;

        if (!string.IsNullOrWhiteSpace(WalletNumber))
            parameters["walletNumber"] = WalletNumber;

        if (OrganizationPrefixMappingId.HasValue)
            parameters["organizationPrefixMappingId"] = OrganizationPrefixMappingId.Value;

        return parameters;
    }
}
