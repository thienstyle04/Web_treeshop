using backend1.Data;
using backend1.Models.Domain;
using backend1.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace backend1.Repositories
{
    public class SQLDiscountRepository : IDiscountRepository
    {
        private readonly AppDbContext _dbContext;
        public SQLDiscountRepository(AppDbContext dbContext) { _dbContext = dbContext; }

        public async Task<List<Discount>> GetAllDiscountsAsync() => await _dbContext.Discounts.ToListAsync();

        public async Task<Discount?> GetDiscountByCodeAsync(string code)
        {
            return await _dbContext.Discounts.FirstOrDefaultAsync(d => d.Code == code && d.IsActive && d.ExpiryDate > DateTime.Now);
        }

        public async Task<Discount> CreateDiscountAsync(AddDiscountRequestDTO request)
        {
            var discount = new Discount
            {
                Code = request.Code,
                DiscountType = request.DiscountType,
                Value = request.Value,
                MinimumOrderAmount = request.MinimumOrderAmount,
                ExpiryDate = request.ExpiryDate,
                IsActive = request.IsActive
            };
            await _dbContext.Discounts.AddAsync(discount);
            await _dbContext.SaveChangesAsync();
            return discount;
        }

        public async Task<Discount?> DeleteDiscountAsync(int id)
        {
            var discount = await _dbContext.Discounts.FindAsync(id);
            if (discount == null) return null;
            _dbContext.Discounts.Remove(discount);
            await _dbContext.SaveChangesAsync();
            return discount;
        }
    }
}