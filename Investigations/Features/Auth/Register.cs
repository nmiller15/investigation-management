using Investigations.Infrastructure.Data.Repositories;
using Investigations.Models;
using Serilog;

namespace Investigations.Features.Auth;

public class Register
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

    public class Handler(UserRepository userRepository, PasswordHasher hasher)
    {
        private readonly UserRepository _userRepository = userRepository;
        private readonly PasswordHasher _hasher = hasher;

        public async Task<MethodResponse<Result>> Handle(Command command)
        {
            var email = command.Email;
            var password = command.Password;

            if (await CheckUserExists(email))
                return MethodResponse<Result>.Failure("Invalid email and password combination.");

            var hash = _hasher.Hash(password);
            var userKey = await _userRepository.AddUser(email, hash);
            if (userKey <= 0)
            {
                Log.Error("Could not create user with email {Email}", email);
                return MethodResponse<Result>.Failure("Could not create user.");
            }

            Log.Information("Created user with email {Email}", email);

            var user = await _userRepository.GetUser(userKey);

            return MethodResponse<Result>.Success(new Result
            {
                NameIdentifierClaimValue = user.UserKey.ToString(),
                EmailClaimValue = user.Email,
                RoleClaimValue = user.Role.ToString(),
                NameClaimValue = user.Email,
                UserInformationIsComplete = false,
                UserKey = user.UserKey
            });
        }

        public async Task<bool> CheckUserExists(string email)
        {
            var user = await _userRepository.GetUserByEmail(email);
            return user.UserKey > 0;
        }
    }
}
