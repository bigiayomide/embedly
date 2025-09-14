using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Embedly.SDK.Models.Requests.Customers;
using Embedly.SDK.Models.Responses.Common;
using Embedly.SDK.Models.Responses.Customers;

namespace Embedly.SDK.Services.Customers;

/// <summary>
///     Interface for customer management operations.
/// </summary>
public interface ICustomerService
{
    /// <summary>
    ///     Creates a new customer profile.
    /// </summary>
    /// <param name="request">The customer creation request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The API response containing the created customer.</returns>
    Task<ApiResponse<Customer>> CreateAsync(CreateCustomerRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Retrieves a customer by their unique identifier.
    /// </summary>
    /// <param name="customerId">The customer identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The API response containing the customer if found.</returns>
    Task<ApiResponse<Customer>> GetByIdAsync(string customerId, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Retrieves all customers for the organization.
    /// </summary>
    /// <param name="page">The page number (1-based).</param>
    /// <param name="pageSize">The number of items per page.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The API response containing a list of customers.</returns>
    Task<ApiResponse<IEnumerable<Customer>>> GetAllAsync(int page = 1, int pageSize = 50,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Retrieves customers with advanced filtering options.
    /// </summary>
    /// <param name="request">The customer list request with filters.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The API response containing a list of customers.</returns>
    Task<ApiResponse<IEnumerable<Customer>>> GetAllAsync(GetCustomersRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Updates a customer's name.
    /// </summary>
    /// <param name="customerId">The customer identifier.</param>
    /// <param name="firstName">The new first name.</param>
    /// <param name="lastName">The new last name.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The API response containing the updated customer.</returns>
    Task<ApiResponse<Customer>> UpdateNameAsync(string customerId, string firstName, string lastName,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Updates a customer's name using request object.
    /// </summary>
    /// <param name="request">The update request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The API response containing the updated customer.</returns>
    Task<ApiResponse<Customer>> UpdateNameAsync(UpdateCustomerNameRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Gets customer verification properties and KYC status.
    /// </summary>
    /// <param name="customerId">The customer identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The API response containing the customer verification details.</returns>
    Task<ApiResponse<CustomerVerificationProperties>> GetVerificationPropertiesAsync(string customerId,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Upgrades customer KYC using NIN (National Identification Number).
    /// </summary>
    /// <param name="request">The NIN KYC upgrade request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The API response containing the KYC upgrade result.</returns>
    Task<ApiResponse<KycUpgradeResult>> UpgradeKycWithNinAsync(NinKycUpgradeRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Upgrades customer KYC using BVN (Bank Verification Number).
    /// </summary>
    /// <param name="request">The BVN KYC upgrade request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The API response containing the KYC upgrade result.</returns>
    Task<ApiResponse<KycUpgradeResult>> UpgradeKycWithBvnAsync(BvnKycUpgradeRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Completes address verification for a customer.
    /// </summary>
    /// <param name="request">The address verification request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The API response containing the address verification result.</returns>
    Task<ApiResponse<AddressVerificationResult>> VerifyAddressAsync(AddressVerificationRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Retrieves all available customer types.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The API response containing a list of customer types.</returns>
    Task<ApiResponse<IEnumerable<CustomerType>>> GetCustomerTypesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    ///     Retrieves all available countries.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The API response containing a list of countries.</returns>
    Task<ApiResponse<IEnumerable<Country>>> GetCountriesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    ///     Updates customer type for a specific customer.
    /// </summary>
    /// <param name="customerId">The customer identifier.</param>
    /// <param name="customerTypeId">The new customer type identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The API response containing the updated customer.</returns>
    Task<ApiResponse<Customer>> UpdateCustomerTypeAsync(string customerId, string customerTypeId,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Updates customer contact information.
    /// </summary>
    /// <param name="customerId">The customer identifier.</param>
    /// <param name="request">The contact update request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The API response containing the updated customer.</returns>
    Task<ApiResponse<Customer>> UpdateContactAsync(string customerId, UpdateCustomerContactRequest request,
        CancellationToken cancellationToken = default);
}