using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Embedly.SDK.Models.Requests.ProductLimits;
using Embedly.SDK.Models.Responses.Common;
using Embedly.SDK.Models.Responses.ProductLimits;

namespace Embedly.SDK.Services.ProductLimits;

/// <summary>
/// Interface for product limit management operations.
/// </summary>
public interface IProductLimitService
{
    /// <summary>
    /// Creates a new product limit.
    /// </summary>
    /// <param name="request">The product limit creation request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The created product limit.</returns>
    Task<ApiResponse<ProductLimit>> CreateProductLimitAsync(CreateProductLimitRequest request, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets a product limit by its ID.
    /// </summary>
    /// <param name="limitId">The product limit ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The product limit details.</returns>
    Task<ApiResponse<ProductLimit>> GetProductLimitAsync(string limitId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets product limits based on the request criteria.
    /// </summary>
    /// <param name="request">The product limits request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Paginated list of product limits.</returns>
    Task<ApiResponse<PaginatedResponse<ProductLimit>>> GetProductLimitsAsync(GetProductLimitsRequest request, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets all limits for a specific product.
    /// </summary>
    /// <param name="productId">The product ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of product limits.</returns>
    Task<ApiResponse<List<ProductLimit>>> GetProductLimitsByProductAsync(string productId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Updates a product limit.
    /// </summary>
    /// <param name="limitId">The product limit ID.</param>
    /// <param name="request">The update request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The updated product limit.</returns>
    Task<ApiResponse<ProductLimit>> UpdateProductLimitAsync(string limitId, CreateProductLimitRequest request, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Activates a product limit.
    /// </summary>
    /// <param name="limitId">The product limit ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The updated product limit.</returns>
    Task<ApiResponse<ProductLimit>> ActivateProductLimitAsync(string limitId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Deactivates a product limit.
    /// </summary>
    /// <param name="limitId">The product limit ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The updated product limit.</returns>
    Task<ApiResponse<ProductLimit>> DeactivateProductLimitAsync(string limitId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Resets the usage counter for a product limit.
    /// </summary>
    /// <param name="limitId">The product limit ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The updated product limit.</returns>
    Task<ApiResponse<ProductLimit>> ResetProductLimitUsageAsync(string limitId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Deletes a product limit.
    /// </summary>
    /// <param name="limitId">The product limit ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Success response.</returns>
    Task<ApiResponse<object>> DeleteProductLimitAsync(string limitId, CancellationToken cancellationToken = default);
}