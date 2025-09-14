using System;
using System.Security.Cryptography;
using System.Text;

namespace Embedly.SDK.Utilities;

/// <summary>
///     Utility class for encrypting PINs using RSA encryption as required by the Embedly Cards API.
/// </summary>
public static class PinEncryption
{
    /// <summary>
    ///     Encrypts a user PIN using RSA encryption with PKCS#1 v1.5 padding.
    /// </summary>
    /// <param name="userPin">The user's PIN to encrypt.</param>
    /// <param name="publicKeyContent">The RSA public key in PEM format.</param>
    /// <returns>The encrypted PIN encoded as Base64.</returns>
    /// <exception cref="ArgumentNullException">Thrown when userPin or publicKeyContent is null.</exception>
    /// <exception cref="ArgumentException">Thrown when userPin or publicKeyContent is empty.</exception>
    /// <exception cref="CryptographicException">Thrown when encryption fails.</exception>
    public static string EncryptPin(string userPin, string publicKeyContent)
    {
        if (string.IsNullOrEmpty(userPin))
            throw new ArgumentException("User PIN cannot be null or empty.", nameof(userPin));

        if (string.IsNullOrEmpty(publicKeyContent))
            throw new ArgumentException("Public key content cannot be null or empty.", nameof(publicKeyContent));

        try
        {
            using var rsa = RSA.Create();

            // Import the RSA public key from PEM format
            rsa.ImportFromPem(publicKeyContent);

            // Convert PIN to UTF-8 bytes
            var dataBytes = Encoding.UTF8.GetBytes(userPin);

            // Encrypt using PKCS#1 v1.5 padding as required by the API
            var encrypted = rsa.Encrypt(dataBytes, RSAEncryptionPadding.Pkcs1);

            // Return as Base64 encoded string
            return Convert.ToBase64String(encrypted);
        }
        catch (Exception ex) when (!(ex is ArgumentException))
        {
            throw new CryptographicException("Failed to encrypt PIN. Ensure the public key is valid.", ex);
        }
    }

    /// <summary>
    ///     Validates that a PIN meets basic requirements before encryption.
    /// </summary>
    /// <param name="pin">The PIN to validate.</param>
    /// <returns>True if the PIN is valid, false otherwise.</returns>
    public static bool IsValidPin(string pin)
    {
        if (string.IsNullOrWhiteSpace(pin))
            return false;

        // PIN should be 4-6 digits
        if (pin.Length < 4 || pin.Length > 6)
            return false;

        // PIN should only contain digits
        foreach (var c in pin)
            if (!char.IsDigit(c))
                return false;

        return true;
    }
}