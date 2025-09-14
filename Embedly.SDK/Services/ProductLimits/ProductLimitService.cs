using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Embedly.SDK.Configuration;
using Embedly.SDK.Http;
using Embedly.SDK.Models.Requests.ProductLimits;
using Embedly.SDK.Models.Responses.Common;
using Embedly.SDK.Models.Responses.ProductLimits;
using Embedly.SDK.Helpers;

namespace Embedly.SDK.Services.ProductLimits;

/// <summary>
/// Service for product limit management operations.
/// </summary>
internal sealed class ProductLimitService : BaseService, IProductLimitService
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ProductLimitService"/> class.
    /// </summary>
    /// <param name="httpClient">The HTTP client.</param>
    /// <param name="options">The configuration options.</param>
    public ProductLimitService(IEmbedlyHttpClient httpClient, IOptions<EmbedlyOptions> options)
        : base(httpClient, options)
    {
    }

    /// <inheritdoc />
    public async Task<ApiResponse<ProductLimit>> CreateProductLimitAsync(CreateProductLimitRequest request, CancellationToken cancellationToken = default)
    {
        Guard.ThrowIfNull(request, nameof(request));
        
        var url = BuildUrl("api/v1/product-limits");
        return await HttpClient.PostAsync<CreateProductLimitRequest, ProductLimit>(url, request, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<ProductLimit>> GetProductLimitAsync(string limitId, CancellationToken cancellationToken = default)
    {
        Guard.ThrowIfNullOrWhiteSpace(limitId, nameof(limitId));
        
        var url = BuildUrl($"api/v1/product-limits/{limitId}");
        return await HttpClient.GetAsync<ProductLimit>(url, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<PaginatedResponse<ProductLimit>>> GetProductLimitsAsync(GetProductLimitsRequest request, CancellationToken cancellationToken = default)
    {
        Guard.ThrowIfNull(request, nameof(request));
        
        var url = BuildUrl("api/v1/product-limits");
        return await HttpClient.GetAsync<PaginatedResponse<ProductLimit>>(url, request.ToQueryParameters(), cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<List<ProductLimit>>> GetProductLimitsByProductAsync(string productId, CancellationToken cancellationToken = default)
    {
        Guard.ThrowIfNullOrWhiteSpace(productId, nameof(productId));
        
        var queryParams = new Dictionary<string, object?>
        {
            ["productId"] = productId
        };
        
        var url = BuildUrl($"api/v1/products/{productId}/limits");
        return await HttpClient.GetAsync<List<ProductLimit>>(url, queryParams, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<ProductLimit>> UpdateProductLimitAsync(string limitId, CreateProductLimitRequest request, CancellationToken cancellationToken = default)
    {
        Guard.ThrowIfNullOrWhiteSpace(limitId, nameof(limitId));
        Guard.ThrowIfNull(request, nameof(request));
        
        var url = BuildUrl($"api/v1/product-limits/{limitId}");
        return await HttpClient.PutAsync<CreateProductLimitRequest, ProductLimit>(url, request, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<ProductLimit>> ActivateProductLimitAsync(string limitId, CancellationToken cancellationToken = default)
    {
        Guard.ThrowIfNullOrWhiteSpace(limitId, nameof(limitId));
        
        var url = BuildUrl($"api/v1/product-limits/{limitId}/activate");
        return await HttpClient.PatchAsync<object, ProductLimit>(url, new { }, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<ProductLimit>> DeactivateProductLimitAsync(string limitId, CancellationToken cancellationToken = default)
    {
        Guard.ThrowIfNullOrWhiteSpace(limitId, nameof(limitId));
        
        var url = BuildUrl($"api/v1/product-limits/{limitId}/deactivate");
        return await HttpClient.PatchAsync<object, ProductLimit>(url, new { }, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<ProductLimit>> ResetProductLimitUsageAsync(string limitId, CancellationToken cancellationToken = default)
    {
        Guard.ThrowIfNullOrWhiteSpace(limitId, nameof(limitId));
        
        var url = BuildUrl($"api/v1/product-limits/{limitId}/reset");
        return await HttpClient.PatchAsync<object, ProductLimit>(url, new { }, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<object>> DeleteProductLimitAsync(string limitId, CancellationToken cancellationToken = default)
    {
        Guard.ThrowIfNullOrWhiteSpace(limitId, nameof(limitId));
        
        var url = BuildUrl($"api/v1/product-limits/{limitId}");
        return await HttpClient.DeleteAsync<object>(url, cancellationToken);
    }
}