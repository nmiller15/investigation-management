using System.Net.Mail;

namespace Investigations.Models;

public class EmailAddress(string address) : MailAddress(address), IContactMethod
{
    public IContactMethod.Types Type => IContactMethod.Types.Email;
    public bool IsPrimary { get; set; }
}
