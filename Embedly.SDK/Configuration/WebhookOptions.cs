using System;

namespace Embedly.SDK.Configuration;

/// <summary>
///     Configuration options for webhook handling.
/// </summary>
public sealed class WebhookOptions
{
    /// <summary>
    ///     Gets or sets whether to validate webhook signatures.
    /// </summary>
    public bool ValidateSignature { get; set; } = true;

    /// <summary>
    ///     Gets or sets the tolerance window for webhook timestamp validation.
    /// </summary>
    public TimeSpan TimestampTolerance { get; set; } = TimeSpan.FromMinutes(5);

    /// <summary>
    ///     Gets or sets whether to automatically retry failed webhook processing.
    /// </summary>
    public bool EnableRetry { get; set; } = true;

    /// <summary>
    ///     Gets or sets the maximum number of retry attempts for webhook processing.
    /// </summary>
    public int MaxRetryAttempts { get; set; } = 3;

    /// <summary>
    ///     Gets or sets the delay between retry attempts.
    /// </summary>
    public TimeSpan RetryDelay { get; set; } = TimeSpan.FromSeconds(2);
}