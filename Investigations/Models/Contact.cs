namespace Investigations.Models;

public class Contact : BaseAuditModel
{
    public int ContactKey { get; set; } = 0;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public EmailAddress? Email { get; set; }
    public List<PhoneNumber> PhoneNumbers { get; set; } = [];
    public List<string> Notes { get; set; } = [];

    public bool IsNew => ContactKey == 0;
    public IContactMethod.Types? PrimaryPhoneType => PhoneNumbers.FirstOrDefault(p => p.IsPrimary)?.Type;

    public Contact Name(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
        return this;
    }

    public Contact EmailAddress(string email)
    {
        Email = new EmailAddress(email);
        return this;
    }

    public Contact MobilePhone(string mobilePhone)
    {
        var phone = PhoneNumbers.FirstOrDefault(p => p.Type == IContactMethod.Types.Mobile);
        if (phone != null)
        {
            phone.Value = mobilePhone;
        }
        else
        {
            PhoneNumbers.Add(new PhoneNumber(mobilePhone, IContactMethod.Types.Mobile));
        }
        return this;
    }

    public Contact WorkPhone(string workPhone)
    {
        var phone = PhoneNumbers.FirstOrDefault(p => p.Type == IContactMethod.Types.Work);
        if (phone != null)
        {
            phone.Value = workPhone;
        }
        else
        {
            PhoneNumbers.Add(new PhoneNumber(workPhone, IContactMethod.Types.Work));
        }
        return this;
    }

    public Contact HomePhone(string homePhone)
    {
        var phone = PhoneNumbers.FirstOrDefault(p => p.Type == IContactMethod.Types.Home);
        if (phone != null)
        {
            phone.Value = homePhone;
        }
        else
        {
            PhoneNumbers.Add(new PhoneNumber(homePhone, IContactMethod.Types.Home));
        }
        return this;
    }

    public Contact PrimaryContactMethod(IContactMethod.Types type)
    {
        PhoneNumbers.ForEach(p => p.IsPrimary = false);
        Email?.IsPrimary = false;

        if ((int)type == (int)IContactMethod.Types.Email)
        {
            Email?.IsPrimary = true;
            return this;
        }

        var phone = PhoneNumbers.FirstOrDefault(p => (int)p.Type == (int)type);
        if (phone == null)
        {
            throw new InvalidOperationException($"No contact method of type {type} found.");
        }

        phone.IsPrimary = true;
        return this;
    }
}
