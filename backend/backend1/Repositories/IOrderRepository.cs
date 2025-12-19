using backend1.Models.Domain;
using backend1.Models.DTO;

namespace backend1.Repositories
{
    public interface IOrderRepository
    {
        Task<List<OrderDTO>> GetAllOrdersAsync();
        Task<OrderDTO?> GetOrderByIdAsync(int id);
        Task<List<OrderDTO>> GetOrdersByUserIdAsync(int userId);
        Task<Order> CreateOrderAsync(CreateOrderRequestDTO request);
        Task<Order?> UpdateOrderStatusAsync(int id, string status);
        Task<Order?> DeleteOrderAsync(int id);
    }
}