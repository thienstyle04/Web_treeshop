using Microsoft.AspNetCore.Mvc;
using backend1.Models.DTO;
using backend1.Repositories;
using backend1.Models.Domain;
using Microsoft.AspNetCore.Authorization; // Dùng cho Delete

namespace backend1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        // 1. Khai báo private field cho IProductRepository
        private readonly IProductRepository _productRepository;

        // 2. Constructor để inject IProductRepository
        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet("get-all-products")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllProducts(
            [FromQuery] string? filterOn,
            [FromQuery] string? filterQuery,
            [FromQuery] string? sortBy,
            [FromQuery] bool isAscending = true,
            [FromQuery] int pageNumber = 1000)
        {
            var allProductsDTO = await _productRepository.GetAllProductsAsync(); 

            return Ok(allProductsDTO);
        }

        [HttpGet]
        [Route("get-product-by-id/{id:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetProductById([FromRoute] int id)
        {
            var productDTO = await _productRepository.GetProductByIdAsync(id); 

            if (productDTO == null)
            {
                return NotFound();
            }

            return Ok(productDTO);
        }

        [HttpPost("add-product")]
        public async Task<IActionResult> AddProduct([FromBody] AddProductRequestDTO addProductRequestDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var productDomainModel = await _productRepository.AddProductAsync(addProductRequestDTO); 

            // Trả về 201 Created
            return CreatedAtAction(nameof(GetProductById), new { id = productDomainModel.Id }, addProductRequestDTO);
        }

        [HttpPut("update-product-by-id/{id:int}")]
        public async Task<IActionResult> UpdateProductById([FromRoute] int id, [FromBody] AddProductRequestDTO updateProductDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var productDomain = await _productRepository.UpdateProductByIdAsync(id, updateProductDTO); 

            if (productDomain == null)
            {
                return NotFound();
            }

            return Ok(updateProductDTO);
        }

        [HttpDelete("delete-product-by-id/{id:int}")]
        public async Task<IActionResult> DeleteProductById([FromRoute] int id)
        {
            var productDomain = await _productRepository.DeleteProductByIdAsync(id); 

            if (productDomain == null)
            {
                // Trả về 404 nếu không tìm thấy, mặc dù thao tác xóa không cần thiết phải tìm
                return NotFound();
            }

            return Ok(productDomain);
        }
    }
}