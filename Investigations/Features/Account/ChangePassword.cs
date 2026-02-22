using Investigations.Configuration;
using Investigations.Features.Auth;
using Investigations.Infrastructure.Data;
using Investigations.Infrastructure.Data.Parsers;
using Investigations.Infrastructure.Data.Repositories;
using Investigations.Models;
using Serilog;

namespace Investigations.Features.Account;

public class ChangePassword
{
    public class Command
    {
        public int UserKey { get; set; }
        public string Email { get; set; } = string.Empty;
        public string CurrentPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }

    public class Handler(
            PasswordRepository passwordRepository,
            PasswordHasher passwordHasher,
            IConnectionStrings connectionStrings,
            CurrentUser currentUser)
    {
        private readonly PasswordRepository _passwordRepository = passwordRepository;
        private readonly PasswordHasher _passwordHasher = passwordHasher;
        private readonly IConnectionStrings _connectionStrings = connectionStrings;
        private readonly CurrentUser _currentUser = currentUser;

        public async Task<MethodResponse<bool>> Handle(Command command)
        {
            var hash = await GetPasswordHash(command.UserKey);
            if (string.IsNullOrEmpty(hash))
                return MethodResponse<bool>.Failure("User not found.");

            var verifyResult = _passwordHasher.Verify(hash, command.CurrentPassword);
            if (!verifyResult.IsSuccess)
                return MethodResponse<bool>.Failure("Current password is incorrect.");

            var newHash = _passwordHasher.Hash(command.NewPassword);
            await _passwordRepository.UpdatePassword(command.UserKey,
                    newHash,
                    _currentUser.UserKey.GetValueOrDefault());

            return MethodResponse<bool>.Success(true, "Password changed successfully.");
        }

        private async Task<string> GetPasswordHash(int userKey)
        {
            var dcs = new DataCallSettings()
            {
                ConnectionString = _connectionStrings.DefaultConnection,
                FunctionName = "get_user_password_hash_by_user_key",
            };
            dcs.AddParameter("p_user_key", userKey);

            var passwordHash = await NpgsqlDataProvider.ExecuteFunctionScalar(dcs, new StringParser());

            return passwordHash;
        }
    }
}

