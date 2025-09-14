namespace Embedly.SDK.Configuration;

/// <summary>
///     Contains the service URLs for different Embedly API endpoints.
/// </summary>
public sealed class ServiceUrls
{
    /// <summary>
    ///     Gets or sets the base API URL for core services.
    /// </summary>
    public string Base { get; init; } = string.Empty;

    /// <summary>
    ///     Gets or sets the payout service URL.
    /// </summary>
    public string Payout { get; init; } = string.Empty;

    /// <summary>
    ///     Gets or sets the checkout service URL.
    /// </summary>
    public string Checkout { get; init; } = string.Empty;

    /// <summary>
    ///     Gets or sets the card management service URL.
    /// </summary>
    public string Cards { get; init; } = string.Empty;

    /// <summary>
    ///     Creates service URLs for the staging environment.
    /// </summary>
    public static ServiceUrls Staging => new()
    {
        Base = "https://waas-staging.embedly.ng",
        Payout = "https://payout-staging.embedly.ng",
        Checkout = "https://checkout-staging.embedly.ng",
        Cards = "https://waas-card-middleware-api-staging.embedly.ng"
    };

    /// <summary>
    ///     Creates service URLs for the production environment.
    /// </summary>
    public static ServiceUrls Production => new()
    {
        Base = "https://waas-prod.embedly.ng",
        Payout = "https://payout-prod.embedly.ng",
        Checkout = "https://checkout-prod.embedly.ng",
        Cards = "https://waas-card-middleware-api-prod.embedly.ng"
    };
}