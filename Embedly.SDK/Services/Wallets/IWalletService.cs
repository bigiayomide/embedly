using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Embedly.SDK.Models.Requests.Wallets;
using Embedly.SDK.Models.Responses.Common;
using Embedly.SDK.Models.Responses.Wallets;

namespace Embedly.SDK.Services.Wallets;

/// <summary>
///     Interface for wallet management operations based on actual WaasCore API endpoints.
/// </summary>
public interface IWalletService
{
    // ===== WALLET CREATION =====

    /// <summary>
    ///     Creates a new wallet.
    ///     POST /api/v1/wallets/add
    /// </summary>
    Task<ApiResponse<Wallet>> CreateWalletAsync(CreateWalletRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Creates a wallet for a corporate customer.
    ///     POST /api/v1/corporate/customers/{customerId}/wallets
    /// </summary>
    Task<ApiResponse<Wallet>> CreateCorporateWalletAsync(string customerId, CreateCorporateWalletRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Adds a wallet to an organization.
    ///     POST /api/v1/organizations/add/wallet
    /// </summary>
    Task<ApiResponse<object>> AddOrganizationWalletAsync(AddOrganizationWalletRequest request,
        CancellationToken cancellationToken = default);

    // ===== WALLET RETRIEVAL =====

    /// <summary>
    ///     Gets a wallet by ID.
    ///     GET /api/v1/wallets/get/wallet/{walletId}
    /// </summary>
    Task<ApiResponse<Wallet>> GetWalletAsync(string walletId, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Gets a wallet by account number.
    ///     GET /api/v1/wallets/get/wallet/account/{accountNumber}
    /// </summary>
    Task<ApiResponse<Wallet>> GetWalletByAccountNumberAsync(string accountNumber,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Gets wallets for a customer.
    ///     GET /api/v1/wallets/get/wallet/customer/{customerId}
    /// </summary>
    Task<ApiResponse<List<Wallet>>> GetCustomerWalletsAsync(string customerId,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Gets a specific wallet for a customer.
    ///     GET /api/v1/wallets/get/customer/{customerId}/wallet/{walletId}
    /// </summary>
    Task<ApiResponse<Wallet>> GetCustomerWalletAsync(string customerId, string walletId,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Gets organization wallets.
    ///     GET /api/v1/wallets/get/organization-wallets
    /// </summary>
    Task<ApiResponse<List<Wallet>>> GetOrganizationWalletsAsync(CancellationToken cancellationToken = default);

    // ===== WALLET SEARCH =====

    /// <summary>
    ///     Searches wallets by email.
    ///     POST /api/v1/wallets/search/email
    /// </summary>
    Task<ApiResponse<List<Wallet>>> SearchWalletsByEmailAsync(SearchWalletByEmailRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Searches wallets by mobile number.
    ///     POST /api/v1/wallets/search/mobile
    /// </summary>
    Task<ApiResponse<List<Wallet>>> SearchWalletsByMobileAsync(SearchWalletByMobileRequest request,
        CancellationToken cancellationToken = default);

    // ===== WALLET HISTORY & TRANSACTIONS =====

    /// <summary>
    ///     Gets wallet transaction history.
    ///     GET /api/v1/wallets/history
    /// </summary>
    Task<ApiResponse<List<WalletTransaction>>> GetWalletHistoryAsync(Guid walletId,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Gets wallet history by account number.
    ///     GET /api/v1/wallets/account-number/history
    /// </summary>
    Task<ApiResponse<List<WalletTransaction>>> GetWalletHistoryByAccountNumberAsync(string accountNumber,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Gets organization transaction history.
    ///     GET /api/v1/wallets/history/organization/transactions
    /// </summary>
    Task<ApiResponse<List<WalletTransaction>>> GetOrganizationTransactionHistoryAsync(Guid customerId,
        CancellationToken cancellationToken = default);

    // ===== WALLET TRANSACTIONS =====

    /// <summary>
    ///     Posts a wallet transaction operation.
    ///     PUT /api/v1/operations/wallet/transaction/operations/post
    /// </summary>
    Task<ApiResponse<object>> PostWalletTransactionAsync(PendingTransactionRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Reverses a wallet transaction.
    ///     PUT /api/v1/operations/wallet/transaction/reverse
    /// </summary>
    Task<ApiResponse<object>> ReverseTransactionAsync(ReverseTransactionRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Posts a transaction by account number.
    ///     PUT /api/v1/operations/wallet/transaction/account-number/post
    /// </summary>
    Task<ApiResponse<object>> PostTransactionByAccountNumberAsync(PendingTransactionRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Completes a wallet transaction.
    ///     PUT /api/v1/wallets/wallet/transaction/post/complete
    /// </summary>
    Task<ApiResponse<object>> CompleteWalletTransactionAsync(CompleteTransactionRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Performs wallet-to-wallet transfer (v2).
    ///     PUT /api/v1/wallets/wallet/transaction/v2/wallet-to-wallet
    /// </summary>
    Task<ApiResponse<WalletTransferResult>> WalletToWalletTransferAsync(WalletToWalletTransferRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Gets wallet-to-wallet transfer status.
    ///     GET /api/v1/wallets/wallet/transaction/wallet-to-wallet/status/{reference}
    /// </summary>
    Task<ApiResponse<WalletTransferStatus>> GetWalletTransferStatusAsync(string reference,
        CancellationToken cancellationToken = default);

    // ===== WALLET RESTRICTIONS =====

    /// <summary>
    ///     Gets wallet restriction types.
    ///     GET /api/v1/wallets/get/restriction/types
    /// </summary>
    Task<ApiResponse<List<WalletRestrictionType>>> GetWalletRestrictionTypesAsync(
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Restricts a wallet.
    ///     PATCH /api/v1/wallets/wallet/restrict
    /// </summary>
    Task<ApiResponse<object>> RestrictWalletAsync(RestrictWalletRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Restricts a wallet by account number.
    ///     PATCH /api/v1/wallets/wallet/restrict/account/{accountNumber}
    /// </summary>
    Task<ApiResponse<object>> RestrictWalletByAccountNumberAsync(string accountNumber, RestrictWalletRequest request,
        CancellationToken cancellationToken = default);

    // ===== WALLET TYPES & METADATA =====

    /// <summary>
    ///     Gets wallet types.
    ///     GET /api/v1/wallets/wallet/types/get
    /// </summary>
    Task<ApiResponse<List<WalletType>>> GetWalletTypesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    ///     Gets virtual account types.
    ///     GET /api/v1/wallets/wallet/virtual/account/types/get
    /// </summary>
    Task<ApiResponse<List<VirtualAccountType>>> GetVirtualAccountTypesAsync(
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Gets monthly interest for a wallet.
    ///     GET /api/v1/interests/get/month/wallet/{walletId}
    /// </summary>
    Task<ApiResponse<WalletInterest>> GetMonthlyWalletInterestAsync(string walletId,
        CancellationToken cancellationToken = default);
}