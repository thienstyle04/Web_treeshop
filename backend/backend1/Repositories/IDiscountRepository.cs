using backend1.Models.Domain;
using backend1.Models.DTO;

namespace backend1.Repositories
{
    public interface IDiscountRepository
    {
        Task<List<Discount>> GetAllDiscountsAsync();
        Task<Discount?> GetDiscountByIdAsync(int id);
        Task<Discount?> GetDiscountByCodeAsync(string code);
        Task<Discount> CreateDiscountAsync(AddDiscountRequestDTO request);
        Task<Discount?> UpdateDiscountAsync(int id, AddDiscountRequestDTO request);
        Task<Discount?> DeleteDiscountAsync(int id);
    }
}