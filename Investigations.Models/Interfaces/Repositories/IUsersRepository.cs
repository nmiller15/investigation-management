namespace Investigations.Models.Interfaces.Repositories;

public interface IUsersRepository
{
    Task<List<User>> GetUsers();
    Task<User> GetUser(int userKey);
    Task<User> GetUserByEmail(string email);
}
