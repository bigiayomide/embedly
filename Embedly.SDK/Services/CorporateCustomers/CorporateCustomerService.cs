using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Embedly.SDK.Configuration;
using Embedly.SDK.Helpers;
using Embedly.SDK.Http;
using Embedly.SDK.Models.Requests.CorporateCustomers;
using Embedly.SDK.Models.Requests.Wallets;
using Embedly.SDK.Models.Responses.Common;
using Embedly.SDK.Models.Responses.CorporateCustomers;
using Embedly.SDK.Models.Responses.Wallets;
using Microsoft.Extensions.Options;

namespace Embedly.SDK.Services.CorporateCustomers;

/// <summary>
///     Implementation of corporate customer management operations.
/// </summary>
internal sealed class CorporateCustomerService : BaseService, ICorporateCustomerService
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="CorporateCustomerService" /> class.
    /// </summary>
    /// <param name="httpClient">The HTTP client.</param>
    /// <param name="options">The configuration options.</param>
    public CorporateCustomerService(IEmbedlyHttpClient httpClient, IOptions<EmbedlyOptions> options)
        : base(httpClient, options)
    {
    }

    /// <inheritdoc />
    public async Task<ApiResponse<CorporateCustomer>> CreateAsync(
        CreateCorporateCustomerRequest request,
        CancellationToken cancellationToken = default)
    {
        Guard.ThrowIfNull(request, nameof(request));

        var url = BuildUrl(ServiceUrls.Base, "api/v1/corporate/customers");
        return await HttpClient.PostAsync<CreateCorporateCustomerRequest, CorporateCustomer>(url, request, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<CorporateCustomer>> GetByIdAsync(
        string corporateCustomerId,
        CancellationToken cancellationToken = default)
    {
        Guard.ThrowIfNullOrWhiteSpace(corporateCustomerId, nameof(corporateCustomerId));

        var url = BuildUrl(ServiceUrls.Base, $"api/v1/corporate/customers/{corporateCustomerId}");
        return await HttpClient.GetAsync<CorporateCustomer>(url, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<CorporateCustomer>> UpdateAsync(
        string corporateCustomerId,
        UpdateCorporateCustomerRequest request,
        CancellationToken cancellationToken = default)
    {
        Guard.ThrowIfNullOrWhiteSpace(corporateCustomerId, nameof(corporateCustomerId));
        Guard.ThrowIfNull(request, nameof(request));

        var url = BuildUrl(ServiceUrls.Base, $"api/v1/corporate/customers/{corporateCustomerId}");
        return await HttpClient.PutAsync<UpdateCorporateCustomerRequest, CorporateCustomer>(url, request, cancellationToken);
    }


    #region Director Management

    /// <inheritdoc />
    public async Task<ApiResponse<AddDirectorResponse>> AddDirectorAsync(
        string corporateCustomerId,
        AddDirectorRequest request,
        CancellationToken cancellationToken = default)
    {
        Guard.ThrowIfNullOrWhiteSpace(corporateCustomerId, nameof(corporateCustomerId));
        Guard.ThrowIfNull(request, nameof(request));

        var url = BuildUrl(ServiceUrls.Base, $"api/v1/corporate/customers/{corporateCustomerId}/directors");
        return await HttpClient.PostAsync<AddDirectorRequest, AddDirectorResponse>(url, request, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<IEnumerable<Director>>> GetDirectorsAsync(
        string corporateCustomerId,
        int page = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        Guard.ThrowIfNullOrWhiteSpace(corporateCustomerId, nameof(corporateCustomerId));

        var queryParams = new Dictionary<string, object?>
        {
            ["page"] = page,
            ["pageSize"] = pageSize
        };

        var url = BuildUrl(ServiceUrls.Base, $"api/v1/corporate/customers/{corporateCustomerId}/directors");
        var response = await HttpClient.GetAsync<List<Director>>(url, queryParams, cancellationToken);

        // Convert List<Director> response to IEnumerable<Director>
        return new ApiResponse<IEnumerable<Director>>
        {
            Success = response.Success,
            Message = response.Message,
            Data = response.Data,
            Error = response.Error,
            Pagination = response.Pagination,
            Timestamp = response.Timestamp,
            RequestId = response.RequestId
        };
    }

    /// <inheritdoc />
    public async Task<ApiResponse<Director>> GetDirectorByIdAsync(
        string corporateCustomerId,
        string directorId,
        CancellationToken cancellationToken = default)
    {
        Guard.ThrowIfNullOrWhiteSpace(corporateCustomerId, nameof(corporateCustomerId));
        Guard.ThrowIfNullOrWhiteSpace(directorId, nameof(directorId));

        var url = BuildUrl(ServiceUrls.Base, $"api/v1/corporate/customers/{corporateCustomerId}/directors/{directorId}");
        return await HttpClient.GetAsync<Director>(url, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<Director>> UpdateDirectorAsync(
        string corporateCustomerId,
        string directorId,
        AddDirectorRequest request,
        CancellationToken cancellationToken = default)
    {
        Guard.ThrowIfNullOrWhiteSpace(corporateCustomerId, nameof(corporateCustomerId));
        Guard.ThrowIfNullOrWhiteSpace(directorId, nameof(directorId));
        Guard.ThrowIfNull(request, nameof(request));

        var url = BuildUrl(ServiceUrls.Base, $"api/v1/corporate/customers/{corporateCustomerId}/directors/{directorId}");
        return await HttpClient.PutAsync<AddDirectorRequest, Director>(url, request, cancellationToken);
    }


    #endregion

    #region Document Management

    /// <inheritdoc />
    public async Task<ApiResponse<CorporateDocument>> AddDocumentAsync(
        string corporateCustomerId,
        AddCorporateDocumentRequest request,
        CancellationToken cancellationToken = default)
    {
        Guard.ThrowIfNullOrWhiteSpace(corporateCustomerId, nameof(corporateCustomerId));
        Guard.ThrowIfNull(request, nameof(request));

        var url = BuildUrl(ServiceUrls.Base, $"api/v1/corporate/customers/{corporateCustomerId}/documents");
        return await HttpClient.PostAsync<AddCorporateDocumentRequest, CorporateDocument>(url, request, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<IEnumerable<CorporateDocument>>> GetDocumentsAsync(
        string corporateCustomerId,
        CancellationToken cancellationToken = default)
    {
        Guard.ThrowIfNullOrWhiteSpace(corporateCustomerId, nameof(corporateCustomerId));

        var url = BuildUrl(ServiceUrls.Base, $"api/v1/corporate/customers/{corporateCustomerId}/documents");
        return await HttpClient.GetAsync<IEnumerable<CorporateDocument>>(url, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<CorporateDocument>> UpdateDocumentAsync(
        string corporateCustomerId,
        UpdateCorporateDocumentRequest request,
        CancellationToken cancellationToken = default)
    {
        Guard.ThrowIfNullOrWhiteSpace(corporateCustomerId, nameof(corporateCustomerId));
        Guard.ThrowIfNull(request, nameof(request));

        var url = BuildUrl(ServiceUrls.Base, $"api/v1/corporate/customers/{corporateCustomerId}/documents");
        return await HttpClient.PutAsync<UpdateCorporateDocumentRequest, CorporateDocument>(url, request, cancellationToken);
    }


    #endregion

    #region Wallet Management

    /// <inheritdoc />
    public async Task<ApiResponse<Wallet>> CreateWalletAsync(
        string corporateCustomerId,
        CreateCorporateWalletRequest request,
        CancellationToken cancellationToken = default)
    {
        Guard.ThrowIfNullOrWhiteSpace(corporateCustomerId, nameof(corporateCustomerId));
        Guard.ThrowIfNull(request, nameof(request));

        var url = BuildUrl(ServiceUrls.Base, $"api/v1/corporate/customers/{corporateCustomerId}/wallets");
        return await HttpClient.PostAsync<CreateCorporateWalletRequest, Wallet>(url, request, cancellationToken);
    }

    #endregion
}