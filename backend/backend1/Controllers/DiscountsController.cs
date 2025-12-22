using backend1.Models.DTO;
using backend1.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DiscountsController : ControllerBase
    {
        private readonly IDiscountRepository _discountRepository;
        public DiscountsController(IDiscountRepository discountRepository) { _discountRepository = discountRepository; }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _discountRepository.GetAllDiscountsAsync());

        [HttpGet("check/{code}")]
        public async Task<IActionResult> CheckCode(string code)
        {
            var discount = await _discountRepository.GetDiscountByCodeAsync(code);
            return discount == null ? NotFound("Mã không hợp lệ hoặc đã hết hạn") : Ok(discount);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddDiscountRequestDTO request)
        {
            var discount = await _discountRepository.CreateDiscountAsync(request);
            return Ok(discount);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var res = await _discountRepository.DeleteDiscountAsync(id);
            return res == null ? NotFound() : Ok(res);
        }
    }
}