using Investigations.Infrastructure.Data.Parsers;
using Investigations.Models.Configuration;
using Investigations.Models.Users;

namespace Investigations.Infrastructure.Data.Repositories;

public class UserRepository : BaseSqlRepository, IUserRepository
{
    private readonly UserParser _userParser = new();
    private readonly IntParser _intParser = new();
    private readonly StringParser _stringParser = new();

    public UserRepository(IConnectionStrings connectionStrings) : base(connectionStrings)
    { }

    public async Task<List<User>> GetUsers()
    {
        var dcs = GetFunctionCallDcsInstance("get_users");
        var records = await NpgsqlDataProvider.ExecuteFunction(dcs, _userParser);
        return records;
    }

    public async Task<User> GetUser(int userKey)
    {
        var dcs = GetFunctionCallDcsInstance("get_user_by_key");
        dcs.AddParameter("p_user_key", userKey);
        var records = await NpgsqlDataProvider.ExecuteFunction(dcs, _userParser);
        return records.FirstOrDefault() ?? new();
    }

    public async Task<User?> GetUserByEmail(string email)
    {
        var dcs = GetFunctionCallDcsInstance("get_user_by_email");
        dcs.AddParameter("p_email", email);
        var records = await NpgsqlDataProvider.ExecuteFunction(dcs, _userParser);
        return records.Count != 0
            ? records.First()
            : null;
    }

    public async Task<string> GetPasswordHash(string email)
    {
        var dcs = GetFunctionCallDcsInstance("get_user_password_hash");
        dcs.AddParameter("p_email", email);
        var passwordHash = await NpgsqlDataProvider.ExecuteFunctionScalar(dcs, _stringParser);
        return passwordHash;
    }

    public async Task<User> AddUser(string email, string passwordHash, int insertedByUserKey = 100)
    {
        var dcs = GetFunctionCallDcsInstance("add_user");
        dcs.AddParameter("p_email", email);
        dcs.AddParameter("p_password_hash", passwordHash);
        dcs.AddParameter("p_inserted_by_user_key", 100);
        var userId = await NpgsqlDataProvider.ExecuteFunctionScalar(dcs, _intParser);

        return await GetUser(userId);
    }

    public async Task<int> Save(User user, string? passwordHash = null, int updatedByUserKey = 100)
    {
        if (user.IsNew && string.IsNullOrEmpty(passwordHash))
            throw new InvalidDataException("Cannot create new user without password hash.");

        if (user.IsNew)
        {
            var newUser = await AddUser(user.Email, passwordHash!);
            user.UserKey = newUser.UserKey;
            user.UpdatedByUserKey = newUser.UserKey;
        }

        var dcs = GetFunctionCallDcsInstance("update_user");
        dcs.AddParameter("p_user_key", user.UserKey);
        dcs.AddParameter("p_first_name", user.FirstName);
        dcs.AddParameter("p_last_name", user.LastName);
        dcs.AddParameter("p_email", user.Email);
        dcs.AddParameter("p_birthdate", user.Birthdate!, typeof(DateTime));
        dcs.AddParameter("p_role_code_key", (int)user.Role);
        dcs.AddParameter("p_updated_by_user_key", updatedByUserKey);

        var userId = await NpgsqlDataProvider.ExecuteFunctionScalar(dcs, _intParser);

        return userId;
    }

    public async Task<bool> UpdatePassword(int userKey, string passwordHash, int updatedByUserKey = 100)
    {
        var dcs = GetFunctionCallDcsInstance("update_user_password");
        dcs.AddParameter("p_user_key", userKey);
        dcs.AddParameter("p_password_hash", passwordHash);
        dcs.AddParameter("p_updated_by_user_key", updatedByUserKey);
        var rows = await NpgsqlDataProvider.ExecuteFunctionNonQuery(dcs);
        return rows > 0;
    }
}
