using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Embedly.SDK.Configuration;
using Embedly.SDK.Helpers;
using Embedly.SDK.Http;
using Embedly.SDK.Models.Requests.Customers;
using Embedly.SDK.Models.Responses.Customers;
using Embedly.SDK.Models.Responses.Common;

namespace Embedly.SDK.Services.Customers;

/// <summary>
/// Implementation of customer management operations.
/// </summary>
internal sealed class CustomerService : BaseService, ICustomerService
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CustomerService"/> class.
    /// </summary>
    /// <param name="httpClient">The HTTP client.</param>
    /// <param name="options">The configuration options.</param>
    public CustomerService(IEmbedlyHttpClient httpClient, IOptions<EmbedlyOptions> options) 
        : base(httpClient, options)
    {
    }

    /// <inheritdoc />
    public async Task<ApiResponse<Customer>> CreateAsync(CreateCustomerRequest request, CancellationToken cancellationToken = default)
    {
        Guard.ThrowIfNull(request);
        
        var url = BuildUrl(ServiceUrls.Base, "api/v1/customers/add");
        return await HttpClient.PostAsync<CreateCustomerRequest, Customer>(url, request, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<Customer>> GetByIdAsync(string customerId, CancellationToken cancellationToken = default)
    {
        Guard.ThrowIfNullOrWhiteSpace(customerId);
        
        var url = BuildUrl(ServiceUrls.Base, $"api/v1/customers/get/id/{customerId}");
        return await HttpClient.GetAsync<Customer>(url, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<IEnumerable<Customer>>> GetAllAsync(int page = 1, int pageSize = 50, CancellationToken cancellationToken = default)
    {
        var request = new GetCustomersRequest { Page = page, PageSize = pageSize };
        return await GetAllAsync(request, cancellationToken);
    }
    
    /// <inheritdoc />
    public async Task<ApiResponse<IEnumerable<Customer>>> GetAllAsync(GetCustomersRequest request, CancellationToken cancellationToken = default)
    {
        Guard.ThrowIfNull(request);
        
        var url = BuildUrl(ServiceUrls.Base, "api/v1/customers/get/all");
        var queryParams = request.ToQueryParameters();
        var response = await HttpClient.GetAsync<List<Customer>>(url, queryParams, cancellationToken);
        
        // Convert List<Customer> response to IEnumerable<Customer>
        return new ApiResponse<IEnumerable<Customer>>
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
    public async Task<ApiResponse<Customer>> UpdateNameAsync(string customerId, string firstName, string lastName, CancellationToken cancellationToken = default)
    {
        Guard.ThrowIfNullOrWhiteSpace(customerId);
        Guard.ThrowIfNullOrWhiteSpace(firstName);
        Guard.ThrowIfNullOrWhiteSpace(lastName);
        
        var request = new UpdateCustomerNameRequest
        {
            CustomerId = customerId,
            FirstName = firstName,
            LastName = lastName
        };
        
        return await UpdateNameAsync(request, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<Customer>> UpdateNameAsync(UpdateCustomerNameRequest request, CancellationToken cancellationToken = default)
    {
        Guard.ThrowIfNull(request);
        Guard.ThrowIfNullOrWhiteSpace(request.CustomerId);
        
        var url = BuildUrl(ServiceUrls.Base, $"api/v1/customers/customer/{request.CustomerId}/updatename");
        return await HttpClient.PatchAsync<UpdateCustomerNameRequest, Customer>(url, request, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<CustomerVerificationProperties>> GetVerificationPropertiesAsync(string customerId, CancellationToken cancellationToken = default)
    {
        Guard.ThrowIfNullOrWhiteSpace(customerId);
        
        var url = BuildUrl(ServiceUrls.Base, $"api/v1/customers/customer-verification-properties/{customerId}");
        return await HttpClient.GetAsync<CustomerVerificationProperties>(url, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<KycUpgradeResult>> UpgradeKycWithNinAsync(NinKycUpgradeRequest request, CancellationToken cancellationToken = default)
    {
        Guard.ThrowIfNull(request);
        
        var url = BuildUrl(ServiceUrls.Base, "api/v1/customers/kyc/customer/nin");
        return await HttpClient.PostAsync<NinKycUpgradeRequest, KycUpgradeResult>(url, request, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<KycUpgradeResult>> UpgradeKycWithBvnAsync(BvnKycUpgradeRequest request, CancellationToken cancellationToken = default)
    {
        Guard.ThrowIfNull(request);
        Guard.ThrowIfNullOrWhiteSpace(request.CustomerId);
        
        var url = BuildUrl(ServiceUrls.Base, $"api/v1/customers/kyc/monnify/kyc/customer/{request.CustomerId}");
        return await HttpClient.PostAsync<BvnKycUpgradeRequest, KycUpgradeResult>(url, request, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<AddressVerificationResult>> VerifyAddressAsync(AddressVerificationRequest request, CancellationToken cancellationToken = default)
    {
        Guard.ThrowIfNull(request);
        
        var url = BuildUrl(ServiceUrls.Base, "api/v1/customers/kyc/address-verification");
        return await HttpClient.PostAsync<AddressVerificationRequest, AddressVerificationResult>(url, request, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<IEnumerable<CustomerType>>> GetCustomerTypesAsync(CancellationToken cancellationToken = default)
    {
        var url = BuildUrl(ServiceUrls.Base, "api/v1/customers/types/all");
        var response = await HttpClient.GetAsync<List<CustomerType>>(url, cancellationToken);
        
        // Convert List<CustomerType> response to IEnumerable<CustomerType>
        return new ApiResponse<IEnumerable<CustomerType>>
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
    public async Task<ApiResponse<IEnumerable<Country>>> GetCountriesAsync(CancellationToken cancellationToken = default)
    {
        var url = BuildUrl(ServiceUrls.Base, "api/v1/customers/countries/get");
        var response = await HttpClient.GetAsync<List<Country>>(url, cancellationToken);
        
        // Convert List<Country> response to IEnumerable<Country>
        return new ApiResponse<IEnumerable<Country>>
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
    public async Task<ApiResponse<Customer>> UpdateCustomerTypeAsync(string customerId, string customerTypeId, CancellationToken cancellationToken = default)
    {
        Guard.ThrowIfNullOrWhiteSpace(customerId);
        Guard.ThrowIfNullOrWhiteSpace(customerTypeId);
        
        var url = BuildUrl(ServiceUrls.Base, $"api/v1/customers/customer/{customerId}/customertype/{customerTypeId}/update");
        return await HttpClient.PatchAsync<object, Customer>(url, new object(), cancellationToken);
    }

    /// <inheritdoc />
    public async Task<ApiResponse<Customer>> UpdateContactAsync(string customerId, UpdateCustomerContactRequest request, CancellationToken cancellationToken = default)
    {
        Guard.ThrowIfNullOrWhiteSpace(customerId);
        Guard.ThrowIfNull(request);
        
        var url = BuildUrl(ServiceUrls.Base, $"api/v1/customers/customer/{customerId}/updatecontact");
        return await HttpClient.PatchAsync<UpdateCustomerContactRequest, Customer>(url, request, cancellationToken);
    }
}