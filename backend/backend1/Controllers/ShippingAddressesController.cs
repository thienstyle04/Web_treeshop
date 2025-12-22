using backend1.Models.DTO;
using backend1.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ShippingAddressesController : ControllerBase
    {
        private readonly IShippingAddressRepository _repo;
        public ShippingAddressesController(IShippingAddressRepository repo) { _repo = repo; }

        [HttpGet("user/{userId:int}")]
        public async Task<IActionResult> GetByUser(int userId) => Ok(await _repo.GetAddressesByUserIdAsync(userId));

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddShippingAddressDTO request)
        {
            var addr = await _repo.AddAddressAsync(request);
            return Ok(addr);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var res = await _repo.DeleteAddressAsync(id);
            return res == null ? NotFound() : Ok(res);
        }
    }
}