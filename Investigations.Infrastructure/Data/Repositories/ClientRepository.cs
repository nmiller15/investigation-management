using Investigations.Infrastructure.Data.Parsers;
using Investigations.Models.Clients;
using Investigations.Models.Configuration;
using Investigations.Models.Data;

namespace Investigations.Infrastructure.Data.Repositories;

public class ClientRepository(IConnectionStrings connectionStrings)
    : BaseSqlRepository(connectionStrings), IClientRepository
{
    private readonly ClientParser _clientParser = new();
    private readonly IntParser _intParser = new();

    public async Task<List<Client>> GetClients()
    {
        var dcs = GetFunctionCallDcsInstance("get_clients");

        var records = await NpgsqlDataProvider.ExecuteFunction(dcs, _clientParser);
        return records;
    }

    public async Task<Client> GetClient(int clientKey)
    {
        var dcs = GetFunctionCallDcsInstance("get_client_by_key");
        AddClientKeyParameter(clientKey, dcs);

        var records = await NpgsqlDataProvider.ExecuteFunction(dcs, _clientParser);
        return records.FirstOrDefault() ?? new();
    }

    public async Task<int> AddClient(Client client, int insertedByUserKey = 100)
    {
        var dcs = GetFunctionCallDcsInstance("add_client");
        AddClientKeyParameter(client.ClientKey, dcs);
        AddClientParameters(client, dcs);
        dcs.AddParameter("p_inserted_by_user_key", insertedByUserKey);

        var clientKey = await NpgsqlDataProvider.ExecuteFunctionScalar(dcs, _intParser);
        return clientKey;
    }

    public async Task<int> UpdateClient(Client client, int updatedByUserKey = 100)
    {
        var dcs = GetFunctionCallDcsInstance("update_client");
        AddClientKeyParameter(client.ClientKey, dcs);
        AddClientParameters(client, dcs);
        dcs.AddParameter("p_updated_by_user_key", updatedByUserKey);

        var clientKey = await NpgsqlDataProvider.ExecuteFunctionScalar(dcs, _intParser);
        return clientKey;
    }

    public async Task<int> DeleteClient(int clientKey)
    {
        var dcs = GetFunctionCallDcsInstance("delete_client");
        AddClientKeyParameter(clientKey, dcs);

        var returnKey = await NpgsqlDataProvider.ExecuteFunctionScalar(dcs, _intParser);
        return returnKey;
    }

    public static void AddClientKeyParameter(int clientKey, DataCallSettings dcs)
    {
        dcs.AddParameter("p_client_key", clientKey);
    }

    public static void AddClientParameters(Client client, DataCallSettings dcs)
    {
        dcs.AddParameter("p_client_name", client.ClientName);
        dcs.AddParameter("p_primary_contact_key", client.PrimaryContactKey);
    }
}
