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

namespace Embedly.SDK;

/// <summary>
///     Main interface for accessing Embedly API services.
/// </summary>
public interface IEmbedlyClient
{
    /// <summary>
    ///     Gets the customer management service.
    /// </summary>
    ICustomerService Customers { get; }

    /// <summary>
    ///     Gets the corporate customer management service.
    /// </summary>
    ICorporateCustomerService CorporateCustomers { get; }

    /// <summary>
    ///     Gets the wallet management service.
    /// </summary>
    IWalletService Wallets { get; }

    // /// <summary>
    // ///     Gets the wallet group management service.
    // /// </summary>
    // IWalletGroupService WalletGroups { get; }  // Commented out - not in public API docs

    // /// <summary>
    // ///     Gets the product management service.
    // /// </summary>
    // IProductService Products { get; }  // Commented out - not in public API docs

    // /// <summary>
    // ///     Gets the product limit management service.
    // /// </summary>
    // IProductLimitService ProductLimits { get; }  // Commented out - not in public API docs

    /// <summary>
    ///     Gets the checkout service.
    /// </summary>
    ICheckoutService Checkout { get; }

    /// <summary>
    ///     Gets the payout service.
    /// </summary>
    IPayoutService Payouts { get; }

    /// <summary>
    ///     Gets the card management service.
    /// </summary>
    ICardService Cards { get; }

    /// <summary>
    ///     Gets the utility service.
    /// </summary>
    IUtilityService Utilities { get; }
}