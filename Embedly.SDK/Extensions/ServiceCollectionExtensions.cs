using System;
using System.Net;
using System.Net.Http;
using Embedly.SDK.Configuration;
using Embedly.SDK.Http;
using Embedly.SDK.Http.Handlers;
using Embedly.SDK.Notifications;
using Embedly.SDK.Services.Cards;
using Embedly.SDK.Services.Checkout;
using Embedly.SDK.Services.CorporateCustomers;
using Embedly.SDK.Services.Customers;
using Embedly.SDK.Services.Payout;
// using Embedly.SDK.Services.ProductLimits;  // Commented out - not in public API docs
// using Embedly.SDK.Services.Products;       // Commented out - not in public API docs
using Embedly.SDK.Services.Utilities;
// using Embedly.SDK.Services.WalletGroups;   // Commented out - not in public API docs
using Embedly.SDK.Services.Wallets;
using Embedly.SDK.Webhooks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;

namespace Embedly.SDK.Extensions;

/// <summary>
///     Extension methods for configuring Embedly SDK services in dependency injection.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    ///     Adds Embedly SDK services to the dependency injection container.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configureOptions">Action to configure Embedly options.</param>
    /// <returns>The configured service collection.</returns>
    public static IServiceCollection AddEmbedly(
        this IServiceCollection services,
        Action<EmbedlyOptions> configureOptions)
    {
        if (services == null) throw new ArgumentNullException(nameof(services));

        if (configureOptions == null) throw new ArgumentNullException(nameof(configureOptions));

        // Configure options
        services.Configure(configureOptions);
        services.PostConfigure<EmbedlyOptions>(options => options.Validate());

        return AddEmbedlyCore(services);
    }

    /// <summary>
    ///     Adds Embedly SDK services to the dependency injection container using configuration.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="optionsSection">The configuration section containing Embedly options.</param>
    /// <returns>The configured service collection.</returns>
    public static IServiceCollection AddEmbedly(
        this IServiceCollection services,
        IConfigurationSection optionsSection)
    {
        if (services == null) throw new ArgumentNullException(nameof(services));

        if (optionsSection == null) throw new ArgumentNullException(nameof(optionsSection));

        // Configure options from configuration
        services.Configure<EmbedlyOptions>(optionsSection);
        services.PostConfigure<EmbedlyOptions>(options => options.Validate());

        return AddEmbedlyCore(services);
    }

    private static IServiceCollection AddEmbedlyCore(IServiceCollection services)
    {
        // Register HTTP client with handlers
        services.TryAddTransient<AuthenticationHandler>();
        services.TryAddTransient<LoggingHandler>();

        // Default no-op availability notifier. Apps that want circuit-state alerts override
        // this with their own singleton implementation BEFORE calling AddEmbedly (TryAdd is
        // a no-op when something is already registered) — or the SDK's TryAdd registers the
        // null implementation as a fallback.
        services.TryAddSingleton<IEmbedlyAvailabilityNotifier, NullEmbedlyAvailabilityNotifier>();

        services.AddHttpClient<IEmbedlyHttpClient, EmbedlyHttpClient>((serviceProvider, client) =>
            {
                var options = serviceProvider.GetRequiredService<IOptions<EmbedlyOptions>>().Value;
                var serviceUrls = options.GetServiceUrls();

                client.BaseAddress = new Uri(serviceUrls.Base);
                client.Timeout = options.Timeout;
            })
            .AddHttpMessageHandler<AuthenticationHandler>()
            .AddHttpMessageHandler<LoggingHandler>()
            .AddPolicyHandler(GetRetryPolicy())
            .AddPolicyHandler((sp, _) => GetCircuitBreakerPolicy(sp));

        // Register all services
        services.TryAddScoped<ICustomerService, CustomerService>();
        services.TryAddScoped<ICorporateCustomerService, CorporateCustomerService>();
        services.TryAddScoped<IWalletService, WalletService>();
        // services.TryAddScoped<IWalletGroupService, WalletGroupService>();  // Commented out - not in public API docs
        // services.TryAddScoped<IProductService, ProductService>();          // Commented out - not in public API docs
        // services.TryAddScoped<IProductLimitService, ProductLimitService>();// Commented out - not in public API docs
        services.TryAddScoped<ICheckoutService, CheckoutService>();
        services.TryAddScoped<IPayoutService, PayoutService>();
        services.TryAddScoped<IPinEncryptionService, PinEncryptionService>();
        services.TryAddScoped<ICardService, CardService>();
        services.TryAddScoped<IUtilityService, UtilityService>();

        // Register main client
        services.TryAddScoped<IEmbedlyClient, EmbedlyClient>();

        return services;
    }

    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .OrResult(msg => msg.StatusCode == HttpStatusCode.TooManyRequests)
            .WaitAndRetryAsync(
                3,
                retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                (outcome, timespan, retryCount, context) =>
                {
                    // Optional: Add logging here
                });
    }

    // Polly's circuit breaker is stateful — every HTTP call must hit the SAME policy
    // instance for break/half-open/reset transitions to work correctly. AddPolicyHandler's
    // factory overload is invoked per-request, so we cache the policy in a static field
    // (lock-protected) on first resolution. The cached policy holds a captured reference
    // to the IEmbedlyAvailabilityNotifier resolved from DI.
    private static readonly object _circuitBreakerLock = new();
    private static IAsyncPolicy<HttpResponseMessage>? _cachedCircuitBreakerPolicy;

    private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy(IServiceProvider serviceProvider)
    {
        if (_cachedCircuitBreakerPolicy is { } cached) return cached;

        lock (_circuitBreakerLock)
        {
            if (_cachedCircuitBreakerPolicy is { } cachedAfterLock) return cachedAfterLock;

            var notifier = serviceProvider.GetRequiredService<IEmbedlyAvailabilityNotifier>();

            _cachedCircuitBreakerPolicy = HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(
                    handledEventsAllowedBeforeBreaking: 5,
                    durationOfBreak: TimeSpan.FromSeconds(30),
                    onBreak: (outcome, breakDuration) =>
                    {
                        notifier.OnCircuitBreak(new EmbedlyCircuitBreakInfo(
                            outcome.Exception,
                            (int?)outcome.Result?.StatusCode,
                            breakDuration));
                    },
                    onReset: () => notifier.OnCircuitReset(),
                    onHalfOpen: () => notifier.OnCircuitHalfOpen());

            return _cachedCircuitBreakerPolicy;
        }
    }

    /// <summary>
    ///     Test hook: clears the cached circuit-breaker policy so each test gets a fresh
    ///     instance. Production code never calls this.
    /// </summary>
    internal static void ResetCircuitBreakerPolicyCache()
    {
        lock (_circuitBreakerLock)
        {
            _cachedCircuitBreakerPolicy = null;
        }
    }
}

/// <summary>
///     Extension methods for configuring Embedly SDK with a fluent interface.
/// </summary>
public static class EmbedlyServiceCollectionExtensions
{
    /// <summary>
    ///     Configures the Embedly SDK for a specific environment.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="apiKey">The API key.</param>
    /// <param name="environment">The target environment.</param>
    /// <returns>The configured service collection.</returns>
    public static IServiceCollection AddEmbedly(
        this IServiceCollection services,
        string apiKey,
        EmbedlyEnvironment environment = EmbedlyEnvironment.Staging)
    {
        return services.AddEmbedly(options =>
        {
            options.ApiKey = apiKey;
            options.Environment = environment;
        });
    }

    /// <summary>
    ///     Configures the Embedly SDK with custom service URLs.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="apiKey">The API key.</param>
    /// <param name="customUrls">Custom service URLs.</param>
    /// <returns>The configured service collection.</returns>
    public static IServiceCollection AddEmbedly(
        this IServiceCollection services,
        string apiKey,
        ServiceUrls customUrls)
    {
        return services.AddEmbedly(options =>
        {
            options.ApiKey = apiKey;
            options.CustomServiceUrls = customUrls;
        });
    }

    /// <summary>
    ///     Adds Embedly webhook processing services.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="webhookSecret">The webhook secret for signature validation.</param>
    /// <returns>The configured service collection.</returns>
    public static IServiceCollection AddEmbedlyWebhooks(
        this IServiceCollection services,
        string webhookSecret)
    {
        services.TryAddSingleton<IWebhookValidator>(new WebhookValidator(webhookSecret));
        services.TryAddScoped<IWebhookProcessor, WebhookProcessor>();
        return services;
    }

    /// <summary>
    ///     Adds a custom webhook handler.
    /// </summary>
    /// <typeparam name="THandler">The webhook handler type.</typeparam>
    /// <param name="services">The service collection.</param>
    /// <returns>The configured service collection.</returns>
    public static IServiceCollection AddWebhookHandler<THandler>(this IServiceCollection services)
        where THandler : class, IWebhookHandler
    {
        services.TryAddScoped<IWebhookHandler, THandler>();
        return services;
    }
}