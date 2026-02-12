using Investigations.Models.Shared;

namespace Investigations.Models.Contacts;

public interface IContactService
{
    Task<MethodResponse<List<Contact>>> GetContacts();
    Task<MethodResponse<Contact>> GetContactByKey(int contactKey);
    Task<MethodResponse<int>> Save(Contact contact);
    Task<MethodResponse<int>> DeleteContact(int contactKey);
}
