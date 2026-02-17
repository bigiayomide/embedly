using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Embedly.SDK.Models.Responses.Wallets;

/// <summary>
///     Paginated response for wallet transaction history.
/// </summary>
public sealed class WalletHistoryResponse
{
    /// <summary>
    ///     Gets or sets the list of wallet transaction history entries.
    /// </summary>
    [JsonPropertyName("walletHistories")]
    public List<WalletTransaction> WalletHistories { get; set; } = new();

    /// <summary>
    ///     Gets or sets the total count of transactions.
    /// </summary>
    [JsonPropertyName("totalCount")]
    public int TotalCount { get; set; }

    /// <summary>
    ///     Gets or sets the total number of pages.
    /// </summary>
    [JsonPropertyName("totalPages")]
    public int TotalPages { get; set; }

    /// <summary>
    ///     Gets or sets the current page number.
    /// </summary>
    [JsonPropertyName("currentPage")]
    public int CurrentPage { get; set; }

    /// <summary>
    ///     Gets or sets the page size.
    /// </summary>
    [JsonPropertyName("pageSize")]
    public int PageSize { get; set; }
}
