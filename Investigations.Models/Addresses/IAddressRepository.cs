namespace Investigations.Models.Addresses;

public interface IAddressRepository
{
    Task<List<Address>> GetAddresses();
    Task<Address> GetAddress(int addressKey);
    Task<int> AddAddress(Address address, int insertedByUserKey = 100);
    Task<int> UpdateAddress(Address address, int updatedByUserKey = 100);
    Task<int> DeleteAddress(int addressKey, int deletedByUserKey = 100);
}
