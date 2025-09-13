using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Embedly.SDK.Models.Requests.Customers;

/// <summary>
/// Simple request for getting customers with pagination and filtering.
/// </summary>
public sealed class GetCustomersRequest
{
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
    /// Gets or sets the customer status filter.
    /// </summary>
    public string? Status { get; set; }
    
    /// <summary>
    /// Gets or sets the verification status filter.
    /// </summary>
    public string? VerificationStatus { get; set; }
    
    /// <summary>
    /// Gets or sets the search query.
    /// </summary>
    public string? Search { get; set; }
    
    /// <summary>
    /// Converts to a simple query parameters dictionary - no reflection needed!
    /// </summary>
    public Dictionary<string, object?> ToQueryParameters()
    {
        var parameters = new Dictionary<string, object?>
        {
            ["page"] = Page,
            ["pageSize"] = PageSize
        };
        
        if (!string.IsNullOrWhiteSpace(Status))
            parameters["status"] = Status;
            
        if (!string.IsNullOrWhiteSpace(VerificationStatus))
            parameters["verificationStatus"] = VerificationStatus;
            
        if (!string.IsNullOrWhiteSpace(Search))
            parameters["search"] = Search;
            
        return parameters;
    }
}