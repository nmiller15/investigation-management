using Investigations.Configuration;
using Investigations.Infrastructure.Data.Parsers;

namespace Investigations.Infrastructure.Data.Repositories;

public class PasswordRepository(IConnectionStrings connectionStrings)
    : BaseSqlRepository(connectionStrings)
{
    private readonly StringParser _stringParser = new();

    public async Task<string> GetPasswordHash(string email)
    {
        var dcs = GetFunctionCallDcsInstance("get_user_password_hash");
        dcs.AddParameter("p_email", email);
        var passwordHash = await NpgsqlDataProvider.ExecuteFunctionScalar(dcs, _stringParser);
        return passwordHash;
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
