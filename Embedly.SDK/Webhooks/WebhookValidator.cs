using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Embedly.SDK.Helpers;

namespace Embedly.SDK.Webhooks;

/// <summary>
///     Validates webhook signatures from Embedly.
/// </summary>
public sealed class WebhookValidator : IWebhookValidator
{
    private readonly string _webhookSecret;

    /// <summary>
    ///     Initializes a new instance of the WebhookValidator class.
    /// </summary>
    /// <param name="webhookSecret">The webhook secret key.</param>
    public WebhookValidator(string webhookSecret)
    {
        _webhookSecret = Guard.ThrowIfNullOrWhiteSpace(webhookSecret, nameof(webhookSecret));
    }

    /// <inheritdoc />
    public bool ValidateSignature(string payload, string signature)
    {
        if (string.IsNullOrWhiteSpace(payload))
            return false;

        if (string.IsNullOrWhiteSpace(signature))
            return false;

        var computedSignature = ComputeSignature(payload);
        return string.Equals(computedSignature, signature, StringComparison.OrdinalIgnoreCase);
    }

    /// <inheritdoc />
    public WebhookEvent? ParseEvent(string payload, string signature)
    {
        if (!ValidateSignature(payload, signature))
            throw new InvalidOperationException("Invalid webhook signature");

        try
        {
            return JsonSerializer.Deserialize<WebhookEvent>(payload, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
        catch (JsonException ex)
        {
            throw new InvalidOperationException("Failed to parse webhook event", ex);
        }
    }

    /// <inheritdoc />
    public T? ParseEventData<T>(string payload, string signature) where T : class
    {
        var webhookEvent = ParseEvent(payload, signature);
        return webhookEvent?.GetData<T>();
    }

    /// <summary>
    ///     Computes the HMAC signature for the given payload.
    /// </summary>
    /// <param name="payload">The webhook payload.</param>
    /// <returns>The computed signature.</returns>
    private string ComputeSignature(string payload)
    {
        var keyBytes = Encoding.UTF8.GetBytes(_webhookSecret);
        var payloadBytes = Encoding.UTF8.GetBytes(payload);

        using var hmac = new HMACSHA256(keyBytes);
        var hash = hmac.ComputeHash(payloadBytes);
        return Convert.ToHexString(hash).ToLowerInvariant();
    }
}

/// <summary>
///     Interface for webhook validation.
/// </summary>
public interface IWebhookValidator
{
    /// <summary>
    ///     Validates a webhook signature.
    /// </summary>
    /// <param name="payload">The webhook payload.</param>
    /// <param name="signature">The signature to validate.</param>
    /// <returns>True if the signature is valid, false otherwise.</returns>
    bool ValidateSignature(string payload, string signature);

    /// <summary>
    ///     Parses and validates a webhook event.
    /// </summary>
    /// <param name="payload">The webhook payload.</param>
    /// <param name="signature">The signature to validate.</param>
    /// <returns>The parsed webhook event.</returns>
    /// <exception cref="InvalidOperationException">Thrown when signature is invalid or parsing fails.</exception>
    WebhookEvent? ParseEvent(string payload, string signature);

    /// <summary>
    ///     Parses and validates webhook event data to a specific type.
    /// </summary>
    /// <typeparam name="T">The type to deserialize to.</typeparam>
    /// <param name="payload">The webhook payload.</param>
    /// <param name="signature">The signature to validate.</param>
    /// <returns>The parsed event data.</returns>
    /// <exception cref="InvalidOperationException">Thrown when signature is invalid or parsing fails.</exception>
    T? ParseEventData<T>(string payload, string signature) where T : class;
}