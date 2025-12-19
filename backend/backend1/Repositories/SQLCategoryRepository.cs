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

        public async Task<List<CategoryDTO>> GetAllCategoriesAsync()
        {
            var categories = await _dbContext.Categories.ToListAsync();
            return categories.Select(c => new CategoryDTO
            {
                Id = c.Id,
                Name = c.Name,
                UrlHandler = c.UrlHandler,
                Description = c.Description
            }).ToList();
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