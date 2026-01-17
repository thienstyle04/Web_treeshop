using backend1.Models.Domain;
using backend1.Models.DTO;

namespace backend1.Repositories
{
    public interface IOrderRepository
    {
        Task<List<OrderDTO>> GetAllOrdersAsync(
            string? filterOn = null, 
            string? filterQuery = null,
            string? sortBy = null, 
            bool isAscending = true,
            int pageNumber = 1, 
            int pageSize = 1000);
        Task<OrderDTO?> GetOrderByIdAsync(int id);
        Task<List<OrderDTO>> GetOrdersByUserIdAsync(int userId);
        Task<Order> CreateOrderAsync(CreateOrderRequestDTO request);
        Task<Order?> UpdateOrderStatusAsync(int id, string status);
        Task<Order?> DeleteOrderAsync(int id);
    }
}