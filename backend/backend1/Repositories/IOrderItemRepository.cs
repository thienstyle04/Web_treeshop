using backend1.Models.Domain;
using backend1.Models.DTO;

namespace backend1.Repositories
{
    public interface IOrderItemRepository
    {
        Task<List<OrderItemDetailDTO>> GetByOrderIdAsync(int orderId);
        Task<OrderItem?> UpdateOrderItemAsync(int id, UpdateOrderItemDTO request);
        Task<OrderItem?> DeleteOrderItemAsync(int id);
    }
}