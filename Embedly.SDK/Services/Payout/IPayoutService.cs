using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Embedly.SDK.Models.Requests.Payout;
using Embedly.SDK.Models.Responses.Common;
using Embedly.SDK.Models.Responses.Payout;

namespace Embedly.SDK.Services.Payout;

/// <summary>
/// Interface for payout operations with all 15 endpoints (8 payout + 7 payout limits).
/// </summary>
public interface IPayoutService
{
    // ===== PAYOUT OPERATIONS (8 endpoints) =====
    
    /// <summary>
    /// Gets all available banks with optional search filter.
    /// </summary>
    /// <param name="search">Optional search filter.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of available banks.</returns>
    Task<ApiResponse<List<Bank>>> GetBanksAsync(string? search = null, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Performs bank account name enquiry.
    /// </summary>
    /// <param name="request">The name enquiry request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The account name details.</returns>
    Task<ApiResponse<NameEnquiryResponse>> NameEnquiryAsync(NameEnquiryRequest request, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Initiates an inter-bank transfer.
    /// </summary>
    /// <param name="request">The bank transfer request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The transfer transaction details.</returns>
    Task<ApiResponse<PayoutTransaction>> InterBankTransferAsync(BankTransferRequest request, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets the status of a payout transaction.
    /// </summary>
    /// <param name="transactionReference">The transaction reference.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The transaction status details.</returns>
    Task<ApiResponse<PayoutTransaction>> GetTransactionStatusAsync(string transactionReference, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets organization payout data.
    /// </summary>
    /// <param name="organizationId">The organization ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Organization payout data.</returns>
    Task<ApiResponse<OrganizationPayoutData>> GetOrganizationPayoutDataAsync(Guid organizationId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Creates payout profile for organization.
    /// </summary>
    /// <param name="request">The payout request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The created profile response.</returns>
    Task<ApiResponse<PayoutProfileResponse>> CreatePayoutProfileAsync(PayoutRequestDto request, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets all payouts.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of all payouts.</returns>
    Task<ApiResponse<List<PayoutTransaction>>> GetAllPayoutsAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets wallet details by account number.
    /// </summary>
    /// <param name="accountNumber">The account number.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Wallet details.</returns>
    Task<ApiResponse<WalletDetails>> GetWalletByAccountNumberAsync(string accountNumber, CancellationToken cancellationToken = default);
    
    // ===== PAYOUT LIMITS OPERATIONS (7 endpoints) =====
    
    /// <summary>
    /// Adds a global payout limit.
    /// </summary>
    /// <param name="request">The add global payout limit request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The created global payout limit.</returns>
    Task<ApiResponse<GlobalPayoutLimit>> AddGlobalPayoutLimitAsync(AddGlobalPayoutLimitRequest request, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets global payout limits with pagination.
    /// </summary>
    /// <param name="request">The pagination request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Paginated list of global payout limits.</returns>
    Task<ApiResponse<PaginatedResponse<GlobalPayoutLimit>>> GetGlobalPayoutLimitsAsync(GetPayoutLimitsRequest request, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Updates a global payout limit by ID.
    /// </summary>
    /// <param name="globalLimitId">The global limit ID.</param>
    /// <param name="request">The update request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The updated global payout limit.</returns>
    Task<ApiResponse<GlobalPayoutLimit>> UpdateGlobalPayoutLimitAsync(Guid globalLimitId, AddGlobalPayoutLimitRequest request, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Updates global payout limit for organization and currency.
    /// </summary>
    /// <param name="organizationId">The organization ID.</param>
    /// <param name="currencyId">The currency ID.</param>
    /// <param name="request">The update request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The updated global payout limit.</returns>
    Task<ApiResponse<GlobalPayoutLimit>> UpdateOrganizationCurrencyLimitAsync(Guid organizationId, Guid currencyId, AddGlobalPayoutLimitRequest request, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Creates default global payout limit.
    /// </summary>
    /// <param name="request">The add global payout limit request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The created default global payout limit.</returns>
    Task<ApiResponse<GlobalPayoutLimit>> CreateDefaultGlobalPayoutLimitAsync(AddGlobalPayoutLimitRequest request, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Updates default global payout limit.
    /// </summary>
    /// <param name="defaultLimitId">The default limit ID.</param>
    /// <param name="request">The update request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The updated default global payout limit.</returns>
    Task<ApiResponse<GlobalPayoutLimit>> UpdateDefaultGlobalPayoutLimitAsync(Guid defaultLimitId, AddGlobalPayoutLimitRequest request, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Enables or disables global payout limit.
    /// </summary>
    /// <param name="limitId">The limit ID.</param>
    /// <param name="enabled">Whether to enable or disable the limit.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The updated global payout limit.</returns>
    Task<ApiResponse<GlobalPayoutLimit>> EnableOrDisableGlobalPayoutLimitAsync(Guid limitId, bool enabled, CancellationToken cancellationToken = default);
}