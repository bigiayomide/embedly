using System;
using System.Collections.Generic;

namespace Embedly.SDK.Exceptions;

/// <summary>
///     Exception thrown when webhook signature validation fails.
/// </summary>
[Serializable]
public class WebhookValidationException : EmbedlyException
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="WebhookValidationException" /> class.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="validationType">The type of validation that failed.</param>
    public WebhookValidationException(string message,
        WebhookValidationType validationType = WebhookValidationType.Signature)
        : base(message, "WEBHOOK_VALIDATION_ERROR", CreateErrorContext(validationType))
    {
        ValidationType = validationType;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="WebhookValidationException" /> class with an inner exception.
    /// </summary>
    /// <param name="message">The error message.</param>
    /// <param name="validationType">The type of validation that failed.</param>
    /// <param name="innerException">The inner exception.</param>
    public WebhookValidationException(
        string message,
        WebhookValidationType validationType,
        Exception innerException)
        : base(message, "WEBHOOK_VALIDATION_ERROR", CreateErrorContext(validationType), innerException)
    {
        ValidationType = validationType;
    }

    /// <summary>
    ///     Gets the type of webhook validation that failed.
    /// </summary>
    public WebhookValidationType ValidationType { get; }

    private static Dictionary<string, object?> CreateErrorContext(WebhookValidationType validationType)
    {
        return new Dictionary<string, object?>
        {
            ["ValidationType"] = validationType.ToString(),
            ["Timestamp"] = DateTimeOffset.UtcNow
        };
    }

    /// <summary>
    ///     Creates an exception for signature validation failures.
    /// </summary>
    public static WebhookValidationException InvalidSignature(string? details = null)
    {
        var message = "Webhook signature validation failed.";
        if (!string.IsNullOrEmpty(details)) message += $" Details: {details}";

        return new WebhookValidationException(message);
    }

    /// <summary>
    ///     Creates an exception for timestamp validation failures.
    /// </summary>
    public static WebhookValidationException InvalidTimestamp(DateTimeOffset webhookTime, TimeSpan tolerance)
    {
        return new WebhookValidationException(
            $"Webhook timestamp is outside the acceptable tolerance. Webhook time: {webhookTime:O}, Tolerance: {tolerance}",
            WebhookValidationType.Timestamp);
    }

    /// <summary>
    ///     Creates an exception for malformed webhook payloads.
    /// </summary>
    public static WebhookValidationException MalformedPayload(string? details = null)
    {
        var message = "Webhook payload is malformed or cannot be parsed.";
        if (!string.IsNullOrEmpty(details)) message += $" Details: {details}";

        return new WebhookValidationException(message, WebhookValidationType.Payload);
    }
}

/// <summary>
///     Defines the types of webhook validation.
/// </summary>
public enum WebhookValidationType
{
    /// <summary>
    ///     Signature validation.
    /// </summary>
    Signature,

    /// <summary>
    ///     Timestamp validation.
    /// </summary>
    Timestamp,

    /// <summary>
    ///     Payload structure validation.
    /// </summary>
    Payload
}