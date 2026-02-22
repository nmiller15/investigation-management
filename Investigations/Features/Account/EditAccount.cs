using Investigations.Features.Auth;
using Investigations.Infrastructure.Data.Repositories;
using Investigations.Models;
using Serilog;

namespace Investigations.Features.Account;

public class EditAccount
{
    public record Query
    {
        public int UserKey { get; set; }

        public record Result
        {
            public string FirstName { get; set; } = string.Empty;
            public string LastName { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public DateTime Birthdate { get; set; } = DateTime.MinValue;
            public string Role { get; set; } = string.Empty;
        }
    }

    public record Command
    {
        public required int UserKey { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required DateTime Birthdate { get; set; }
    }

    public class Handler(UserRepository userRepository, CurrentUser currentUser)
    {
        private readonly UserRepository _userRepository = userRepository;
        private readonly CurrentUser _currentUser = currentUser;

        public async Task<MethodResponse<Query.Result>> Handle(Query query)
        {
            var user = await _userRepository.GetUser(query.UserKey);

            return MethodResponse<Query.Result>.Success(new Query.Result
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Birthdate = user.Birthdate.GetValueOrDefault(DateTime.MinValue),
                Role = user.Role.ToString()
            });
        }

        public async Task<MethodResponse<bool>> Handle(Command command)
        {
            var userKey = await _userRepository.UpdateUser(new User
            {
                UserKey = command.UserKey,
                FirstName = command.FirstName,
                LastName = command.LastName,
                Email = command.Email,
                Birthdate = command.Birthdate
            }, null, _currentUser.UserKey.GetValueOrDefault());

            return userKey > 0
                ? MethodResponse<bool>.Success(true)
                : MethodResponse<bool>.Failure("Failed to update account information.");
        }

        public bool UserCanEdit(int userKey)
        {
            var editingCurrentUser = _currentUser.IsAuthenticated && _currentUser.UserKey == userKey;
            return _currentUser.IsAuthenticated && (editingCurrentUser ||
                                                           _currentUser.IsAccountOwner() ||
                                                           _currentUser.IsSystemAdministrator());
        }
    }
}
