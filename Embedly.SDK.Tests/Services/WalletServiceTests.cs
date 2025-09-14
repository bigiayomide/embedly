using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Embedly.SDK.Models.Requests.Wallets;
using Embedly.SDK.Models.Responses.Wallets;
using Embedly.SDK.Services.Wallets;
using Embedly.SDK.Tests.Testing;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Embedly.SDK.Tests.Services;

/// <summary>
///     Comprehensive unit tests for WalletService following SDK patterns.
///     Tests all wallet management operations including CRUD, transactions, and restrictions.
/// </summary>
[TestFixture]
public class WalletServiceTests : ServiceTestBase
{
    private WalletService _walletService = null!;

    protected override void OnSetUp()
    {
        _walletService = new WalletService(MockHttpClient.Object, MockOptions.Object);
    }

    [Test]
    public async Task CreateWalletAsync_WithValidRequest_ReturnsSuccessfulResponse()
    {
        // Arrange
        var request = CreateValidWalletRequest();
        var expectedWallet = CreateTestWallet();
        var apiResponse = CreateSuccessfulApiResponse(expectedWallet);

        MockHttpClient
            .Setup(x => x.PostAsync<CreateWalletRequest, Wallet>(
                It.Is<string>(url => url.Contains("api/v1/wallets/add")),
                request,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _walletService.CreateWalletAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.CustomerId.Should().Be(expectedWallet.CustomerId);
        result.Data.Name.Should().Be(expectedWallet.Name);

        VerifyHttpClientPostCall<CreateWalletRequest, Wallet>("api/v1/wallets/add", request);
    }

    [Test]
    public void CreateWalletAsync_WithNullRequest_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.ThrowsAsync<ArgumentNullException>(() =>
            _walletService.CreateWalletAsync(null!, CancellationToken.None));
    }

    [Test]
    public async Task CreateCorporateWalletAsync_WithValidRequest_ReturnsSuccessfulResponse()
    {
        // Arrange
        var customerId = CreateTestGuid().ToString();
        var request = CreateValidCorporateWalletRequest();
        var expectedWallet = CreateTestWallet();
        var apiResponse = CreateSuccessfulApiResponse(expectedWallet);

        MockHttpClient
            .Setup(x => x.PostAsync<CreateCorporateWalletRequest, Wallet>(
                It.Is<string>(url => url.Contains($"api/v1/corporate/customers/{customerId}/wallets")),
                request,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _walletService.CreateCorporateWalletAsync(customerId, request);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
    }

    [Test]
    public async Task GetWalletAsync_WithValidId_ReturnsWallet()
    {
        // Arrange
        var walletId = CreateTestGuid().ToString();
        var expectedWallet = CreateTestWallet();
        var apiResponse = CreateSuccessfulApiResponse(expectedWallet);

        MockHttpClient
            .Setup(x => x.GetAsync<Wallet>(
                It.Is<string>(url => url.Contains($"api/v1/wallets/get/wallet/{walletId}")),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _walletService.GetWalletAsync(walletId);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().Be(expectedWallet);

        VerifyHttpClientGetCall<Wallet>($"api/v1/wallets/get/wallet/{walletId}");
    }

    [Test]
    public async Task GetWalletByAccountNumberAsync_WithValidAccountNumber_ReturnsWallet()
    {
        // Arrange
        var accountNumber = "1234567890";
        var expectedWallet = CreateTestWallet();
        var apiResponse = CreateSuccessfulApiResponse(expectedWallet);

        MockHttpClient
            .Setup(x => x.GetAsync<Wallet>(
                It.Is<string>(url => url.Contains($"api/v1/wallets/get/wallet/account/{accountNumber}")),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _walletService.GetWalletByAccountNumberAsync(accountNumber);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().Be(expectedWallet);
    }

    [Test]
    public async Task GetCustomerWalletsAsync_WithValidCustomerId_ReturnsWalletList()
    {
        // Arrange
        var customerId = CreateTestGuid().ToString();
        var expectedWallets = new List<Wallet> { CreateTestWallet(), CreateTestWallet() };
        var apiResponse = CreateSuccessfulApiResponse(expectedWallets);

        MockHttpClient
            .Setup(x => x.GetAsync<List<Wallet>>(
                It.Is<string>(url => url.Contains($"api/v1/wallets/get/wallet/customer/{customerId}")),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _walletService.GetCustomerWalletsAsync(customerId);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Should().HaveCount(2);
    }

    [Test]
    public async Task SearchWalletsByEmailAsync_WithValidRequest_ReturnsMatchingWallets()
    {
        // Arrange
        var request = new SearchWalletByEmailRequest
        {
            Email = "test@example.com"
        };
        var expectedWallets = new List<Wallet> { CreateTestWallet() };
        var apiResponse = CreateSuccessfulApiResponse(expectedWallets);

        MockHttpClient
            .Setup(x => x.PostAsync<SearchWalletByEmailRequest, List<Wallet>>(
                It.Is<string>(url => url.Contains("api/v1/wallets/search/email")),
                request,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _walletService.SearchWalletsByEmailAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Should().HaveCount(1);
    }

    [Test]
    public async Task SearchWalletsByMobileAsync_WithValidRequest_ReturnsMatchingWallets()
    {
        // Arrange
        var request = new SearchWalletByMobileRequest
        {
            MobileNumber = "+2348012345678"
        };
        var expectedWallets = new List<Wallet> { CreateTestWallet() };
        var apiResponse = CreateSuccessfulApiResponse(expectedWallets);

        MockHttpClient
            .Setup(x => x.PostAsync<SearchWalletByMobileRequest, List<Wallet>>(
                It.Is<string>(url => url.Contains("api/v1/wallets/search/mobile")),
                request,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _walletService.SearchWalletsByMobileAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Should().HaveCount(1);
    }

    [Test]
    public async Task WalletToWalletTransferAsync_WithValidRequest_ReturnsTransferResult()
    {
        // Arrange
        var request = CreateValidWalletTransferRequest();
        var expectedResult = CreateTestWalletTransferResult();
        var apiResponse = CreateSuccessfulApiResponse(expectedResult);

        MockHttpClient
            .Setup(x => x.PutAsync<WalletToWalletTransferRequest, WalletTransferResult>(
                It.Is<string>(url => url.Contains("api/v1/wallets/wallet/transaction/v2/wallet-to-wallet")),
                request,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _walletService.WalletToWalletTransferAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.TransactionReference.Should().Be(expectedResult.TransactionReference);
    }

    [Test]
    public async Task GetWalletTransferStatusAsync_WithValidReference_ReturnsTransferStatus()
    {
        // Arrange
        var reference = "TXN_12345";
        var expectedStatus = CreateTestWalletTransferStatus();
        var apiResponse = CreateSuccessfulApiResponse(expectedStatus);

        MockHttpClient
            .Setup(x => x.GetAsync<WalletTransferStatus>(
                It.Is<string>(url =>
                    url.Contains($"api/v1/wallets/wallet/transaction/wallet-to-wallet/status/{reference}")),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _walletService.GetWalletTransferStatusAsync(reference);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.TransactionReference.Should().Be(expectedStatus.TransactionReference);
    }

    [Test]
    public async Task RestrictWalletAsync_WithValidRequest_ReturnsSuccess()
    {
        // Arrange
        var request = CreateValidRestrictWalletRequest();
        var apiResponse = CreateSuccessfulApiResponse(new object());

        MockHttpClient
            .Setup(x => x.PatchAsync<RestrictWalletRequest, object>(
                It.Is<string>(url => url.Contains("api/v1/wallets/wallet/restrict")),
                request,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _walletService.RestrictWalletAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
    }

    [Test]
    public async Task GetWalletRestrictionTypesAsync_ReturnsRestrictionTypes()
    {
        // Arrange
        var expectedTypes = new List<WalletRestrictionType>
        {
            CreateTestWalletRestrictionType("TRANSACTION_LIMIT"),
            CreateTestWalletRestrictionType("WITHDRAWAL_BLOCK")
        };
        var apiResponse = CreateSuccessfulApiResponse(expectedTypes);

        MockHttpClient
            .Setup(x => x.GetAsync<List<WalletRestrictionType>>(
                It.Is<string>(url => url.Contains("api/v1/wallets/get/restriction/types")),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _walletService.GetWalletRestrictionTypesAsync();

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Should().HaveCount(2);
    }

    [Test]
    public void GetWalletAsync_WithNullId_ThrowsArgumentException()
    {
        // Act & Assert
        Assert.ThrowsAsync<ArgumentException>(() => _walletService.GetWalletAsync(null!));
    }

    [Test]
    public void GetWalletAsync_WithEmptyId_ThrowsArgumentException()
    {
        // Act & Assert
        Assert.ThrowsAsync<ArgumentException>(() => _walletService.GetWalletAsync(string.Empty));
    }

    [Test]
    public void SearchWalletsByEmailAsync_WithNullRequest_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.ThrowsAsync<ArgumentNullException>(() => _walletService.SearchWalletsByEmailAsync(null!));
    }

    private CreateWalletRequest CreateValidWalletRequest()
    {
        return new CreateWalletRequest
        {
            CustomerId = CreateTestGuid().ToString(),
            CurrencyId = "fd5e474d-bb42-4db1-ab74-e8d2a01047e9",
            WalletClassificationId = CreateTestGuid().ToString(),
            CustomerTypeId = "f671da57-e281-4b40-965f-a96f4205405e",
            IsInternal = false,
            IsDefault = true,
            Name = "Test Wallet"
        };
    }

    private CreateCorporateWalletRequest CreateValidCorporateWalletRequest()
    {
        return new CreateCorporateWalletRequest
        {
            CurrencyId = Guid.Parse("fd5e474d-bb42-4db1-ab74-e8d2a01047e9"),
            Name = "Corporate Test Wallet",
            VirtualAccount = new WalletVirtualAccount
            {
                AccountNumber = "1234567890",
                BankCode = "011",
                BankName = "First Bank"
            }
        };
    }

    private WalletToWalletTransferRequest CreateValidWalletTransferRequest()
    {
        return new WalletToWalletTransferRequest
        {
            FromAccount = "1234567890",
            ToAccount = "0987654321",
            Amount = 1000.00,
            Remarks = "Test transfer",
            TransactionReference = "TXN_TEST_12345"
        };
    }

    private RestrictWalletRequest CreateValidRestrictWalletRequest()
    {
        return new RestrictWalletRequest
        {
            WalletId = CreateTestGuid(),
            WalletRestrictionId = CreateTestGuid(),
            Reason = "Test restriction"
        };
    }

    private Wallet CreateTestWallet()
    {
        return new Wallet
        {
            Id = CreateTestGuid(),
            CustomerId = CreateTestGuid(),
            Name = "Test Wallet",
            CurrencyId = Guid.Parse("fd5e474d-bb42-4db1-ab74-e8d2a01047e9"),
            LedgerBalance = 50000.00,
            AvailableBalance = 45000.00,
            IsDefault = true,
            VirtualAccount = CreateTestVirtualAccount(),
            WalletGroupId = CreateTestGuid(),
            WalletClassificationId = CreateTestGuid(),
            CustomerTypeId = Guid.Parse("f671da57-e281-4b40-965f-a96f4205405e"),
            IsInternal = false
        };
    }

    private WalletVirtualAccount CreateTestVirtualAccount()
    {
        return new WalletVirtualAccount
        {
            AccountNumber = "1234567890",
            BankName = "Test Bank",
            BankCode = "001"
        };
    }

    private WalletTransferResult CreateTestWalletTransferResult()
    {
        return new WalletTransferResult
        {
            TransactionReference = "TXN_12345",
            Success = true,
            Message = "Transfer completed successfully",
            TransactionId = CreateTestGuid()
        };
    }

    private WalletTransferStatus CreateTestWalletTransferStatus()
    {
        return new WalletTransferStatus
        {
            TransactionReference = "TXN_12345",
            Status = "successful",
            StatusDescription = "Transfer completed successfully",
            Amount = 1000.00,
            FromAccount = "1234567890",
            ToAccount = "0987654321",
            TransactionDate = CreateTestTimestamp().DateTime
        };
    }

    private WalletRestrictionType CreateTestWalletRestrictionType(string code)
    {
        return new WalletRestrictionType
        {
            Id = CreateTestGuid(),
            Name = $"Test Restriction - {code}",
            Description = $"Test description for {code}",
            IsActive = true
        };
    }
}