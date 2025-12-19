using backend1.Data;
using backend1.Models.Domain;
using backend1.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace backend1.Repositories
{
    public class SQLUserRepository : IUserRepository
    {
        private readonly AppDbContext _dbContext;

        public SQLUserRepository(AppDbContext dbContext) { _dbContext = dbContext; }

        public async Task<List<UserDTO>> GetAllUsersAsync()
        {
            var users = await _dbContext.Users.ToListAsync();
            return users.Select(u => new UserDTO { Id = u.Id, Name = u.Name, FullName = u.FullName, Role = u.Role, Description = u.Description }).ToList();
        }

        public async Task<UserDTO?> GetUserByIdAsync(int id)
        {
            var user = await _dbContext.Users.FindAsync(id);
            if (user == null) return null;
            return new UserDTO { Id = user.Id, Name = user.Name, FullName = user.FullName, Role = user.Role, Description = user.Description };
        }

        public async Task<User> AddUserAsync(AddUserRequestDTO request)
        {
            // Lưu ý: Trong thực tế bạn nên Hash Password trước khi lưu
            var user = new User
            {
                Name = request.Name,
                FullName = request.FullName,
                PasswordHash = request.Password, // Nên dùng thư viện Hash (vd: BCrypt)
                Description = request.Description,
                Role = request.Role
            };
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            return user;
        }

        public async Task<User?> UpdateUserAsync(int id, AddUserRequestDTO request)
        {
            var user = await _dbContext.Users.FindAsync(id);
            if (user == null) return null;

            user.FullName = request.FullName;
            user.Description = request.Description;
            user.Role = request.Role;
            // Nếu có password mới thì update
            if (!string.IsNullOrEmpty(request.Password)) user.PasswordHash = request.Password;

            await _dbContext.SaveChangesAsync();
            return user;
        }

        public async Task<User?> DeleteUserAsync(int id)
        {
            var user = await _dbContext.Users.FindAsync(id);
            if (user == null) return null;
            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();
            return user;
        }
    }
}