using backend1.Data;
using backend1.Models.Domain;
using backend1.Models.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace backend1.Repositories
{
    public class SQLAuthRepository : IAuthRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly IConfiguration _configuration;

        public SQLAuthRepository(AppDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        public async Task<User?> RegisterAsync(RegisterRequestDTO request)
        {
            // Kiểm tra user đã tồn tại chưa (dùng Email)
            var existingUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == request.Email || u.Name == request.Email);
            if (existingUser != null)
            {
                return null; // User đã tồn tại
            }

            // Hash password bằng BCrypt
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var user = new User
            {
                Name = request.Email, // Dùng Email làm tên đăng nhập
                FullName = request.FullName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                PasswordHash = passwordHash,
                Role = "Customer" // Mặc định là Customer
            };

            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            return user;
        }

        public async Task<LoginResponseDTO?> LoginAsync(LoginRequestDTO request)
        {
            // Tìm user theo tên đăng nhập HOẶC email
            var user = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Name == request.Name || u.Email == request.Name);
            if (user == null)
            {
                return null; // User không tồn tại
            }

            // Xác thực password
            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return null; // Sai password
            }

            // Tạo JWT Token
            var token = CreateJwtToken(user);

            return new LoginResponseDTO
            {
                Token = token,
                UserId = user.Id,
                Name = user.Name ?? string.Empty,
                FullName = user.FullName ?? string.Empty,
                Role = user.Role ?? "Customer"
            };
        }

        public async Task<User?> GetUserByNameAsync(string name)
        {
            return await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Name == name);
        }

        private string CreateJwtToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name ?? string.Empty),
                new Claim(ClaimTypes.GivenName, user.FullName ?? string.Empty),
                new Claim(ClaimTypes.Role, user.Role ?? "Customer")
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(7), // Token hết hạn sau 7 ngày
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
