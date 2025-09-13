using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using Embedly.SDK.Webhooks;
using Embedly.SDK.Tests.Testing;

namespace Embedly.SDK.Tests.Integration;

/// <summary>
/// Integration tests for webhook processing following SDK patterns.
/// Tests webhook validation, event handling, and error scenarios in realistic conditions.
/// </summary>
[TestFixture]
[Category("Integration")]
[Category("Webhook")]
public class WebhookIntegrationTests : IntegrationTestBase
{
    private WebhookValidator _webhookValidator = null!;
    private TestWebhookHandler _webhookHandler = null!;
    private const string TestWebhookSecret = "integration-test-webhook-secret-key-2024";

    [SetUp]
    public void SetUp()
    {
        _webhookValidator = new WebhookValidator(TestWebhookSecret);
        var logger = ServiceProvider.GetService(typeof(ILogger<WebhookHandler>)) as ILogger<WebhookHandler>;
        _webhookHandler = new TestWebhookHandler(logger);
    }

    /// <summary>
    /// Complete webhook processing workflow: Validation → Parsing → Handling → Response
    /// </summary>
    [Test, Order(1)]
    public async Task CompleteWebhookProcessingWorkflow_ShouldHandleRealWebhookScenarios()
    {
        LogStep("Starting Complete Webhook Processing Workflow");

        // Step 1: Customer Created Webhook
        await TestCustomerCreatedWebhook();

        // Step 2: Wallet Transaction Webhook
        await TestWalletTransactionWebhook();

        // Step 3: Card Status Changed Webhook
        await TestCardStatusChangedWebhook();

        // Step 4: KYC Update Webhook
        await TestKycUpdateWebhook();

        // Step 5: Unknown Event Webhook
        await TestUnknownEventWebhook();

        LogSuccess("Complete Webhook Processing Workflow completed successfully");
    }

    /// <summary>
    /// Tests webhook security scenarios including signature validation and replay protection.
    /// </summary>
    [Test, Order(2)]
    public async Task WebhookSecurityWorkflow_ShouldHandleSecurityThreats()
    {
        LogStep("Starting Webhook Security Workflow");

        // Step 1: Invalid signature attack
        await TestInvalidSignatureAttack();

        // Step 2: Payload tampering attack
        await TestPayloadTamperingAttack();

        // Step 3: Replay attack simulation
        await TestReplayAttackScenario();

        // Step 4: Large payload handling
        await TestLargePayloadHandling();

        // Step 5: Malformed JSON handling
        await TestMalformedJsonHandling();

        LogSuccess("Webhook Security Workflow completed");
    }

    /// <summary>
    /// Tests webhook error handling and resilience.
    /// </summary>
    [Test, Order(3)]
    public async Task WebhookErrorHandlingWorkflow_ShouldHandleFailureScenarios()
    {
        LogStep("Starting Webhook Error Handling Workflow");

        // Step 1: Handler exception scenarios
        await TestHandlerExceptionScenarios();

        // Step 2: Network timeout simulations
        await TestNetworkTimeoutSimulations();

        // Step 3: Concurrent webhook processing
        await TestConcurrentWebhookProcessing();

        // Step 4: Memory pressure scenarios
        await TestMemoryPressureScenarios();

        LogSuccess("Webhook Error Handling Workflow completed");
    }

    /// <summary>
    /// Tests webhook performance under high load.
    /// </summary>
    [Test, Order(4)]
    [Category("Performance")]
    public async Task WebhookPerformanceWorkflow_ShouldHandleHighThroughput()
    {
        LogStep("Starting Webhook Performance Workflow");

        const int webhookCount = 50;
        var processingTasks = new Task[webhookCount];

        var startTime = DateTimeOffset.UtcNow;

        // Process multiple webhooks concurrently
        for (int i = 0; i < webhookCount; i++)
        {
            var webhookIndex = i;
            processingTasks[i] = Task.Run(async () =>
            {
                try
                {
                    var payload = CreateTestWebhookPayload($"performance_test_{webhookIndex}");
                    var signature = ComputeSignature(payload);

                    var webhookEvent = _webhookValidator.ParseEvent(payload, signature);
                    webhookEvent.Should().NotBeNull();

                    await _webhookHandler.HandleEventAsync(webhookEvent!, CancellationToken.None);

                    TestContext.WriteLine($"✓ Webhook {webhookIndex} processed successfully");
                }
                catch (Exception ex)
                {
                    TestContext.WriteLine($"✗ Webhook {webhookIndex} failed: {ex.Message}");
                    throw;
                }
            });
        }

        // Wait for all webhooks to process
        await Task.WhenAll(processingTasks);

        var endTime = DateTimeOffset.UtcNow;
        var totalTime = endTime - startTime;
        var throughput = webhookCount / totalTime.TotalSeconds;

        LogSuccess($"Processed {webhookCount} webhooks in {totalTime.TotalMilliseconds:F0}ms");
        LogSuccess($"Throughput: {throughput:F2} webhooks/second");

        // Performance assertions
        totalTime.Should().BeLessThan(TimeSpan.FromSeconds(30), "Should process webhooks efficiently");
        throughput.Should().BeGreaterThan(1, "Should maintain reasonable throughput");
    }

    #region Individual Webhook Test Methods

    private async Task TestCustomerCreatedWebhook()
    {
        LogStep("Testing Customer Created Webhook");

        var payload = CreateCustomerCreatedWebhookPayload();
        var signature = ComputeSignature(payload);

        LogApiCall("Customer Created Webhook", new WebhookProcessingLog
        {
            PayloadSize = payload.Length,
            HasSignature = !string.IsNullOrEmpty(signature)
        });

        // Validate signature
        var isValid = _webhookValidator.ValidateSignature(payload, signature);
        isValid.Should().BeTrue("Webhook signature should be valid");

        // Parse event
        var webhookEvent = _webhookValidator.ParseEvent(payload, signature);
        webhookEvent.Should().NotBeNull();
        webhookEvent!.Event.Should().Be("customer.created");

        // Handle event
        await _webhookHandler.HandleEventAsync(webhookEvent, CancellationToken.None);

        LogApiResponse("Customer Webhook Processing", new WebhookProcessingResult
        {
            EventId = webhookEvent.Id,
            Processed = true
        });
        LogSuccess("Customer Created Webhook processed successfully");
    }

    private async Task TestWalletTransactionWebhook()
    {
        LogStep("Testing Wallet Transaction Webhook");

        var payload = CreateWalletTransactionWebhookPayload();
        var signature = ComputeSignature(payload);

        LogApiCall("Wallet Transaction Webhook", new WebhookProcessingLog
        {
            PayloadSize = payload.Length
        });

        var webhookEvent = _webhookValidator.ParseEvent(payload, signature);
        webhookEvent.Should().NotBeNull();
        webhookEvent!.Event.Should().Be("wallet.transaction.completed");

        await _webhookHandler.HandleEventAsync(webhookEvent, CancellationToken.None);

        LogSuccess("Wallet Transaction Webhook processed successfully");
    }

    private async Task TestCardStatusChangedWebhook()
    {
        LogStep("Testing Card Status Changed Webhook");

        var payload = CreateCardStatusChangedWebhookPayload();
        var signature = ComputeSignature(payload);

        var webhookEvent = _webhookValidator.ParseEvent(payload, signature);
        webhookEvent.Should().NotBeNull();
        webhookEvent!.Event.Should().Be("card.status.changed");

        await _webhookHandler.HandleEventAsync(webhookEvent, CancellationToken.None);

        LogSuccess("Card Status Changed Webhook processed successfully");
    }

    private async Task TestKycUpdateWebhook()
    {
        LogStep("Testing KYC Update Webhook");

        var payload = CreateKycUpdateWebhookPayload();
        var signature = ComputeSignature(payload);

        var webhookEvent = _webhookValidator.ParseEvent(payload, signature);
        webhookEvent.Should().NotBeNull();
        webhookEvent!.Event.Should().Be("customer.kyc.updated");

        await _webhookHandler.HandleEventAsync(webhookEvent, CancellationToken.None);

        LogSuccess("KYC Update Webhook processed successfully");
    }

    private async Task TestUnknownEventWebhook()
    {
        LogStep("Testing Unknown Event Webhook");

        var payload = CreateUnknownEventWebhookPayload();
        var signature = ComputeSignature(payload);

        var webhookEvent = _webhookValidator.ParseEvent(payload, signature);
        webhookEvent.Should().NotBeNull();
        webhookEvent!.Event.Should().Be("unknown.event.type");

        // Should handle unknown events gracefully
        await _webhookHandler.HandleEventAsync(webhookEvent, CancellationToken.None);

        LogSuccess("Unknown Event Webhook handled gracefully");
    }

    #endregion

    #region Security Test Methods

    private async Task TestInvalidSignatureAttack()
    {
        LogStep("Testing Invalid Signature Attack");

        var payload = CreateTestWebhookPayload("attack_test");
        const string invalidSignature = "invalid_signature_attempt";

        // Should throw exception for invalid signature
        var exception = Assert.Throws<InvalidOperationException>(
            () => _webhookValidator.ParseEvent(payload, invalidSignature));

        exception.Should().NotBeNull();
        exception!.Message.Should().Contain("Invalid webhook signature");

        LogSuccess("Invalid signature attack correctly blocked");
    }

    private async Task TestPayloadTamperingAttack()
    {
        LogStep("Testing Payload Tampering Attack");

        var originalPayload = CreateTestWebhookPayload("tamper_test");
        var validSignature = ComputeSignature(originalPayload);

        // Tamper with payload after signature generation
        var tamperedPayload = originalPayload.Replace("tamper_test", "tampered_data");

        // Should detect tampering
        var isValid = _webhookValidator.ValidateSignature(tamperedPayload, validSignature);
        isValid.Should().BeFalse("Tampered payload should be rejected");

        LogSuccess("Payload tampering attack correctly detected");
    }

    private async Task TestReplayAttackScenario()
    {
        LogStep("Testing Replay Attack Scenario");

        var payload = CreateTestWebhookPayload("replay_test");
        var signature = ComputeSignature(payload);

        // Process same webhook multiple times (simulating replay attack)
        for (int i = 0; i < 3; i++)
        {
            var webhookEvent = _webhookValidator.ParseEvent(payload, signature);
            webhookEvent.Should().NotBeNull();

            await _webhookHandler.HandleEventAsync(webhookEvent!, CancellationToken.None);
        }

        // Note: In production, replay protection should be implemented at application level
        // using timestamp validation, nonce tracking, or idempotency keys
        LogSuccess("Replay attack scenario tested (protection should be at application level)");
    }

    private async Task TestLargePayloadHandling()
    {
        LogStep("Testing Large Payload Handling");

        var largePayload = CreateLargeWebhookPayload();
        var signature = ComputeSignature(largePayload);

        largePayload.Length.Should().BeGreaterThan(10000, "Payload should be significantly large");

        var webhookEvent = _webhookValidator.ParseEvent(largePayload, signature);
        webhookEvent.Should().NotBeNull();

        await _webhookHandler.HandleEventAsync(webhookEvent!, CancellationToken.None);

        LogSuccess($"Large payload ({largePayload.Length} bytes) handled successfully");
    }

    private async Task TestMalformedJsonHandling()
    {
        LogStep("Testing Malformed JSON Handling");

        var malformedPayloads = new[]
        {
            "{\"invalid_json\":}",
            "{\"unclosed_object\":",
            "[{\"array_but_expecting_object\": true}]",
            "not_json_at_all",
            ""
        };

        foreach (var malformedPayload in malformedPayloads)
        {
            var signature = ComputeSignature(malformedPayload);

            var exception = Assert.Throws<InvalidOperationException>(
                () => _webhookValidator.ParseEvent(malformedPayload, signature));

            exception.Should().NotBeNull();
            exception!.Message.Should().Contain("Failed to parse webhook event");

            TestContext.WriteLine($"✓ Malformed JSON correctly rejected: {malformedPayload.Substring(0, Math.Min(20, malformedPayload.Length))}...");
        }

        LogSuccess("Malformed JSON handling validated");
    }

    #endregion

    #region Error Handling Test Methods

    private async Task TestHandlerExceptionScenarios()
    {
        LogStep("Testing Handler Exception Scenarios");

        // Configure handler to throw exception
        _webhookHandler.ShouldThrowException = true;

        var payload = CreateTestWebhookPayload("exception_test");
        var signature = ComputeSignature(payload);

        var webhookEvent = _webhookValidator.ParseEvent(payload, signature);

        // Should throw exception from handler
        var exception = Assert.ThrowsAsync<InvalidOperationException>(
            () => _webhookHandler.HandleEventAsync(webhookEvent!, CancellationToken.None));

        exception.Should().NotBeNull();

        _webhookHandler.ShouldThrowException = false; // Reset for other tests

        LogSuccess("Handler exception scenarios tested");
    }

    private async Task TestNetworkTimeoutSimulations()
    {
        LogStep("Testing Network Timeout Simulations");

        // Configure handler to simulate delay
        _webhookHandler.SimulateDelay = TimeSpan.FromSeconds(1);

        var payload = CreateTestWebhookPayload("timeout_test");
        var signature = ComputeSignature(payload);

        var webhookEvent = _webhookValidator.ParseEvent(payload, signature);

        var startTime = DateTimeOffset.UtcNow;

        // Should complete but take time
        await _webhookHandler.HandleEventAsync(webhookEvent!, CancellationToken.None);

        var elapsed = DateTimeOffset.UtcNow - startTime;
        elapsed.Should().BeGreaterThanOrEqualTo(TimeSpan.FromSeconds(1));

        _webhookHandler.SimulateDelay = TimeSpan.Zero; // Reset

        LogSuccess($"Network delay simulation completed ({elapsed.TotalMilliseconds:F0}ms)");
    }

    private async Task TestConcurrentWebhookProcessing()
    {
        LogStep("Testing Concurrent Webhook Processing");

        const int concurrentCount = 10;
        var tasks = new Task[concurrentCount];

        for (int i = 0; i < concurrentCount; i++)
        {
            var index = i;
            tasks[i] = Task.Run(async () =>
            {
                var payload = CreateTestWebhookPayload($"concurrent_{index}");
                var signature = ComputeSignature(payload);

                var webhookEvent = _webhookValidator.ParseEvent(payload, signature);
                await _webhookHandler.HandleEventAsync(webhookEvent!, CancellationToken.None);
            });
        }

        await Task.WhenAll(tasks);

        LogSuccess($"Processed {concurrentCount} webhooks concurrently");
    }

    private async Task TestMemoryPressureScenarios()
    {
        LogStep("Testing Memory Pressure Scenarios");

        // Process many webhooks to test memory usage
        for (int i = 0; i < 100; i++)
        {
            var payload = CreateTestWebhookPayload($"memory_test_{i}");
            var signature = ComputeSignature(payload);

            var webhookEvent = _webhookValidator.ParseEvent(payload, signature);
            await _webhookHandler.HandleEventAsync(webhookEvent!, CancellationToken.None);

            // Occasional garbage collection to test memory cleanup
            if (i % 25 == 0)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        LogSuccess("Memory pressure scenarios completed");
    }

    #endregion

    #region Helper Methods

    private string CreateTestWebhookPayload(string eventId)
    {
        return System.Text.Json.JsonSerializer.Serialize(new
        {
            id = eventId,
            @event = "test.event",
            created_at = DateTimeOffset.UtcNow,
            data = new
            {
                test_data = "integration_test_data",
                timestamp = DateTimeOffset.UtcNow
            }
        });
    }

    private string CreateCustomerCreatedWebhookPayload()
    {
        return System.Text.Json.JsonSerializer.Serialize(new
        {
            id = $"evt_customer_created_{CreateTestId()}",
            @event = "customer.created",
            created_at = DateTimeOffset.UtcNow,
            data = new
            {
                customer_id = $"cust_{CreateTestId()}",
                first_name = "Integration",
                last_name = "Test",
                email = CreateTestEmail("webhook"),
                phone_number = CreateTestPhoneNumber(),
                created_at = DateTimeOffset.UtcNow
            }
        });
    }

    private string CreateWalletTransactionWebhookPayload()
    {
        return System.Text.Json.JsonSerializer.Serialize(new
        {
            id = $"evt_wallet_txn_{CreateTestId()}",
            @event = "wallet.transaction.completed",
            created_at = DateTimeOffset.UtcNow,
            data = new
            {
                transaction_id = $"txn_{CreateTestId()}",
                wallet_id = $"wlt_{CreateTestId()}",
                amount = 100000,
                currency = "NGN",
                type = "credit",
                status = "completed",
                reference = $"ref_{CreateTestId()}"
            }
        });
    }

    private string CreateCardStatusChangedWebhookPayload()
    {
        return System.Text.Json.JsonSerializer.Serialize(new
        {
            id = $"evt_card_status_{CreateTestId()}",
            @event = "card.status.changed",
            created_at = DateTimeOffset.UtcNow,
            data = new
            {
                card_id = $"card_{CreateTestId()}",
                customer_id = $"cust_{CreateTestId()}",
                old_status = "issued",
                new_status = "active",
                changed_at = DateTimeOffset.UtcNow
            }
        });
    }

    private string CreateKycUpdateWebhookPayload()
    {
        return System.Text.Json.JsonSerializer.Serialize(new
        {
            id = $"evt_kyc_update_{CreateTestId()}",
            @event = "customer.kyc.updated",
            created_at = DateTimeOffset.UtcNow,
            data = new
            {
                customer_id = $"cust_{CreateTestId()}",
                old_level = "TIER_1",
                new_level = "TIER_2",
                verification_method = "NIN",
                updated_at = DateTimeOffset.UtcNow
            }
        });
    }

    private string CreateUnknownEventWebhookPayload()
    {
        return System.Text.Json.JsonSerializer.Serialize(new
        {
            id = $"evt_unknown_{CreateTestId()}",
            @event = "unknown.event.type",
            created_at = DateTimeOffset.UtcNow,
            data = new
            {
                message = "This is an unknown event type for testing"
            }
        });
    }

    private string CreateLargeWebhookPayload()
    {
        var largeData = string.Join("", Enumerable.Range(0, 1000).Select(i => $"data_item_{i}_"));

        return System.Text.Json.JsonSerializer.Serialize(new
        {
            id = $"evt_large_{CreateTestId()}",
            @event = "large.data.event",
            created_at = DateTimeOffset.UtcNow,
            data = new
            {
                large_field = largeData,
                metadata = new
                {
                    size_test = true,
                    generated_at = DateTimeOffset.UtcNow
                }
            }
        });
    }

    private string ComputeSignature(string payload)
    {
        var keyBytes = System.Text.Encoding.UTF8.GetBytes(TestWebhookSecret);
        var payloadBytes = System.Text.Encoding.UTF8.GetBytes(payload);

        using var hmac = new System.Security.Cryptography.HMACSHA256(keyBytes);
        var hash = hmac.ComputeHash(payloadBytes);
        return Convert.ToHexString(hash).ToLowerInvariant();
    }

    #endregion
}

/// <summary>
/// Test webhook handler for integration testing.
/// </summary>
internal class TestWebhookHandler : WebhookHandler
{
    public bool ShouldThrowException { get; set; }
    public TimeSpan SimulateDelay { get; set; } = TimeSpan.Zero;

    public TestWebhookHandler(ILogger<WebhookHandler>? logger = null) : base(logger)
    {
    }

    protected override void RegisterHandlers()
    {
        RegisterHandler("customer.created", HandleCustomerCreated);
        RegisterHandler("wallet.transaction.completed", HandleWalletTransaction);
        RegisterHandler("card.status.changed", HandleCardStatusChanged);
        RegisterHandler("customer.kyc.updated", HandleKycUpdate);
        RegisterHandler("test.event", HandleTestEvent);
    }

    private async Task HandleCustomerCreated(WebhookEvent webhookEvent, CancellationToken cancellationToken)
    {
        if (SimulateDelay > TimeSpan.Zero)
            await Task.Delay(SimulateDelay, cancellationToken);

        if (ShouldThrowException)
            throw new InvalidOperationException("Test exception in webhook handler");

        // Process customer created event
        TestContext.WriteLine($"Processing customer.created event: {webhookEvent.Id}");
    }

    private async Task HandleWalletTransaction(WebhookEvent webhookEvent, CancellationToken cancellationToken)
    {
        if (SimulateDelay > TimeSpan.Zero)
            await Task.Delay(SimulateDelay, cancellationToken);

        if (ShouldThrowException)
            throw new InvalidOperationException("Test exception in webhook handler");

        TestContext.WriteLine($"Processing wallet.transaction.completed event: {webhookEvent.Id}");
    }

    private async Task HandleCardStatusChanged(WebhookEvent webhookEvent, CancellationToken cancellationToken)
    {
        if (SimulateDelay > TimeSpan.Zero)
            await Task.Delay(SimulateDelay, cancellationToken);

        if (ShouldThrowException)
            throw new InvalidOperationException("Test exception in webhook handler");

        TestContext.WriteLine($"Processing card.status.changed event: {webhookEvent.Id}");
    }

    private async Task HandleKycUpdate(WebhookEvent webhookEvent, CancellationToken cancellationToken)
    {
        if (SimulateDelay > TimeSpan.Zero)
            await Task.Delay(SimulateDelay, cancellationToken);

        if (ShouldThrowException)
            throw new InvalidOperationException("Test exception in webhook handler");

        TestContext.WriteLine($"Processing customer.kyc.updated event: {webhookEvent.Id}");
    }

    private async Task HandleTestEvent(WebhookEvent webhookEvent, CancellationToken cancellationToken)
    {
        if (SimulateDelay > TimeSpan.Zero)
            await Task.Delay(SimulateDelay, cancellationToken);

        if (ShouldThrowException)
            throw new InvalidOperationException("Test exception in webhook handler");

        TestContext.WriteLine($"Processing test.event: {webhookEvent.Id}");
    }
}

#region Typed Logging Models

/// <summary>
/// Strongly typed model for logging webhook processing information.
/// </summary>
internal sealed record WebhookProcessingLog
{
    public int PayloadSize { get; init; }
    public bool HasSignature { get; init; }
}

/// <summary>
/// Strongly typed model for logging webhook processing results.
/// </summary>
internal sealed record WebhookProcessingResult
{
    public string EventId { get; init; } = string.Empty;
    public bool Processed { get; init; }
}

#endregion
