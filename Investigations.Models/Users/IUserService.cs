using Investigations.Models.Shared;

namespace Investigations.Models.Users;

public interface IUserService
{
    Task<MethodResponse<List<User>>> GetUsers();
    Task<MethodResponse<User>> GetUser(int userKey);
    Task<MethodResponse<User>> GetUserByEmail(string email);
    Task<MethodResponse<string>> GetPasswordHash(string email);
    Task<MethodResponse<User>> AddUser(string email, string passwordHash);
    Task<MethodResponse<int>> Save(User user, string passwordHash = "");
    Task<MethodResponse<bool>> UpdatePassword(int userKey, string passwordHash);
}
