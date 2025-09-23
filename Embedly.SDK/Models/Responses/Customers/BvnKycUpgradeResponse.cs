using System;
using System.Text.Json.Serialization;

namespace Embedly.SDK.Models.Responses.Customers;

/// <summary>
///     Root object for the BVN KYC response payload.
/// </summary>
public class BvnKycUpgradeResponse
{
    /// <summary>
    /// Indicates whether KYC process was completed.
    /// </summary>
    [JsonPropertyName("kycCompleted")]
    public bool KycCompleted { get; set; }


    /// <summary>
    /// The detailed response object containing identity and verification data.
    /// </summary>
    [JsonPropertyName("response")]
    public BvnResultResponse? Response { get; set; }


    /// <summary>
    /// Indicates whether the overall operation was successful.
    /// </summary>
    [JsonPropertyName("successful")]
    public bool Successful { get; set; }
}

/// <summary>
///     The main response container.
/// </summary>
public class BvnResultResponse
{
    /// <summary>
    ///     Identifier for the response entry.
    /// </summary>
    [JsonPropertyName("id")]
    public long Id { get; set; }


    /// <summary>
    ///     Applicant personal information.
    /// </summary>
    [JsonPropertyName("applicant")]
    public Applicant? Applicant { get; set; }


    /// <summary>
    ///     Summary of checks performed (BVN, NIN, etc.).
    /// </summary>
    [JsonPropertyName("summary")]
    public Summary? Summary { get; set; }


    /// <summary>
    ///     Status object describing the processing state.
    /// </summary>
    [JsonPropertyName("status")]
    public Status? Status { get; set; }


    /// <summary>
    ///     BVN specific information returned from the enrolment/bvn lookup.
    /// </summary>
    [JsonPropertyName("bvn")]
    public BvnInfo? Bvn { get; set; }
}

/// <summary>
///     Applicant demographics.
/// </summary>
public class Applicant
{
    /// <summary>
    ///     Applicant's first name.
    /// </summary>
    [JsonPropertyName("firstname")]
    public string Firstname { get; set; } = string.Empty;

    /// <summary>
    ///     Applicant's last name.
    /// </summary>
    [JsonPropertyName("lastname")]
    public string Lastname { get; set; } = string.Empty;

    /// <summary>
    ///     Date of birth as returned by the provider (kept as string because provider formats vary).
    /// </summary>
    [JsonPropertyName("dob")]
    public DateTime? DateOfBirth { get; set; }
}

/// <summary>
///     Summary of verification checks.
/// </summary>
public class Summary
{
    /// <summary>
    ///     BVN check result (may be null if not performed).
    /// </summary>
    [JsonPropertyName("bvn_check")]
    public BvnCheck? BvnCheck { get; set; }

    /// <summary>
    ///     NIN check result (nullable — provider may return null).
    /// </summary>
    [JsonPropertyName("nin_check")]
    public NinCheck? NinCheck { get; set; }
}

/// <summary>
///     Result of a BVN check.
/// </summary>
public class BvnCheck
{
    /// <summary>
    /// Overall status of the BVN check (e.g. EXACT_MATCH).
    /// </summary>
    [JsonPropertyName("status")]
    public string? Status { get; set; }


    /// <summary>
    /// Field-level match results (firstname, lastname, etc.).
    /// </summary>
    [JsonPropertyName("fieldMatches")]
    public FieldMatches? FieldMatches { get; set; }
}

/// <summary>
///     Field-level boolean matches returned by BVN check.
/// </summary>
public class FieldMatches
{
    /// <summary>
    /// Whether the firstname matched the BVN data.
    /// </summary>
    [JsonPropertyName("firstname")]
    public bool? Firstname { get; set; }

    /// <summary>
    /// Whether the lastname matched the BVN data.
    /// </summary>
    [JsonPropertyName("lastname")]
    public bool? Lastname { get; set; }
}

/// <summary>
///     Placeholder for NIN check details.
/// </summary>
public class NinCheck
{
}

/// <summary>
///     Generic status object describing processing state and human-readable status.
/// </summary>
public class Status
{
    /// <summary>
    ///     Machine-readable state (e.g. "complete").
    /// </summary>
    [JsonPropertyName("state")]
    public string? State { get; set; }

    /// <summary>
    ///     Human-readable status (e.g. "verified").
    /// </summary>
    [JsonPropertyName("status")]
    public string? Description { get; set; }
}

/// <summary>
///     BVN information structure returned by the provider.
/// </summary>
public class BvnInfo
{
    /// <summary>
    /// The BVN number.
    /// </summary>
    [JsonPropertyName("bvn")]
    public string? Bvn { get; set; }

    /// <summary>
    /// First name on the BVN record.
    /// </summary>
    [JsonPropertyName("firstname")]
    public string? Firstname { get; set; }

    /// <summary>
    /// </summary>
    [JsonPropertyName("lastname")]
    public string? Lastname { get; set; }

    /// <summary>
    /// Middle name on the BVN record (may be empty string).
    /// </summary>
    [JsonPropertyName("middlename")]
    public string? Middlename { get; set; }

    /// <summary>
    /// Birthdate as returned by the provider (string format preserved).
    /// </summary>
    [JsonPropertyName("birthdate")]
    public string? Birthdate { get; set; }

    /// <summary>
    /// Gender (e.g. "Male").
    /// </summary>
    [JsonPropertyName("gender")]
    public string? Gender { get; set; }

    /// <summary>
    /// Phone number if provided; otherwise null.
    /// </summary>
    [JsonPropertyName("phone")]
    public string? Phone { get; set; }

    /// <summary>
    /// Photo as returned by provider (could be URL or base64 string).
    /// </summary>
    [JsonPropertyName("photo")]
    public string? Photo { get; set; }

    /// <summary>
    /// Local government area of residence.
    /// </summary>
    [JsonPropertyName("lga_of_residence")]
    public string? LgaOfResidence { get; set; }

    /// <summary>
    /// Marital status (e.g. "Single").
    /// </summary>
    [JsonPropertyName("marital_status")]
    public string? MaritalStatus { get; set; }

    /// <summary>
    /// Nationality (e.g. "Nigeria").
    /// </summary>
    [JsonPropertyName("nationality")]
    public string? Nationality { get; set; }

    /// <summary>
    /// Residential address as returned by the provider.
    /// </summary>
    [JsonPropertyName("residential_address")]
    public string? ResidentialAddress { get; set; }

    /// <summary>
    /// State of residence.
    /// </summary>
    [JsonPropertyName("state_of_residence")]
    public string? StateOfResidence { get; set; }

    /// <summary>
    /// Email address if available.
    /// </summary>
    [JsonPropertyName("email")]
    public string? Email { get; set; }

    /// <summary>
    /// Enrollment bank code (e.g. "011").
    /// </summary>
    [JsonPropertyName("enrollment_bank")]
    public string? EnrollmentBank { get; set; }

    /// <summary>
    /// Watchlisted indicator (e.g. "NO").
    /// </summary>
    [JsonPropertyName("watch_listed")]
    public string? WatchListed { get; set; }
}
