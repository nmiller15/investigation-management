using Investigations.Configuration;
using Investigations.Infrastructure.Data;
using Investigations.Infrastructure.Data.Parsers;
using Investigations.Models;

namespace Investigations.Features.Contacts;

public class CreateContact
{
    public class Command
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public string MobilePhone { get; set; } = string.Empty;
        public string WorkPhone { get; set; } = string.Empty;
        public string HomePhone { get; set; } = string.Empty;

        public record Result(int ContactKey);
    }

    public class Handler(IConnectionStrings connectionStrings)
    {
        public async Task<MethodResponse<Command.Result>> Handle(Command req)
        {
            var contact = new Contact()
                .Name(req.FirstName, req.LastName)
                .EmailAddress(req.Email)
                .MobilePhone(req.MobilePhone)
                .WorkPhone(req.WorkPhone)
                .HomePhone(req.HomePhone);

            var dcs = new DataCallSettings()
            {
                ConnectionString = connectionStrings.DefaultConnection,
                SqlQuery = @"INSERT INTO contacts (first_name, last_name, email, mobile_phone, work_phone, home_phone)
                             VALUES (@FirstName, @LastName, @Email, @MobilePhone, @WorkPhone, @HomePhone)
                             RETURNING contact_key;"
            };

            dcs.AddParameter("@FirstName", contact.FirstName);
            dcs.AddParameter("@LastName", contact.LastName);
            dcs.AddParameter("@Email", contact.Email?.Address ?? string.Empty);
            dcs.AddParameter("@MobilePhone", contact.PhoneNumbers.FirstOrDefault(p => p.Type == IContactMethod.Types.Mobile)?.Value ?? string.Empty);
            dcs.AddParameter("@WorkPhone", contact.PhoneNumbers.FirstOrDefault(p => p.Type == IContactMethod.Types.Work)?.Value ?? string.Empty);
            dcs.AddParameter("@HomePhone", contact.PhoneNumbers.FirstOrDefault(p => p.Type == IContactMethod.Types.Home)?.Value ?? string.Empty);

            var result = await NpgsqlDataProvider.ExecuteRaw(dcs, new IntParser());
            var contactKey = result.FirstOrDefault();

            return MethodResponse<Command.Result>.Success(new Command.Result(contactKey));
        }
    }

}
