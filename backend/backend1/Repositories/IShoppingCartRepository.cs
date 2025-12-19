using backend1.Models.Domain;
using backend1.Models.DTO;

namespace backend1.Repositories
{
    public interface IShoppingCartRepository
    {
        Task<List<CartItemDTO>> GetCartByUserIdAsync(int userId);
        Task<ShoppingCartItem> AddToCartAsync(AddToCartRequestDTO request);
        Task<ShoppingCartItem?> RemoveFromCartAsync(int cartItemId);
    }
}