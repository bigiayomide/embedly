using System;
using Microsoft.Extensions.Options;
using Embedly.SDK.Configuration;
using Embedly.SDK.Utilities;

namespace Embedly.SDK.Services.Cards;

/// <summary>
/// Service for encrypting PINs for card operations.
/// </summary>
internal sealed class PinEncryptionService : IPinEncryptionService
{
    private readonly EmbedlyOptions _options;

    /// <summary>
    /// Initializes a new instance of the <see cref="PinEncryptionService"/> class.
    /// </summary>
    /// <param name="options">The Embedly configuration options.</param>
    public PinEncryptionService(IOptions<EmbedlyOptions> options)
    {
        _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
    }

    /// <inheritdoc />
    public string GetRsaPublicKey()
    {
        // TODO: In production, this should be retrieved from configuration or a secure key management service
        // For now, return the public key from options or throw if not configured
        
        if (string.IsNullOrEmpty(_options.RsaPublicKey))
        {
            throw new InvalidOperationException(
                "RSA public key is not configured. Please set the RsaPublicKey in EmbedlyOptions. " +
                "Contact your Embedly administrator to obtain the latest RSA public key for PIN encryption.");
        }

        return _options.RsaPublicKey;
    }

    /// <inheritdoc />
    public string EncryptPin(string plainTextPin)
    {
        if (string.IsNullOrWhiteSpace(plainTextPin))
        {
            throw new ArgumentException("PIN cannot be null or empty.", nameof(plainTextPin));
        }

        if (!PinEncryption.IsValidPin(plainTextPin))
        {
            throw new ArgumentException("PIN must be 4-6 digits.", nameof(plainTextPin));
        }

        var publicKey = GetRsaPublicKey();
        return PinEncryption.EncryptPin(plainTextPin, publicKey);
    }
}