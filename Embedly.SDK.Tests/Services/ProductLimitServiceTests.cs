using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Embedly.SDK.Models.Requests.ProductLimits;
using Embedly.SDK.Models.Responses.Common;
using Embedly.SDK.Models.Responses.ProductLimits;
using Embedly.SDK.Services.ProductLimits;
using Embedly.SDK.Tests.Testing;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Embedly.SDK.Tests.Services;

/// <summary>
///     Unit tests for ProductLimitService following SDK patterns.
///     Tests product limit CRUD operations and management.
/// </summary>
[TestFixture]
public class ProductLimitServiceTests : ServiceTestBase
{
    private ProductLimitService _productLimitService = null!;

    protected override void OnSetUp()
    {
        _productLimitService = new ProductLimitService(MockHttpClient.Object, MockOptions.Object);
    }

    [Test]
    public async Task CreateProductLimitAsync_WithValidRequest_ReturnsCreatedLimit()
    {
        // Arrange
        var request = CreateValidProductLimitRequest();
        var expectedLimit = CreateTestProductLimit();
        var apiResponse = CreateSuccessfulApiResponse(expectedLimit);

        MockHttpClient
            .Setup(x => x.PostAsync<CreateProductLimitRequest, ProductLimit>(
                It.IsAny<string>(),
                request,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _productLimitService.CreateProductLimitAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Id.Should().Be(expectedLimit.Id);
        result.Data.ProductId.Should().Be(expectedLimit.ProductId);
    }

    [Test]
    public void CreateProductLimitAsync_WithNullRequest_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.ThrowsAsync<ArgumentNullException>(() =>
            _productLimitService.CreateProductLimitAsync(null!, CancellationToken.None));
    }

    [Test]
    public async Task GetProductLimitAsync_WithValidId_ReturnsProductLimit()
    {
        // Arrange
        var limitId = CreateTestStringId();
        var expectedLimit = CreateTestProductLimit();
        var apiResponse = CreateSuccessfulApiResponse(expectedLimit);

        MockHttpClient
            .Setup(x => x.GetAsync<ProductLimit>(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _productLimitService.GetProductLimitAsync(limitId);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Id.Should().Be(expectedLimit.Id);
    }

    [Test]
    public void GetProductLimitAsync_WithNullId_ThrowsArgumentException()
    {
        // Act & Assert
        Assert.ThrowsAsync<ArgumentException>(() =>
            _productLimitService.GetProductLimitAsync(null!, CancellationToken.None));
    }

    [Test]
    public void GetProductLimitAsync_WithEmptyId_ThrowsArgumentException()
    {
        // Act & Assert
        Assert.ThrowsAsync<ArgumentException>(() =>
            _productLimitService.GetProductLimitAsync(string.Empty, CancellationToken.None));
    }

    [Test]
    public async Task GetProductLimitsAsync_WithValidRequest_ReturnsPaginatedLimits()
    {
        // Arrange
        var request = CreateValidGetProductLimitsRequest();
        var expectedLimits = new List<ProductLimit> { CreateTestProductLimit() };
        var paginatedResponse = CreateTestPaginatedResponse(expectedLimits, 1);
        var apiResponse = CreateSuccessfulApiResponse(paginatedResponse);

        MockHttpClient
            .Setup(x => x.GetAsync<PaginatedResponse<ProductLimit>>(
                It.IsAny<string>(),
                It.IsAny<Dictionary<string, object?>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _productLimitService.GetProductLimitsAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Items.Should().HaveCount(1);
        result.Data.Pagination!.TotalItems.Should().Be(1);
    }

    [Test]
    public async Task GetProductLimitsByProductAsync_WithValidProductId_ReturnsLimitsList()
    {
        // Arrange
        var productId = CreateTestStringId();
        var expectedLimits = new List<ProductLimit>
        {
            CreateTestProductLimit(),
            CreateTestProductLimit("limit-2")
        };
        var apiResponse = CreateSuccessfulApiResponse(expectedLimits);

        MockHttpClient
            .Setup(x => x.GetAsync<List<ProductLimit>>(
                It.IsAny<string>(),
                It.IsAny<Dictionary<string, object?>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _productLimitService.GetProductLimitsByProductAsync(productId);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Should().HaveCount(2);
    }

    [Test]
    public async Task UpdateProductLimitAsync_WithValidRequest_ReturnsUpdatedLimit()
    {
        // Arrange
        var limitId = CreateTestStringId();
        var request = CreateValidProductLimitRequest();
        var expectedLimit = CreateTestProductLimit();
        var apiResponse = CreateSuccessfulApiResponse(expectedLimit);

        MockHttpClient
            .Setup(x => x.PutAsync<CreateProductLimitRequest, ProductLimit>(
                It.IsAny<string>(),
                request,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _productLimitService.UpdateProductLimitAsync(limitId, request);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Id.Should().Be(expectedLimit.Id);
    }

    [Test]
    public void UpdateProductLimitAsync_WithNullId_ThrowsArgumentException()
    {
        // Arrange
        var request = CreateValidProductLimitRequest();

        // Act & Assert
        Assert.ThrowsAsync<ArgumentException>(() =>
            _productLimitService.UpdateProductLimitAsync(null!, request, CancellationToken.None));
    }

    [Test]
    public void UpdateProductLimitAsync_WithNullRequest_ThrowsArgumentNullException()
    {
        // Arrange
        var limitId = CreateTestStringId();

        // Act & Assert
        Assert.ThrowsAsync<ArgumentNullException>(() =>
            _productLimitService.UpdateProductLimitAsync(limitId, null!, CancellationToken.None));
    }

    [Test]
    public async Task ActivateProductLimitAsync_WithValidId_ReturnsActivatedLimit()
    {
        // Arrange
        var limitId = CreateTestStringId();
        var expectedLimit = CreateTestProductLimit();
        expectedLimit.IsActive = true;
        var apiResponse = CreateSuccessfulApiResponse(expectedLimit);

        MockHttpClient
            .Setup(x => x.PatchAsync<object, ProductLimit>(
                It.IsAny<string>(),
                It.IsAny<object>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _productLimitService.ActivateProductLimitAsync(limitId);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.IsActive.Should().BeTrue();
    }

    [Test]
    public async Task DeactivateProductLimitAsync_WithValidId_ReturnsDeactivatedLimit()
    {
        // Arrange
        var limitId = CreateTestStringId();
        var expectedLimit = CreateTestProductLimit();
        expectedLimit.IsActive = false;
        var apiResponse = CreateSuccessfulApiResponse(expectedLimit);

        MockHttpClient
            .Setup(x => x.PatchAsync<object, ProductLimit>(
                It.IsAny<string>(),
                It.IsAny<object>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _productLimitService.DeactivateProductLimitAsync(limitId);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.IsActive.Should().BeFalse();
    }

    [Test]
    public async Task ResetProductLimitUsageAsync_WithValidId_ReturnsResetLimit()
    {
        // Arrange
        var limitId = CreateTestStringId();
        var expectedLimit = CreateTestProductLimit();
        expectedLimit.CurrentUsage = 0;
        var apiResponse = CreateSuccessfulApiResponse(expectedLimit);

        MockHttpClient
            .Setup(x => x.PatchAsync<object, ProductLimit>(
                It.IsAny<string>(),
                It.IsAny<object>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _productLimitService.ResetProductLimitUsageAsync(limitId);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.CurrentUsage.Should().Be(0);
    }

    [Test]
    public async Task DeleteProductLimitAsync_WithValidId_ReturnsSuccessResponse()
    {
        // Arrange
        var limitId = CreateTestStringId();
        var apiResponse = CreateSuccessfulApiResponse(new object());

        MockHttpClient
            .Setup(x => x.DeleteAsync<object>(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _productLimitService.DeleteProductLimitAsync(limitId);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
    }

    [Test]
    public void DeleteProductLimitAsync_WithNullId_ThrowsArgumentException()
    {
        // Act & Assert
        Assert.ThrowsAsync<ArgumentException>(() =>
            _productLimitService.DeleteProductLimitAsync(null!, CancellationToken.None));
    }

    private CreateProductLimitRequest CreateValidProductLimitRequest()
    {
        return new CreateProductLimitRequest
        {
            ProductId = CreateTestStringId(),
            Type = "daily",
            Value = 100000,
            Period = "daily",
            Currency = "NGN",
            Description = "Daily transaction limit",
            IsActive = true
        };
    }

    private GetProductLimitsRequest CreateValidGetProductLimitsRequest()
    {
        return new GetProductLimitsRequest
        {
            Page = 1,
            PageSize = 10,
            ProductId = CreateTestStringId(),
            Type = null,
            IsActive = null
        };
    }

    private ProductLimit CreateTestProductLimit(string? id = null)
    {
        return new ProductLimit
        {
            Id = id ?? CreateTestStringId(),
            ProductId = CreateTestStringId(),
            Type = "daily",
            Value = 100000,
            Period = "daily",
            Currency = "NGN",
            CurrentUsage = 0,
            Remaining = 100000,
            Description = "Daily transaction limit",
            IsActive = true,
            Status = ProductLimitStatus.Active,
            CreatedAt = CreateTestTimestamp(),
            UpdatedAt = CreateTestTimestamp()
        };
    }
}