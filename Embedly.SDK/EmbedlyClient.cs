using System;
using Embedly.SDK.Configuration;
using Embedly.SDK.Http;
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
using Microsoft.Extensions.Options;

namespace Embedly.SDK;

/// <summary>
///     Main client for accessing Embedly API services.
/// </summary>
public sealed class EmbedlyClient : IEmbedlyClient, IDisposable
{
    private readonly IEmbedlyHttpClient _httpClient;
    private readonly EmbedlyOptions _options;
    private bool _disposed;

    /// <summary>
    ///     Initializes a new instance of the <see cref="EmbedlyClient" /> class.
    /// </summary>
    /// <param name="httpClient">The HTTP client for API communication.</param>
    /// <param name="options">The configuration options.</param>
    /// <param name="customerService">The customer service.</param>
    /// <param name="corporateCustomerService">The corporate customer service.</param>
    /// <param name="walletService">The wallet service.</param>
    /// <param name="checkoutService">The checkout service.</param>
    /// <param name="payoutService">The payout service.</param>
    /// <param name="cardService">The card service.</param>
    /// <param name="utilityService">The utility service.</param>
    public EmbedlyClient(
        IEmbedlyHttpClient httpClient,
        IOptions<EmbedlyOptions> options,
        ICustomerService customerService,
        ICorporateCustomerService corporateCustomerService,
        IWalletService walletService,
        // IWalletGroupService walletGroupService,  // Commented out - not in public API docs
        // IProductService productService,          // Commented out - not in public API docs
        // IProductLimitService productLimitService, // Commented out - not in public API docs
        ICheckoutService checkoutService,
        IPayoutService payoutService,
        ICardService cardService,
        IUtilityService utilityService)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));

        // Validate configuration
        _options.Validate();

        Customers = customerService ?? throw new ArgumentNullException(nameof(customerService));
        CorporateCustomers = corporateCustomerService ?? throw new ArgumentNullException(nameof(corporateCustomerService));
        Wallets = walletService ?? throw new ArgumentNullException(nameof(walletService));
        // WalletGroups = walletGroupService ?? throw new ArgumentNullException(nameof(walletGroupService));  // Commented out - not in public API docs
        // Products = productService ?? throw new ArgumentNullException(nameof(productService));              // Commented out - not in public API docs
        // ProductLimits = productLimitService ?? throw new ArgumentNullException(nameof(productLimitService)); // Commented out - not in public API docs
        Checkout = checkoutService ?? throw new ArgumentNullException(nameof(checkoutService));
        Payouts = payoutService ?? throw new ArgumentNullException(nameof(payoutService));
        Cards = cardService ?? throw new ArgumentNullException(nameof(cardService));
        Utilities = utilityService ?? throw new ArgumentNullException(nameof(utilityService));
    }

    /// <inheritdoc />
    public void Dispose()
    {
        if (_disposed) return;

        if (_httpClient is IDisposable disposableClient) disposableClient.Dispose();

        _disposed = true;
    }

    /// <inheritdoc />
    public ICustomerService Customers { get; }

    /// <inheritdoc />
    public ICorporateCustomerService CorporateCustomers { get; }

    /// <inheritdoc />
    public IWalletService Wallets { get; }

    // /// <inheritdoc />
    // public IWalletGroupService WalletGroups { get; }  // Commented out - not in public API docs

    // /// <inheritdoc />
    // public IProductService Products { get; }  // Commented out - not in public API docs

    // /// <inheritdoc />
    // public IProductLimitService ProductLimits { get; }  // Commented out - not in public API docs

    /// <inheritdoc />
    public ICheckoutService Checkout { get; }

    /// <inheritdoc />
    public IPayoutService Payouts { get; }

    /// <inheritdoc />
    public ICardService Cards { get; }

    /// <inheritdoc />
    public IUtilityService Utilities { get; }
}