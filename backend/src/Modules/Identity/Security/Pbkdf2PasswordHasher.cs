using System.Security.Cryptography;

namespace Identity.Security;

public class Pbkdf2PasswordHasher : IPasswordHasher
{
    private const int SaltSize = 16;
    private const int HashSize = 32;
    private const int Iterations = 100_000;

    public (byte[] Hash, byte[] Salt) Hash(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(SaltSize);
        return (Derive(password, salt), salt);
    }

    public bool Verify(string password, byte[] salt, byte[] hash)
        => CryptographicOperations.FixedTimeEquals(Derive(password, salt), hash);

    private static byte[] Derive(string password, byte[] salt)
    {
        using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256);
        return pbkdf2.GetBytes(HashSize);
    }
}
