using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Embedly.SDK.Models.Requests.Wallets;

/// <summary>
///     Request model for getting wallet transaction history by account number.
/// </summary>
public sealed record GetWalletHistoryByAccountRequest
{
    /// <summary>
    ///     Gets or sets the account number.
    /// </summary>
    [Required(ErrorMessage = "Account number is required")]
    [JsonPropertyName("accountNumber")]
    public string AccountNumber { get; init; } = string.Empty;

    /// <summary>
    ///     Gets or sets the start date for the history query.
    /// </summary>
    [Required(ErrorMessage = "From date is required")]
    [JsonPropertyName("from")]
    public DateTime From { get; init; }

    /// <summary>
    ///     Gets or sets the end date for the history query.
    /// </summary>
    [Required(ErrorMessage = "To date is required")]
    [JsonPropertyName("to")]
    public DateTime To { get; init; }

    /// <summary>
    ///     Gets or sets the page number for pagination.
    /// </summary>
    [JsonPropertyName("pageNumber")]
    public int? PageNumber { get; init; }

    /// <summary>
    ///     Gets or sets the page size for pagination.
    /// </summary>
    [JsonPropertyName("pageSize")]
    public int? PageSize { get; init; }

    /// <summary>
    ///     Converts the request to query parameters for the URL.
    /// </summary>
    public Dictionary<string, object?> ToQueryParameters()
    {
        var queryParams = new Dictionary<string, object?>
        {
            { "AccountNumber", AccountNumber },
            { "From", From.ToString("yyyy-MM-dd") },
            { "To", To.ToString("yyyy-MM-dd") }
        };

        if (PageNumber.HasValue)
            queryParams["PageNumber"] = PageNumber.Value;

        if (PageSize.HasValue)
            queryParams["PageSize"] = PageSize.Value;

        return queryParams;
    }
}
