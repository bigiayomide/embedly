using System;
using System.Text.Json.Serialization;

namespace Embedly.SDK.Models.Responses.Checkout;

/// <summary>
///     Represents how a checkout wallet's payment is allocated to a single split beneficiary.
/// </summary>
public sealed class CheckoutSplitConfiguration
{
    /// <summary>
    ///     Gets or sets the split configuration ID.
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    /// <summary>
    ///     Gets or sets the beneficiary ID.
    /// </summary>
    [JsonPropertyName("beneficiaryId")]
    public Guid BeneficiaryId { get; set; }

    /// <summary>
    ///     Gets or sets the amount or percentage allocated to the beneficiary, depending on the split type.
    /// </summary>
    [JsonPropertyName("splitValue")]
    public decimal SplitValue { get; set; }

    /// <summary>
    ///     Gets or sets the fee amount to be deducted.
    /// </summary>
    [JsonPropertyName("feeValue")]
    public decimal FeeValue { get; set; }

    /// <summary>
    ///     Gets or sets whether this beneficiary bears the transaction fees.
    /// </summary>
    [JsonPropertyName("feeBearer")]
    public bool FeeBearer { get; set; }

    /// <summary>
    ///     Gets or sets the beneficiary details.
    /// </summary>
    [JsonPropertyName("beneficiary")]
    public SplitBeneficiary? Beneficiary { get; set; }
}
