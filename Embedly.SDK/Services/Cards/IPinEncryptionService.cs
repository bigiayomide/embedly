namespace Embedly.SDK.Services.Cards;

/// <summary>
/// Interface for PIN encryption service used by card operations.
/// </summary>
public interface IPinEncryptionService
{
    /// <summary>
    /// Gets the current RSA public key for PIN encryption.
    /// </summary>
    /// <returns>The RSA public key in PEM format.</returns>
    string GetRsaPublicKey();
    
    /// <summary>
    /// Encrypts a PIN using the current RSA public key.
    /// </summary>
    /// <param name="plainTextPin">The plain text PIN to encrypt.</param>
    /// <returns>The encrypted PIN as Base64 string.</returns>
    string EncryptPin(string plainTextPin);
}