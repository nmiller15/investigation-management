using Investigations.Features.Auth;
using Investigations.Infrastructure.Data.Repositories;
using Investigations.Models;
using Investigations.Models.Users;

namespace Investigations.Features.Account;

public class ListAccounts
{
    public record Query
    {
        public string SortColumn { get; set; } = "LastName";
        public string SortDirection { get; set; } = "ASC";

        public record Result
        {
            public int CurrentUserKey { get; set; }
            public bool IsAuthenticated { get; set; }
            public bool CanViewAccountList { get; set; }
            public List<Account> Accounts { get; set; } = [];
        }

    }


    public class Handler(UserRepository userRepository, CurrentUser currentUser)
    {
        public async Task<MethodResponse<Query.Result>> Handle(Query request)
        {
            var users = await userRepository.GetUsers();

            users = request.SortColumn.ToLower() switch
            {
                "lastname" => request.SortDirection.Equals("ASC", StringComparison.OrdinalIgnoreCase)
                    ? users.OrderBy(u => u.LastName).ThenBy(u => u.FirstName).ToList()
                    : users.OrderByDescending(u => u.LastName).ThenByDescending(u => u.FirstName).ToList(),

                "email" => request.SortDirection.Equals("ASC", StringComparison.OrdinalIgnoreCase)
                    ? users.OrderBy(u => u.Email).ToList()
                    : users.OrderByDescending(u => u.Email).ToList(),

                "role" => request.SortDirection.Equals("ASC", StringComparison.OrdinalIgnoreCase)
                    ? users.OrderBy(u => u.Role).ToList()
                    : users.OrderByDescending(u => u.Role).ToList(),
                _ => users.OrderBy(u => u.LastName).ThenBy(u => u.FirstName).ToList()
            };

            return MethodResponse<Query.Result>.Success(new Query.Result
            {
                CurrentUserKey = currentUser.UserKey.GetValueOrDefault(),
                Accounts = users.Select(Account.From).ToList(),
            });
        }
    }

    public record Account
    {
        public int UserKey { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;

        public static Account From(User user)
        {
            return new Account
            {
                UserKey = user.UserKey,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Role = user.Role.ToDisplayString()
            };
        }
    }
}
