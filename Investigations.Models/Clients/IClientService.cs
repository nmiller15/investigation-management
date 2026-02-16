using Investigations.Models.Shared;

namespace Investigations.Models.Clients;

public interface IClientService
{
    Task<MethodResponse<List<Client>>> GetClients();
    Task<MethodResponse<Client>> GetClient(int clientKey);
    Task<MethodResponse<int>> Save(Client client);
    Task<MethodResponse<int>> DeleteClient(int clientKey);
}
