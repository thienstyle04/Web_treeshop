using backend1.Models.Domain;
using backend1.Models.DTO;

namespace backend1.Repositories
{
    public interface IProductRepository
    {
        Task<List<ProductDetailsDTO>> GetAllProductsAsync(
            string? filterOn = null,
            string? filterQuery = null,
            string? sortBy = null,
            bool isAscending = true,
            int pageNumber = 1,
            int pageSize = 1000);
        Task<ProductDetailsDTO?> GetProductByIdAsync(int id);
        Task<Product> AddProductAsync(AddProductRequestDTO addProductRequestDTO);
        Task<Product?> UpdateProductByIdAsync(int id, AddProductRequestDTO updateProductDTO);
        Task<Product?> DeleteProductByIdAsync(int id);
    }
}
