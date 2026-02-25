using System.Data;
using Investigations.Configuration;
using Investigations.Features.Auth;
using Investigations.Infrastructure.Data;
using Investigations.Infrastructure.Data.Extensions;
using Investigations.Infrastructure.Data.Parsers;
using Investigations.Infrastructure.Data.Repositories;
using Investigations.Models;
using Investigations.Models.Data;
using Serilog;

namespace Investigations.Features.Account;

public class ChangeRole
{
    public record Query
    {
        public int UserKey { get; set; }

        public record Result
        {
            public User.Roles Role { get; set; }
            public string Name { get; set; } = string.Empty;
        }
    }

    public record Command
    {
        public int UserKey { get; set; }
        public User.Roles Role { get; set; }
    }

    public class Handler(IConnectionStrings connectionStrings, CurrentUser currentUser)
    {
        private readonly IConnectionStrings _connectionStrings = connectionStrings;

        public async Task<MethodResponse<Query.Result>> Handle(Query query)
        {
            Log.Debug("Query received to get role for UserKey {UserKey}", query.UserKey);
            var dcs = new DataCallSettings()
            {
                ConnectionString = _connectionStrings.DefaultConnection,
                SqlQuery = """
                    SELECT first_name, last_name, role_code_key
                    FROM users
                    WHERE user_key = @user_key;
                    """,
            };

            dcs.AddParameter("user_key", query.UserKey);

            try
            {
                var response = await NpgsqlDataProvider.ExecuteRaw(dcs, new ChangeRoleQueryResultParser());
                var result = response.FirstOrDefault();
                return result != null
                    ? MethodResponse<Query.Result>.Success(result)
                    : MethodResponse<Query.Result>.Failure("User not found or has no role assigned.");
            }
            catch (Exception ex)
            {
                ex.Data.Add("UserKey", query.UserKey);
                Log.Error(ex, "Error retrieving user role for UserKey {UserKey}", query.UserKey);
                return MethodResponse<Query.Result>.Failure("An error occurred while retrieving the user role.");
            }
        }

        public async Task<MethodResponse<bool>> Handle(Command command)
        {
            Log.Debug("Command received to change role for UserKey {UserKey} to Role {Role}", command.UserKey, command.Role);
            var dcs = new DataCallSettings()
            {
                ConnectionString = _connectionStrings.DefaultConnection,
                SqlQuery = """
                    UPDATE users
                    SET 
                        role_code_key = @role_code_key,
                        updated_by_user_key = @updated_by_user_key,
                        updated_datetime = NOW()
                    WHERE 
                        user_key = @user_key;
                    """,
            };

            dcs.AddParameter("user_key", command.UserKey);
            dcs.AddParameter("updated_by_user_key", currentUser.UserKey);
            dcs.AddParameter("role_code_key", (int)command.Role);

            try
            {
                await NpgsqlDataProvider.ExecuteRawNonQuery(dcs);
                return MethodResponse<bool>.Success(true, "User role updated successfully.");
            }
            catch (Exception ex)
            {
                ex.Data.Add("UserKey", command.UserKey);
                Log.Error(ex, "Error updating user role for UserKey {UserKey}", command.UserKey);
            }

            return MethodResponse<bool>.Failure("An error occurred while updating the user role.");
        }
    }
}

public class ChangeRoleQueryResultParser : ISqlDataParser<ChangeRole.Query.Result>
{
    public ChangeRole.Query.Result Model { get; set; } = new ChangeRole.Query.Result();

    public ChangeRole.Query.Result Parse(IDataReader reader)
    {
        Model.Name = $"{reader.ParseString("first_name")} {reader.ParseString("last_name")}";
        Model.Role = (User.Roles)reader.ParseInt32("role_code_key");

        return Model;
    }
}
