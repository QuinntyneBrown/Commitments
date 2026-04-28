namespace Identity.Security;

public interface IPasswordHasher
{
    (byte[] Hash, byte[] Salt) Hash(string password);
    bool Verify(string password, byte[] salt, byte[] hash);
}
