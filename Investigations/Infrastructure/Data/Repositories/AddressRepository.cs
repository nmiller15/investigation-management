using Investigations.Configuration;
using Investigations.Infrastructure.Data.Parsers;
using Investigations.Models;

namespace Investigations.Infrastructure.Data.Repositories;

public class AddressRepository(IConnectionStrings connectionStrings) : BaseSqlRepository(connectionStrings)
{
    private readonly AddressParser _addressParser = new();
    private readonly IntParser _intParser = new();

    public async Task<List<Address>> GetAddresses()
    {
        var dcs = GetFunctionCallDcsInstance("get_addresses");
        var records = await NpgsqlDataProvider.ExecuteFunction(dcs, _addressParser);
        return records ?? [];
    }

    public async Task<Address> GetAddress(int addressKey)
    {
        var dcs = GetFunctionCallDcsInstance("get_address_by_key");
        AddAddressKeyParameter(dcs, addressKey);
        var records = await NpgsqlDataProvider.ExecuteFunction(dcs, _addressParser);
        return records?.FirstOrDefault() ?? new Address();
    }

    public async Task<int> AddAddress(Address address, int insertedByUserKey = 100)
    {
        var dcs = GetFunctionCallDcsInstance("add_address");
        AddAddressKeyParameter(dcs, address.AddressKey);
        AddAddressParameters(dcs, address);
        dcs.AddParameter("p_inserted_by_user_key", insertedByUserKey);
        var records = await NpgsqlDataProvider.ExecuteFunction(dcs, _intParser);
        return records?.FirstOrDefault() ?? -1;
    }

    public async Task<int> UpdateAddress(Address address, int updatedByUserKey = 100)
    {
        var dcs = GetFunctionCallDcsInstance("update_address");
        AddAddressKeyParameter(dcs, address.AddressKey);
        AddAddressParameters(dcs, address);
        dcs.AddParameter("p_updated_by_user_key", updatedByUserKey);
        var records = await NpgsqlDataProvider.ExecuteFunction(dcs, _intParser);
        return records?.FirstOrDefault() ?? -1;
    }

    public async Task<int> DeleteAddress(int addressKey, int deletedByUserKey = 100)
    {
        var dcs = GetFunctionCallDcsInstance("delete_address_by_key");
        AddAddressKeyParameter(dcs, addressKey);
        var records = await NpgsqlDataProvider.ExecuteFunction(dcs, _intParser);
        return records?.FirstOrDefault() ?? -1;
    }

    private static void AddAddressKeyParameter(DataCallSettings dcs, int addressKey)
    {
        dcs.AddParameter("p_address_key", addressKey);
    }

    private static void AddAddressParameters(DataCallSettings dcs, Address address)
    {
        dcs.AddParameter("p_line_one", address.LineOne);
        dcs.AddParameter("p_line_two", address.LineTwo);
        dcs.AddParameter("p_state_code_key", address.StateCodeKey);
        dcs.AddParameter("p_country_code_key", address.CountryCodeKey);
        dcs.AddParameter("p_zip", address.Zip);
    }
}
