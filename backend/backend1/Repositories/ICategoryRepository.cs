using backend1.Models.Domain;
using backend1.Models.DTO;

namespace backend1.Repositories
{
    public interface ICategoryRepository
    {
        Task<List<CategoryDTO>> GetAllCategoriesAsync(
            string? query = null,
            string? sortBy = null,
            string? sortDirection = "asc",
            int pageNumber = 1,
            int pageSize = 100);
        Task<CategoryDTO?> GetCategoryByIdAsync(int id);
        Task<Category> AddCategoryAsync(AddCategoryRequestDTO request);
        Task<Category?> UpdateCategoryAsync(int id, AddCategoryRequestDTO request);
        Task<Category?> DeleteCategoryAsync(int id);
    }
}