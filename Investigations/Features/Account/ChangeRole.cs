using Investigations.Configuration;
using Investigations.Infrastructure.Data;
using Investigations.Infrastructure.Data.Repositories;
using Investigations.Models;
using Serilog;

namespace Investigations.Features.Account;

public class ChangeRole
{
    public record Command
    {
        public int UserKey { get; set; }
        public User.Roles Role { get; set; }
    }

    public class Handler(IConnectionStrings connectionStrings)
    {
        private readonly IConnectionStrings _connectionStrings = connectionStrings;

        public async Task<MethodResponse<bool>> Handle(Command command)
        {
            Log.Debug("Command received to change role for UserKey {UserKey} to Role {Role}", command.UserKey, command.Role);
            var dcs = new DataCallSettings()
            {
                ConnectionString = _connectionStrings.DefaultConnection,
                SqlQuery = """
                    UPDATE users
                    SET 
                        role_code_key = @role_code_key
                    WHERE 
                        user_key = @user_key;
                    """,
            };

            dcs.AddParameter("user_key", command.UserKey);
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
