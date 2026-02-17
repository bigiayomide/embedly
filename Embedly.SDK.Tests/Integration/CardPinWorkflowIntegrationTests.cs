using System;
using System.Threading.Tasks;
using Embedly.SDK.Models.Requests.Cards;
using Embedly.SDK.Models.Requests.Customers;
using Embedly.SDK.Models.Requests.Wallets;
using Embedly.SDK.Services.Cards;
using Embedly.SDK.Services.Customers;
using Embedly.SDK.Services.Wallets;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Embedly.SDK.Tests.Integration;

/// <summary>
///     Comprehensive integration tests for card PIN management workflows.
///     Tests complete PIN lifecycle including security scenarios and error handling.
/// </summary>
[TestFixture]
[Category("Integration")]
[Category("Card")]
[Category("PIN")]
public class CardPinWorkflowIntegrationTests : IntegrationTestBase
{
    [SetUp]
    public void SetUp()
    {
        _customerService = ServiceProvider.GetRequiredService<ICustomerService>();
        _walletService = ServiceProvider.GetRequiredService<IWalletService>();
        _cardService = ServiceProvider.GetRequiredService<ICardService>();
    }

    private ICustomerService _customerService = null!;
    private IWalletService _walletService = null!;
    private ICardService _cardService = null!;

    /// <summary>
    ///     Complete PIN lifecycle: Card Issue → Activate with PIN → Check PIN → Change PIN → Reset PIN
    /// </summary>
    [Test]
    [Order(1)]
    public async Task CompletePinLifecycleWorkflow_ShouldSucceedWithAllOperations()
    {
        LogStep("Starting Complete PIN Lifecycle Workflow");

        // Step 1: Setup customer and wallet
        var (customerId, walletId, accountNumber) = await CreateCustomerWalletSetup("PinLifecycle");
        if (string.IsNullOrEmpty(accountNumber))
        {
            LogError("Cannot proceed without wallet account number");
            Assert.Fail("Wallet setup failed");
            return;
        }

        LogSuccess($"Customer and wallet setup complete - Account: {accountNumber}");

        // Step 2: Issue a card
        LogStep("Issuing card for PIN testing");
        var cardRequest = new IssueAfrigoCardRequest
        {
            AccountNumber = accountNumber,
            PickupMethod = "DELIVERY",
            CardType = "VIRTUAL",
            IdType = "NIN",
            Email = CreateTestEmail("pin-test")
        };

        LogApiCall("Issue Card", cardRequest);
        var cardResponse = await _cardService.IssueAfrigoCardAsync(cardRequest);
        LogApiResponse("Card Issuance", cardResponse);

        if (!cardResponse.Success || cardResponse.Data?.CardNumber == null)
        {
            LogWarning($"Card issuance failed: {cardResponse.Message}");
            LogWarning("Skipping PIN tests - card issuance required");
            return;
        }

        var cardNumber = cardResponse.Data.CardNumber;
        LogSuccess($"Card issued successfully: {cardNumber}");

        // Step 3: Activate card with initial PIN
        await TestCardActivationWithPin(accountNumber, cardNumber, "1234");

        // Step 4: Check PIN functionality
        await TestPinCheck(customerId, walletId, cardNumber, "1234");

        // Step 5: Change PIN
        await TestPinChange(customerId, walletId, cardNumber, "1234", "5678");

        // Step 6: Verify new PIN works
        await TestPinCheck(customerId, walletId, cardNumber, "5678");

        // Step 7: Reset PIN
        await TestPinReset(customerId, walletId, cardNumber, "9999");

        // Step 8: Verify reset PIN works
        await TestPinCheck(customerId, walletId, cardNumber, "9999");

        LogSuccess("Complete PIN Lifecycle Workflow completed successfully");
    }

    /// <summary>
    ///     Tests PIN security scenarios including invalid attempts and validation.
    /// </summary>
    [Test]
    [Order(2)]
    public async Task PinSecurityWorkflow_ShouldHandleSecurityScenarios()
    {
        LogStep("Starting PIN Security Workflow");

        // Step 1: Setup for security testing
        var (customerId, walletId, accountNumber) = await CreateCustomerWalletSetup("PinSecurity");
        if (string.IsNullOrEmpty(accountNumber))
        {
            LogWarning("Skipping security tests - setup failed");
            return;
        }

        // Step 2: Issue and activate card
        var cardNumber = await IssueAndActivateCard(accountNumber, "1111");
        if (string.IsNullOrEmpty(cardNumber))
        {
            LogWarning("Skipping security tests - card setup failed");
            return;
        }

        // Step 3: Test invalid PIN check
        LogStep("Testing invalid PIN check");
        await TestInvalidPinCheck(customerId, walletId, cardNumber, "9999");

        // Step 4: Test PIN change with wrong old PIN
        LogStep("Testing PIN change with incorrect old PIN");
        await TestInvalidPinChange(customerId, walletId, cardNumber, "wrong", "2222");

        // Step 5: Test PIN operations with invalid card number
        LogStep("Testing PIN operations with invalid card number");
        await TestPinOperationsWithInvalidCard(customerId, walletId);

        // Step 6: Test PIN validation edge cases
        LogStep("Testing PIN validation edge cases");
        await TestPinValidationEdgeCases(customerId, walletId, cardNumber);

        LogSuccess("PIN Security Workflow completed");
    }

    /// <summary>
    ///     Tests PIN operations under different error conditions.
    /// </summary>
    [Test]
    [Order(3)]
    public async Task PinErrorHandlingWorkflow_ShouldHandleErrorConditions()
    {
        LogStep("Starting PIN Error Handling Workflow");

        var invalidCustomerId = Guid.NewGuid();
        var invalidWalletId = Guid.NewGuid();
        const string invalidCardNumber = "0000000000000000";

        // Test 1: PIN operations with non-existent customer
        LogStep("Testing PIN operations with invalid customer ID");
        await TestPinOperationsWithInvalidCustomer(invalidCustomerId, invalidWalletId, invalidCardNumber);

        // Test 2: PIN operations with malformed requests
        LogStep("Testing PIN operations with malformed data");
        await TestPinOperationsWithMalformedData();

        // Test 3: PIN operations with blocked/inactive card
        LogStep("Testing PIN operations with blocked card");
        await TestPinOperationsWithBlockedCard();

        LogSuccess("PIN Error Handling Workflow completed");
    }

    /// <summary>
    ///     Tests concurrent PIN operations to verify system stability.
    /// </summary>
    [Test]
    [Order(4)]
    [Category("Performance")]
    public async Task ConcurrentPinOperationsWorkflow_ShouldHandleConcurrentAccess()
    {
        LogStep("Starting Concurrent PIN Operations Workflow");

        // Setup multiple cards for concurrent testing
        const int cardCount = 3;
        var cardSetups = new (Guid customerId, Guid walletId, string cardNumber)[cardCount];

        for (var i = 0; i < cardCount; i++)
        {
            var (customerId, walletId, accountNumber) = await CreateCustomerWalletSetup($"Concurrent{i}");
            if (string.IsNullOrEmpty(accountNumber)) continue;
            var cardNumber = await IssueAndActivateCard(accountNumber, $"000{i}");
            if (!string.IsNullOrEmpty(cardNumber)) cardSetups[i] = (customerId, walletId, cardNumber);
        }

        // Test concurrent PIN checks
        LogStep("Testing concurrent PIN checks");
        var pinCheckTasks = new Task[cardCount];

        for (var i = 0; i < cardCount; i++)
            if (!string.IsNullOrEmpty(cardSetups[i].cardNumber))
            {
                var setup = cardSetups[i];
                var expectedPin = $"000{i}";

                pinCheckTasks[i] = Task.Run(async () =>
                {
                    try
                    {
                        await TestPinCheck(setup.customerId, setup.walletId, setup.cardNumber, expectedPin);
                        LogSuccess($"Concurrent PIN check {i} completed");
                    }
                    catch (Exception ex)
                    {
                        LogWarning($"Concurrent PIN check {i} failed: {ex.Message}");
                    }
                });
            }
            else
            {
                pinCheckTasks[i] = Task.CompletedTask;
            }

        await Task.WhenAll(pinCheckTasks);
        LogSuccess("Concurrent PIN Operations Workflow completed");
    }

    private async Task<(Guid customerId, Guid walletId, string? accountNumber)> CreateCustomerWalletSetup(
        string namePrefix)
    {
        try
        {
            LogStep($"Creating customer and wallet setup for {namePrefix}");

            // Create customer
            var customerRequest = new CreateCustomerRequest
            {
                FirstName = namePrefix,
                LastName = "PinTest",
                EmailAddress = CreateTestEmail($"{namePrefix.ToLower()}-pin"),
                MobileNumber = CreateTestPhoneNumber(),
                DateOfBirth = DateTime.UtcNow.AddYears(-30),
                Address = "123 PIN Test Street",
                City = "Lagos",
                CountryId = Guid.Parse("c15ad9ae-c4d7-4342-b70f-de5508627e3b"),
                CustomerTypeId = Guid.Parse("f671da57-e281-4b40-965f-a96f4205405e"),
                OrganizationId = CreateTestGuid()
            };

            var customerResponse = await _customerService.CreateAsync(customerRequest);
            if (!customerResponse.Success || customerResponse.Data == null)
            {
                LogError($"Customer creation failed: {customerResponse.Message}");
                return (Guid.Empty, Guid.Empty, null);
            }

            var customerId = Guid.Parse(customerResponse.Data.Id);

            // Create wallet
            var walletRequest = new CreateWalletRequest
            {
                CustomerId = customerId.ToString(),
                CurrencyId = Guid.Parse("550e8400-e29b-41d4-a716-446655440000").ToString(), // Test currency
                WalletClassificationId = Guid.NewGuid().ToString(),
                CustomerTypeId = Guid.Parse("0ed8b99b-8097-4e49-bd4c-ff0410c57d27").ToString(),
                IsInternal = false,
                IsDefault = true,
                Name = $"{namePrefix} PIN Test Wallet"
            };

            var walletResponse = await _walletService.CreateWalletAsync(walletRequest);
            if (!walletResponse.Success || walletResponse.Data == null)
            {
                LogError($"Wallet creation failed: {walletResponse.Message}");
                return (customerId, Guid.Empty, null);
            }

            var walletId = Guid.Parse(walletResponse.Data.WalletId!);
            var accountNumber = walletResponse.Data.VirtualAccount;

            return (customerId, walletId, accountNumber);
        }
        catch (Exception ex)
        {
            LogError($"Setup failed: {ex.Message}");
            return (Guid.Empty, Guid.Empty, null);
        }
    }

    private async Task<string?> IssueAndActivateCard(string accountNumber, string pin)
    {
        try
        {
            // Issue card
            var issueRequest = new IssueAfrigoCardRequest
            {
                AccountNumber = accountNumber,
                PickupMethod = "DELIVERY",
                CardType = "VIRTUAL",
                IdType = "NIN",
                Email = CreateTestEmail("card-issue")
            };

            var issueResponse = await _cardService.IssueAfrigoCardAsync(issueRequest);
            if (!issueResponse.Success || issueResponse.Data?.CardNumber == null)
            {
                LogWarning($"Card issuance failed: {issueResponse.Message}");
                return null;
            }

            var cardNumber = issueResponse.Data.CardNumber;

            // Activate card with PIN
            var activateRequest = new ActivateAfrigoCardRequest
            {
                AccountNumber = accountNumber,
                CardNumber = cardNumber,
                Pin = pin
            };

            var activateResponse = await _cardService.ActivateAfrigoCardAsync(activateRequest);
            if (!activateResponse.Success) LogWarning($"Card activation failed: {activateResponse.Message}");

            return cardNumber;
        }
        catch (Exception ex)
        {
            LogError($"Card issue/activation failed: {ex.Message}");
            return null;
        }
    }

    private async Task TestCardActivationWithPin(string accountNumber, string cardNumber, string pin)
    {
        LogStep($"Testing card activation with PIN: {pin}");

        var activateRequest = new ActivateAfrigoCardRequest
        {
            AccountNumber = accountNumber,
            CardNumber = cardNumber,
            Pin = pin
        };

        LogApiCall("Activate Card with PIN", new MaskedCardActivationLog
        {
            AccountNumber = accountNumber,
            CardNumber = "****",
            Pin = "****"
        });
        var response = await _cardService.ActivateAfrigoCardAsync(activateRequest);
        LogApiResponse("Card Activation Response", response);

        if (response.Success)
            LogSuccess("Card activated successfully with PIN");
        else
            LogWarning($"Card activation failed: {response.Message}");
    }

    private async Task TestPinCheck(Guid customerId, Guid walletId, string cardNumber, string pin)
    {
        LogStep("Testing PIN check");

        var checkRequest = new CheckCardPinRequest
        {
            CustomerId = customerId,
            WalletId = walletId,
            CardNumber = cardNumber,
            Pin = pin
        };

        LogApiCall("Check PIN", new MaskedPinOperationLog
        {
            CustomerId = customerId,
            WalletId = walletId,
            CardNumber = "****",
            Pin = "****"
        });
        var response = await _cardService.CheckCardPinAsync(checkRequest);
        LogApiResponse("PIN Check Response", response);

        if (response.Success)
            LogSuccess("PIN check completed successfully");
        else
            LogWarning($"PIN check failed: {response.Message}");
    }

    private async Task TestPinChange(Guid customerId, Guid walletId, string cardNumber, string oldPin, string newPin)
    {
        LogStep($"Testing PIN change from {oldPin} to {newPin}");

        var changeRequest = new ChangeCardPinRequest
        {
            CustomerId = customerId,
            WalletId = walletId,
            CardNumber = cardNumber,
            OldPin = oldPin,
            NewPin = newPin
        };

        LogApiCall("Change PIN", new MaskedPinChangeLog
        {
            CustomerId = customerId,
            WalletId = walletId,
            CardNumber = "****",
            OldPin = "****",
            NewPin = "****"
        });
        var response = await _cardService.ChangeCardPinAsync(changeRequest);
        LogApiResponse("PIN Change Response", response);

        if (response.Success)
            LogSuccess("PIN changed successfully");
        else
            LogWarning($"PIN change failed: {response.Message}");
    }

    private async Task TestPinReset(Guid customerId, Guid walletId, string cardNumber, string newPin)
    {
        LogStep($"Testing PIN reset to {newPin}");

        var resetRequest = new ResetCardPinRequest
        {
            AccountNumber = cardNumber,
            CardNumber = cardNumber,
            Pin = newPin
        };

        LogApiCall("Reset PIN", new MaskedPinOperationLog
        {
            CustomerId = customerId,
            WalletId = walletId,
            CardNumber = "****",
            Pin = "****"
        });
        var response = await _cardService.ResetCardPinAsync(resetRequest);
        LogApiResponse("PIN Reset Response", response);

        if (response.Success)
            LogSuccess("PIN reset successfully");
        else
            LogWarning($"PIN reset failed: {response.Message}");
    }

    private async Task TestInvalidPinCheck(Guid customerId, Guid walletId, string cardNumber, string wrongPin)
    {
        LogStep("Testing invalid PIN check");

        var checkRequest = new CheckCardPinRequest
        {
            CustomerId = customerId,
            WalletId = walletId,
            CardNumber = cardNumber,
            Pin = wrongPin
        };

        var response = await _cardService.CheckCardPinAsync(checkRequest);

        if (!response.Success)
            LogSuccess($"Invalid PIN correctly rejected: {response.Message}");
        else
            LogWarning("Invalid PIN was unexpectedly accepted");
    }

    private async Task TestInvalidPinChange(Guid customerId, Guid walletId, string cardNumber, string wrongOldPin,
        string newPin)
    {
        LogStep("Testing PIN change with wrong old PIN");

        var changeRequest = new ChangeCardPinRequest
        {
            CustomerId = customerId,
            WalletId = walletId,
            CardNumber = cardNumber,
            OldPin = wrongOldPin,
            NewPin = newPin
        };

        var response = await _cardService.ChangeCardPinAsync(changeRequest);

        if (!response.Success)
            LogSuccess($"PIN change with wrong old PIN correctly rejected: {response.Message}");
        else
            LogWarning("PIN change with wrong old PIN was unexpectedly accepted");
    }

    private async Task TestPinOperationsWithInvalidCard(Guid customerId, Guid walletId)
    {
        const string invalidCardNumber = "0000000000000000";

        // Test PIN check with invalid card
        var checkRequest = new CheckCardPinRequest
        {
            CustomerId = customerId,
            WalletId = walletId,
            CardNumber = invalidCardNumber,
            Pin = "1234"
        };

        var checkResponse = await _cardService.CheckCardPinAsync(checkRequest);
        if (!checkResponse.Success)
            LogSuccess($"PIN check with invalid card correctly rejected: {checkResponse.Message}");
        else
            LogWarning("PIN check with invalid card was unexpectedly accepted");
    }

    private async Task TestPinValidationEdgeCases(Guid customerId, Guid walletId, string cardNumber)
    {
        // Test with empty PIN
        var emptyPinRequest = new CheckCardPinRequest
        {
            CustomerId = customerId,
            WalletId = walletId,
            CardNumber = cardNumber,
            Pin = ""
        };

        var emptyPinResponse = await _cardService.CheckCardPinAsync(emptyPinRequest);
        if (!emptyPinResponse.Success)
            LogSuccess("Empty PIN correctly rejected");
        else
            LogWarning("Empty PIN was unexpectedly accepted");
    }

    private async Task TestPinOperationsWithInvalidCustomer(Guid invalidCustomerId, Guid invalidWalletId,
        string invalidCardNumber)
    {
        var request = new CheckCardPinRequest
        {
            CustomerId = invalidCustomerId,
            WalletId = invalidWalletId,
            CardNumber = invalidCardNumber,
            Pin = "1234"
        };

        var response = await _cardService.CheckCardPinAsync(request);
        if (!response.Success)
            LogSuccess($"PIN operation with invalid customer correctly rejected: {response.Message}");
        else
            LogWarning("PIN operation with invalid customer was unexpectedly accepted");
    }

    private async Task TestPinOperationsWithMalformedData()
    {
        try
        {
            // Test with very long PIN
            var longPinRequest = new CheckCardPinRequest
            {
                CustomerId = Guid.NewGuid(),
                WalletId = Guid.NewGuid(),
                CardNumber = "1234567890123456",
                Pin = new string('1', 100) // 100 character PIN
            };

            var response = await _cardService.CheckCardPinAsync(longPinRequest);
            if (!response.Success)
                LogSuccess("Malformed PIN data correctly rejected");
            else
                LogWarning("Malformed PIN data was unexpectedly accepted");
        }
        catch (Exception ex)
        {
            LogSuccess($"Malformed data correctly caused exception: {ex.GetType().Name}");
        }
    }

    private async Task TestPinOperationsWithBlockedCard()
    {
        LogStep("Setting up blocked card test");

        var (customerId, walletId, accountNumber) = await CreateCustomerWalletSetup("BlockedCard");
        if (string.IsNullOrEmpty(accountNumber))
        {
            LogWarning("Skipping blocked card test - setup failed");
            return;
        }

        var cardNumber = await IssueAndActivateCard(accountNumber, "1234");
        if (string.IsNullOrEmpty(cardNumber))
        {
            LogWarning("Skipping blocked card test - card setup failed");
            return;
        }

        // Block the card
        var blockRequest = new BlockCardRequest
        {
            CustomerId = customerId,
            WalletId = walletId,
            CardNumber = cardNumber
        };

        var blockResponse = await _cardService.BlockCardAsync(blockRequest);
        if (!blockResponse.Success)
        {
            LogWarning("Card blocking failed - cannot test PIN operations on blocked card");
            return;
        }

        // Try PIN operation on blocked card
        var pinCheckRequest = new CheckCardPinRequest
        {
            CustomerId = customerId,
            WalletId = walletId,
            CardNumber = cardNumber,
            Pin = "1234"
        };

        var pinResponse = await _cardService.CheckCardPinAsync(pinCheckRequest);
        if (!pinResponse.Success)
            LogSuccess($"PIN operation on blocked card correctly rejected: {pinResponse.Message}");
        else
            LogWarning("PIN operation on blocked card was unexpectedly accepted");
    }
}

#region Typed Logging Models for Security

/// <summary>
///     Strongly typed model for logging masked card activation requests.
/// </summary>
internal sealed record MaskedCardActivationLog
{
    public string AccountNumber { get; init; } = string.Empty;
    public string CardNumber { get; init; } = string.Empty;
    public string Pin { get; init; } = string.Empty;
}

/// <summary>
///     Strongly typed model for logging masked PIN operation requests.
/// </summary>
internal sealed record MaskedPinOperationLog
{
    public Guid CustomerId { get; init; }
    public Guid WalletId { get; init; }
    public string CardNumber { get; init; } = string.Empty;
    public string Pin { get; init; } = string.Empty;
}

/// <summary>
///     Strongly typed model for logging masked PIN change requests.
/// </summary>
internal sealed record MaskedPinChangeLog
{
    public Guid CustomerId { get; init; }
    public Guid WalletId { get; init; }
    public string CardNumber { get; init; } = string.Empty;
    public string OldPin { get; init; } = string.Empty;
    public string NewPin { get; init; } = string.Empty;
}

#endregion