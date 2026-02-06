using System.Security.Cryptography;
using System.Text;

namespace SecureMed.SharedKernel.Infrastructure.Security;

public interface IEncryptionService
{
    string Encrypt(string plainText);
    string Decrypt(string cipherText);

    string CreateHash(string input);
}


/*
 * AES-256-CBC ENCRYPTION SERVICE (STATLESS)
 * ----------------------------------------
 * This service implements Data-at-Rest protection for PII/PHI.
 * * SECURITY IMPLEMENTATION:
 * - KEY: 256-bit master key derived via SHA256.
 * - ALGORITHM: AES (Advanced Encryption Standard) in CBC mode with PKCS7 padding.
 * - NON-DETERMINISTIC: Uses a unique 128-bit IV (Initialization Vector) per operation
 * via RandomNumberGenerator. This ensures identical inputs produce different ciphertexts.
 * - STORAGE: The 16-byte IV is prepended to the ciphertext.
 */
public class AesEncryptionService : IEncryptionService
{
    private readonly byte[] _key;

    public AesEncryptionService(string key)
    {
        // We verwachten een Base64 string van 32 bytes (256 bits)
        // Als je een kortere string hebt, hashen we deze naar 32 bytes
        _key  = System.Security.Cryptography.SHA256.HashData(Encoding.UTF8.GetBytes(key));
    }

    public string Encrypt(string plainText)
    {
        if (string.IsNullOrWhiteSpace(plainText)) return plainText;

        using var aes = Aes.Create();
        aes.Key = _key;

        // Genereer IV handmatig
        byte[] iv = RandomNumberGenerator.GetBytes(16);
        byte[] inputBytes = Encoding.UTF8.GetBytes(plainText);

        byte[] cipherText = aes.EncryptCbc(inputBytes, iv, PaddingMode.PKCS7);

        // Combineer IV + CipherText
        byte[] result = new byte[iv.Length + cipherText.Length];
        Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
        Buffer.BlockCopy(cipherText, 0, result, iv.Length, cipherText.Length);

        return Convert.ToBase64String(result);
    }

    public string Decrypt(string cipherText)
    {
        if (string.IsNullOrWhiteSpace(cipherText)) return cipherText;

        byte[] fullCipher = Convert.FromBase64String(cipherText);

        using var aes = Aes.Create();
        aes.Key = _key;

        byte[] iv = fullCipher.Take(16).ToArray();
        byte[] cipher = fullCipher.Skip(16).ToArray();

        // Statische decryptie methode
        byte[] plainBytes = aes.DecryptCbc(cipher, iv, PaddingMode.PKCS7);

        return Encoding.UTF8.GetString(plainBytes);
    }

    public string CreateHash(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return input;

        using var hmac = new HMACSHA256(_key);
        var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(input));
        return Convert.ToBase64String(hashBytes);
    }
}
