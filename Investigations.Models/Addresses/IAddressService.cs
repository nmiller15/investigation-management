using Investigations.Models.Shared;

namespace Investigations.Models.Addresses;

public interface IAddressService
{
    Task<MethodResponse<List<Address>>> GetAddresses();
    Task<MethodResponse<Address>> GetAddress(int addressKey);
    Task<MethodResponse<int>> Save(Address address);
    Task<MethodResponse<int>> DeleteAddress(int addressKey);
}
