using Investigations.Infrastructure.Data.Repositories;
using Investigations.Models;
using Investigations.Models.Users;

namespace Investigations.Features.Account;

public class ViewAccount
{
    public class Query
    {
        public int UserKey { get; set; }

        public class Result
        {
            public string FirstName { get; set; } = string.Empty;
            public string LastName { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public string Birthdate { get; set; } = string.Empty;
            public string AccountCreatedOn { get; set; } = string.Empty;
            public string LastUpdatedOn { get; set; } = string.Empty;
            public string Role { get; set; } = string.Empty;
        }
    }

    public class Handler(UserRepository userRepository)
    {
        private readonly UserRepository _userRepository = userRepository;

        public async Task<MethodResponse<Query.Result>> Handle(Query query)
        {
            var user = await _userRepository.GetUser(query.UserKey);

            if (user == null)
                return MethodResponse<Query.Result>.Failure("User not found.");

            return MethodResponse<Query.Result>.Success(new Query.Result
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                AccountCreatedOn = user.InsertedDateTime.ToString("MMMM dd, yyyy"),
                Birthdate = user.Birthdate.HasValue ? user.Birthdate.Value.ToString("MMMM dd, yyyy") : "N/A",
                LastUpdatedOn = user.UpdatedDateTime.HasValue ? user.UpdatedDateTime.Value.ToString("MMMM dd, yyyy hh:mm") : "N/A",
                Role = user.Role.ToDisplayString()
            });
        }
    }
}

