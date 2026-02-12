namespace Investigations.Models.Contacts;

public interface IContactRepository
{
    Task<List<Contact>> GetContacts();
    Task<Contact> GetContactByKey(int contactKey);
    Task<int> AddContact(Contact contact, int insertedByUserKey);
    Task<int> UpdateContact(Contact contact, int updatedByUserKey);
    Task<int> DeleteContact(int contactKey);
}
