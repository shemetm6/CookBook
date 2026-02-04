using System.Text;
using System.Security.Cryptography;

namespace CookBook.Utils;

public static class PasswordHasher
{
    private const int HashingIterations = 100000;
    private const int HashSize = 32;
    private const int SaltSize = 16;

    private static readonly HashAlgorithmName _hashAlgorithm = HashAlgorithmName.SHA256;

    public static byte[] HashPassword(string password)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);

        byte[] hash = Rfc2898DeriveBytes
            .Pbkdf2(password, salt, HashingIterations, _hashAlgorithm, HashSize);

        return Encoding.UTF8.GetBytes($"{Convert.ToHexString(salt)}-{Convert.ToHexString(hash)}");
    }

    public static bool VerifyPassword(byte[] hashedPassword, string password)
    {
        var hashedPasswordString = Encoding.UTF8.GetString(hashedPassword);
        string[] parts = hashedPasswordString.Split('-');

        if (parts.Length != 2)
            throw new FormatException("Invalid hashed password format.");

        byte[] salt = Convert.FromHexString(parts[0]);
        byte[] hash = Convert.FromHexString(parts[1]);

        byte[] computedHash = Rfc2898DeriveBytes
            .Pbkdf2(password, salt, HashingIterations, _hashAlgorithm, HashSize);

        return CryptographicOperations.FixedTimeEquals(hash, computedHash);
    }
}
