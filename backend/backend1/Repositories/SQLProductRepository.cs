using backend1.Data;
using backend1.Models.Domain;
using backend1.Models.DTO;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace backend1.Repositories
{
    // Class triển khai Interface và chứa logic tương tác với AppDbContext
    public class SQLProductRepository : IProductRepository
    {
        private readonly AppDbContext _dbContext;

        // Dependency Injection cho AppDbContext
        public SQLProductRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<ProductDetailsDTO>> GetAllProductsAsync()
        {
            // Logic truy cập DB (sử dụng Include và ToListAsync)
            var productsDomain = await _dbContext.Products
                .Include(p => p.Category)
                .Include(p => p.Images)
                .ToListAsync();

            // Ánh xạ Domain Model sang DTO
            return productsDomain.Select(product => new ProductDetailsDTO()
            {
                Id = product.Id,
                Name = product.Name,
                ScientificName = product.ScientificName,
                Description = product.Description,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                DateAdded = product.DateAdded,
                CategoryName = product.Category?.Name,
                ImageUrls = product.Images.Select(i => i.FilePath).ToList()
            }).ToList();
        }

        public async Task<ProductDetailsDTO?> GetProductByIdAsync(int id)
        {
            // Logic truy cập DB (sử dụng FirstOrDefaultAsync)
            var productDomain = await _dbContext.Products
                .Include(p => p.Category)
                .Include(p => p.Images)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (productDomain == null)
            {
                return null;
            }

            // Ánh xạ Domain Model sang DTO
            return new ProductDetailsDTO
            {
                Id = productDomain.Id,
                Name = productDomain.Name,
                ScientificName = productDomain.ScientificName,
                Description = productDomain.Description,
                Price = productDomain.Price,
                StockQuantity = productDomain.StockQuantity,
                DateAdded = productDomain.DateAdded,
                CategoryName = productDomain.Category?.Name,
                ImageUrls = productDomain.Images.Select(i => i.FilePath).ToList()
            };
        }

        public async Task<Product> AddProductAsync(AddProductRequestDTO addProductRequestDTO)
        {
            // Ánh xạ DTO sang Domain Model
            var productDomainModel = new Product
            {
                Name = addProductRequestDTO.Name,
                ScientificName = addProductRequestDTO.ScientificName,
                Description = addProductRequestDTO.Description,
                Price = addProductRequestDTO.Price,
                StockQuantity = addProductRequestDTO.StockQuantity,
                DateAdded = addProductRequestDTO.DateAdded == default ? DateTime.Now : addProductRequestDTO.DateAdded,
                CategoryId = addProductRequestDTO.CategoryId,
            };

            // Thêm vào DB và lưu thay đổi
            await _dbContext.Products.AddAsync(productDomainModel);
            await _dbContext.SaveChangesAsync();

            return productDomainModel;
        }

        public async Task<Product?> UpdateProductByIdAsync(int id, AddProductRequestDTO updateProductDTO)
        {
            // Tìm Product Domain Model
            var productDomain = await _dbContext.Products.FirstOrDefaultAsync(x => x.Id == id);

            if (productDomain == null)
            {
                return null;
            }

            // Cập nhật thông tin từ DTO
            productDomain.Name = updateProductDTO.Name;
            productDomain.ScientificName = updateProductDTO.ScientificName;
            productDomain.Description = updateProductDTO.Description;
            productDomain.Price = updateProductDTO.Price;
            productDomain.StockQuantity = updateProductDTO.StockQuantity;
            productDomain.DateAdded = updateProductDTO.DateAdded;
            productDomain.CategoryId = updateProductDTO.CategoryId;

            // Lưu thay đổi
            await _dbContext.SaveChangesAsync();

            return productDomain;
        }

        public async Task<Product?> DeleteProductByIdAsync(int id)
        {
            // Tìm Product Domain Model
            var productDomain = await _dbContext.Products.FirstOrDefaultAsync(x => x.Id == id);

            if (productDomain == null)
            {
                return null;
            }

            // Xóa
            _dbContext.Products.Remove(productDomain);
            await _dbContext.SaveChangesAsync();

            return productDomain;
        }
    }
}