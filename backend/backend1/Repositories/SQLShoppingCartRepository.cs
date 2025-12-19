using backend1.Data;
using backend1.Models.Domain;
using backend1.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace backend1.Repositories
{
    public class SQLShoppingCartRepository : IShoppingCartRepository
    {
        private readonly AppDbContext _dbContext;
        public SQLShoppingCartRepository(AppDbContext dbContext) { _dbContext = dbContext; }

        public async Task<List<CartItemDTO>> GetCartByUserIdAsync(int userId)
        {
            var items = await _dbContext.ShoppingCartItems
                .Include(c => c.Product)
                .ThenInclude(p => p.Images)
                .Where(c => c.UserId == userId)
                .ToListAsync();

            return items.Select(i => new CartItemDTO
            {
                Id = i.Id,
                ProductId = i.ProductId,
                ProductName = i.Product?.Name ?? "Unknown",
                Price = i.Product?.Price ?? 0,
                Quantity = i.Quantity,
                ImageUrl = i.Product?.Images?.FirstOrDefault()?.FilePath ?? ""
            }).ToList();
        }

        public async Task<ShoppingCartItem> AddToCartAsync(AddToCartRequestDTO request)
        {
            // Kiểm tra xem sản phẩm đã có trong giỏ chưa
            var existingItem = await _dbContext.ShoppingCartItems
                .FirstOrDefaultAsync(c => c.UserId == request.UserId && c.ProductId == request.ProductId);

            if (existingItem != null)
            {
                existingItem.Quantity += request.Quantity;
                await _dbContext.SaveChangesAsync();
                return existingItem;
            }

            var newItem = new ShoppingCartItem
            {
                UserId = request.UserId,
                ProductId = request.ProductId,
                Quantity = request.Quantity,
                DateCreated = DateTime.Now
            };
            await _dbContext.ShoppingCartItems.AddAsync(newItem);
            await _dbContext.SaveChangesAsync();
            return newItem;
        }

        public async Task<ShoppingCartItem?> RemoveFromCartAsync(int cartItemId)
        {
            var item = await _dbContext.ShoppingCartItems.FindAsync(cartItemId);
            if (item == null) return null;
            _dbContext.ShoppingCartItems.Remove(item);
            await _dbContext.SaveChangesAsync();
            return item;
        }
    }
}