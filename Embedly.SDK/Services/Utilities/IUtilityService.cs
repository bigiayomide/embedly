using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Embedly.SDK.Models.Requests.Utilities;
using Embedly.SDK.Models.Responses.Common;
using Embedly.SDK.Models.Responses.Utilities;

namespace Embedly.SDK.Services.Utilities;

/// <summary>
/// Interface for utility operations based on actual waas-staging API.
/// </summary>
public interface IUtilityService
{
    /// <summary>
    /// Gets all available currencies from utilities endpoint.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of available currencies.</returns>
    Task<ApiResponse<List<Currency>>> GetCurrenciesAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets all available currencies from currencies endpoint.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of available currencies.</returns>
    Task<ApiResponse<List<Currency>>> GetCurrenciesAlternateAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Adds a new currency.
    /// </summary>
    /// <param name="request">The currency creation request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The created currency.</returns>
    Task<ApiResponse<Currency>> CreateCurrencyAsync(CreateCurrencyRequest request, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets all available countries from utilities endpoint.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of available countries.</returns>
    Task<ApiResponse<List<Country>>> GetCountriesAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Gets all available countries from countries endpoint.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>List of available countries.</returns>
    Task<ApiResponse<List<Country>>> GetCountriesAlternateAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Adds a new country.
    /// </summary>
    /// <param name="request">The country creation request.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The created country.</returns>
    Task<ApiResponse<Country>> CreateCountryAsync(CreateCountryRequest request, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Uploads a file using multipart/form-data.
    /// </summary>
    /// <param name="fileStream">The file stream to upload.</param>
    /// <param name="fileName">The name of the file.</param>
    /// <param name="contentType">The content type of the file.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The file upload response with URL and metadata.</returns>
    Task<ApiResponse<FileUploadResponse>> UploadFileAsync(Stream fileStream, string fileName, string contentType = "application/octet-stream", CancellationToken cancellationToken = default);
}