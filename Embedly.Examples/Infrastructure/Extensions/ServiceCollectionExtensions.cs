using Embedly.Examples.Examples;
using Embedly.Examples.Infrastructure.Configuration;
using Embedly.Examples.Infrastructure.Services;
using Embedly.SDK.Configuration;
using Embedly.SDK.Extensions;
using Microsoft.Extensions.Options;

namespace Embedly.Examples.Infrastructure.Extensions;

/// <summary>
///     Extension methods for configuring services.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    ///     Adds and configures application services.
    /// </summary>
    public static IServiceCollection AddApplicationServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        // Configuration
        services.Configure<ApplicationSettings>(configuration.GetSection(ApplicationSettings.SectionName));
        services.Configure<EmbedlySettings>(configuration.GetSection(EmbedlySettings.SectionName));
        services.Configure<LoggingSettings>(configuration.GetSection(LoggingSettings.SectionName));

        // Validate Embedly settings
        services.AddSingleton<IValidateOptions<EmbedlySettings>, EmbedlySettingsValidator>();

        // Core services
        services.AddSingleton<ICorrelationService, CorrelationService>();
        services.AddScoped<IRetryService, RetryService>();
        services.AddScoped<IHealthService, HealthService>();

        // HTTP clients
        services.AddHttpClient();

        // Embedly SDK
        var embedlySettings = configuration.GetSection(EmbedlySettings.SectionName).Get<EmbedlySettings>()
                              ?? throw new InvalidOperationException("Embedly settings are required");

        services.AddEmbedly(options =>
        {
            options.ApiKey = embedlySettings.ApiKey;
            options.OrganizationId = embedlySettings.OrganizationId;
            options.Environment = Enum.Parse<EmbedlyEnvironment>(embedlySettings.Environment);
            options.Timeout = embedlySettings.Timeout;
            options.EnableLogging = embedlySettings.EnableLogging;
            options.LogRequestBodies = embedlySettings.LogRequestBodies;
        });

        return services;
    }

    /// <summary>
    ///     Adds example services for demonstration.
    /// </summary>
    public static IServiceCollection AddExampleServices(this IServiceCollection services)
    {
        // Example services
        services.AddScoped<CustomerExamples>();
        services.AddScoped<WalletExamples>();

        return services;
    }

    /// <summary>
    ///     Adds webhook services using the SDK's built-in webhook system.
    /// </summary>
    public static IServiceCollection AddWebhookServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Get webhook secret from configuration
        var webhookSecret = configuration.GetValue<string>("Embedly:WebhookSecret");
        if (string.IsNullOrEmpty(webhookSecret)) webhookSecret = "default-webhook-secret"; // For demo purposes only

        // Add the SDK's built-in webhook services
        services.AddEmbedlyWebhooks(webhookSecret);

        // Add our custom webhook handler
        services.AddWebhookHandler<EmbedlyWebhookHandler>();

        return services;
    }
}

/// <summary>
///     Validates Embedly configuration settings.
/// </summary>
public class EmbedlySettingsValidator : IValidateOptions<EmbedlySettings>
{
    public ValidateOptionsResult Validate(string? name, EmbedlySettings options)
    {
        var errors = options.Validate().ToList();

        if (errors.Count > 0) return ValidateOptionsResult.Fail(errors);

        return ValidateOptionsResult.Success;
    }
}