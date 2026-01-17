using backend1.Data;
using backend1.Models.Domain;
using backend1.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace backend1.Repositories
{
    public class SQLCategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _dbContext;
        public SQLCategoryRepository(AppDbContext dbContext) { _dbContext = dbContext; }

        public async Task<List<CategoryDTO>> GetAllCategoriesAsync(
        string? query = null,
        string? sortBy = null,
        string? sortDirection = "asc",
        int pageNumber = 1,
        int pageSize = 100)
        {
            //  Khởi tạo Query
            var categories = _dbContext.Categories.AsQueryable();

            //  Filter (Tìm kiếm theo tên)
            if (!string.IsNullOrWhiteSpace(query))
            {
                categories = categories.Where(x => x.Name.Contains(query));
            }

            //  Sorting (Sắp xếp)
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                if (sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    categories = sortDirection == "asc"
                        ? categories.OrderBy(x => x.Name)
                        : categories.OrderByDescending(x => x.Name);
                }
            }

            //  Pagination (Phân trang)
            var skipResults = (pageNumber - 1) * pageSize;

            //  Execute & Map to DTO
            return await categories
                .Skip(skipResults)
                .Take(pageSize)
                .Select(x => new CategoryDTO { Id = x.Id, Name = x.Name }) 
                .ToListAsync();
        }

        public async Task<CategoryDTO?> GetCategoryByIdAsync(int id)
        {
            var c = await _dbContext.Categories.FindAsync(id);
            return c == null ? null : new CategoryDTO { Id = c.Id, Name = c.Name, UrlHandler = c.UrlHandler, Description = c.Description };
        }

        public async Task<Category> AddCategoryAsync(AddCategoryRequestDTO request)
        {
            var category = new Category { Name = request.Name, UrlHandler = request.UrlHandler, Description = request.Description };
            await _dbContext.Categories.AddAsync(category);
            await _dbContext.SaveChangesAsync();
            return category;
        }

        public async Task<Category?> UpdateCategoryAsync(int id, AddCategoryRequestDTO request)
        {
            var category = await _dbContext.Categories.FindAsync(id);
            if (category == null) return null;
            category.Name = request.Name;
            category.UrlHandler = request.UrlHandler;
            category.Description = request.Description;
            await _dbContext.SaveChangesAsync();
            return category;
        }

        public async Task<Category?> DeleteCategoryAsync(int id)
        {
            var category = await _dbContext.Categories.FindAsync(id);
            if (category == null) return null;
            _dbContext.Categories.Remove(category);
            await _dbContext.SaveChangesAsync();
            return category;
        }
    }
}