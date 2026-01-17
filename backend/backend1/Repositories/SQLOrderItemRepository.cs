using backend1.Data;
using backend1.Models.Domain;
using backend1.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace backend1.Repositories
{
    public class SQLOrderItemRepository : IOrderItemRepository
    {
        private readonly AppDbContext _dbContext;
        public SQLOrderItemRepository(AppDbContext dbContext) { _dbContext = dbContext; }

        public async Task<List<OrderItemDetailDTO>> GetByOrderIdAsync(int orderId)
        {
            var items = await _dbContext.OrderItems
                .Include(oi => oi.Product)
                .Where(oi => oi.OrderId == orderId)
                .ToListAsync();

            return items.Select(i => new OrderItemDetailDTO
            {
                Id = i.Id,
                OrderId = i.OrderId,
                ProductId = i.ProductId,
                ProductName = i.Product?.Name,
                Quantity = i.Quantity,
                Price = i.UnitPrice
            }).ToList();
        }

        public async Task<OrderItem?> UpdateOrderItemAsync(int id, UpdateOrderItemDTO request)
        {
            var item = await _dbContext.OrderItems.FindAsync(id);
            if (item == null) return null;

            item.Quantity = request.Quantity;
            // Lưu ý: Có thể cần cập nhật lại TotalAmount của bảng Order tại đây nếu muốn logic chặt chẽ

            await _dbContext.SaveChangesAsync();
            return item;
        }

        public async Task<OrderItem?> DeleteOrderItemAsync(int id)
        {
            var item = await _dbContext.OrderItems.FindAsync(id);
            if (item == null) return null;
            _dbContext.OrderItems.Remove(item);
            await _dbContext.SaveChangesAsync();
            return item;
        }
    }
}