using backend1.Models.DTO;
using backend1.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace backend1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemsController : ControllerBase
    {
        private readonly IOrderItemRepository _repo;
        public OrderItemsController(IOrderItemRepository repo) { _repo = repo; }

        [HttpGet("order/{orderId:int}")]
        public async Task<IActionResult> GetByOrder(int orderId)
        {
            return Ok(await _repo.GetByOrderIdAsync(orderId));
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateOrderItemDTO request)
        {
            var result = await _repo.UpdateOrderItemAsync(id, request);
            return result == null ? NotFound() : Ok(result);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _repo.DeleteOrderItemAsync(id);
            return result == null ? NotFound() : Ok(result);
        }
    }
}