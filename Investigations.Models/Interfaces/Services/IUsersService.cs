using Investigations.App.Models;

namespace Investigations.Models.Interfaces.Services;

public interface IUsersService
{
    Task<MethodResponse<List<User>>> GetUsers();
    Task<MethodResponse<User>> GetUser(int userKey);
    Task<MethodResponse<User>> GetUserByEmail(string email);
}
