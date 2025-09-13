using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Embedly.SDK.Models.Requests.Wallets;

/// <summary>
/// Request model for getting wallet transaction history.
/// </summary>
public sealed class GetWalletTransactionsRequest
{
    /// <summary>
    /// Gets or sets the wallet ID.
    /// </summary>
    [Required(ErrorMessage = "Wallet ID is required")]
    public string WalletId { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the page number (1-based).
    /// </summary>
    [Range(1, int.MaxValue, ErrorMessage = "Page number must be greater than 0")]
    public int Page { get; set; } = 1;
    
    /// <summary>
    /// Gets or sets the number of items per page.
    /// </summary>
    [Range(1, 100, ErrorMessage = "Page size must be between 1 and 100")]
    public int PageSize { get; set; } = 50;
    
    /// <summary>
    /// Gets or sets the transaction type filter.
    /// </summary>
    public string? TransactionType { get; set; }
    
    /// <summary>
    /// Gets or sets the start date filter.
    /// </summary>
    public DateTime? StartDate { get; set; }
    
    /// <summary>
    /// Gets or sets the end date filter.
    /// </summary>
    public DateTime? EndDate { get; set; }
    
    /// <summary>
    /// Converts to query parameters.
    /// </summary>
    public Dictionary<string, object?> ToQueryParameters()
    {
        var parameters = new Dictionary<string, object?>
        {
            ["page"] = Page,
            ["pageSize"] = PageSize
        };
        
        if (!string.IsNullOrWhiteSpace(TransactionType))
            parameters["transactionType"] = TransactionType;
            
        if (StartDate.HasValue)
            parameters["startDate"] = StartDate.Value.ToString("yyyy-MM-dd");
            
        if (EndDate.HasValue)
            parameters["endDate"] = EndDate.Value.ToString("yyyy-MM-dd");
            
        return parameters;
    }
}