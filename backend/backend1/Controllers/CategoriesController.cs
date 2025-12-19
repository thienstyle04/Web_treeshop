using backend1.Models.DTO;
using backend1.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace backend1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _repo;
        public CategoriesController(ICategoryRepository repo) { _repo = repo; }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _repo.GetAllCategoriesAsync());

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _repo.GetCategoryByIdAsync(id);
            return result == null ? NotFound() : Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddCategoryRequestDTO request)
        {
            var result = await _repo.AddCategoryAsync(request);
            return Ok(result);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] AddCategoryRequestDTO request)
        {
            var result = await _repo.UpdateCategoryAsync(id, request);
            return result == null ? NotFound() : Ok(result);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _repo.DeleteCategoryAsync(id);
            return result == null ? NotFound() : Ok(result);
        }
    }
}