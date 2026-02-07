namespace Investigations.Models.Auth;

public interface IPasswordHasher
{
    string Hash(string password);
    VerifyResult Verify(string hash, string password);
}
