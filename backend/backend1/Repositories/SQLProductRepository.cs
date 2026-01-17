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
        public async Task<List<ProductDetailsDTO>> GetAllProductsAsync(
            string? filterOn = null,
            string? filterQuery = null,
            string? sortBy = null,
            bool isAscending = true,
            int pageNumber = 1,
            int pageSize = 1000)
        {
            // Tạo Queryable từ bảng Products và ánh xạ sang DTO
            var allProducts = _dbContext.Products
                .Include(p => p.Category)
                .Include(p => p.Images)
                .Select(product => new ProductDetailsDTO()
                {
                    Id = product.Id,
                    Name = product.Name,
                    ScientificName = product.ScientificName,
                    Description = product.Description,
                    Price = product.Price,
                    StockQuantity = product.StockQuantity,
                    DateAdded = product.DateAdded,
                    CategoryId = product.CategoryId,
                    CategoryName = product.Category != null ? product.Category.Name : null,
                    ImageUrls = product.Images.Select(i => i.FilePath ?? "").ToList()
                })
                .AsQueryable(); // Chuyển sang IQueryable để xử lý tiếp

            // Lọc dữ liệu
            if (!string.IsNullOrWhiteSpace(filterOn) && !string.IsNullOrWhiteSpace(filterQuery))
            {
                // Lọc theo Tên sản phẩm (Name)
                if (filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    allProducts = allProducts.Where(x => x.Name.Contains(filterQuery));
                }
                // Có thể thêm các trường khác nếu muốn, ví dụ CategoryName
            }

            //Sắp xếp dữ liệu
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                if (sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    allProducts = isAscending ? allProducts.OrderBy(x => x.Name) : allProducts.OrderByDescending(x => x.Name);
                }
                else if (sortBy.Equals("Price", StringComparison.OrdinalIgnoreCase))
                {
                    allProducts = isAscending ? allProducts.OrderBy(x => x.Price) : allProducts.OrderByDescending(x => x.Price);
                }
            }

            //Phân trang
            var skipResults = (pageNumber - 1) * pageSize;

            // Thực thi query và trả về kết quả
            // Fetch active flash sales first (efficient in memory for small number of sales)
            var now = DateTime.UtcNow;
            var activeFlashSales = await _dbContext.Discounts
                .Where(d => d.IsActive && d.AppliesToProductId != null && d.StartDate <= now && d.EndDate > now)
                .ToListAsync();

            // Execute query
            var products = await allProducts.Skip(skipResults).Take(pageSize).ToListAsync();

            // Apply Flash Sales Logic
            foreach (var product in products)
            {
                var sale = activeFlashSales.FirstOrDefault(s => s.AppliesToProductId == product.Id);
                if (sale != null)
                {
                    product.OriginalPrice = product.Price;
                    product.IsFlashSale = true;
                    product.FlashSaleEndTime = sale.EndDate;

                    if (sale.DiscountType == "Percent")
                    {
                        product.DiscountPercentage = (double)sale.Value;
                        product.Price = product.Price * (1 - (sale.Value / 100m));
                    }
                    else // Fixed amount
                    {
                        product.Price = product.Price - sale.Value;
                        if (product.Price < 0) product.Price = 0;
                        product.DiscountPercentage = (double)((product.OriginalPrice - product.Price) / product.OriginalPrice * 100);
                    }
                }
            }

            return products;
        }

        public async Task<ProductDetailsDTO?> GetProductByIdAsync(int id)
        {
            // Logic truy cập DB (sử dụng FirstOrDefaultAsync)
            var productDomain = await _dbContext.Products
                .Include(p => p.Category)
                .Include(p => p.Images)
                .Include(p => p.reviews)
                .ThenInclude(r => r.User)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (productDomain == null)
            {
                return null;
            }

            // Ánh xạ Domain Model sang DTO
            var dto = new ProductDetailsDTO
            {
                Id = productDomain.Id,
                Name = productDomain.Name,
                ScientificName = productDomain.ScientificName,
                Description = productDomain.Description,
                Price = productDomain.Price,
                StockQuantity = productDomain.StockQuantity,
                DateAdded = productDomain.DateAdded,
                CategoryId = productDomain.CategoryId,
                CategoryName = productDomain.Category?.Name,
                ImageUrls = productDomain.Images.Select(i => i.FilePath ?? "").ToList(),
                Reviews = productDomain.reviews.Select(r => new ReviewDTO
                {
                    Id = r.Id,
                    Rating = r.Rating,
                    Comment = r.Comment,
                    ReviewDate = r.ReviewDate,
                    ProductId = r.ProductId,
                    UserId = r.UserId,
                    UserName = r.User?.Name ?? "Unknown"
                }).ToList()
            };

            // Flash Sale Logic
            var now = DateTime.UtcNow;
            var sale = await _dbContext.Discounts.FirstOrDefaultAsync(d => d.IsActive && d.AppliesToProductId == id && d.StartDate <= now && d.EndDate > now);
            if (sale != null)
            {
                dto.OriginalPrice = dto.Price;
                dto.IsFlashSale = true;
                dto.FlashSaleEndTime = sale.EndDate;

                if (sale.DiscountType == "Percent")
                {
                    dto.DiscountPercentage = (double)sale.Value;
                    dto.Price = dto.Price * (1 - (sale.Value / 100m));
                }
                else
                {
                    dto.Price = dto.Price - sale.Value;
                    if (dto.Price < 0) dto.Price = 0;
                    dto.DiscountPercentage = (double)((dto.OriginalPrice - dto.Price) / dto.OriginalPrice * 100);
                }
            }

            return dto;
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