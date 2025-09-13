using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Embedly.SDK.Configuration;
using Embedly.SDK.Http;
using Embedly.SDK.Models.Requests.Products;
using Embedly.SDK.Models.Responses.Common;
using Embedly.SDK.Models.Responses.Products;
using Embedly.SDK.Helpers;

namespace Embedly.SDK.Services.Products;

/// <summary>
/// Service for product management operations.
/// </summary>
internal sealed class ProductService : BaseService, IProductService
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ProductService"/> class.
    /// </summary>
    /// <param name="httpClient">The HTTP client.</param>
    /// <param name="options">The configuration options.</param>
    public ProductService(IEmbedlyHttpClient httpClient, IOptions<EmbedlyOptions> options)
        : base(httpClient, options)
    {
    }

    /// <inheritdoc />
    public async Task<ApiResponse<Product>> CreateProductAsync(CreateProductRequest request, CancellationToken cancellationToken = default)
    {
        Guard.ThrowIfNull(request);
        
        var url = BuildUrl(ServiceUrls.Base, "api/v1/products/add");
        return await HttpClient.PostAsync<CreateProductRequest, Product>(url, request, cancellationToken);
    }


    /// <inheritdoc />
    public async Task<ApiResponse<PaginatedResponse<Product>>> GetProductsAsync(GetProductsRequest request, CancellationToken cancellationToken = default)
    {
        Guard.ThrowIfNull(request);
        
        var url = BuildUrl(ServiceUrls.Base, "api/v1/products/get");
        return await HttpClient.GetAsync<PaginatedResponse<Product>>(url, request.ToQueryParameters(), cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<Product>> UpdateProductAsync(string productId, CreateProductRequest request, CancellationToken cancellationToken = default)
    {
        Guard.ThrowIfNullOrWhiteSpace(productId);
        Guard.ThrowIfNull(request);
        
        var requestWithId = new { productId, product = request };
        
        var url = BuildUrl(ServiceUrls.Base, "api/v1/products/update");
        return await HttpClient.PatchAsync<object, Product>(url, requestWithId, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<Product>> ActivateProductAsync(string productId, CancellationToken cancellationToken = default)
    {
        Guard.ThrowIfNullOrWhiteSpace(productId);
        
        var url = BuildUrl(ServiceUrls.Base, $"api/v1/products/activate/{productId}");
        return await HttpClient.PatchAsync<object, Product>(url, new { }, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<Product>> DeactivateProductAsync(string productId, CancellationToken cancellationToken = default)
    {
        Guard.ThrowIfNullOrWhiteSpace(productId);
        
        var url = BuildUrl(ServiceUrls.Base, $"api/v1/products/deactivate/{productId}");
        return await HttpClient.PatchAsync<object, Product>(url, new { }, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<object>> DeleteProductAsync(string productId, CancellationToken cancellationToken = default)
    {
        Guard.ThrowIfNullOrWhiteSpace(productId);
        
        var url = BuildUrl(ServiceUrls.Base, $"api/v1/products/{productId}");
        return await HttpClient.DeleteAsync<object>(url, cancellationToken);
    }

    // ===== PRODUCT LIMITS OPERATIONS =====

    /// <inheritdoc />
    public async Task<ApiResponse<Models.Responses.Products.ProductLimits>> GetDefaultLimitsAsync(CancellationToken cancellationToken = default)
    {
        var url = BuildUrl(ServiceUrls.Base, "api/v1/limits/default");
        return await HttpClient.GetAsync<Models.Responses.Products.ProductLimits>(url, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<Models.Responses.Products.ProductLimits>> GetProductLimitsAsync(Guid productId, Guid currencyId, CancellationToken cancellationToken = default)
    {
        var url = BuildUrl(ServiceUrls.Base, $"api/v1/limits/product/{productId}/currency/{currencyId}");
        return await HttpClient.GetAsync<Models.Responses.Products.ProductLimits>(url, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<Models.Responses.Products.ProductLimits>> GetProductLimitsByCustomerAsync(Guid productId, Guid currencyId, string customerId, CancellationToken cancellationToken = default)
    {
        Guard.ThrowIfNullOrWhiteSpace(customerId);
        
        var url = BuildUrl(ServiceUrls.Base, $"api/v1/limits/product/{productId}/currency/{currencyId}/customer/{customerId}");
        return await HttpClient.GetAsync<Models.Responses.Products.ProductLimits>(url, cancellationToken);
    }
}