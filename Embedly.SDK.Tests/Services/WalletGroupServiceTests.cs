// ============================================================================
// COMMENTED OUT - Services not in public API documentation
// These tests are disabled because WalletGroupService is not in the public Embedly API.
// To re-enable, remove the #if false / #endif directives.
// ============================================================================

#if false

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Embedly.SDK.Models.Requests.WalletGroups;
using Embedly.SDK.Models.Responses.WalletGroups;
using Embedly.SDK.Services.WalletGroups;
using Embedly.SDK.Tests.Testing;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Embedly.SDK.Tests.Services;

/// <summary>
///     Unit tests for WalletGroupService following SDK patterns.
///     Tests wallet group management operations and features.
/// </summary>
[TestFixture]
public class WalletGroupServiceTests : ServiceTestBase
{
    private WalletGroupService _walletGroupService = null!;

    protected override void OnSetUp()
    {
        _walletGroupService = new WalletGroupService(MockHttpClient.Object, MockOptions.Object);
    }

    [Test]
    public async Task CreateWalletGroupAsync_WithValidRequest_ReturnsCreatedGroup()
    {
        // Arrange
        var request = CreateValidWalletGroupRequest();
        var expectedGroup = CreateTestWalletGroup();
        var apiResponse = CreateSuccessfulApiResponse(expectedGroup);

        MockHttpClient
            .Setup(x => x.PostAsync<CreateWalletGroupRequest, WalletGroup>(
                It.IsAny<string>(),
                request,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _walletGroupService.CreateWalletGroupAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Id.Should().Be(expectedGroup.Id);
        result.Data.Name.Should().Be(expectedGroup.Name);
    }

    [Test]
    public void CreateWalletGroupAsync_WithNullRequest_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.ThrowsAsync<ArgumentNullException>(() =>
            _walletGroupService.CreateWalletGroupAsync(null!, CancellationToken.None));
    }

    [Test]
    public async Task GetWalletGroupAsync_WithValidId_ReturnsWalletGroup()
    {
        // Arrange
        var groupId = CreateTestStringId();
        var expectedGroup = CreateTestWalletGroup();
        var apiResponse = CreateSuccessfulApiResponse(expectedGroup);

        MockHttpClient
            .Setup(x => x.GetAsync<WalletGroup>(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _walletGroupService.GetWalletGroupAsync(groupId);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Id.Should().Be(expectedGroup.Id);
    }

    [Test]
    public void GetWalletGroupAsync_WithNullId_ThrowsArgumentException()
    {
        // Act & Assert
        Assert.ThrowsAsync<ArgumentException>(() =>
            _walletGroupService.GetWalletGroupAsync(null!, CancellationToken.None));
    }

    [Test]
    public void GetWalletGroupAsync_WithEmptyId_ThrowsArgumentException()
    {
        // Act & Assert
        Assert.ThrowsAsync<ArgumentException>(() =>
            _walletGroupService.GetWalletGroupAsync(string.Empty, CancellationToken.None));
    }

    [Test]
    public async Task GetWalletGroupsAsync_ReturnsListOfWalletGroups()
    {
        // Arrange
        var expectedGroups = new List<WalletGroup>
        {
            CreateTestWalletGroup(),
            CreateTestWalletGroup(CreateTestGuid().ToString(), "Premium Group")
        };
        var apiResponse = CreateSuccessfulApiResponse(expectedGroups);

        MockHttpClient
            .Setup(x => x.GetAsync<List<WalletGroup>>(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _walletGroupService.GetWalletGroupsAsync();

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Should().HaveCount(2);
    }

    [Test]
    public async Task AddWalletGroupFeatureAsync_WithValidRequest_ReturnsSuccessResponse()
    {
        // Arrange
        var request = CreateValidAddWalletGroupFeatureRequest();
        var apiResponse = CreateSuccessfulApiResponse(new object());

        MockHttpClient
            .Setup(x => x.PostAsync<AddWalletGroupFeatureRequest, object>(
                It.IsAny<string>(),
                request,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _walletGroupService.AddWalletGroupFeatureAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
    }

    [Test]
    public void AddWalletGroupFeatureAsync_WithNullRequest_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.ThrowsAsync<ArgumentNullException>(() =>
            _walletGroupService.AddWalletGroupFeatureAsync(null!, CancellationToken.None));
    }

    [Test]
    public async Task GetWalletGroupFeaturesAsync_ReturnsListOfFeatures()
    {
        // Arrange
        var expectedFeatures = new List<WalletGroupFeature>
        {
            CreateTestWalletGroupFeature("feature-1", "Card Management"),
            CreateTestWalletGroupFeature("feature-2", "Transfer Limits")
        };
        var apiResponse = CreateSuccessfulApiResponse(expectedFeatures);

        MockHttpClient
            .Setup(x => x.GetAsync<List<WalletGroupFeature>>(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _walletGroupService.GetWalletGroupFeaturesAsync();

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Should().HaveCount(2);
    }

    [Test]
    public async Task AddWalletToGroupAsync_WithValidIds_ReturnsSuccessResponse()
    {
        // Arrange
        var groupId = CreateTestStringId();
        var walletId = CreateTestStringId();
        var apiResponse = CreateSuccessfulApiResponse(new object());

        MockHttpClient
            .Setup(x => x.PostAsync<object, object>(
                It.IsAny<string>(),
                It.IsAny<object>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _walletGroupService.AddWalletToGroupAsync(groupId, walletId);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
    }

    [Test]
    public void AddWalletToGroupAsync_WithNullGroupId_ThrowsArgumentException()
    {
        // Arrange
        var walletId = CreateTestStringId();

        // Act & Assert
        Assert.ThrowsAsync<ArgumentException>(() =>
            _walletGroupService.AddWalletToGroupAsync(null!, walletId, CancellationToken.None));
    }

    [Test]
    public void AddWalletToGroupAsync_WithNullWalletId_ThrowsArgumentException()
    {
        // Arrange
        var groupId = CreateTestStringId();

        // Act & Assert
        Assert.ThrowsAsync<ArgumentException>(() =>
            _walletGroupService.AddWalletToGroupAsync(groupId, null!, CancellationToken.None));
    }

    [Test]
    public void AddWalletToGroupAsync_WithEmptyGroupId_ThrowsArgumentException()
    {
        // Arrange
        var walletId = CreateTestStringId();

        // Act & Assert
        Assert.ThrowsAsync<ArgumentException>(() =>
            _walletGroupService.AddWalletToGroupAsync(string.Empty, walletId, CancellationToken.None));
    }

    [Test]
    public void AddWalletToGroupAsync_WithEmptyWalletId_ThrowsArgumentException()
    {
        // Arrange
        var groupId = CreateTestStringId();

        // Act & Assert
        Assert.ThrowsAsync<ArgumentException>(() =>
            _walletGroupService.AddWalletToGroupAsync(groupId, string.Empty, CancellationToken.None));
    }

    private CreateWalletGroupRequest CreateValidWalletGroupRequest()
    {
        return new CreateWalletGroupRequest
        {
            Name = "Standard Group",
            Description = "Standard wallet group with basic features",
            CustomerId = CreateTestGuid().ToString()
        };
    }

    private WalletGroup CreateTestWalletGroup(string? id = null, string? name = null)
    {
        return new WalletGroup
        {
            Id = id != null ? Guid.Parse(id) : CreateTestGuid(),
            Name = name ?? "Standard Group",
            WalletGroupFeatures = new List<WalletGroupFeature>()
        };
    }

    private AddWalletGroupFeatureRequest CreateValidAddWalletGroupFeatureRequest()
    {
        return new AddWalletGroupFeatureRequest
        {
            GroupId = CreateTestGuid(),
            FeatureId = CreateTestGuid(),
            Param1 = 50.0m,
            Param2 = 100000
        };
    }

    private WalletGroupFeature CreateTestWalletGroupFeature(string id, string name)
    {
        return new WalletGroupFeature
        {
            FeatureName = name,
            FeaturePropertyName = "IsActive",
            FeaturePropertyValue = "true"
        };
    }
}

#endif
