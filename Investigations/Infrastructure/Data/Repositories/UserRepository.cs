using Investigations.Infrastructure.Data.Parsers;
using Investigations.Models;
using Investigations.Configuration;

namespace Investigations.Infrastructure.Data.Repositories;

public class UserRepository : BaseSqlRepository
{
    private readonly UserParser _userParser = new();
    private readonly IntParser _intParser = new();

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

    public async Task<User> GetUserByEmail(string email)
    {
        var dcs = GetFunctionCallDcsInstance("get_user_by_email");
        dcs.AddParameter("p_email", email);
        var records = await NpgsqlDataProvider.ExecuteFunction(dcs, _userParser);
        return records.FirstOrDefault() ?? new User();
    }

    public async Task<int> AddUser(string email, string passwordHash)
    {
        var dcs = GetFunctionCallDcsInstance("add_user");
        dcs.AddParameter("p_email", email);
        dcs.AddParameter("p_password_hash", passwordHash);
        dcs.AddParameter("p_inserted_by_user_key", 100);
        var userKey = await NpgsqlDataProvider.ExecuteFunctionScalar(dcs, _intParser);

        return userKey;
    }

    public async Task<int> UpdateUser(User user, string? passwordHash = null, int updatedByUserKey = 100)
    {
        if (user.IsNew && string.IsNullOrEmpty(passwordHash))
            throw new InvalidDataException("Cannot create new user without password hash.");

        if (user.IsNew)
        {
            var newUserKey = await AddUser(user.Email, passwordHash!);
            user.UserKey = newUserKey;
            user.UpdatedByUserKey = newUserKey;
        }

        var dcs = GetFunctionCallDcsInstance("update_user");
        dcs.AddParameter("p_user_key", user.UserKey);
        dcs.AddParameter("p_first_name", user.FirstName);
        dcs.AddParameter("p_last_name", user.LastName);
        dcs.AddParameter("p_email", user.Email);
        dcs.AddParameter("p_birthdate", user.Birthdate!, typeof(DateTime));
        dcs.AddParameter("p_role_code_key", (int)user.Role);
        dcs.AddParameter("p_updated_by_user_key", updatedByUserKey);

        var userKey = await NpgsqlDataProvider.ExecuteFunctionScalar(dcs, _intParser);

        return userKey;
    }

}
