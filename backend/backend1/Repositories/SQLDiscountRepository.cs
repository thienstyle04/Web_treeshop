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

        public async Task<Discount?> GetDiscountByIdAsync(int id)
        {
            return await _dbContext.Discounts.FindAsync(id);
        }

        public async Task<Discount?> GetDiscountByCodeAsync(string code)
        {
            var today = DateTime.UtcNow;
            var discount = await _dbContext.Discounts.FirstOrDefaultAsync(d => d.Code == code && d.IsActive);

            if (discount == null) return null;

            // Date validation
            if (discount.StartDate > today || discount.EndDate < today) return null;

            // Usage limit validation
            if (discount.UsageLimit > 0 && discount.UsedCount >= discount.UsageLimit) return null;

            // Product specific validation (Codes shouldn't be for specific products usually, or handle in service)
            // For now, allow retrieving it, Controller can decide further validity based on cart context

            return discount;
        }

        public async Task<Discount> CreateDiscountAsync(AddDiscountRequestDTO request)
        {
            var discount = new Discount
            {
                Code = request.Code,
                DiscountType = request.DiscountType,
                Value = request.Value,
                MinimumOrderAmount = request.MinimumOrderAmount,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                AppliesToProductId = request.AppliesToProductId,
                UsageLimit = request.UsageLimit,
                UsedCount = 0,
                IsActive = request.IsActive
            };
            await _dbContext.Discounts.AddAsync(discount);
            await _dbContext.SaveChangesAsync();
            return discount;
        }

        public async Task<Discount?> UpdateDiscountAsync(int id, AddDiscountRequestDTO request)
        {
            var discount = await _dbContext.Discounts.FindAsync(id);
            if (discount == null) return null;

            discount.Code = request.Code;
            discount.DiscountType = request.DiscountType;
            discount.Value = request.Value;
            discount.MinimumOrderAmount = request.MinimumOrderAmount;
            discount.StartDate = request.StartDate;
            discount.EndDate = request.EndDate;
            discount.AppliesToProductId = request.AppliesToProductId;
            discount.UsageLimit = request.UsageLimit;
            discount.IsActive = request.IsActive;

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