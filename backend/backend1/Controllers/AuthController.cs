using backend1.Models.DTO;
using backend1.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace backend1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;

        public AuthController(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        /// <summary>
        /// Đăng ký tài khoản mới
        /// </summary>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _authRepository.RegisterAsync(request);
            
            if (user == null)
            {
                return BadRequest(new { Message = "Tên đăng nhập đã tồn tại" });
            }

            return CreatedAtAction(nameof(Register), new 
            { 
                Message = "Đăng ký thành công",
                UserId = user.Id,
                Name = user.Name,
                FullName = user.FullName
            });
        }

        /// <summary>
        /// Đăng nhập và nhận JWT Token
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authRepository.LoginAsync(request);
            
            if (result == null)
            {
                return Unauthorized(new { Message = "Tên đăng nhập hoặc mật khẩu không đúng" });
            }

            return Ok(result);
        }
    }
}
