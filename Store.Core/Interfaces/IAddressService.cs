using Store.Core.Entities;

namespace Store.Core.Interfaces
{
    public interface IAddressService
    {
        Task<Address?> GetUserAddressAsync(string email);
        Task<Address?> UpdateUserAddressAsync(string email, Address address);
    }
}
