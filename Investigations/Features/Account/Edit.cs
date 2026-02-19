using Investigations.Features.Auth;
using Investigations.Infrastructure.Data.Repositories;

namespace Investigations.Features.Account;

public class Edit
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
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string Birthdate { get; set; }

        public record Result
        {

        }
    }

    public class Handler(UserRepository userRepository, CurrentUser currentUser)
    {
        private readonly UserRepository _userRepository = userRepository;

        public async Task<Query.Result> Handle(Query query)
        {
            var user = await _userRepository.GetUser(query.UserKey);

            return new Query.Result
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Birthdate = user.Birthdate.GetValueOrDefault(DateTime.MinValue),
                Role = user.Role.ToString()
            };
        }

        public async Task<Command.Result> Handle(Command command)
        {
            throw new NotImplementedException();
        }
    }
}
