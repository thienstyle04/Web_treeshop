using backend1.Models.Domain;
using backend1.Models.DTO;

namespace backend1.Repositories
{
    public interface IShippingAddressRepository
    {
        Task<List<ShippingAddress>> GetAddressesByUserIdAsync(int userId);
        Task<ShippingAddress> AddAddressAsync(AddShippingAddressDTO request);
        Task<ShippingAddress?> DeleteAddressAsync(int id);
    }
}