using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using Embedly.SDK.Configuration;
using Embedly.SDK.Extensions;

namespace Embedly.SDK.Tests.Integration;

/// <summary>
/// Base class for integration tests with real API calls.
/// Uses user secrets for secure credential management and follows SDK patterns.
/// </summary>
[Category("Integration")]
public abstract class IntegrationTestBase
{
    protected IServiceProvider ServiceProvider { get; private set; } = null!;
    protected IEmbedlyClient EmbedlyClient { get; private set; } = null!;
    protected IConfiguration Configuration { get; private set; } = null!;
    protected string ApiKey { get; private set; } = null!;
    protected string OrganizationId { get; private set; } = null!;

    [OneTimeSetUp]
    public virtual void OneTimeSetUp()
    {
        // Build configuration with user secrets and environment variables
        var configurationBuilder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.test.json", optional: true)
            .AddUserSecrets<IntegrationTestBase>()
            .AddEnvironmentVariables("EMBEDLY_");

        Configuration = configurationBuilder.Build();

        // Get credentials from configuration
        ApiKey = Configuration["Embedly:ApiKey"]
                ?? Configuration["ApiKey"]
                ?? throw new InvalidOperationException(
                    "Embedly API key not found. Please set via user secrets: " +
                    "dotnet user-secrets set \"Embedly:ApiKey\" \"YOUR_API_KEY\" " +
                    "or environment variable EMBEDLY_ApiKey");

        OrganizationId = Configuration["Embedly:OrganizationId"]
                        ?? Configuration["OrganizationId"]
                        ?? throw new InvalidOperationException(
                            "Embedly Organization ID not found. Please set via user secrets: " +
                            "dotnet user-secrets set \"Embedly:OrganizationId\" \"YOUR_ORG_ID\" " +
                            "or environment variable EMBEDLY_OrganizationId");

        // Setup service collection
        var services = new ServiceCollection();

        // Add logging with detailed output for integration tests
        services.AddLogging(builder =>
        {
            builder.AddConsole();
            builder.AddDebug();
            builder.SetMinimumLevel(LogLevel.Information);
        });

        // Add Embedly SDK with staging environment for integration tests
        services.AddEmbedly(options =>
        {
            options.ApiKey = ApiKey;
            options.OrganizationId = OrganizationId;
            options.Environment = EmbedlyEnvironment.Staging;
            options.EnableLogging = true;
            options.LogRequestBodies = true; // Enable for debugging
            // options.LogResponseBodies = true; // Property doesn't exist - removed for now
            options.Timeout = TimeSpan.FromMinutes(5); // Longer timeout for integration tests
        });

        ServiceProvider = services.BuildServiceProvider();
        EmbedlyClient = ServiceProvider.GetRequiredService<IEmbedlyClient>();

        TestContext.WriteLine("=== Integration Test Configuration ===");
        TestContext.WriteLine($"Organization ID: {OrganizationId}");
        TestContext.WriteLine($"API Key: {MaskApiKey(ApiKey)}");
        TestContext.WriteLine($"Environment: Staging");
        TestContext.WriteLine($"Timeout: 5 minutes");
        TestContext.WriteLine("=======================================");
    }

    [OneTimeTearDown]
    public virtual void OneTimeTearDown()
    {
        (ServiceProvider as IDisposable)?.Dispose();
    }

    /// <summary>
    /// Logs an API operation start with request details.
    /// </summary>
    /// <param name="operation">The operation name.</param>
    /// <param name="request">The request object (optional).</param>
    protected void LogApiCall(string operation, object? request = null)
    {
        TestContext.WriteLine($"\n=== {operation} ===");
        TestContext.WriteLine($"Timestamp: {DateTimeOffset.UtcNow:yyyy-MM-dd HH:mm:ss} UTC");

        if (request != null)
        {
            var requestJson = System.Text.Json.JsonSerializer.Serialize(request, new System.Text.Json.JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase
            });
            TestContext.WriteLine($"Request:\n{requestJson}");
        }
    }

    /// <summary>
    /// Logs an API operation result with response details.
    /// </summary>
    /// <typeparam name="T">The response type.</typeparam>
    /// <param name="operation">The operation name.</param>
    /// <param name="response">The response object.</param>
    protected void LogApiResponse<T>(string operation, T response)
    {
        var responseJson = System.Text.Json.JsonSerializer.Serialize(response, new System.Text.Json.JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase
        });

        TestContext.WriteLine($"Response:\n{responseJson}");
        TestContext.WriteLine($"=== End {operation} ===\n");
    }

    /// <summary>
    /// Logs a test step for better readability.
    /// </summary>
    /// <param name="step">The step description.</param>
    protected void LogStep(string step)
    {
        TestContext.WriteLine($"STEP: {step}");
    }

    /// <summary>
    /// Logs a successful operation.
    /// </summary>
    /// <param name="message">The success message.</param>
    protected void LogSuccess(string message)
    {
        TestContext.WriteLine($"✓ SUCCESS: {message}");
    }

    /// <summary>
    /// Logs a warning message.
    /// </summary>
    /// <param name="message">The warning message.</param>
    protected void LogWarning(string message)
    {
        TestContext.WriteLine($"⚠ WARNING: {message}");
    }

    /// <summary>
    /// Logs an error message.
    /// </summary>
    /// <param name="message">The error message.</param>
    protected void LogError(string message)
    {
        TestContext.WriteLine($"✗ ERROR: {message}");
    }

    /// <summary>
    /// Masks API key for logging (shows first 8 characters only).
    /// </summary>
    /// <param name="apiKey">The API key to mask.</param>
    /// <returns>Masked API key.</returns>
    private static string MaskApiKey(string apiKey)
    {
        if (string.IsNullOrEmpty(apiKey) || apiKey.Length <= 8)
            return "***";

        return $"{apiKey[..8]}...";
    }

    /// <summary>
    /// Creates a unique test identifier for consistent testing.
    /// </summary>
    /// <param name="prefix">Optional prefix.</param>
    /// <returns>A unique test identifier.</returns>
    protected static string CreateTestId(string prefix = "test")
    {
        var timestamp = DateTimeOffset.UtcNow.ToString("yyyyMMddHHmmss");
        var random = new Random().Next(1000, 9999);
        return $"{prefix}-{timestamp}-{random}";
    }

    /// <summary>
    /// Creates a unique test email address.
    /// </summary>
    /// <param name="prefix">Optional prefix.</param>
    /// <returns>A unique test email.</returns>
    protected static string CreateTestEmail(string prefix = "integration")
    {
        return $"{prefix}-{CreateTestId()}@embedly-sdk-test.com";
    }

    /// <summary>
    /// Creates a unique test phone number.
    /// </summary>
    /// <returns>A unique Nigerian test phone number.</returns>
    protected static string CreateTestPhoneNumber()
    {
        var random = new Random();
        var suffix = random.Next(1000, 9999).ToString();
        return $"+23480123{suffix}";
    }

    /// <summary>
    /// Creates a test GUID for consistent testing.
    /// </summary>
    /// <returns>A deterministic test GUID.</returns>
    protected static Guid CreateTestGuid()
    {
        return Guid.Parse("550e8400-e29b-41d4-a716-446655440000");
    }
}