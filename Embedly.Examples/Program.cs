using Embedly.Examples.Infrastructure.Configuration;
using Embedly.Examples.Infrastructure.Extensions;
using Embedly.Examples.Infrastructure.Services;
using Embedly.Examples.Examples;
using Microsoft.Extensions.Options;
using Serilog;
using System.Reflection;
using FastEndpoints;
using FastEndpoints.Swagger;

namespace Embedly.Examples;

/// <summary>
/// Entry point for the Embedly SDK Examples application.
/// Demonstrates professional patterns for financial services integration.
/// </summary>
internal class Program
{
    private static async Task<int> Main(string[] args)
    {
        // Configure Serilog early for startup logging
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateBootstrapLogger();

        try
        {
            Log.Information("Starting Embedly SDK Examples Application");

            var host = CreateHost(args);

            // Validate configuration before running
            await ValidateConfigurationAsync(host.Services);

            var runMode = DetermineRunMode(args);

            return runMode switch
            {
                RunMode.WebApi => await RunWebApiAsync(host),
                RunMode.Console => await RunConsoleAsync(host),
                RunMode.Scenario => await RunScenarioAsync(host, args),
                _ => await RunInteractiveAsync(host)
            };
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Application terminated unexpectedly");
            return 1;
        }
        finally
        {
            await Log.CloseAndFlushAsync();
        }
    }

    private static IHost CreateHost(string[] args)
    {
        // Check if we need WebApplication for API mode
        if (args.Contains("--web") || args.Contains("--api"))
        {
            return CreateWebApplication(args);
        }

        var builder = Host.CreateDefaultBuilder(args)
            .UseSerilog((context, configuration) =>
            {
                configuration.ReadFrom.Configuration(context.Configuration);
            })
            .ConfigureAppConfiguration((context, config) =>
            {
                // Load configuration in order of precedence
                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                config.AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json",
                    optional: true, reloadOnChange: true);
                config.AddUserSecrets<Program>(optional: true);
                config.AddEnvironmentVariables("EMBEDLY_");
                config.AddCommandLine(args);
            })
            .ConfigureServices((context, services) =>
            {
                // Add application services
                services.AddApplicationServices(context.Configuration);
                services.AddExampleServices();

                // Add health checks
                services.AddHealthChecks()
                    .AddCheck<EmbedlyHealthCheck>("embedly");
            });

        return builder.Build();
    }

    private static WebApplication CreateWebApplication(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Configure Serilog
        builder.Host.UseSerilog((context, configuration) =>
        {
            configuration.ReadFrom.Configuration(context.Configuration);
        });

        // Add services
        builder.Services.AddApplicationServices(builder.Configuration);
        builder.Services.AddExampleServices();
        builder.Services.AddWebhookServices(builder.Configuration);

        // Check if FastEndpoints is enabled
        var useFastEndpoints = builder.Configuration.GetValue<bool>("UseFastEndpoints") ||
                              args.Contains("--fastendpoints") ||
                              args.Contains("--fe");

        if (useFastEndpoints)
        {
            // Add FastEndpoints
            builder.Services.AddFastEndpoints();
            builder.Services.SwaggerDocument(o =>
            {
                o.DocumentSettings = s =>
                {
                    s.Title = "Embedly SDK Examples API (FastEndpoints)";
                    s.Version = "v1";
                    s.Description = "Production-ready examples using FastEndpoints for Embedly financial services integration";
                };
            });
        }
        else
        {
            // Add traditional controllers
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new()
                {
                    Title = "Embedly SDK Examples API (Controllers)",
                    Version = "v1",
                    Description = "Production-ready examples using Controllers for Embedly financial services integration"
                });

                // Include XML comments if available
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                {
                    c.IncludeXmlComments(xmlPath);
                }
            });
        }

        // Add health checks
        builder.Services.AddHealthChecks()
            .AddCheck<EmbedlyHealthCheck>("embedly");

        return builder.Build();
    }

    private static async Task ValidateConfigurationAsync(IServiceProvider services)
    {
        var embedlySettings = services.GetRequiredService<IOptions<EmbedlySettings>>();
        var logger = services.GetRequiredService<ILogger<Program>>();

        var validationErrors = embedlySettings.Value.Validate().ToList();
        if (validationErrors.Count > 0)
        {
            foreach (var error in validationErrors)
            {
                logger.LogError("Configuration Error: {Error}", error);
            }

            logger.LogError("Please check your configuration. Use 'dotnet user-secrets' for secure credential storage");
            logger.LogInformation("Example: dotnet user-secrets set \"Embedly:ApiKey\" \"BSK-your-api-key\"");

            throw new InvalidOperationException("Invalid configuration. See logs for details.");
        }

        logger.LogInformation("Configuration validated successfully");
        logger.LogInformation("Environment: {Environment}", embedlySettings.Value.Environment);
        logger.LogInformation("Organization: {Organization}", MaskValue(embedlySettings.Value.OrganizationId));
    }

    private static RunMode DetermineRunMode(string[] args)
    {
        if (args.Contains("--web") || args.Contains("--api"))
            return RunMode.WebApi;

        if (args.Contains("--scenario"))
            return RunMode.Scenario;

        if (args.Contains("--console"))
            return RunMode.Console;

        return RunMode.Interactive;
    }

    private static async Task<int> RunWebApiAsync(IHost host)
    {
        var logger = host.Services.GetRequiredService<ILogger<Program>>();
        var configuration = host.Services.GetRequiredService<IConfiguration>();

        // Determine if using FastEndpoints
        var useFastEndpoints = configuration.GetValue<bool>("UseFastEndpoints") ||
                              Environment.GetCommandLineArgs().Contains("--fastendpoints") ||
                              Environment.GetCommandLineArgs().Contains("--fe");

        var apiType = useFastEndpoints ? "FastEndpoints" : "Controllers";
        logger.LogInformation("Starting in Web API mode using {ApiType}", apiType);

        // Configure the HTTP request pipeline
        var app = (WebApplication)host;

        if (app.Environment.IsDevelopment())
        {
            if (useFastEndpoints)
            {
                app.UseSwaggerGen();
            }
            else
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Embedly SDK Examples API v1");
                    c.RoutePrefix = string.Empty; // Serve Swagger UI at root
                });
            }
        }

        app.UseHttpsRedirection();

        if (useFastEndpoints)
        {
            // Configure FastEndpoints pipeline
            app.UseFastEndpoints();
        }
        else
        {
            // Configure Controllers pipeline
            app.UseRouting();
            app.MapControllers();
        }

        app.MapHealthChecks("/health");

        // Welcome endpoint
        app.MapGet("/api/info", (IOptions<ApplicationSettings> settings) => new
        {
            Application = settings.Value.Name,
            Version = settings.Value.Version,
            Environment = settings.Value.Environment,
            ApiType = apiType,
            Timestamp = DateTime.UtcNow,
            Endpoints = new
            {
                Health = "/health",
                Swagger = useFastEndpoints ? "/swagger" : "/",
                Webhooks = "/api/webhooks/embedly",
                TestSignature = "/api/webhooks/test-signature"
            }
        });

        logger.LogInformation("Web API started using {ApiType}. Navigate to the root URL to see Swagger documentation", apiType);

        await app.RunAsync();
        return 0;
    }

    private static async Task<int> RunConsoleAsync(IHost host)
    {
        var logger = host.Services.GetRequiredService<ILogger<Program>>();
        logger.LogInformation("Starting in Console mode");

        // Console-specific logic would go here
        // This would run predefined scenarios without user interaction

        await Task.Delay(1000); // Placeholder
        logger.LogInformation("Console mode execution completed");

        return 0;
    }

    private static async Task<int> RunScenarioAsync(IHost host, string[] args)
    {
        var logger = host.Services.GetRequiredService<ILogger<Program>>();

        var scenarioName = args.SkipWhile(arg => arg != "--scenario")
            .Skip(1)
            .FirstOrDefault();

        if (string.IsNullOrEmpty(scenarioName))
        {
            logger.LogError("Scenario name is required when using --scenario");
            logger.LogInformation("Available scenarios: onboarding, payment, payout");
            return 1;
        }

        logger.LogInformation("Running scenario: {Scenario}", scenarioName);

        // Scenario execution logic would go here
        await Task.Delay(1000); // Placeholder

        logger.LogInformation("Scenario {Scenario} completed successfully", scenarioName);
        return 0;
    }

    private static async Task<int> RunInteractiveAsync(IHost host)
    {
        var logger = host.Services.GetRequiredService<ILogger<Program>>();
        var correlationService = host.Services.GetRequiredService<ICorrelationService>();

        logger.LogInformation("Starting in Interactive mode");
        correlationService.GenerateNew();

        Console.WriteLine();
        Console.WriteLine("🚀 Embedly .NET SDK Examples - Professional Edition");
        Console.WriteLine("====================================================");
        Console.WriteLine();
        Console.WriteLine("This application demonstrates enterprise-grade integration");
        Console.WriteLine("patterns with the Embedly financial services platform.");
        Console.WriteLine();

        while (true)
        {
            DisplayMainMenu();

            var choice = Console.ReadLine()?.Trim().ToLowerInvariant();

            try
            {
                var result = choice switch
                {
                    "1" => RunHealthCheckAsync(host),
                    "2" => RunCustomerScenarioAsync(host),
                    "3" => RunWalletScenarioAsync(host),
                    "4" => RunCardScenarioAsync(host),
                    "5" => RunPayoutScenarioAsync(host),
                    "6" => RunWebhookTestAsync(host),
                    "7" => RunFullWorkflowAsync(host),
                    "health" => RunHealthCheckAsync(host),
                    "config" => ShowConfigurationAsync(host),
                    "help" => Task.FromResult(ShowHelp()),
                    "exit" or "quit" or "q" => Task.FromResult(false),
                    _ => Task.FromResult(ShowInvalidOption())
                };

                if (!await result)
                {
                    break;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error executing menu option: {Choice}", choice);
                Console.WriteLine($"❌ Error: {ex.Message}");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }

            Console.WriteLine();
        }

        logger.LogInformation("Interactive session ended");
        return 0;
    }

    private static void DisplayMainMenu()
    {
        Console.WriteLine("📋 Main Menu");
        Console.WriteLine("=============");
        Console.WriteLine("1️⃣  System Health Check");
        Console.WriteLine("2️⃣  Customer Management Scenario");
        Console.WriteLine("3️⃣  Wallet Operations Scenario");
        Console.WriteLine("4️⃣  Card Management Scenario");
        Console.WriteLine("5️⃣  Payout Services Scenario");
        Console.WriteLine("6️⃣  Webhook Integration Test");
        Console.WriteLine("7️⃣  Complete End-to-End Workflow");
        Console.WriteLine();
        Console.WriteLine("🔧 config  - Show configuration");
        Console.WriteLine("🏥 health  - Run health check");
        Console.WriteLine("❓ help    - Show help");
        Console.WriteLine("❌ exit    - Exit application");
        Console.WriteLine();
        Console.Write("Select an option: ");
    }

    private static async Task<bool> RunHealthCheckAsync(IHost host)
    {
        var healthService = host.Services.GetRequiredService<IHealthService>();

        Console.WriteLine("\n🏥 Running Health Check...");
        Console.WriteLine("==========================");

        var result = await healthService.CheckHealthAsync();

        Console.WriteLine($"Status: {GetStatusEmoji(result.Status)} {result.Status}");
        Console.WriteLine($"Description: {result.Description}");

        if (result.Data?.Any() == true)
        {
            Console.WriteLine("\nDetails:");
            foreach (var kvp in result.Data)
            {
                Console.WriteLine($"  {kvp.Key}: {kvp.Value}");
            }
        }

        if (result.Exception != null)
        {
            Console.WriteLine($"\nError: {result.Exception.Message}");
        }

        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
        return true;
    }

    private static async Task<bool> RunCustomerScenarioAsync(IHost host)
    {
        Console.WriteLine("\n👥 Customer Management Scenario");
        Console.WriteLine("===============================");

        var customerExamples = host.Services.GetRequiredService<CustomerExamples>();
        var result = await customerExamples.DemonstrateCustomerWorkflowAsync();

        if (!result.IsSuccess)
        {
            Console.WriteLine($"❌ Scenario failed: {result.Error}");
        }

        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
        return true;
    }

    private static async Task<bool> RunWalletScenarioAsync(IHost host)
    {
        Console.WriteLine("\n💰 Wallet Operations Scenario");
        Console.WriteLine("=============================");

        var walletExamples = host.Services.GetRequiredService<WalletExamples>();
        var result = await walletExamples.DemonstrateWalletWorkflowAsync();

        if (!result.IsSuccess)
        {
            Console.WriteLine($"❌ Scenario failed: {result.Error}");
        }

        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
        return true;
    }

    private static async Task<bool> RunCardScenarioAsync(IHost host)
    {
        Console.WriteLine("\n💳 Card Management Scenario");
        Console.WriteLine("===========================");
        Console.WriteLine("Card management examples require customer and wallet setup first.");
        Console.WriteLine("This feature will be implemented in the next version.");

        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
        return true;
    }

    private static async Task<bool> RunPayoutScenarioAsync(IHost host)
    {
        Console.WriteLine("\n🏦 Payout Services Scenario");
        Console.WriteLine("===========================");
        Console.WriteLine("Payout examples require bank integration setup.");
        Console.WriteLine("This feature will be implemented in the next version.");

        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
        return true;
    }

    private static async Task<bool> RunWebhookTestAsync(IHost host)
    {
        Console.WriteLine("\n🎣 Webhook Integration Test");
        Console.WriteLine("===========================");
        Console.WriteLine("Webhook endpoints are available in Web API mode.");
        Console.WriteLine("Run with --web flag to start the webhook API server.");
        Console.WriteLine("\nEndpoints:");
        Console.WriteLine("  POST /api/webhooks/embedly - Main webhook endpoint");
        Console.WriteLine("  POST /api/webhooks/test-signature - Test signature validation");
        Console.WriteLine("  GET  /api/webhooks/health - Health check");

        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
        return true;
    }

    private static async Task<bool> RunFullWorkflowAsync(IHost host)
    {
        Console.WriteLine("\n🔄 Complete End-to-End Workflow");
        Console.WriteLine("===============================");
        Console.WriteLine("Running customer and wallet workflows sequentially...");

        // Run customer workflow
        var customerExamples = host.Services.GetRequiredService<CustomerExamples>();
        var customerResult = await customerExamples.DemonstrateCustomerWorkflowAsync();

        if (customerResult.IsSuccess)
        {
            Console.WriteLine("\n" + "=".PadRight(50, '='));

            // Run wallet workflow
            var walletExamples = host.Services.GetRequiredService<WalletExamples>();
            var walletResult = await walletExamples.DemonstrateWalletWorkflowAsync();

            if (walletResult.IsSuccess)
            {
                Console.WriteLine("\n✅ Complete end-to-end workflow finished successfully!");
            }
            else
            {
                Console.WriteLine($"\n❌ Wallet workflow failed: {walletResult.Error}");
            }
        }
        else
        {
            Console.WriteLine($"\n❌ Customer workflow failed: {customerResult.Error}");
        }

        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
        return true;
    }

    private static async Task<bool> ShowConfigurationAsync(IHost host)
    {
        var embedlySettings = host.Services.GetRequiredService<IOptions<EmbedlySettings>>();
        var appSettings = host.Services.GetRequiredService<IOptions<ApplicationSettings>>();

        Console.WriteLine("\n🔧 Configuration");
        Console.WriteLine("=================");
        Console.WriteLine($"Application: {appSettings.Value.Name} v{appSettings.Value.Version}");
        Console.WriteLine($"Environment: {appSettings.Value.Environment}");
        Console.WriteLine($"Embedly Environment: {embedlySettings.Value.Environment}");
        Console.WriteLine($"API Key: {MaskValue(embedlySettings.Value.ApiKey)}");
        Console.WriteLine($"Organization ID: {MaskValue(embedlySettings.Value.OrganizationId)}");
        Console.WriteLine($"Timeout: {embedlySettings.Value.Timeout}");
        Console.WriteLine($"Logging Enabled: {embedlySettings.Value.EnableLogging}");

        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
        return true;
    }

    private static bool ShowHelp()
    {
        Console.WriteLine("\n❓ Help");
        Console.WriteLine("========");
        Console.WriteLine("This application provides comprehensive examples for integrating");
        Console.WriteLine("with the Embedly financial services platform.");
        Console.WriteLine();
        Console.WriteLine("Command Line Options:");
        Console.WriteLine("  --web            Start in Web API mode (Controllers)");
        Console.WriteLine("  --web --fe       Start in Web API mode (FastEndpoints)");
        Console.WriteLine("  --fastendpoints  Start in Web API mode using FastEndpoints");
        Console.WriteLine("  --console        Start in Console mode");
        Console.WriteLine("  --scenario       Run a specific scenario");
        Console.WriteLine();
        Console.WriteLine("Configuration:");
        Console.WriteLine("  Use user secrets or environment variables for credentials");
        Console.WriteLine("  Example: dotnet user-secrets set \"Embedly:ApiKey\" \"BSK-your-key\"");
        Console.WriteLine();
        Console.WriteLine("Resources:");
        Console.WriteLine("  • Documentation: https://developer.embedly.ng");
        Console.WriteLine("  • API Reference: https://waas-staging.embedly.ng/swagger");

        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
        return true;
    }

    private static bool ShowInvalidOption()
    {
        Console.WriteLine("❌ Invalid option. Type 'help' for more information.");
        return true;
    }

    private static string GetStatusEmoji(Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus status) => status switch
    {
        Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Healthy => "✅",
        Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Degraded => "⚠️",
        Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Unhealthy => "❌",
        _ => "❓"
    };

    private static string MaskValue(string value)
    {
        if (string.IsNullOrEmpty(value))
            return "[Not configured]";

        return value.Length <= 8
            ? $"{value[..3]}***"
            : $"{value[..8]}***";
    }

    private enum RunMode
    {
        Interactive,
        WebApi,
        Console,
        Scenario
    }
}