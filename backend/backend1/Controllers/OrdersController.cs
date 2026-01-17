using backend1.Models.DTO;
using backend1.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepository _repo;
        public OrdersController(IOrderRepository repo) { _repo = repo; }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? filterOn, 
            [FromQuery] string? filterQuery,
            [FromQuery] string? sortBy, 
            [FromQuery] bool isAscending = true,
            [FromQuery] int pageNumber = 1, 
            [FromQuery] int pageSize = 1000) 
            => Ok(await _repo.GetAllOrdersAsync());

        [HttpGet("user/{userId:int}")]
        public async Task<IActionResult> GetByUser(int userId) => Ok(await _repo.GetOrdersByUserIdAsync(userId));

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _repo.GetOrderByIdAsync(id);
            return result == null ? NotFound() : Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrderRequestDTO request)
        {
            var result = await _repo.CreateOrderAsync(request);
            return Ok(new { Message = "Order created", OrderId = result.Id });
        }

        [HttpPut("{id:int}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] string status)
        {
            var result = await _repo.UpdateOrderStatusAsync(id, status);
            return result == null ? NotFound() : Ok(result);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _repo.DeleteOrderAsync(id);
            return result == null ? NotFound() : Ok(result);
        }
    }
}