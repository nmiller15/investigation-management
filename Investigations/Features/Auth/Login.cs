using Investigations.Infrastructure.Data.Repositories;
using Investigations.Models;
using Serilog;

namespace Investigations.Features.Auth;

public class Login
{
    public record Command
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }

    public record Result
    {
        public string NameIdentifierClaimValue { get; set; } = string.Empty;
        public string EmailClaimValue { get; set; } = string.Empty;
        public string RoleClaimValue { get; set; } = string.Empty;
        public string NameClaimValue { get; set; } = string.Empty;
        public bool UserInformationIsComplete { get; set; } = false;
        public int UserKey { get; set; }
    }

    public class Handler(UserRepository userRepository,
                         PasswordRepository passwordRepository,
                         PasswordHasher hasher)
    {

        private readonly UserRepository _userRepository = userRepository;
        private readonly PasswordRepository _passwordRepository = passwordRepository;
        private readonly PasswordHasher _hasher = hasher;

        public async Task<MethodResponse<Result>> Handle(Command command)
        {
            var email = command.Email;
            var password = command.Password;

            Log.Debug($"{nameof(Login)}({email}, {password})");

            const string failureMessage = "Invalid email and password combination.";

            var user = await _userRepository.GetUserByEmail(email);
            var userExists = !user.IsNew;
            var hash = await _passwordRepository.GetPasswordHash(email);

            if (!userExists || string.IsNullOrEmpty(hash))
            {
                var description = userExists
                        ? "could not get hash"
                        : "no user exists";

                Log.Warning("Failed login attempt for email {Email}, {Description}", email, description);
                Log.Debug("Received {UserResult} and {HashResult}", userExists, string.IsNullOrEmpty(hash) ? "empty hash" : "hash retrieved");
                return MethodResponse<Result>.Failure(failureMessage);
            }

            var verifyResult = _hasher.Verify(hash, password);
            if (!verifyResult.IsSuccess)
            {
                Log.Warning("Failed login attempt for email {Email}, invalid password", email);
                return MethodResponse<Result>.Failure(failureMessage);
            }

            if (verifyResult.RehashNeeded)
            {
                var newHash = _hasher.Hash(password);
                var updateSuccess = await _passwordRepository.UpdatePassword(user.UserKey, newHash);
                if (!updateSuccess)
                    Log.Error("Unable to update password hash for user {Email} after successful login.", email);
            }

            return MethodResponse<Result>.Success(new Result
            {
                NameIdentifierClaimValue = user.UserKey.ToString(),
                EmailClaimValue = user.Email,
                RoleClaimValue = user.Role.ToString(),
                NameClaimValue = user.FirstName + " " + user.LastName,
                UserInformationIsComplete = string.IsNullOrEmpty(user.Email),
                UserKey = user.UserKey
            });
        }
    }
}

