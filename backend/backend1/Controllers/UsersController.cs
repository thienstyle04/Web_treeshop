using backend1.Models.DTO;
using backend1.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace backend1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        public UsersController(IUserRepository userRepository) { _userRepository = userRepository; }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _userRepository.GetAllUsersAsync());

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            return user == null ? NotFound() : Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddUserRequestDTO request)
        {
            var user = await _userRepository.AddUserAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] AddUserRequestDTO request)
        {
            var user = await _userRepository.UpdateUserAsync(id, request);
            return user == null ? NotFound() : Ok(user);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userRepository.DeleteUserAsync(id);
            return user == null ? NotFound() : Ok(user);
        }
    }
}