using Investigations.Infrastructure.Data.Parsers;
using Investigations.Models;
using Investigations.Configuration;

namespace Investigations.Infrastructure.Data.Repositories;

public class ContactRepository(IConnectionStrings connectionStrings)
    : BaseSqlRepository(connectionStrings)
{
    private readonly ContactParser _contactParser = new ContactParser();

    public async Task<List<Contact>> GetContacts()
    {
        var dcs = GetFunctionCallDcsInstance("get_contacts");

        var records = await NpgsqlDataProvider.ExecuteFunction(dcs, _contactParser);
        return records ?? [];
    }

    public async Task<Contact> GetContactByKey(int contactKey)
    {
        var dcs = GetFunctionCallDcsInstance("get_contact_by_key");
        AddContactKeyParameter(contactKey, dcs);

        var records = await NpgsqlDataProvider.ExecuteFunction(dcs, _contactParser);
        return records.FirstOrDefault() ?? new();
    }

    public async Task<int> AddContact(Contact contact, int insertedByUserKey)
    {
        var dcs = GetFunctionCallDcsInstance("add_contact");
        AddContactParameters(contact, dcs);
        dcs.AddParameter("p_inserted_by_user_key", insertedByUserKey);

        var contactKey = await NpgsqlDataProvider.ExecuteFunctionScalar(dcs, new IntParser());
        return contactKey;
    }

    public async Task<int> UpdateContact(Contact contact, int updatedByUserKey)
    {
        var dcs = GetFunctionCallDcsInstance("update_contact");
        AddContactKeyParameter(contact.ContactKey, dcs);
        AddContactParameters(contact, dcs);
        dcs.AddParameter("p_updated_by_user_key", updatedByUserKey);

        var contactKey = await NpgsqlDataProvider.ExecuteFunctionScalar(dcs, new IntParser());
        return contactKey;
    }

    public async Task<int> DeleteContact(int contactKey)
    {
        var dcs = GetFunctionCallDcsInstance("delete_contact");
        AddContactKeyParameter(contactKey, dcs);

        var returnedKey = await NpgsqlDataProvider.ExecuteFunctionScalar(dcs, new IntParser());
        return returnedKey;
    }

    private static void AddContactKeyParameter(int contactKey, DataCallSettings dcs)
    {
        dcs.AddParameter("p_contact_key", contactKey);
    }

    private static void AddContactParameters(Contact contact, DataCallSettings dcs)
    {
        dcs.AddParameter("p_first_name", contact.FirstName);
        dcs.AddParameter("p_last_name", contact.LastName);
        dcs.AddParameter("p_email", contact.Email);
        dcs.AddParameter("p_mobile_phone", contact.MobilePhone);
        dcs.AddParameter("p_work_phone", contact.WorkPhone);
        dcs.AddParameter("p_home_phone", contact.HomePhone);
        dcs.AddParameter("p_notes", contact.Notes);
    }
}
