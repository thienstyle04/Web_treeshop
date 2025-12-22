using backend1.Models.DTO;
using backend1.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingCartRepository _cartRepository;
        public ShoppingCartController(IShoppingCartRepository cartRepository) { _cartRepository = cartRepository; }

        [HttpGet("{userId:int}")]
        public async Task<IActionResult> GetCart(int userId)
        {
            return Ok(await _cartRepository.GetCartByUserIdAsync(userId));
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartRequestDTO request)
        {
            var item = await _cartRepository.AddToCartAsync(request);
            return Ok(item);
        }

        [HttpDelete("remove/{id:int}")]
        public async Task<IActionResult> Remove(int id)
        {
            var item = await _cartRepository.RemoveFromCartAsync(id);
            return item == null ? NotFound() : Ok(item);
        }
    }
}