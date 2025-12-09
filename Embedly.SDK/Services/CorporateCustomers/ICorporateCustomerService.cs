using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Embedly.SDK.Models.Requests.CorporateCustomers;
using Embedly.SDK.Models.Requests.Wallets;
using Embedly.SDK.Models.Responses.Common;
using Embedly.SDK.Models.Responses.CorporateCustomers;
using Embedly.SDK.Models.Responses.Wallets;

namespace Embedly.SDK.Services.CorporateCustomers;

/// <summary>
///     Interface for corporate customer management operations.
/// </summary>
public interface ICorporateCustomerService
{
    /// <summary>
    ///     Creates a new corporate customer.
    /// </summary>
    /// <param name="request">The create corporate customer request.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>The created corporate customer.</returns>
    Task<ApiResponse<CorporateCustomer>> CreateAsync(
        CreateCorporateCustomerRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Retrieves a corporate customer by ID.
    /// </summary>
    /// <param name="corporateCustomerId">The corporate customer identifier.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>The corporate customer details.</returns>
    Task<ApiResponse<CorporateCustomer>> GetByIdAsync(
        string corporateCustomerId,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Updates a corporate customer.
    /// </summary>
    /// <param name="corporateCustomerId">The corporate customer identifier.</param>
    /// <param name="request">The update corporate customer request.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>The updated corporate customer.</returns>
    Task<ApiResponse<CorporateCustomer>> UpdateAsync(
        string corporateCustomerId,
        UpdateCorporateCustomerRequest request,
        CancellationToken cancellationToken = default);

    #region Director Management

    /// <summary>
    ///     Adds a director to a corporate customer.
    /// </summary>
    /// <param name="corporateCustomerId">The corporate customer identifier.</param>
    /// <param name="request">The add director request.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>The added director.</returns>
    Task<ApiResponse<AddDirectorResponse>> AddDirectorAsync(
        string corporateCustomerId,
        AddDirectorRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Retrieves all directors of a corporate customer.
    /// </summary>
    /// <param name="corporateCustomerId">The corporate customer identifier.</param>
    /// <param name="page">Page number (1-based).</param>
    /// <param name="pageSize">Number of items per page.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>List of directors.</returns>
    Task<ApiResponse<IEnumerable<Director>>> GetDirectorsAsync(
        string corporateCustomerId,
        int page = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Retrieves a specific director by ID.
    /// </summary>
    /// <param name="corporateCustomerId">The corporate customer identifier.</param>
    /// <param name="directorId">The director identifier.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>The director details.</returns>
    Task<ApiResponse<Director>> GetDirectorByIdAsync(
        string corporateCustomerId,
        string directorId,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Updates a director's information.
    /// </summary>
    /// <param name="corporateCustomerId">The corporate customer identifier.</param>
    /// <param name="directorId">The director identifier.</param>
    /// <param name="request">The update director request.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>The updated director.</returns>
    Task<ApiResponse<Director>> UpdateDirectorAsync(
        string corporateCustomerId,
        string directorId,
        AddDirectorRequest request,
        CancellationToken cancellationToken = default);

    #endregion

    #region Document Management

    /// <summary>
    ///     Adds a document to a corporate customer.
    /// </summary>
    /// <param name="corporateCustomerId">The corporate customer identifier.</param>
    /// <param name="request">The add document request.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>The added document.</returns>
    Task<ApiResponse<CorporateDocument>> AddDocumentAsync(
        string corporateCustomerId,
        AddCorporateDocumentRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Retrieves all documents of a corporate customer.
    /// </summary>
    /// <param name="corporateCustomerId">The corporate customer identifier.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>List of documents.</returns>
    Task<ApiResponse<IEnumerable<CorporateDocument>>> GetDocumentsAsync(
        string corporateCustomerId,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Updates documents for a corporate customer.
    /// </summary>
    /// <param name="corporateCustomerId">The corporate customer identifier.</param>
    /// <param name="request">The update document request.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>The updated document.</returns>
    Task<ApiResponse<CorporateDocument>> UpdateDocumentAsync(
        string corporateCustomerId,
        UpdateCorporateDocumentRequest request,
        CancellationToken cancellationToken = default);

    #endregion

    #region Wallet Management

    /// <summary>
    ///     Creates a wallet for a corporate customer.
    /// </summary>
    /// <param name="corporateCustomerId">The corporate customer identifier.</param>
    /// <param name="request">The create wallet request.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>The created wallet.</returns>
    Task<ApiResponse<Wallet>> CreateWalletAsync(
        string corporateCustomerId,
        CreateCorporateWalletRequest request,
        CancellationToken cancellationToken = default);

    #endregion
}