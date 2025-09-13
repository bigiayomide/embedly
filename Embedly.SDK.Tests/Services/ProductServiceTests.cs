using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using Embedly.SDK.Models.Requests.Products;
using Embedly.SDK.Models.Responses.Products;
using Embedly.SDK.Models.Responses.Common;
using Embedly.SDK.Services.Products;
using Embedly.SDK.Tests.Testing;

namespace Embedly.SDK.Tests.Services;

/// <summary>
/// Comprehensive unit tests for ProductService following SDK patterns.
/// Tests all product management operations including CRUD and limits.
/// </summary>
[TestFixture]
public class ProductServiceTests : ServiceTestBase
{
    private ProductService _productService = null!;

    protected override void OnSetUp()
    {
        _productService = new ProductService(MockHttpClient.Object, MockOptions.Object);
    }

    #region Product CRUD Tests

    [Test]
    public async Task CreateProductAsync_WithValidRequest_ReturnsSuccessfulResponse()
    {
        // Arrange
        var request = CreateValidProductRequest();
        var expectedProduct = CreateTestProduct();
        var apiResponse = CreateSuccessfulApiResponse(expectedProduct);

        MockHttpClient
            .Setup(x => x.PostAsync<CreateProductRequest, Product>(
                It.IsAny<string>(),
                request,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _productService.CreateProductAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Name.Should().Be(expectedProduct.Name);
    }

    [Test]
    public void CreateProductAsync_WithNullRequest_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.ThrowsAsync<ArgumentNullException>(
            () => _productService.CreateProductAsync(null!));
    }

    [Test]
    public async Task GetProductsAsync_WithValidRequest_ReturnsPaginatedProducts()
    {
        // Arrange
        var request = CreateValidGetProductsRequest();
        var expectedProducts = new PaginatedResponse<Product>
        {
            Items = [CreateTestProduct(), CreateTestProduct()],
            Pagination = new PaginationInfo
            {
                Page = 1,
                PageSize = 10,
            }
        };

        var apiResponse = CreateSuccessfulApiResponse(expectedProducts);

        MockHttpClient
            .Setup(x => x.GetAsync<PaginatedResponse<Product>>(
                It.IsAny<string>(),
                It.IsAny<Dictionary<string, object?>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _productService.GetProductsAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Items.Should().HaveCount(2);
        result.Data.Pagination.TotalItems.Should().Be(2);
    }

    [Test]
    public async Task UpdateProductAsync_WithValidRequest_ReturnsUpdatedProduct()
    {
        // Arrange
        var productId = CreateTestGuid().ToString();
        var request = CreateValidProductRequest();
        var expectedProduct = CreateTestProduct();
        expectedProduct.Name = "Updated Product";
        var apiResponse = CreateSuccessfulApiResponse(expectedProduct);

        MockHttpClient
            .Setup(x => x.PutAsync<CreateProductRequest, Product>(
                It.Is<string>(url => url.Contains(productId)),
                request,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _productService.UpdateProductAsync(productId, request);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Name.Should().Be("Updated Product");
    }

    [Test]
    public async Task ActivateProductAsync_WithValidId_ReturnsActivatedProduct()
    {
        // Arrange
        var productId = CreateTestGuid().ToString();
        var expectedProduct = CreateTestProduct();
        expectedProduct.Status = ProductStatus.Active;
        var apiResponse = CreateSuccessfulApiResponse(expectedProduct);

        MockHttpClient
            .Setup(x => x.PatchAsync<object, Product>(
                It.Is<string>(url => url.Contains(productId) && url.Contains("activate")),
                It.IsAny<object>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _productService.ActivateProductAsync(productId);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Status.Should().Be(ProductStatus.Active);
    }

    [Test]
    public async Task DeactivateProductAsync_WithValidId_ReturnsDeactivatedProduct()
    {
        // Arrange
        var productId = CreateTestGuid().ToString();
        var expectedProduct = CreateTestProduct();
        expectedProduct.Status = ProductStatus.Inactive;
        var apiResponse = CreateSuccessfulApiResponse(expectedProduct);

        MockHttpClient
            .Setup(x => x.PatchAsync<object, Product>(
                It.Is<string>(url => url.Contains(productId) && url.Contains("deactivate")),
                It.IsAny<object>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _productService.DeactivateProductAsync(productId);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Status.Should().Be(ProductStatus.Inactive);
    }

    [Test]
    public async Task DeleteProductAsync_WithValidId_ReturnsSuccess()
    {
        // Arrange
        var productId = CreateTestGuid().ToString();
        var apiResponse = CreateSuccessfulApiResponse(new object());

        MockHttpClient
            .Setup(x => x.DeleteAsync<object>(
                It.Is<string>(url => url.Contains(productId)),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _productService.DeleteProductAsync(productId);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
    }

    #endregion

    #region Product Limits Tests

    [Test]
    public async Task GetDefaultLimitsAsync_ReturnsDefaultLimits()
    {
        // Arrange
        var expectedLimits = CreateTestProductLimits();
        var apiResponse = CreateSuccessfulApiResponse(expectedLimits);

        MockHttpClient
            .Setup(x => x.GetAsync<Models.Responses.Products.ProductLimits>(
                It.Is<string>(url => url.Contains("default")),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _productService.GetDefaultLimitsAsync();

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.DailyTransactionLimit.Should().Be(expectedLimits.DailyTransactionLimit);
    }

    [Test]
    public async Task GetProductLimitsAsync_WithValidIds_ReturnsProductLimits()
    {
        // Arrange
        var productId = CreateTestGuid();
        var currencyId = CreateTestGuid();
        var expectedLimits = CreateTestProductLimits();
        var apiResponse = CreateSuccessfulApiResponse(expectedLimits);

        MockHttpClient
            .Setup(x => x.GetAsync<Models.Responses.Products.ProductLimits>(
                It.Is<string>(url => url.Contains(productId.ToString()) && url.Contains(currencyId.ToString())),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _productService.GetProductLimitsAsync(productId, currencyId);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
    }

    [Test]
    public async Task GetProductLimitsByCustomerAsync_WithValidIds_ReturnsCustomerLimits()
    {
        // Arrange
        var productId = CreateTestGuid();
        var currencyId = CreateTestGuid();
        var customerId = CreateTestGuid().ToString();
        var expectedLimits = CreateTestProductLimits();
        var apiResponse = CreateSuccessfulApiResponse(expectedLimits);

        MockHttpClient
            .Setup(x => x.GetAsync<Models.Responses.Products.ProductLimits>(
                It.Is<string>(url => url.Contains(productId.ToString()) &&
                                    url.Contains(currencyId.ToString()) &&
                                    url.Contains(customerId)),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _productService.GetProductLimitsByCustomerAsync(productId, currencyId, customerId);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
    }

    #endregion

    #region Validation Tests

    [Test]
    public void UpdateProductAsync_WithNullProductId_ThrowsArgumentException()
    {
        // Arrange
        var request = CreateValidProductRequest();

        // Act & Assert
        Assert.ThrowsAsync<ArgumentException>(
            () => _productService.UpdateProductAsync(null!, request));
    }

    [Test]
    public void UpdateProductAsync_WithEmptyProductId_ThrowsArgumentException()
    {
        // Arrange
        var request = CreateValidProductRequest();

        // Act & Assert
        Assert.ThrowsAsync<ArgumentException>(
            () => _productService.UpdateProductAsync(string.Empty, request));
    }

    [Test]
    public void ActivateProductAsync_WithNullProductId_ThrowsArgumentException()
    {
        // Act & Assert
        Assert.ThrowsAsync<ArgumentException>(
            () => _productService.ActivateProductAsync(null!));
    }

    [Test]
    public void DeleteProductAsync_WithNullProductId_ThrowsArgumentException()
    {
        // Act & Assert
        Assert.ThrowsAsync<ArgumentException>(
            () => _productService.DeleteProductAsync(null!));
    }

    #endregion

    #region Helper Methods

    private CreateProductRequest CreateValidProductRequest()
    {
        return new CreateProductRequest
        {
            Name = "Test Product",
            Type = "TEST_PRODUCT",
            Description = "A test product for unit testing",
            IsActive = true
        };
    }

    private GetProductsRequest CreateValidGetProductsRequest()
    {
        return new GetProductsRequest
        {
            Page = 1,
            PageSize = 10,
            Type = null,
            IsActive = null
        };
    }

    private Product CreateTestProduct()
    {
        return new Product
        {
            Id = CreateTestGuid().ToString(),
            Name = "Test Product",
            Type = "TEST_PRODUCT",
            Description = "A test product for unit testing",
            Status = ProductStatus.Active,
            IsActive = true,
            CreatedAt = CreateTestTimestamp(),
            UpdatedAt = CreateTestTimestamp()
        };
    }

    private Models.Responses.Products.ProductLimits CreateTestProductLimits()
    {
        return new Models.Responses.Products.ProductLimits
        {
            ProductId = CreateTestGuid(),
            CurrencyId = CreateTestGuid(),
            DailyTransactionLimit = 100000m,
            MonthlyTransactionLimit = 3000000m,
            MaxSingleTransactionAmount = 50000m,
            MinSingleTransactionAmount = 100m,
            DailyTransactionCount = 100,
            MonthlyTransactionCount = 3000,
            IsDefault = false,
            CreatedAt = DateTime.UtcNow
        };
    }

    #endregion
}