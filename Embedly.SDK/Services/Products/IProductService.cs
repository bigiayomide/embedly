using System;
using System.Threading;
using System.Threading.Tasks;
using Embedly.SDK.Models.Requests.Products;
using Embedly.SDK.Models.Responses.Common;
using Embedly.SDK.Models.Responses.Products;

namespace Embedly.SDK.Services.Products;

/// <summary>
///     Interface for product management operations.
/// </summary>
public interface IProductService
{
    /// <summary>
    ///     Creates a new product.
    /// </summary>
    /// <param name="request">The product creation request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The created product.</returns>
    Task<ApiResponse<Product>> CreateProductAsync(CreateProductRequest request,
        CancellationToken cancellationToken = default);


    /// <summary>
    ///     Gets products based on the request criteria.
    /// </summary>
    /// <param name="request">The products request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Paginated list of products.</returns>
    Task<ApiResponse<PaginatedResponse<Product>>> GetProductsAsync(GetProductsRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Updates a product.
    /// </summary>
    /// <param name="productId">The product ID.</param>
    /// <param name="request">The update request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The updated product.</returns>
    Task<ApiResponse<Product>> UpdateProductAsync(string productId, CreateProductRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Activates a product.
    /// </summary>
    /// <param name="productId">The product ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The updated product.</returns>
    Task<ApiResponse<Product>> ActivateProductAsync(string productId, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Deactivates a product.
    /// </summary>
    /// <param name="productId">The product ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The updated product.</returns>
    Task<ApiResponse<Product>> DeactivateProductAsync(string productId, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Deletes a product.
    /// </summary>
    /// <param name="productId">The product ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Success response.</returns>
    Task<ApiResponse<object>> DeleteProductAsync(string productId, CancellationToken cancellationToken = default);

    // ===== PRODUCT LIMITS OPERATIONS =====

    /// <summary>
    ///     Gets default limits.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Default limits.</returns>
    Task<ApiResponse<Models.Responses.Products.ProductLimits>> GetDefaultLimitsAsync(
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Gets product limits by product and currency.
    /// </summary>
    /// <param name="productId">The product ID.</param>
    /// <param name="currencyId">The currency ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Product limits.</returns>
    Task<ApiResponse<Models.Responses.Products.ProductLimits>> GetProductLimitsAsync(Guid productId, Guid currencyId,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Gets product limits by product, currency, and customer.
    /// </summary>
    /// <param name="productId">The product ID.</param>
    /// <param name="currencyId">The currency ID.</param>
    /// <param name="customerId">The customer ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Product limits.</returns>
    Task<ApiResponse<Models.Responses.Products.ProductLimits>> GetProductLimitsByCustomerAsync(Guid productId,
        Guid currencyId, string customerId, CancellationToken cancellationToken = default);
}