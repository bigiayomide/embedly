using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Embedly.SDK.Configuration;

/// <summary>
///     Configuration options for the Embedly SDK.
/// </summary>
public sealed class EmbedlyOptions
{
    /// <summary>
    ///     Configuration section name for binding from appsettings.
    /// </summary>
    public const string SectionName = "Embedly";

    /// <summary>
    ///     Gets or sets the API key for authentication.
    /// </summary>
    [Required(ErrorMessage = "API key is required")]
    public string ApiKey { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the target environment.
    /// </summary>
    public EmbedlyEnvironment Environment { get; set; } = EmbedlyEnvironment.Staging;

    /// <summary>
    ///     Gets or sets the HTTP request timeout.
    /// </summary>
    public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(30);

    /// <summary>
    ///     Gets or sets the number of retry attempts for failed requests.
    /// </summary>
    [Range(0, 10, ErrorMessage = "Retry count must be between 0 and 10")]
    public int RetryCount { get; set; } = 3;

    /// <summary>
    ///     Gets or sets the base delay for exponential backoff retry policy.
    /// </summary>
    public TimeSpan RetryBaseDelay { get; set; } = TimeSpan.FromSeconds(1);

    /// <summary>
    ///     Gets or sets whether to enable request/response logging.
    /// </summary>
    public bool EnableLogging { get; set; } = true;

    /// <summary>
    ///     Gets or sets whether to log request/response bodies (may contain sensitive data).
    /// </summary>
    public bool LogRequestBodies { get; set; } = false;

    /// <summary>
    ///     Gets or sets the webhook configuration options.
    /// </summary>
    public WebhookOptions Webhooks { get; set; } = new();

    /// <summary>
    ///     Gets or sets custom service URLs. If null, URLs are determined by Environment property.
    /// </summary>
    public ServiceUrls? CustomServiceUrls { get; set; }

    /// <summary>
    ///     Gets or sets the RSA public key for PIN encryption in PEM format.
    ///     Required for card operations that involve PIN encryption.
    ///     Contact your Embedly administrator to obtain the latest RSA public key.
    /// </summary>
    public string? RsaPublicKey { get; set; }

    /// <summary>
    ///     Organisation Id
    /// </summary>
    public string? OrganizationId { get; set; }

    /// <summary>
    ///     Gets the service URLs based on the configured environment or custom URLs.
    /// </summary>
    public ServiceUrls GetServiceUrls()
    {
        return CustomServiceUrls ?? Environment switch
        {
            EmbedlyEnvironment.Production => ServiceUrls.Production,
            _ => ServiceUrls.Staging
        };
    }

    /// <summary>
    ///     Validates the configuration options.
    /// </summary>
    public void Validate()
    {
        var results = new List<ValidationResult>();
        var context = new ValidationContext(this);

        if (!Validator.TryValidateObject(this, context, results, true))
        {
            var errors = string.Join("; ", results.Select(r => r.ErrorMessage));
            throw new InvalidOperationException($"Invalid Embedly configuration: {errors}");
        }

        if (string.IsNullOrWhiteSpace(ApiKey))
            throw new InvalidOperationException("Embedly API key cannot be empty or whitespace");

        // Validate Timeout separately since Range attribute doesn't work with TimeSpan
        if (Timeout.TotalSeconds < 5 || Timeout.TotalSeconds > 300)
            throw new InvalidOperationException("Timeout must be between 5 and 300 seconds");
    }
}