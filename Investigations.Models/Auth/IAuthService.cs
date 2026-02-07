using Investigations.Models.Shared;
using Investigations.Models.Users;

namespace Investigations.Models.Auth;

public interface IAuthService
{
    Task<MethodResponse<UserIdentity>> Register(string email, string password);
    Task<MethodResponse<UserIdentity>> Login(string email, string password);
    Task<MethodResponse<bool>> ResetPassword(string passwordHash, UserIdentity user);
    Task<MethodResponse<bool>> CheckPassword(int userKey, string password);
    Task<MethodResponse<bool>> UpdatePassword(int userKey, string newPassword);
}
