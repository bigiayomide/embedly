using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Embedly.SDK.Configuration;
using Embedly.SDK.Http;
using Embedly.SDK.Models.Requests.Utilities;
using Embedly.SDK.Models.Responses.Common;
using Embedly.SDK.Models.Responses.Utilities;
using Embedly.SDK.Helpers;

namespace Embedly.SDK.Services.Utilities;

/// <summary>
/// Service for utility operations based on actual waas-staging API.
/// </summary>
internal sealed class UtilityService : BaseService, IUtilityService
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UtilityService"/> class.
    /// </summary>
    /// <param name="httpClient">The HTTP client.</param>
    /// <param name="options">The configuration options.</param>
    public UtilityService(IEmbedlyHttpClient httpClient, IOptions<EmbedlyOptions> options)
        : base(httpClient, options)
    {
    }

    /// <inheritdoc />
    public async Task<ApiResponse<List<Currency>>> GetCurrenciesAsync(CancellationToken cancellationToken = default)
    {
        var url = BuildUrl("api/v1/utilities/currencies/get");
        return await HttpClient.GetAsync<List<Currency>>(url, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<List<Currency>>> GetCurrenciesAlternateAsync(CancellationToken cancellationToken = default)
    {
        var url = BuildUrl("api/v1/currencies/get");
        return await HttpClient.GetAsync<List<Currency>>(url, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<Currency>> CreateCurrencyAsync(CreateCurrencyRequest request, CancellationToken cancellationToken = default)
    {
        Guard.ThrowIfNull(request, nameof(request));
        
        var url = BuildUrl("api/v1/currencies/add");
        return await HttpClient.PostAsync<CreateCurrencyRequest, Currency>(url, request, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<List<Country>>> GetCountriesAsync(CancellationToken cancellationToken = default)
    {
        var url = BuildUrl("api/v1/utilities/countries/get");
        return await HttpClient.GetAsync<List<Country>>(url, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<List<Country>>> GetCountriesAlternateAsync(CancellationToken cancellationToken = default)
    {
        var url = BuildUrl("api/v1/countries/get");
        return await HttpClient.GetAsync<List<Country>>(url, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<Country>> CreateCountryAsync(CreateCountryRequest request, CancellationToken cancellationToken = default)
    {
        Guard.ThrowIfNull(request, nameof(request));
        
        var url = BuildUrl("api/v1/countries/add");
        return await HttpClient.PostAsync<CreateCountryRequest, Country>(url, request, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<FileUploadResponse>> UploadFileAsync(Stream fileStream, string fileName, string contentType = "application/octet-stream", CancellationToken cancellationToken = default)
    {
        Guard.ThrowIfNull(fileStream, nameof(fileStream));
        Guard.ThrowIfNullOrWhiteSpace(fileName, nameof(fileName));
        
        var url = BuildUrl("api/v1/utilities/upload");
        return await HttpClient.PostMultipartAsync<FileUploadResponse>(url, fileStream, fileName, contentType, cancellationToken);
    }
}