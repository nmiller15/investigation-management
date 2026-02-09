namespace Investigations.Models.Users;

public interface IUserRepository
{
    Task<List<User>> GetUsers();
    Task<User> GetUser(int userKey);
    Task<User?> GetUserByEmail(string email);
    Task<string> GetPasswordHash(string email);
    Task<User> AddUser(string email, string passwordHash, int insertedByUserKey = 100);
    Task<int> Save(User user, string? passwordHash = null, int updatedByUserKey = 100);
    Task<bool> UpdatePassword(int userKey, string passwordHash, int updatedByUserKey = 100);
}
