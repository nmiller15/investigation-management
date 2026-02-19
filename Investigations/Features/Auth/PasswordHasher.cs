using Microsoft.AspNetCore.Identity;
using Investigations.Models.Auth;
using Serilog;
using Investigations.Models;

namespace Investigations.Features.Auth;

public class PasswordHasher
{
    private readonly PasswordHasher<User> Hasher = new();

    public string Hash(string password)
        => Hasher.HashPassword(null!, password);

    public VerifyResult Verify(string hash, string password)
    {
        var result = Hasher.VerifyHashedPassword(null!, hash, password);
        Log.Debug("Compared {Hash} and {Password} and got result {Result}",
                hash,
                password,
                result);

        switch (result)
        {
            case PasswordVerificationResult.Success:
                return new VerifyResult { IsSuccess = true, RehashNeeded = false };
            case PasswordVerificationResult.SuccessRehashNeeded:
                return new VerifyResult { IsSuccess = true, RehashNeeded = true };
            default:
                return new VerifyResult { IsSuccess = false, RehashNeeded = false };
        }
    }
}
