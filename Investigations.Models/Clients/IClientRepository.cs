namespace Investigations.Models.Clients;

public interface IClientRepository
{
    Task<List<Client>> GetClients();
    Task<Client> GetClient(int clientKey);
    Task<int> AddClient(Client client, int insertedByUserKey = 100);
    Task<int> UpdateClient(Client client, int updatedByUserKey = 100);
    Task<int> DeleteClient(int clientKey);
}
