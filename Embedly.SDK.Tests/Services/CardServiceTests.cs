using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using Embedly.SDK.Models.Requests.Cards;
using Embedly.SDK.Models.Responses.Cards;
using Embedly.SDK.Models.Responses.Common;
using Embedly.SDK.Services.Cards;
using Embedly.SDK.Tests.Testing;

namespace Embedly.SDK.Tests.Services;

/// <summary>
/// Comprehensive unit tests for CardService following SDK patterns.
/// Tests all card management operations including issuance, activation, PIN management, and blocking.
/// </summary>
[TestFixture]
public class CardServiceTests : ServiceTestBase
{
    private CardService _cardService = null!;
    private Mock<IPinEncryptionService> _mockPinEncryptionService = null!;

    protected override void OnSetUp()
    {
        _mockPinEncryptionService = new Mock<IPinEncryptionService>();
        _cardService = new CardService(MockHttpClient.Object, MockOptions.Object, _mockPinEncryptionService.Object);
    }

    #region Card Issuance Tests

    [Test]
    public async Task IssueAfrigoCardAsync_WithValidRequest_ReturnsSuccessfulResponse()
    {
        // Arrange
        var request = CreateValidCardIssuanceRequest();
        var expectedCard = CreateTestAfrigoCard();
        var apiResponse = CreateSuccessfulApiResponse(expectedCard);

        MockHttpClient
            .Setup(x => x.PostAsync<IssueAfrigoCardRequest, AfrigoCard>(
                It.Is<string>(url => url.Contains("api/v1/operations/cards/afrigo/issue-card")),
                request,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _cardService.IssueAfrigoCardAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Id.Should().Be(expectedCard.Id);
        result.Data.CardNumber.Should().Be(expectedCard.CardNumber);
        result.Data.CardType.Should().Be(expectedCard.CardType);
        result.Data.Status.Should().Be(CardStatus.Issued);

        VerifyHttpClientPostCall<IssueAfrigoCardRequest, AfrigoCard>(
            "api/v1/operations/cards/afrigo/issue-card", request);
    }

    [Test]
    public async Task IssueAfrigoCardAsync_WithNullRequest_ThrowsArgumentNullException()
    {
        // Act & Assert
        AssertThrowsArgumentNullExceptionForNullRequest<IssueAfrigoCardRequest, AfrigoCard>(
            _cardService.IssueAfrigoCardAsync);
    }

    [Test]
    public async Task IssueAfrigoCardAsync_WhenApiReturnsError_ReturnsFailedResponse()
    {
        // Arrange
        var request = CreateValidCardIssuanceRequest();
        var apiResponse = CreateFailedApiResponse<AfrigoCard>("Customer not eligible for card", "INELIGIBLE_CUSTOMER");

        MockHttpClient
            .Setup(x => x.PostAsync<IssueAfrigoCardRequest, AfrigoCard>(
                It.IsAny<string>(),
                It.IsAny<IssueAfrigoCardRequest>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _cardService.IssueAfrigoCardAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeFalse();
        result.Message.Should().Be("Customer not eligible for card");
        result.Data.Should().BeNull();
        result.Error?.Code.Should().Be("INELIGIBLE_CUSTOMER");
    }

    #endregion

    #region Card Activation Tests

    [Test]
    public async Task ActivateAfrigoCardAsync_WithValidRequest_EncryptsPinAndReturnsActivatedCard()
    {
        // Arrange
        var request = CreateValidCardActivationRequest();
        var encryptedPin = "encrypted_1234";
        var expectedCard = CreateTestAfrigoCard() with { Status = CardStatus.Active, ActivatedAt = CreateTestTimestamp() };
        var apiResponse = CreateSuccessfulApiResponse(expectedCard);

        _mockPinEncryptionService
            .Setup(x => x.EncryptPin(request.Pin!))
            .Returns(encryptedPin);

        MockHttpClient
            .Setup(x => x.PostAsync<ActivateAfrigoCardRequest, AfrigoCard>(
                It.Is<string>(url => url.Contains("api/v1/operations/cards/afrigo/activate")),
                It.Is<ActivateAfrigoCardRequest>(r => r.Pin == encryptedPin),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _cardService.ActivateAfrigoCardAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Status.Should().Be(CardStatus.Active);
        result.Data.ActivatedAt.Should().NotBeNull();

        _mockPinEncryptionService.Verify(x => x.EncryptPin(request.Pin!), Times.Once);
        VerifyHttpClientPostCall<ActivateAfrigoCardRequest, AfrigoCard>(
            "api/v1/operations/cards/afrigo/activate",
            It.Is<ActivateAfrigoCardRequest>(r => r.Pin == encryptedPin));
    }

    [Test]
    public async Task ActivateAfrigoCardAsync_WithNullPin_DoesNotEncryptPin()
    {
        // Arrange
        var request = CreateValidCardActivationRequest();
        request.Pin = null;
        var expectedCard = CreateTestAfrigoCard();
        var apiResponse = CreateSuccessfulApiResponse(expectedCard);

        MockHttpClient
            .Setup(x => x.PostAsync<ActivateAfrigoCardRequest, AfrigoCard>(
                It.IsAny<string>(),
                It.IsAny<ActivateAfrigoCardRequest>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _cardService.ActivateAfrigoCardAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();

        _mockPinEncryptionService.Verify(x => x.EncryptPin(It.IsAny<string>()), Times.Never);
    }

    [Test]
    public async Task ActivateAfrigoCardAsync_WithEmptyPin_DoesNotEncryptPin()
    {
        // Arrange
        var request = CreateValidCardActivationRequest();
        request.Pin = string.Empty;
        var expectedCard = CreateTestAfrigoCard();
        var apiResponse = CreateSuccessfulApiResponse(expectedCard);

        MockHttpClient
            .Setup(x => x.PostAsync<ActivateAfrigoCardRequest, AfrigoCard>(
                It.IsAny<string>(),
                It.IsAny<ActivateAfrigoCardRequest>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _cardService.ActivateAfrigoCardAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();

        _mockPinEncryptionService.Verify(x => x.EncryptPin(It.IsAny<string>()), Times.Never);
    }

    [Test]
    public async Task ActivateAfrigoCardAsync_WithNullRequest_ThrowsArgumentNullException()
    {
        // Act & Assert
        AssertThrowsArgumentNullExceptionForNullRequest<ActivateAfrigoCardRequest, AfrigoCard>(
            _cardService.ActivateAfrigoCardAsync);
    }

    #endregion

    #region Card Update Tests

    [Test]
    public async Task UpdateAfrigoCardAsync_WithValidRequest_ReturnsUpdatedCard()
    {
        // Arrange
        var request = CreateValidCardUpdateRequest();
        var expectedCard = CreateTestAfrigoCard() with { UpdatedAt = CreateTestTimestamp() };
        var apiResponse = CreateSuccessfulApiResponse(expectedCard);

        MockHttpClient
            .Setup(x => x.PostAsync<UpdateAfrigoCardRequest, AfrigoCard>(
                It.Is<string>(url => url.Contains("api/v1/operations/cards/afrigo/update-card-info")),
                request,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _cardService.UpdateAfrigoCardAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.UpdatedAt.Should().NotBeNull();
        // Verify that card update was processed (mobile number update doesn't return in response)

        VerifyHttpClientPostCall<UpdateAfrigoCardRequest, AfrigoCard>(
            "api/v1/operations/cards/afrigo/update-card-info", request);
    }

    [Test]
    public async Task UpdateAfrigoCardAsync_WithNullRequest_ThrowsArgumentNullException()
    {
        // Act & Assert
        AssertThrowsArgumentNullExceptionForNullRequest<UpdateAfrigoCardRequest, AfrigoCard>(
            _cardService.UpdateAfrigoCardAsync);
    }

    #endregion

    #region PIN Management Tests

    [Test]
    public async Task ResetCardPinAsync_WithValidRequest_EncryptsPinAndReturnsResult()
    {
        // Arrange
        var request = CreateValidPinResetRequest();
        var encryptedPin = "encrypted_new_pin";
        var expectedResult = CreateSuccessfulPinOperationResult();
        var apiResponse = CreateSuccessfulApiResponse(expectedResult);

        _mockPinEncryptionService
            .Setup(x => x.EncryptPin(request.Pin!))
            .Returns(encryptedPin);

        MockHttpClient
            .Setup(x => x.PostAsync<ResetCardPinRequest, CardPinOperationResult>(
                It.Is<string>(url => url.Contains("api/v1/operations/cards/afrigo/reset-card-pin")),
                It.Is<ResetCardPinRequest>(r => r.Pin == encryptedPin),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _cardService.ResetCardPinAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Success.Should().BeTrue();

        _mockPinEncryptionService.Verify(x => x.EncryptPin(request.Pin!), Times.Once);
        MockHttpClient.Verify(
            x => x.PostAsync<ResetCardPinRequest, CardPinOperationResult>(
                It.Is<string>(url => url.Contains("api/v1/operations/cards/afrigo/reset-card-pin")),
                It.Is<ResetCardPinRequest>(r => r.Pin == encryptedPin),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    public async Task ChangeCardPinAsync_WithValidRequest_EncryptsBothPinsAndReturnsResult()
    {
        // Arrange
        var request = CreateValidPinChangeRequest();
        var encryptedOldPin = "encrypted_old_pin";
        var encryptedNewPin = "encrypted_new_pin";
        var expectedResult = CreateSuccessfulPinOperationResult();
        var apiResponse = CreateSuccessfulApiResponse(expectedResult);

        _mockPinEncryptionService
            .Setup(x => x.EncryptPin(request.OldPin!))
            .Returns(encryptedOldPin);

        _mockPinEncryptionService
            .Setup(x => x.EncryptPin(request.NewPin!))
            .Returns(encryptedNewPin);

        MockHttpClient
            .Setup(x => x.PostAsync<ChangeCardPinRequest, CardPinOperationResult>(
                It.Is<string>(url => url.Contains("api/v1/operations/cards/afrigo/change-card-pin")),
                It.Is<ChangeCardPinRequest>(r => r.OldPin == encryptedOldPin && r.NewPin == encryptedNewPin),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _cardService.ChangeCardPinAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Success.Should().BeTrue();

        _mockPinEncryptionService.Verify(x => x.EncryptPin(request.OldPin!), Times.Once);
        _mockPinEncryptionService.Verify(x => x.EncryptPin(request.NewPin!), Times.Once);
    }

    [Test]
    public async Task CheckCardPinAsync_WithValidRequest_EncryptsPinAndReturnsResult()
    {
        // Arrange
        var request = CreateValidPinCheckRequest();
        var encryptedPin = "encrypted_pin";
        var expectedResult = CreateSuccessfulPinOperationResult();
        var apiResponse = CreateSuccessfulApiResponse(expectedResult);

        _mockPinEncryptionService
            .Setup(x => x.EncryptPin(request.Pin!))
            .Returns(encryptedPin);

        MockHttpClient
            .Setup(x => x.PostAsync<CheckCardPinRequest, CardPinOperationResult>(
                It.Is<string>(url => url.Contains("api/v1/operations/cards/afrigo/check-card-pin")),
                It.Is<CheckCardPinRequest>(r => r.Pin == encryptedPin),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _cardService.CheckCardPinAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Success.Should().BeTrue();

        _mockPinEncryptionService.Verify(x => x.EncryptPin(request.Pin!), Times.Once);
    }

    [Test]
    public async Task ResetCardPinAsync_WithNullRequest_ThrowsArgumentNullException()
    {
        // Act & Assert
        AssertThrowsArgumentNullExceptionForNullRequest<ResetCardPinRequest, CardPinOperationResult>(
            _cardService.ResetCardPinAsync);
    }

    [Test]
    public async Task ChangeCardPinAsync_WithNullRequest_ThrowsArgumentNullException()
    {
        // Act & Assert
        AssertThrowsArgumentNullExceptionForNullRequest<ChangeCardPinRequest, CardPinOperationResult>(
            _cardService.ChangeCardPinAsync);
    }

    [Test]
    public async Task CheckCardPinAsync_WithNullRequest_ThrowsArgumentNullException()
    {
        // Act & Assert
        AssertThrowsArgumentNullExceptionForNullRequest<CheckCardPinRequest, CardPinOperationResult>(
            _cardService.CheckCardPinAsync);
    }

    #endregion

    #region Card Blocking Tests

    [Test]
    public async Task BlockCardAsync_WithValidRequest_ReturnsBlockedCard()
    {
        // Arrange
        var request = CreateValidBlockCardRequest();
        var expectedCard = CreateTestAfrigoCard() with { Status = CardStatus.Blocked };
        // BlockedAt property doesn't exist in AfrigoCard model
        var apiResponse = CreateSuccessfulApiResponse(expectedCard);

        MockHttpClient
            .Setup(x => x.PostAsync<BlockCardRequest, AfrigoCard>(
                It.Is<string>(url => url.Contains("api/v1/operations/cards/afrigo/block-card")),
                request,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _cardService.BlockCardAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Status.Should().Be(CardStatus.Blocked);
        // Verify card is blocked by status instead

        VerifyHttpClientPostCall<BlockCardRequest, AfrigoCard>(
            "api/v1/operations/cards/afrigo/block-card", request);
    }

    [Test]
    public async Task UnblockCardAsync_WithValidRequest_ReturnsUnblockedCard()
    {
        // Arrange
        var request = CreateValidUnblockCardRequest();
        var expectedCard = CreateTestAfrigoCard() with { Status = CardStatus.Active };
        // BlockedAt property doesn't exist in AfrigoCard model
        var apiResponse = CreateSuccessfulApiResponse(expectedCard);

        MockHttpClient
            .Setup(x => x.PostAsync<UnblockCardRequest, AfrigoCard>(
                It.Is<string>(url => url.Contains("api/v1/operations/cards/afrigo/unblock-card")),
                request,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _cardService.UnblockCardAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Status.Should().Be(CardStatus.Active);
        // Verify card is unblocked by status instead

        VerifyHttpClientPostCall<UnblockCardRequest, AfrigoCard>(
            "api/v1/operations/cards/afrigo/unblock-card", request);
    }

    [Test]
    public async Task BlockCardAsync_WithNullRequest_ThrowsArgumentNullException()
    {
        // Act & Assert
        AssertThrowsArgumentNullExceptionForNullRequest<BlockCardRequest, AfrigoCard>(
            _cardService.BlockCardAsync);
    }

    [Test]
    public async Task UnblockCardAsync_WithNullRequest_ThrowsArgumentNullException()
    {
        // Act & Assert
        AssertThrowsArgumentNullExceptionForNullRequest<UnblockCardRequest, AfrigoCard>(
            _cardService.UnblockCardAsync);
    }

    #endregion

    #region Test Data Creation Methods

    private IssueAfrigoCardRequest CreateValidCardIssuanceRequest()
    {
        return new IssueAfrigoCardRequest
        {
            AccountNumber = "1234567890",
            PickupMethod = "DELIVERY",
            CardType = "VIRTUAL",
            IdType = "NIN",
            IdNo = "12345678901",
            Email = CreateTestEmail("cardholder"),
            Address = "123 Test Street, Lagos, Nigeria"
        };
    }

    private ActivateAfrigoCardRequest CreateValidCardActivationRequest()
    {
        return new ActivateAfrigoCardRequest
        {
            CardNumber = "1234567890123456",
            AccountNumber = "1234567890",
            Pin = "1234",
            OrganizationId = CreateTestGuid()
        };
    }

    private UpdateAfrigoCardRequest CreateValidCardUpdateRequest()
    {
        return new UpdateAfrigoCardRequest
        {
            AccountNumber = "1234567890",
            CardNumber = "1234567890123456",
            OldMobile = "+2348012345678",
            NewMobile = "+2348087654321"
        };
    }

    private ResetCardPinRequest CreateValidPinResetRequest()
    {
        return new ResetCardPinRequest
        {
            CustomerId = CreateTestGuid(),
            WalletId = CreateTestGuid(),
            CardNumber = "1234567890123456",
            Pin = "5678"
        };
    }

    private ChangeCardPinRequest CreateValidPinChangeRequest()
    {
        return new ChangeCardPinRequest
        {
            CustomerId = CreateTestGuid(),
            WalletId = CreateTestGuid(),
            CardNumber = "1234567890123456",
            OldPin = "1234",
            NewPin = "5678"
        };
    }

    private CheckCardPinRequest CreateValidPinCheckRequest()
    {
        return new CheckCardPinRequest
        {
            CustomerId = CreateTestGuid(),
            WalletId = CreateTestGuid(),
            CardNumber = "1234567890123456",
            Pin = "1234"
        };
    }

    private BlockCardRequest CreateValidBlockCardRequest()
    {
        return new BlockCardRequest
        {
            CustomerId = CreateTestGuid(),
            WalletId = CreateTestGuid(),
            CardNumber = "1234567890123456",
        };
    }

    private UnblockCardRequest CreateValidUnblockCardRequest()
    {
        return new UnblockCardRequest
        {
            CustomerId = CreateTestGuid(),
            WalletId = CreateTestGuid(),
            CardNumber = "1234567890123456",
        };
    }

    private AfrigoCard CreateTestAfrigoCard()
    {
        return new AfrigoCard
        {
            Id = CreateTestGuid().ToString(),
            CardNumber = "1234-****-****-5678",
            CardType = "Virtual",
            Status = CardStatus.Issued,
            CustomerId = CreateTestGuid().ToString(),
            SpendingLimit = 100000m,
            AvailableBalance = 50000m,
            Currency = "NGN",
            IssuedAt = DateTimeOffset.UtcNow,
            ExpiryDate = "12/30"
        };
    }

    private CardPinOperationResult CreateSuccessfulPinOperationResult()
    {
        return new CardPinOperationResult
        {
            Success = true,
            Message = "PIN operation completed successfully",
            OperationId = CreateTestGuid().ToString(),
            Status = "COMPLETED",
            MaskedCardNumber = "****-****-****-1234",
            Timestamp = DateTime.UtcNow
        };
    }

    #endregion
}