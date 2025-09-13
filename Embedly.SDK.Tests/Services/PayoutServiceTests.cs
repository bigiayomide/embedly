using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using Embedly.SDK.Models.Requests.Payout;
using Embedly.SDK.Models.Responses.Payout;
using Embedly.SDK.Models.Responses.Common;
using Embedly.SDK.Services.Payout;
using Embedly.SDK.Tests.Testing;

namespace Embedly.SDK.Tests.Services;

/// <summary>
/// Unit tests for PayoutService following SDK patterns.
/// Tests all payout operations including bank transfers, limits, and profile management.
/// </summary>
[TestFixture]
public class PayoutServiceTests : ServiceTestBase
{
    private PayoutService _payoutService = null!;

    protected override void OnSetUp()
    {
        _payoutService = new PayoutService(MockHttpClient.Object, MockOptions.Object);
    }

    #region Bank Operations Tests

    [Test]
    public async Task GetBanksAsync_WithoutSearch_ReturnsListOfBanks()
    {
        // Arrange
        var expectedBanks = new List<Bank>
        {
            CreateTestBank("001", "Central Bank of Nigeria"),
            CreateTestBank("011", "First Bank of Nigeria")
        };
        var apiResponse = CreateSuccessfulApiResponse(expectedBanks);

        MockHttpClient
            .Setup(x => x.GetAsync<List<Bank>>(
                It.IsAny<string>(),
                It.IsAny<Dictionary<string, object?>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _payoutService.GetBanksAsync();

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Should().HaveCount(2);
    }

    [Test]
    public async Task GetBanksAsync_WithSearchFilter_ReturnsFilteredBanks()
    {
        // Arrange
        var search = "First Bank";
        var expectedBanks = new List<Bank>
        {
            CreateTestBank("011", "First Bank of Nigeria")
        };
        var apiResponse = CreateSuccessfulApiResponse(expectedBanks);

        MockHttpClient
            .Setup(x => x.GetAsync<List<Bank>>(
                It.IsAny<string>(),
                It.IsAny<Dictionary<string, object?>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _payoutService.GetBanksAsync(search);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Should().HaveCount(1);
    }

    [Test]
    public async Task NameEnquiryAsync_WithValidRequest_ReturnsAccountName()
    {
        // Arrange
        var request = CreateValidNameEnquiryRequest();
        var expectedResponse = CreateTestNameEnquiryResponse();
        var apiResponse = CreateSuccessfulApiResponse(expectedResponse);

        MockHttpClient
            .Setup(x => x.PostAsync<NameEnquiryRequest, NameEnquiryResponse>(
                It.IsAny<string>(),
                request,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _payoutService.NameEnquiryAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.AccountName.Should().NotBeNullOrEmpty();
    }

    [Test]
    public void NameEnquiryAsync_WithNullRequest_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.ThrowsAsync<ArgumentNullException>(
            () => _payoutService.NameEnquiryAsync(null!));
    }

    #endregion

    #region Transfer Operations Tests

    [Test]
    public async Task InterBankTransferAsync_WithValidRequest_ReturnsTransactionDetails()
    {
        // Arrange
        var request = CreateValidBankTransferRequest();
        var expectedTransaction = CreateTestPayoutTransaction();
        var apiResponse = CreateSuccessfulApiResponse(expectedTransaction);

        MockHttpClient
            .Setup(x => x.PostAsync<BankTransferRequest, PayoutTransaction>(
                It.IsAny<string>(),
                request,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _payoutService.InterBankTransferAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Id.Should().Be(expectedTransaction.Id);
        result.Data.Amount.Should().Be(expectedTransaction.Amount);
    }

    [Test]
    public void InterBankTransferAsync_WithNullRequest_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.ThrowsAsync<ArgumentNullException>(
            () => _payoutService.InterBankTransferAsync(null!));
    }

    [Test]
    public async Task GetTransactionStatusAsync_WithValidReference_ReturnsTransactionStatus()
    {
        // Arrange
        var transactionReference = "TXN-123456789";
        var expectedTransaction = CreateTestPayoutTransaction();
        var apiResponse = CreateSuccessfulApiResponse(expectedTransaction);

        MockHttpClient
            .Setup(x => x.GetAsync<PayoutTransaction>(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _payoutService.GetTransactionStatusAsync(transactionReference);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Reference.Should().Be(expectedTransaction.Reference);
    }

    [Test]
    public void GetTransactionStatusAsync_WithNullReference_ThrowsArgumentException()
    {
        // Act & Assert
        Assert.ThrowsAsync<ArgumentException>(
            () => _payoutService.GetTransactionStatusAsync(null!));
    }

    #endregion

    #region Organization and Profile Tests

    [Test]
    public async Task GetOrganizationPayoutDataAsync_WithValidId_ReturnsPayoutData()
    {
        // Arrange
        var organizationId = CreateTestGuid();
        var expectedData = CreateTestOrganizationPayoutData();
        var apiResponse = CreateSuccessfulApiResponse(expectedData);

        MockHttpClient
            .Setup(x => x.GetAsync<OrganizationPayoutData>(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _payoutService.GetOrganizationPayoutDataAsync(organizationId);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.OrganizationId.Should().Be(expectedData.OrganizationId);
    }

    [Test]
    public async Task CreatePayoutProfileAsync_WithValidRequest_ReturnsProfileResponse()
    {
        // Arrange
        var request = CreateValidPayoutRequestDto();
        var expectedProfile = CreateTestPayoutProfileResponse();
        var apiResponse = CreateSuccessfulApiResponse(expectedProfile);

        MockHttpClient
            .Setup(x => x.PostAsync<PayoutRequestDto, PayoutProfileResponse>(
                It.IsAny<string>(),
                request,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _payoutService.CreatePayoutProfileAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.WalletId.Should().Be(expectedProfile.WalletId);
    }

    [Test]
    public async Task GetAllPayoutsAsync_ReturnsListOfPayouts()
    {
        // Arrange
        var expectedPayouts = new List<PayoutTransaction>
        {
            CreateTestPayoutTransaction(),
            CreateTestPayoutTransaction("TXN-987654321")
        };
        var apiResponse = CreateSuccessfulApiResponse(expectedPayouts);

        MockHttpClient
            .Setup(x => x.GetAsync<List<PayoutTransaction>>(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _payoutService.GetAllPayoutsAsync();

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Should().HaveCount(2);
    }

    [Test]
    public async Task GetWalletByAccountNumberAsync_WithValidAccountNumber_ReturnsWalletDetails()
    {
        // Arrange
        var accountNumber = "1234567890";
        var expectedWallet = CreateTestWalletDetails();
        var apiResponse = CreateSuccessfulApiResponse(expectedWallet);

        MockHttpClient
            .Setup(x => x.GetAsync<WalletDetails>(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _payoutService.GetWalletByAccountNumberAsync(accountNumber);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.AccountNumber.Should().Be(expectedWallet.AccountNumber);
    }

    #endregion

    #region Global Payout Limits Tests

    [Test]
    public async Task AddGlobalPayoutLimitAsync_WithValidRequest_ReturnsCreatedLimit()
    {
        // Arrange
        var request = CreateValidAddGlobalPayoutLimitRequest();
        var expectedLimit = CreateTestGlobalPayoutLimit();
        var apiResponse = CreateSuccessfulApiResponse(expectedLimit);

        MockHttpClient
            .Setup(x => x.PostAsync<AddGlobalPayoutLimitRequest, GlobalPayoutLimit>(
                It.IsAny<string>(),
                request,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _payoutService.AddGlobalPayoutLimitAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.GlobalLimitId.Should().Be(expectedLimit.GlobalLimitId);
    }

    [Test]
    public async Task GetGlobalPayoutLimitsAsync_WithValidRequest_ReturnsPaginatedLimits()
    {
        // Arrange
        var request = CreateValidGetPayoutLimitsRequest();
        var expectedLimits = new List<GlobalPayoutLimit> { CreateTestGlobalPayoutLimit() };
        var paginatedResponse = CreateTestPaginatedResponse(expectedLimits, 1, 10, 1);
        var apiResponse = CreateSuccessfulApiResponse(paginatedResponse);

        MockHttpClient
            .Setup(x => x.GetAsync<PaginatedResponse<GlobalPayoutLimit>>(
                It.IsAny<string>(),
                It.IsAny<Dictionary<string, object?>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _payoutService.GetGlobalPayoutLimitsAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Items.Should().HaveCount(1);
    }

    [Test]
    public async Task UpdateGlobalPayoutLimitAsync_WithValidRequest_ReturnsUpdatedLimit()
    {
        // Arrange
        var globalLimitId = CreateTestGuid();
        var request = CreateValidAddGlobalPayoutLimitRequest();
        var expectedLimit = CreateTestGlobalPayoutLimit();
        var apiResponse = CreateSuccessfulApiResponse(expectedLimit);

        MockHttpClient
            .Setup(x => x.PutAsync<AddGlobalPayoutLimitRequest, GlobalPayoutLimit>(
                It.IsAny<string>(),
                request,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _payoutService.UpdateGlobalPayoutLimitAsync(globalLimitId, request);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.GlobalLimitId.Should().Be(expectedLimit.GlobalLimitId);
    }

    [Test]
    public async Task EnableOrDisableGlobalPayoutLimitAsync_WithValidId_ReturnsUpdatedLimit()
    {
        // Arrange
        var limitId = CreateTestGuid();
        var enabled = true;
        var expectedLimit = CreateTestGlobalPayoutLimit();
        expectedLimit.DailyTransactionLimitStatus = enabled;
        var apiResponse = CreateSuccessfulApiResponse(expectedLimit);

        MockHttpClient
            .Setup(x => x.PatchAsync<GlobalPayoutLimit>(
                It.IsAny<string>(),
                It.IsAny<object>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _payoutService.EnableOrDisableGlobalPayoutLimitAsync(limitId, enabled);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.DailyTransactionLimitStatus.Should().Be(enabled);
    }

    #endregion

    #region Helper Methods

    private Bank CreateTestBank(string code, string name)
    {
        return new Bank
        {
            BankCode = code,
            BankName = name,
            IsActive = true,
            Type = "Commercial"
        };
    }

    private NameEnquiryRequest CreateValidNameEnquiryRequest()
    {
        return new NameEnquiryRequest
        {
            BankCode = "011",
            AccountNumber = "1234567890"
        };
    }

    private NameEnquiryResponse CreateTestNameEnquiryResponse()
    {
        return new NameEnquiryResponse
        {
            AccountName = "John Doe",
            AccountNumber = "1234567890",
            BankCode = "011",
            BankName = "First Bank of Nigeria"
        };
    }

    private BankTransferRequest CreateValidBankTransferRequest()
    {
        return new BankTransferRequest
        {
            DestinationBankCode = "011",
            DestinationAccountNumber = "1234567890",
            DestinationAccountName = "Test Account",
            SourceAccountNumber = "0987654321",
            SourceAccountName = "Source Account",
            Amount = 50000.0,
            Remarks = "Test transfer",
            CustomerTransactionReference = "TEST-REF-123"
        };
    }

    private PayoutTransaction CreateTestPayoutTransaction(string? reference = null)
    {
        return new PayoutTransaction
        {
            Id = CreateTestGuid().ToString(),
            Reference = reference ?? "TXN-123456789",
            Amount = 50000,
            Currency = "NGN",
            Status = PayoutStatus.Successful,
            AccountNumber = "1234567890",
            BankCode = "011",
            CreatedAt = CreateTestTimestamp()
        };
    }

    private OrganizationPayoutData CreateTestOrganizationPayoutData()
    {
        return new OrganizationPayoutData
        {
            OrganizationId = CreateTestGuid(),
            OrganizationName = "Test Organization",
            TotalPayoutAmount = 1000000.0,
            PayoutCount = 100,
            LastPayoutDate = DateTime.UtcNow
        };
    }

    private PayoutRequestDto CreateValidPayoutRequestDto()
    {
        return new PayoutRequestDto
        {
            WalletId = CreateTestGuid()
        };
    }

    private PayoutProfileResponse CreateTestPayoutProfileResponse()
    {
        return new PayoutProfileResponse
        {
            ProfileId = CreateTestGuid(),
            OrganizationId = CreateTestGuid(),
            WalletId = CreateTestGuid(),
            Status = "Active",
            CreatedAt = CreateTestTimestamp().DateTime
        };
    }

    private WalletDetails CreateTestWalletDetails()
    {
        return new WalletDetails
        {
            WalletId = CreateTestGuid(),
            AccountNumber = "1234567890",
            AvailableBalance = 100000.0,
            LedgerBalance = 100000.0,
            Currency = "NGN",
            Status = "Active"
        };
    }

    private AddGlobalPayoutLimitRequest CreateValidAddGlobalPayoutLimitRequest()
    {
        return new AddGlobalPayoutLimitRequest
        {
            CurrencyId = Guid.Parse("fd5e474d-bb42-4db1-ab74-e8d2a01047e9"),
            DailyTransactionLimit = 1000000.0,
            DailyTransactionCount = 100,
            DailyTransactionLimitStatus = true,
            DailyTransactionCountStatus = true
        };
    }

    private GetPayoutLimitsRequest CreateValidGetPayoutLimitsRequest()
    {
        return new GetPayoutLimitsRequest
        {
            PageNumber = 1,
            PageSize = 10
        };
    }

    private static GlobalPayoutLimit CreateTestGlobalPayoutLimit()
    {
        return new GlobalPayoutLimit
        {
            GlobalLimitId = CreateTestGuid(),
            CurrencyId = Guid.Parse("fd5e474d-bb42-4db1-ab74-e8d2a01047e9"),
            DailyTransactionLimit = 1000000.0,
            DailyTransactionCount = 100,
            DailyTransactionLimitStatus = true,
            DailyTransactionCountStatus = true,
            CreatedAt = CreateTestTimestamp().DateTime,
            UpdatedAt = CreateTestTimestamp().DateTime
        };
    }

    #endregion
}