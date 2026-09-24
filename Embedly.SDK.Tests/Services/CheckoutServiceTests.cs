using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Embedly.SDK.Http;
using Embedly.SDK.Models.Requests.Checkout;
using Embedly.SDK.Models.Responses.Checkout;
using Embedly.SDK.Services.Checkout;
using Embedly.SDK.Tests.Testing;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Embedly.SDK.Tests.Services;

/// <summary>
///     Unit tests for CheckoutService following SDK patterns.
///     Tests checkout wallet generation and management operations.
/// </summary>
[TestFixture]
public class CheckoutServiceTests : ServiceTestBase
{
    private CheckoutService _checkoutService = null!;

    protected override void OnSetUp()
    {
        _checkoutService = new CheckoutService(MockHttpClient.Object, MockOptions.Object);
    }

    [Test]
    public async Task GenerateCheckoutWalletAsync_WithValidRequest_ReturnsCheckoutWallet()
    {
        // Arrange
        var request = CreateValidGenerateCheckoutWalletRequest();
        var expectedWallet = CreateTestCheckoutWallet();
        var apiResponse = CreateSuccessfulApiResponse(expectedWallet);

        MockHttpClient
            .Setup(x => x.PostAsync<GenerateCheckoutWalletRequest, CheckoutWallet>(
                It.IsAny<string>(),
                request,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _checkoutService.GenerateCheckoutWalletAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Id.Should().Be(expectedWallet.Id);
        result.Data.ExpectedAmount.Should().Be(expectedWallet.ExpectedAmount);
    }

    [Test]
    public void GenerateCheckoutWalletAsync_WithNullRequest_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.ThrowsAsync<ArgumentNullException>(() => _checkoutService.GenerateCheckoutWalletAsync(null!));
    }

    [Test]
    public async Task GenerateCheckoutWalletAsync_WithDocumentedResponse_PostsBodyAndDeserializesWallet()
    {
        // Arrange - documented Create Checkout Wallet response
        const string responseJson = @"{
              ""statusCode"": 200,
              ""message"": ""Checkout wallet reactivated successfully"",
              ""data"": {
                  ""id"": ""bfce68af-4b43-4827-abb8-985a2d3b8a79"",
                  ""walletNumber"": ""2225657965"",
                  ""organizationId"": ""02600494-1a3c-11f0-a818-6045bd97b81d"",
                  ""walletName"": ""Embedly Check Demo"",
                  ""status"": ""Reactivated"",
                  ""createdAt"": ""2025-07-14T16:47:32.297357Z"",
                  ""expiresAt"": ""2025-07-24T14:28:01.1254478Z"",
                  ""reactivatedAt"": ""2025-07-24T13:58:01.1254127Z"",
                  ""expectedAmount"": 20000,
                  ""checkoutRef"": ""CHK202507241358013544205""
              }
            }";

        var handler = new StubHttpMessageHandler(responseJson);
        var httpClient = new EmbedlyHttpClient(new HttpClient(handler), MockOptions.Object);
        var service = new CheckoutService(httpClient, MockOptions.Object);

        var beneficiaryId = Guid.Parse("11111111-2222-3333-4444-555555555555");
        var request = new GenerateCheckoutWalletRequest
        {
            OrganizationId = Guid.Parse("02600494-1a3c-11f0-a818-6045bd97b81d"),
            ExpectedAmount = 20000,
            OrganizationPrefixMappingId = Guid.Parse("8b4432bc-f3c7-4055-b5e5-e232707e79af"),
            InvoiceReference = "INV-001",
            CurrencyCode = "NGN",
            CustomerEmail = "customer@example.com",
            SplitType = "Percentage",
            IncomeSplitConfig = new List<IncomeSplitConfig>
            {
                new() { BeneficiaryId = beneficiaryId, SplitValue = 60, FeeValue = 50, FeeBearer = true }
            }
        };

        // Act
        var result = await service.GenerateCheckoutWalletAsync(request);

        // Assert - request
        handler.LastRequestMethod.Should().Be(HttpMethod.Post);
        handler.LastRequestUri!.AbsoluteUri.Should().Be("https://checkout-staging.embedly.ng/api/v1/checkout-wallet");

        using (var body = JsonDocument.Parse(handler.LastRequestBody!))
        {
            var root = body.RootElement;
            root.GetProperty("organizationId").GetString().Should().Be("02600494-1a3c-11f0-a818-6045bd97b81d");
            root.GetProperty("expectedAmount").GetDecimal().Should().Be(20000);
            root.GetProperty("organizationPrefixMappingId").GetString()
                .Should().Be("8b4432bc-f3c7-4055-b5e5-e232707e79af");
            root.GetProperty("expiryDurationMinutes").GetInt32().Should().Be(30);
            root.GetProperty("invoiceReference").GetString().Should().Be("INV-001");
            root.GetProperty("currencyCode").GetString().Should().Be("NGN");
            root.GetProperty("customerEmail").GetString().Should().Be("customer@example.com");
            root.GetProperty("splitType").GetString().Should().Be("Percentage");

            var split = root.GetProperty("incomeSplitConfig")[0];
            split.GetProperty("beneficiaryId").GetString().Should().Be(beneficiaryId.ToString());
            split.GetProperty("splitValue").GetDecimal().Should().Be(60);
            split.GetProperty("feeValue").GetDecimal().Should().Be(50);
            split.GetProperty("feeBearer").GetBoolean().Should().BeTrue();

            // Unset optional fields are omitted rather than sent as null
            root.TryGetProperty("description", out _).Should().BeFalse();
            root.TryGetProperty("customerName", out _).Should().BeFalse();
            root.TryGetProperty("metadata", out _).Should().BeFalse();
            root.TryGetProperty("accountInquiryResponseName", out _).Should().BeFalse();
        }

        // Assert - response
        result.Success.Should().BeTrue();
        result.StatusCode.Should().Be(200);
        result.Message.Should().Be("Checkout wallet reactivated successfully");

        var wallet = result.Data!;
        wallet.Id.Should().Be(Guid.Parse("bfce68af-4b43-4827-abb8-985a2d3b8a79"));
        wallet.WalletNumber.Should().Be("2225657965");
        wallet.OrganizationId.Should().Be(Guid.Parse("02600494-1a3c-11f0-a818-6045bd97b81d"));
        wallet.WalletName.Should().Be("Embedly Check Demo");
        wallet.Status.Should().Be("Reactivated");
        wallet.CreatedAt.Should().Be(new DateTime(2025, 7, 14, 16, 47, 32, DateTimeKind.Utc).AddTicks(2973570));
        wallet.ExpiresAt.Should().Be(new DateTime(2025, 7, 24, 14, 28, 1, DateTimeKind.Utc).AddTicks(1254478));
        wallet.ReactivatedAt.Should().Be(new DateTime(2025, 7, 24, 13, 58, 1, DateTimeKind.Utc).AddTicks(1254127));
        wallet.ExpectedAmount.Should().Be(20000m);
        wallet.CheckoutRef.Should().Be("CHK202507241358013544205");
    }

    [Test]
    public async Task GetCheckoutWalletsAsync_WithValidRequest_ReturnsWalletList()
    {
        // Arrange
        var request = CreateValidGetCheckoutWalletsRequest();
        var expectedWallets = new List<CheckoutWallet> { CreateTestCheckoutWallet() };
        var apiResponse = CreateSuccessfulApiResponse(expectedWallets);

        MockHttpClient
            .Setup(x => x.GetAsync<List<CheckoutWallet>>(
                It.IsAny<string>(),
                It.IsAny<Dictionary<string, object?>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _checkoutService.GetCheckoutWalletsAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Should().HaveCount(1);
    }

    [Test]
    public async Task GetCheckoutWalletsAsync_WithAllFilters_SendsEveryQueryParam()
    {
        // Arrange
        var request = new GetCheckoutWalletsRequest
        {
            OrganizationId = Guid.Parse("0075b72d-6648-11f0-a7cf-0274f77d4a81"),
            Page = 2,
            PageSize = 25,
            Status = "Expired",
            StartDate = "2025-11-01",
            EndDate = "2025-11-30",
            WalletNumber = "7537432415",
            OrganizationPrefixMappingId = Guid.Parse("8b4432bc-f3c7-4055-b5e5-e232707e79af")
        };
        var handler = new StubHttpMessageHandler(@"{ ""statusCode"": 200, ""message"": ""success"", ""data"": [] }");
        var service = new CheckoutService(
            new EmbedlyHttpClient(new HttpClient(handler), MockOptions.Object), MockOptions.Object);

        // Act
        await service.GetCheckoutWalletsAsync(request);

        // Assert
        handler.LastRequestMethod.Should().Be(HttpMethod.Get);
        handler.LastRequestUri!.AbsoluteUri.Should().Be(
            "https://checkout-staging.embedly.ng/api/v1/checkout-wallet" +
            "?organizationId=0075b72d-6648-11f0-a7cf-0274f77d4a81&page=2&pageSize=25&status=Expired" +
            "&startDate=2025-11-01&endDate=2025-11-30&walletNumber=7537432415" +
            "&organizationPrefixMappingId=8b4432bc-f3c7-4055-b5e5-e232707e79af");
    }

    [Test]
    public void GetCheckoutWalletsRequest_WithOnlyOrganizationId_UsesDefaultPaginationAndOmitsFilters()
    {
        // Arrange
        var organizationId = CreateTestGuid();
        var request = new GetCheckoutWalletsRequest { OrganizationId = organizationId };

        // Act
        var queryParams = request.ToQueryParameters();

        // Assert
        queryParams.Should().BeEquivalentTo(new Dictionary<string, object?>
        {
            ["organizationId"] = organizationId,
            ["page"] = 1,
            ["pageSize"] = 10
        });
    }

    [Test]
    public void GetCheckoutWalletsAsync_WithEmptyOrganizationId_ThrowsArgumentException()
    {
        // Arrange
        var request = new GetCheckoutWalletsRequest { OrganizationId = Guid.Empty };

        // Act & Assert
        var exception = Assert.ThrowsAsync<ArgumentException>(() => _checkoutService.GetCheckoutWalletsAsync(request));
        exception!.ParamName.Should().Be("organizationId");
    }

    [Test]
    public void GetCheckoutWalletsAsync_WithNullRequest_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.ThrowsAsync<ArgumentNullException>(() => _checkoutService.GetCheckoutWalletsAsync(null!));
    }

    [Test]
    public async Task GetCheckoutWalletsAsync_WithDocumentedResponse_DeserializesWalletsSplitsAndPagination()
    {
        // Arrange - documented Get Checkout Wallets response
        const string responseJson = @"{
            ""statusCode"": 200,
            ""message"": ""success"",
            ""data"": [
              {
                ""id"": ""d8c01350-b07a-4547-a9e6-71e3654dca7c"",
                ""walletNumber"": ""7537432415"",
                ""organizationId"": ""0075b72d-6648-11f0-a7cf-0274f77d4a81"",
                ""walletName"": ""Seafinish Global"",
                ""status"": ""Expired"",
                ""createdAt"": ""2025-11-19T17:41:48.083601Z"",
                ""expiresAt"": ""2025-11-19T18:31:48.083613Z"",
                ""usedAt"": null,
                ""expiredAt"": ""2025-11-19T18:32:45.075012Z"",
                ""reactivatedAt"": null,
                ""expectedAmount"": 5000,
                ""invoiceReference"": null,
                ""description"": null,
                ""currencyCode"": ""NGN"",
                ""customerEmail"": ""ada@getnada.com"",
                ""customerName"": null,
                ""metadata"": ""{\""name\"": \""Ada\""}"",
                ""splitType"": ""Fixed"",
                ""splitConfigurations"": [
                  {
                    ""id"": ""1b6bd9e3-0822-4840-b414-a75c22cafe7b"",
                    ""beneficiaryId"": ""cb0ada60-472a-49a3-b1d2-35d25522c5d8"",
                    ""splitValue"": 3000,
                    ""feeValue"": 500,
                    ""feeBearer"": true,
                    ""beneficiary"": {
                      ""id"": ""cb0ada60-472a-49a3-b1d2-35d25522c5d8"",
                      ""beneficiaryName"": ""Ikem"",
                      ""accountNumber"": ""7035336912"",
                      ""bankCode"": ""034"",
                      ""beneficiaryAlias"": ""Ada"",
                      ""isActive"": true
                    }
                  },
                  {
                    ""id"": ""d921f009-2537-4934-ba77-4e6e75b85171"",
                    ""beneficiaryId"": ""3c10b0c8-d1e1-4814-b997-561c45454964"",
                    ""splitValue"": 2000,
                    ""feeValue"": 500,
                    ""feeBearer"": true,
                    ""beneficiary"": {
                      ""id"": ""3c10b0c8-d1e1-4814-b997-561c45454964"",
                      ""beneficiaryName"": ""lope"",
                      ""accountNumber"": ""3469967352"",
                      ""bankCode"": ""099"",
                      ""beneficiaryAlias"": ""Mel"",
                      ""isActive"": true
                    }
                  }
                ],
                ""walletHistories"": [
                  {
                    ""id"": ""31187004-d803-401c-92d9-538b503d08f0"",
                    ""checkoutRef"": ""CHK202511191741489589689"",
                    ""expectedAmount"": 5000,
                    ""generatedAt"": ""2025-11-19T17:41:48.962458Z"",
                    ""usedAt"": null,
                    ""status"": ""Expired"",
                    ""transactionId"": null
                  }
                ]
              }
            ],
            ""pagination"": {
              ""currentPage"": 1,
              ""pageSize"": 10,
              ""totalCount"": 82,
              ""totalPages"": 9,
              ""hasNextPage"": true,
              ""hasPreviousPage"": false
            }
            }";

        var handler = new StubHttpMessageHandler(responseJson);
        var service = new CheckoutService(
            new EmbedlyHttpClient(new HttpClient(handler), MockOptions.Object), MockOptions.Object);

        // Act
        var result = await service.GetCheckoutWalletsAsync(
            new GetCheckoutWalletsRequest { OrganizationId = Guid.Parse("0075b72d-6648-11f0-a7cf-0274f77d4a81") });

        // Assert - request
        handler.LastRequestMethod.Should().Be(HttpMethod.Get);
        handler.LastRequestUri!.AbsoluteUri.Should().Be(
            "https://checkout-staging.embedly.ng/api/v1/checkout-wallet" +
            "?organizationId=0075b72d-6648-11f0-a7cf-0274f77d4a81&page=1&pageSize=10");

        // Assert - wallet
        result.Success.Should().BeTrue();
        var wallet = result.Data.Should().ContainSingle().Subject;
        wallet.Id.Should().Be(Guid.Parse("d8c01350-b07a-4547-a9e6-71e3654dca7c"));
        wallet.WalletNumber.Should().Be("7537432415");
        wallet.OrganizationId.Should().Be(Guid.Parse("0075b72d-6648-11f0-a7cf-0274f77d4a81"));
        wallet.WalletName.Should().Be("Seafinish Global");
        wallet.Status.Should().Be("Expired");
        wallet.UsedAt.Should().BeNull();
        wallet.ExpiredAt.Should().Be(new DateTime(2025, 11, 19, 18, 32, 45, DateTimeKind.Utc).AddTicks(750120));
        wallet.ReactivatedAt.Should().BeNull();
        wallet.ExpectedAmount.Should().Be(5000m);
        wallet.InvoiceReference.Should().BeNull();
        wallet.Description.Should().BeNull();
        wallet.CurrencyCode.Should().Be("NGN");
        wallet.CustomerEmail.Should().Be("ada@getnada.com");
        wallet.CustomerName.Should().BeNull();
        wallet.Metadata.Should().Be("{\"name\": \"Ada\"}");
        wallet.SplitType.Should().Be("Fixed");
        wallet.Transactions.Should().BeNull();

        // Assert - split configurations
        wallet.SplitConfigurations.Should().HaveCount(2);
        var split = wallet.SplitConfigurations![0];
        split.Id.Should().Be(Guid.Parse("1b6bd9e3-0822-4840-b414-a75c22cafe7b"));
        split.BeneficiaryId.Should().Be(Guid.Parse("cb0ada60-472a-49a3-b1d2-35d25522c5d8"));
        split.SplitValue.Should().Be(3000m);
        split.FeeValue.Should().Be(500m);
        split.FeeBearer.Should().BeTrue();
        split.Beneficiary!.Id.Should().Be(split.BeneficiaryId);
        split.Beneficiary.BeneficiaryName.Should().Be("Ikem");
        split.Beneficiary.AccountNumber.Should().Be("7035336912");
        split.Beneficiary.BankCode.Should().Be("034");
        split.Beneficiary.BeneficiaryAlias.Should().Be("Ada");
        split.Beneficiary.IsActive.Should().BeTrue();
        wallet.SplitConfigurations[1].Beneficiary!.BeneficiaryName.Should().Be("lope");

        // Assert - wallet history
        var history = wallet.WalletHistories.Should().ContainSingle().Subject;
        history.CheckoutRef.Should().Be("CHK202511191741489589689");
        history.UsedAt.Should().BeNull();
        history.Status.Should().Be("Expired");
        history.TransactionId.Should().BeNull();

        // Assert - pagination
        result.Pagination!.Page.Should().Be(1);
        result.Pagination.PageSize.Should().Be(10);
        result.Pagination.TotalItems.Should().Be(82);
        result.Pagination.TotalPages.Should().Be(9);
        result.Pagination.HasNext.Should().BeTrue();
        result.Pagination.HasPrevious.Should().BeFalse();
    }

    [Test]
    public void GenerateCheckoutWalletAsync_WithEmptyOrganizationId_ThrowsArgumentException()
    {
        // Arrange
        var request = CreateValidGenerateCheckoutWalletRequest() with { OrganizationId = Guid.Empty };

        // Act & Assert
        var exception = Assert.ThrowsAsync<ArgumentException>(() => _checkoutService.GenerateCheckoutWalletAsync(request));
        exception!.ParamName.Should().Be("organizationId");
        MockHttpClient.VerifyNoOtherCalls();
    }

    [Test]
    public void GetCheckoutWalletWithTransactionsAsync_WithEmptyOrganizationId_ThrowsArgumentException()
    {
        // Act & Assert
        var exception = Assert.ThrowsAsync<ArgumentException>(() =>
            _checkoutService.GetCheckoutWalletWithTransactionsAsync(CreateTestGuid(), Guid.Empty));
        exception!.ParamName.Should().Be("organizationId");
        MockHttpClient.VerifyNoOtherCalls();
    }

    [Test]
    public async Task GetCheckoutWalletWithTransactionsAsync_WithValidIds_ReturnsWalletWithTransactions()
    {
        // Arrange
        var walletId = CreateTestGuid();
        var organizationId = CreateTestGuid();
        var expectedWallet = CreateTestCheckoutWallet();
        var apiResponse = CreateSuccessfulApiResponse(expectedWallet);

        MockHttpClient
            .Setup(x => x.GetAsync<CheckoutWallet>(
                It.IsAny<string>(),
                It.IsAny<Dictionary<string, object?>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _checkoutService.GetCheckoutWalletWithTransactionsAsync(walletId, organizationId);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Id.Should().Be(expectedWallet.Id);
    }

    [Test]
    public async Task GetCheckoutWalletWithTransactionsAsync_WithDocumentedResponse_DeserializesHistoriesAndTransactions()
    {
        // Arrange - documented Get Checkout Wallet Transactions response
        const string responseJson = @"{
              ""statusCode"": 200,
              ""message"": ""success"",
              ""data"": {
                ""id"": ""4e5e4451-654f-4bdd-9636-4f68ea52fa1e"",
                ""walletNumber"": ""2222412513"",
                ""organizationId"": ""02600494-1a3c-11f0-a818-6045bd97b81d"",
                ""walletName"": ""Org Test"",
                ""status"": ""Used"",
                ""createdAt"": ""2025-10-29T09:16:30.606152Z"",
                ""expiresAt"": ""2025-10-29T09:46:30.606152Z"",
                ""usedAt"": ""2025-10-29T09:16:41.769188Z"",
                ""expiredAt"": null,
                ""reactivatedAt"": null,
                ""expectedAmount"": 20000.0,
                ""walletHistories"": [
                  {
                    ""id"": ""eb1a7058-5ac2-4dd5-9be2-d5f5a7fa6b30"",
                    ""checkoutRef"": ""CHK202510290916306114570"",
                    ""expectedAmount"": 20000.0,
                    ""generatedAt"": ""2025-10-29T09:16:30.611608Z"",
                    ""usedAt"": ""2025-10-29T09:16:41.798649Z"",
                    ""status"": ""Used"",
                    ""transactionId"": ""b2381adf-23f1-43d3-bffc-be9f24637220""
                  }
                ],
                ""transactions"": [
                  {
                    ""id"": ""b2381adf-23f1-43d3-bffc-be9f24637220"",
                    ""amount"": 20000.0,
                    ""senderAccountNumber"": ""3333002345"",
                    ""senderName"": ""Nkemakolam Ekeh"",
                    ""recipientAccountNumber"": ""2222412513"",
                    ""recipientName"": ""Org Test"",
                    ""organizationSettlementAccount"": ""9710001442"",
                    ""status"": ""Completed"",
                    ""reference"": ""07f0a353-bd08-4afc-abfd-c8ba5320947c"",
                    ""createdAt"": ""2025-10-29T09:16:41.075592Z"",
                    ""completedAt"": ""2025-10-29T09:16:41.400539Z"",
                    ""sessionId"": ""000001100913103301927365890002"",
                    ""reversalId"": null,
                    ""reversalAttemptedAt"": null
                  }
                ]
              }
            }";

        var handler = new StubHttpMessageHandler(responseJson);
        var httpClient = new EmbedlyHttpClient(new HttpClient(handler), MockOptions.Object);
        var service = new CheckoutService(httpClient, MockOptions.Object);

        var walletId = Guid.Parse("4e5e4451-654f-4bdd-9636-4f68ea52fa1e");
        var organizationId = Guid.Parse("02600494-1a3c-11f0-a818-6045bd97b81d");

        // Act
        var result = await service.GetCheckoutWalletWithTransactionsAsync(walletId, organizationId);

        // Assert - request
        handler.LastRequestMethod.Should().Be(HttpMethod.Get);
        handler.LastRequestUri!.AbsoluteUri.Should().Be(
            "https://checkout-staging.embedly.ng/api/v1/checkout-wallet/4e5e4451-654f-4bdd-9636-4f68ea52fa1e/transactions" +
            "?organizationId=02600494-1a3c-11f0-a818-6045bd97b81d");

        // Assert - wallet
        result.Success.Should().BeTrue();
        result.Message.Should().Be("success");

        var wallet = result.Data!;
        wallet.Id.Should().Be(walletId);
        wallet.WalletNumber.Should().Be("2222412513");
        wallet.OrganizationId.Should().Be(organizationId);
        wallet.WalletName.Should().Be("Org Test");
        wallet.Status.Should().Be("Used");
        wallet.UsedAt.Should().Be(new DateTime(2025, 10, 29, 9, 16, 41, DateTimeKind.Utc).AddTicks(7691880));
        wallet.ExpiredAt.Should().BeNull();
        wallet.ReactivatedAt.Should().BeNull();
        wallet.ExpectedAmount.Should().Be(20000m);

        // Assert - wallet histories
        var history = wallet.WalletHistories.Should().ContainSingle().Subject;
        history.Id.Should().Be(Guid.Parse("eb1a7058-5ac2-4dd5-9be2-d5f5a7fa6b30"));
        history.CheckoutRef.Should().Be("CHK202510290916306114570");
        history.ExpectedAmount.Should().Be(20000m);
        history.GeneratedAt.Should().Be(new DateTime(2025, 10, 29, 9, 16, 30, DateTimeKind.Utc).AddTicks(6116080));
        history.UsedAt.Should().Be(new DateTime(2025, 10, 29, 9, 16, 41, DateTimeKind.Utc).AddTicks(7986490));
        history.Status.Should().Be("Used");
        history.TransactionId.Should().Be(Guid.Parse("b2381adf-23f1-43d3-bffc-be9f24637220"));

        // Assert - transactions
        var transaction = wallet.Transactions.Should().ContainSingle().Subject;
        transaction.Id.Should().Be(Guid.Parse("b2381adf-23f1-43d3-bffc-be9f24637220"));
        transaction.Amount.Should().Be(20000m);
        transaction.SenderAccountNumber.Should().Be("3333002345");
        transaction.SenderName.Should().Be("Nkemakolam Ekeh");
        transaction.RecipientAccountNumber.Should().Be("2222412513");
        transaction.RecipientName.Should().Be("Org Test");
        transaction.OrganizationSettlementAccount.Should().Be("9710001442");
        transaction.Status.Should().Be("Completed");
        transaction.Reference.Should().Be("07f0a353-bd08-4afc-abfd-c8ba5320947c");
        transaction.CreatedAt.Should().Be(new DateTime(2025, 10, 29, 9, 16, 41, DateTimeKind.Utc).AddTicks(755920));
        transaction.CompletedAt.Should().Be(new DateTime(2025, 10, 29, 9, 16, 41, DateTimeKind.Utc).AddTicks(4005390));
        transaction.SessionId.Should().Be("000001100913103301927365890002");
        transaction.ReversalId.Should().BeNull();
        transaction.ReversalAttemptedAt.Should().BeNull();
    }

    [Test]
    public async Task CreateSplitBeneficiaryAsync_WithDocumentedResponse_PostsBodyAndDeserializesBeneficiary()
    {
        // Arrange - documented Create Split Beneficiary response
        const string responseJson = @"{
              ""statusCode"": 200,
              ""message"": ""Split beneficiary created successfully"",
              ""data"": {
                ""id"": ""1c7e2c80-9f8c-4bd8-8c9d-bd63810c5df9"",
                ""organizationId"": ""0075b72d-6648-11f0-a7cf-0274f77d4a81"",
                ""beneficiaryName"": ""string"",
                ""accountNumber"": ""4278816938"",
                ""bankCode"": ""string"",
                ""bankName"": ""string"",
                ""beneficiaryAlias"": ""string"",
                ""isActive"": true,
                ""createdAt"": ""2026-02-16T07:57:04.4688132Z"",
                ""updatedAt"": null
              }
            }";

        var handler = new StubHttpMessageHandler(responseJson);
        var service = new CheckoutService(
            new EmbedlyHttpClient(new HttpClient(handler), MockOptions.Object), MockOptions.Object);

        var request = new CreateSplitBeneficiaryRequest
        {
            OrganizationId = Guid.Parse("0075b72d-6648-11f0-a7cf-0274f77d4a81"),
            BeneficiaryName = "John Doe",
            AccountNumber = "4278816938",
            BankName = "Sterling Bank"
        };

        // Act
        var result = await service.CreateSplitBeneficiaryAsync(request);

        // Assert - request
        handler.LastRequestMethod.Should().Be(HttpMethod.Post);
        handler.LastRequestUri!.AbsoluteUri.Should().Be("https://checkout-staging.embedly.ng/api/v1/split-beneficiaries");

        using (var body = JsonDocument.Parse(handler.LastRequestBody!))
        {
            var root = body.RootElement;
            root.GetProperty("organizationId").GetString().Should().Be("0075b72d-6648-11f0-a7cf-0274f77d4a81");
            root.GetProperty("beneficiaryName").GetString().Should().Be("John Doe");
            root.GetProperty("accountNumber").GetString().Should().Be("4278816938");
            root.GetProperty("bankName").GetString().Should().Be("Sterling Bank");

            // Unset optional fields are omitted rather than sent as null
            root.TryGetProperty("bankCode", out _).Should().BeFalse();
            root.TryGetProperty("beneficiaryAlias", out _).Should().BeFalse();
        }

        // Assert - response
        result.Success.Should().BeTrue();
        result.Message.Should().Be("Split beneficiary created successfully");

        var beneficiary = result.Data!;
        beneficiary.Id.Should().Be(Guid.Parse("1c7e2c80-9f8c-4bd8-8c9d-bd63810c5df9"));
        beneficiary.OrganizationId.Should().Be(Guid.Parse("0075b72d-6648-11f0-a7cf-0274f77d4a81"));
        beneficiary.BeneficiaryName.Should().Be("string");
        beneficiary.AccountNumber.Should().Be("4278816938");
        beneficiary.BankCode.Should().Be("string");
        beneficiary.BankName.Should().Be("string");
        beneficiary.BeneficiaryAlias.Should().Be("string");
        beneficiary.IsActive.Should().BeTrue();
        beneficiary.CreatedAt.Should().Be(new DateTime(2026, 2, 16, 7, 57, 4, DateTimeKind.Utc).AddTicks(4688132));
        beneficiary.UpdatedAt.Should().BeNull();
    }

    [Test]
    public void CreateSplitBeneficiaryAsync_WithNullRequest_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.ThrowsAsync<ArgumentNullException>(() => _checkoutService.CreateSplitBeneficiaryAsync(null!));
    }

    [TestCase(false, "John Doe", "4278816938", "organizationId")]
    [TestCase(true, "  ", "4278816938", "beneficiaryName")]
    [TestCase(true, "John Doe", null, "accountNumber")]
    public void CreateSplitBeneficiaryAsync_WithMissingRequiredField_ThrowsArgumentException(
        bool hasOrganizationId, string beneficiaryName, string? accountNumber, string expectedParamName)
    {
        // Arrange
        var request = new CreateSplitBeneficiaryRequest
        {
            OrganizationId = hasOrganizationId ? CreateTestGuid() : Guid.Empty,
            BeneficiaryName = beneficiaryName,
            AccountNumber = accountNumber!
        };

        // Act & Assert
        var exception = Assert.ThrowsAsync<ArgumentException>(() => _checkoutService.CreateSplitBeneficiaryAsync(request));
        exception!.ParamName.Should().Be(expectedParamName);
        MockHttpClient.VerifyNoOtherCalls();
    }

    [Test]
    public async Task GetSplitBeneficiariesAsync_WithDocumentedResponse_DeserializesBeneficiariesAndPaging()
    {
        // Arrange - documented Get All Split Beneficiaries response (first and last entries of the sample)
        const string responseJson = @"{
            ""statusCode"": 200,
            ""message"": ""Beneficiaries retrieved successfully"",
            ""data"": {
              ""beneficiaries"": [
                {
                  ""id"": ""6b6e94a9-c795-4c22-a94a-bc3fce632cc9"",
                  ""organizationId"": ""0075b72d-6648-11f0-a7cf-0274f77d4a81"",
                  ""beneficiaryName"": ""string"",
                  ""accountNumber"": ""4278816936"",
                  ""bankCode"": ""string"",
                  ""bankName"": ""string"",
                  ""beneficiaryAlias"": ""string"",
                  ""isActive"": true,
                  ""createdAt"": ""2026-02-16T07:59:45.982564Z"",
                  ""updatedAt"": ""2026-02-16T08:01:42.358107Z""
                },
                {
                  ""id"": ""0dc6566a-dc10-4cd4-945a-620c71ef3d11"",
                  ""organizationId"": ""0075b72d-6648-11f0-a7cf-0274f77d4a81"",
                  ""beneficiaryName"": ""John Doe"",
                  ""accountNumber"": ""0000000000"",
                  ""bankCode"": ""000009"",
                  ""bankName"": null,
                  ""beneficiaryAlias"": ""John Doe"",
                  ""isActive"": true,
                  ""createdAt"": ""2026-01-19T17:36:38.598417Z"",
                  ""updatedAt"": ""2026-01-21T17:06:32.333421Z""
                }
              ],
              ""page"": 1,
              ""pageSize"": 20,
              ""totalCount"": 15,
              ""totalPages"": 1
            }
            }";

        var handler = new StubHttpMessageHandler(responseJson);
        var service = new CheckoutService(
            new EmbedlyHttpClient(new HttpClient(handler), MockOptions.Object), MockOptions.Object);

        // Act
        var result = await service.GetSplitBeneficiariesAsync(
            new GetSplitBeneficiariesRequest { OrganizationId = Guid.Parse("0075b72d-6648-11f0-a7cf-0274f77d4a81") });

        // Assert - request: default paging is sent, optional filters are omitted
        handler.LastRequestMethod.Should().Be(HttpMethod.Get);
        handler.LastRequestUri!.AbsoluteUri.Should().Be(
            "https://checkout-staging.embedly.ng/api/v1/split-beneficiaries" +
            "?organizationId=0075b72d-6648-11f0-a7cf-0274f77d4a81&page=1&pageSize=10");

        // Assert - response
        result.Success.Should().BeTrue();
        result.Message.Should().Be("Beneficiaries retrieved successfully");

        var page = result.Data!;
        page.Page.Should().Be(1);
        page.PageSize.Should().Be(20);
        page.TotalCount.Should().Be(15);
        page.TotalPages.Should().Be(1);
        page.Beneficiaries.Should().HaveCount(2);

        var first = page.Beneficiaries[0];
        first.Id.Should().Be(Guid.Parse("6b6e94a9-c795-4c22-a94a-bc3fce632cc9"));
        first.OrganizationId.Should().Be(Guid.Parse("0075b72d-6648-11f0-a7cf-0274f77d4a81"));
        first.BeneficiaryName.Should().Be("string");
        first.AccountNumber.Should().Be("4278816936");
        first.BankCode.Should().Be("string");
        first.BankName.Should().Be("string");
        first.BeneficiaryAlias.Should().Be("string");
        first.IsActive.Should().BeTrue();
        first.CreatedAt.Should().Be(new DateTime(2026, 2, 16, 7, 59, 45, DateTimeKind.Utc).AddTicks(9825640));
        first.UpdatedAt.Should().Be(new DateTime(2026, 2, 16, 8, 1, 42, DateTimeKind.Utc).AddTicks(3581070));

        var last = page.Beneficiaries[1];
        last.AccountNumber.Should().Be("0000000000");
        last.BankCode.Should().Be("000009");
        last.BankName.Should().BeNull();
        last.BeneficiaryAlias.Should().Be("John Doe");
    }

    [Test]
    public async Task GetSplitBeneficiariesAsync_WithAllFilters_SendsEveryQueryParam()
    {
        // Arrange
        var handler = new StubHttpMessageHandler(
            @"{ ""statusCode"": 200, ""message"": ""ok"", ""data"": { ""beneficiaries"": [] } }");
        var service = new CheckoutService(
            new EmbedlyHttpClient(new HttpClient(handler), MockOptions.Object), MockOptions.Object);

        var request = new GetSplitBeneficiariesRequest
        {
            OrganizationId = Guid.Parse("0075b72d-6648-11f0-a7cf-0274f77d4a81"),
            Page = 2,
            PageSize = 50,
            IsActive = false,
            SearchTerm = "John Doe"
        };

        // Act
        await service.GetSplitBeneficiariesAsync(request);

        // Assert - isActive is sent lowercase rather than .NET's "False"
        handler.LastRequestUri!.AbsoluteUri.Should().Be(
            "https://checkout-staging.embedly.ng/api/v1/split-beneficiaries" +
            "?organizationId=0075b72d-6648-11f0-a7cf-0274f77d4a81&page=2&pageSize=50&isActive=false" +
            "&searchTerm=John%20Doe");
    }

    [Test]
    public void GetSplitBeneficiariesAsync_WithNullRequest_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.ThrowsAsync<ArgumentNullException>(() => _checkoutService.GetSplitBeneficiariesAsync(null!));
    }

    [Test]
    public void GetSplitBeneficiariesAsync_WithEmptyOrganizationId_ThrowsArgumentException()
    {
        // Arrange
        var request = new GetSplitBeneficiariesRequest { OrganizationId = Guid.Empty };

        // Act & Assert
        var exception = Assert.ThrowsAsync<ArgumentException>(() => _checkoutService.GetSplitBeneficiariesAsync(request));
        exception!.ParamName.Should().Be("organizationId");
    }

    [TestCase("activate", "Beneficiary activated successfully")]
    [TestCase("deactivate", "Beneficiary deactivated successfully")]
    public async Task SetSplitBeneficiaryStatus_WithDocumentedResponse_PatchesBeneficiaryAndReturnsSuccess(
        string action, string message)
    {
        // Arrange - documented Activate / Deactivate Split Beneficiary response
        var responseJson = @"{
              ""statusCode"": 200,
              ""message"": """ + message + @""",
              ""data"": null
            }";

        var handler = new StubHttpMessageHandler(responseJson);
        var service = new CheckoutService(
            new EmbedlyHttpClient(new HttpClient(handler), MockOptions.Object), MockOptions.Object);

        var request = new SplitBeneficiaryStatusRequest
        {
            BeneficiaryId = Guid.Parse("6b6e94a9-c795-4c22-a94a-bc3fce632cc9"),
            OrganizationId = Guid.Parse("0075b72d-6648-11f0-a7cf-0274f77d4a81")
        };

        // Act
        var result = await SetSplitBeneficiaryStatus(service, action, request);

        // Assert - request
        handler.LastRequestMethod.Should().Be(HttpMethod.Patch);
        handler.LastRequestUri!.AbsoluteUri.Should().Be(
            $"https://checkout-staging.embedly.ng/api/v1/split-beneficiaries/6b6e94a9-c795-4c22-a94a-bc3fce632cc9/{action}");

        using (var body = JsonDocument.Parse(handler.LastRequestBody!))
        {
            body.RootElement.GetProperty("beneficiaryId").GetString().Should().Be("6b6e94a9-c795-4c22-a94a-bc3fce632cc9");
            body.RootElement.GetProperty("organizationId").GetString().Should().Be("0075b72d-6648-11f0-a7cf-0274f77d4a81");
        }

        // Assert - response
        result.Success.Should().BeTrue();
        result.StatusCode.Should().Be(200);
        result.Message.Should().Be(message);
        result.Data.Should().BeNull();
    }

    [TestCase("activate")]
    [TestCase("deactivate")]
    public void SetSplitBeneficiaryStatus_WithNullRequest_ThrowsArgumentNullException(string action)
    {
        // Act & Assert
        Assert.ThrowsAsync<ArgumentNullException>(() => SetSplitBeneficiaryStatus(_checkoutService, action, null!));
    }

    [TestCase("activate", false, true, "beneficiaryId")]
    [TestCase("activate", true, false, "organizationId")]
    [TestCase("deactivate", false, true, "beneficiaryId")]
    [TestCase("deactivate", true, false, "organizationId")]
    public void SetSplitBeneficiaryStatus_WithMissingRequiredField_ThrowsArgumentException(
        string action, bool hasBeneficiaryId, bool hasOrganizationId, string expectedParamName)
    {
        // Arrange
        var request = new SplitBeneficiaryStatusRequest
        {
            BeneficiaryId = hasBeneficiaryId ? CreateTestGuid(2) : Guid.Empty,
            OrganizationId = hasOrganizationId ? CreateTestGuid() : Guid.Empty
        };

        // Act & Assert
        var exception = Assert.ThrowsAsync<ArgumentException>(() =>
            SetSplitBeneficiaryStatus(_checkoutService, action, request));
        exception!.ParamName.Should().Be(expectedParamName);
        MockHttpClient.VerifyNoOtherCalls();
    }

    private static Task<Embedly.SDK.Models.Responses.Common.ApiResponse<object>> SetSplitBeneficiaryStatus(
        CheckoutService service, string action, SplitBeneficiaryStatusRequest request)
    {
        return action == "activate"
            ? service.ActivateSplitBeneficiaryAsync(request)
            : service.DeactivateSplitBeneficiaryAsync(request);
    }

    [Test]
    public async Task GetOrganizationPrefixMappingsAsync_WithRequestObject_CallsPrefixMapEndpointWithQueryParams()
    {
        // Arrange
        var request = new GetOrganizationPrefixMappingsRequest
        {
            OrganizationId = CreateTestGuid(),
            Page = 2,
            PageSize = 25,
            Search = "Demo"
        };
        var expectedMappings = new List<OrganizationPrefixMapping> { CreateTestPrefixMapping() };
        var apiResponse = CreateSuccessfulApiResponse(expectedMappings);

        MockHttpClient
            .Setup(x => x.GetAsync<List<OrganizationPrefixMapping>>(
                It.IsAny<string>(),
                It.IsAny<Dictionary<string, object?>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        var result = await _checkoutService.GetOrganizationPrefixMappingsAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data.Should().BeEquivalentTo(expectedMappings);

        MockHttpClient.Verify(
            x => x.GetAsync<List<OrganizationPrefixMapping>>(
                It.Is<string>(url => url.EndsWith("api/v1/prefix-map/me")),
                It.Is<Dictionary<string, object?>>(q =>
                    (Guid)q["organizationId"]! == request.OrganizationId &&
                    (int)q["page"]! == 2 &&
                    (int)q["pageSize"]! == 25 &&
                    (string)q["search"]! == "Demo"),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    public async Task GetOrganizationPrefixMappingsAsync_WithOrganizationIdOnly_UsesDefaultPaginationAndOmitsSearch()
    {
        // Arrange
        var organizationId = CreateTestGuid();
        var apiResponse = CreateSuccessfulApiResponse(new List<OrganizationPrefixMapping>());

        MockHttpClient
            .Setup(x => x.GetAsync<List<OrganizationPrefixMapping>>(
                It.IsAny<string>(),
                It.IsAny<Dictionary<string, object?>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(apiResponse);

        // Act
        await _checkoutService.GetOrganizationPrefixMappingsAsync(organizationId);

        // Assert
        MockHttpClient.Verify(
            x => x.GetAsync<List<OrganizationPrefixMapping>>(
                It.Is<string>(url => url.EndsWith("api/v1/prefix-map/me")),
                It.Is<Dictionary<string, object?>>(q =>
                    (Guid)q["organizationId"]! == organizationId &&
                    (int)q["page"]! == 1 &&
                    (int)q["pageSize"]! == 10 &&
                    !q.ContainsKey("search")),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    public void GetOrganizationPrefixMappingsAsync_WithNullRequest_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.ThrowsAsync<ArgumentNullException>(() =>
            _checkoutService.GetOrganizationPrefixMappingsAsync((GetOrganizationPrefixMappingsRequest)null!));
    }

    [Test]
    public void GetOrganizationPrefixMappingsAsync_WithEmptyOrganizationId_ThrowsArgumentException()
    {
        // Act & Assert
        var exception = Assert.ThrowsAsync<ArgumentException>(() =>
            _checkoutService.GetOrganizationPrefixMappingsAsync(Guid.Empty));
        exception!.ParamName.Should().Be("organizationId");
    }

    [Test]
    public async Task GetOrganizationPrefixMappingsAsync_WithDocumentedResponse_DeserializesDataAndPagination()
    {
        // Arrange - documented Checkout API response, which has no "success" field
        const string responseJson = @"{
              ""statusCode"": 200,
              ""message"": ""success"",
              ""data"": [
                {
                  ""id"": ""8b4432bc-f3c7-4055-b5e5-e232707e79af"",
                  ""secondaryPrefix"": ""56"",
                  ""primaryPrefixId"": ""2c316406-87ee-494b-a43a-d24629f4eeea"",
                  ""organizationId"": ""02600494-1a3c-11f0-a818-6045bd97b81d"",
                  ""alias"": ""Embedly Check Demo"",
                  ""organizationName"": ""Gentlemens Club"",
                  ""organizationIsActive"": ""active""
                }
              ],
              ""pagination"": {
                ""currentPage"": 1,
                ""pageSize"": 10,
                ""totalCount"": 1,
                ""totalPages"": 1,
                ""hasNextPage"": false,
                ""hasPreviousPage"": false
              }
            }";

        var handler = new StubHttpMessageHandler(responseJson);
        var httpClient = new EmbedlyHttpClient(new HttpClient(handler), MockOptions.Object);
        var service = new CheckoutService(httpClient, MockOptions.Object);

        // Act
        var result = await service.GetOrganizationPrefixMappingsAsync(
            Guid.Parse("02600494-1a3c-11f0-a818-6045bd97b81d"), search: "Embedly Check");

        // Assert - request
        handler.LastRequestUri.Should().NotBeNull();
        handler.LastRequestUri!.AbsoluteUri.Should().StartWith("https://checkout-staging.embedly.ng/api/v1/prefix-map/me?");
        handler.LastRequestUri.Query.Should()
            .Be("?organizationId=02600494-1a3c-11f0-a818-6045bd97b81d&page=1&pageSize=10&search=Embedly%20Check");

        // Assert - response
        result.Success.Should().BeTrue();
        result.StatusCode.Should().Be(200);
        result.Message.Should().Be("success");

        result.Data.Should().ContainSingle();
        var mapping = result.Data![0];
        mapping.Id.Should().Be(Guid.Parse("8b4432bc-f3c7-4055-b5e5-e232707e79af"));
        mapping.SecondaryPrefix.Should().Be("56");
        mapping.PrimaryPrefixId.Should().Be(Guid.Parse("2c316406-87ee-494b-a43a-d24629f4eeea"));
        mapping.OrganizationId.Should().Be(Guid.Parse("02600494-1a3c-11f0-a818-6045bd97b81d"));
        mapping.Alias.Should().Be("Embedly Check Demo");
        mapping.OrganizationName.Should().Be("Gentlemens Club");
        mapping.OrganizationIsActive.Should().Be("active");

        result.Pagination.Should().NotBeNull();
        result.Pagination!.Page.Should().Be(1);
        result.Pagination.PageSize.Should().Be(10);
        result.Pagination.TotalItems.Should().Be(1);
        result.Pagination.TotalPages.Should().Be(1);
        result.Pagination.HasNext.Should().BeFalse();
        result.Pagination.HasPrevious.Should().BeFalse();
    }

    private OrganizationPrefixMapping CreateTestPrefixMapping()
    {
        return new OrganizationPrefixMapping
        {
            Id = CreateTestGuid(),
            SecondaryPrefix = "56",
            PrimaryPrefixId = CreateTestGuid(2),
            OrganizationId = CreateTestGuid(3),
            Alias = "Embedly Check Demo",
            OrganizationName = "Test Organization",
            OrganizationIsActive = "active"
        };
    }

    private sealed class StubHttpMessageHandler : HttpMessageHandler
    {
        private readonly string _responseJson;

        public StubHttpMessageHandler(string responseJson)
        {
            _responseJson = responseJson;
        }

        public Uri? LastRequestUri { get; private set; }

        public HttpMethod? LastRequestMethod { get; private set; }

        public string? LastRequestBody { get; private set; }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            LastRequestUri = request.RequestUri;
            LastRequestMethod = request.Method;
            LastRequestBody = request.Content == null ? null : await request.Content.ReadAsStringAsync();

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(_responseJson, Encoding.UTF8, "application/json")
            };
        }
    }

    private GenerateCheckoutWalletRequest CreateValidGenerateCheckoutWalletRequest()
    {
        return new GenerateCheckoutWalletRequest
        {
            OrganizationId = CreateTestGuid(),
            ExpectedAmount = 2000,
            OrganizationPrefixMappingId = default,
            ExpiryDurationMinutes = 60
        };
    }

    private GetCheckoutWalletsRequest CreateValidGetCheckoutWalletsRequest()
    {
        return new GetCheckoutWalletsRequest
        {
            OrganizationId = CreateTestGuid(),
            Page = 1,
            PageSize = 10,
            Status = null
        };
    }

    private CheckoutWallet CreateTestCheckoutWallet()
    {
        return new CheckoutWallet
        {
            Id = CreateTestGuid(),
            OrganizationId = CreateTestGuid(),
            WalletName = null,
            WalletNumber = null,
            ExpectedAmount = 20000,
            Status = null,
            CreatedAt = default,
            ExpiresAt = default,
            UsedAt = null,
            ExpiredAt = CreateTestTimestamp().AddHours(24).DateTime,
            ReactivatedAt = CreateTestTimestamp().DateTime
        };
    }
}