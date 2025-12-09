using System;
using System.Text.Json.Serialization;

namespace Embedly.SDK.Models.Responses.CorporateCustomers;

/// <summary>
///     Represents the response returned from add director to corporate customer request.
/// </summary>
public class AddDirectorResponse
{
    /// <summary>
    ///     Gets or sets the director Id.
    /// </summary>
    [JsonPropertyName("directorId")]
    public Guid DirectorId { get; set; }
    /// <summary>
    ///     Gets or sets the customer Id.
    /// </summary>
    [JsonPropertyName("customerId")]
    public  Guid CustomerId { get; set; }
}