using backend1.Models.Domain;
using backend1.Models.DTO;

namespace backend1.Repositories
{
    public interface IUserRepository
    {
        Task<List<UserDTO>> GetAllUsersAsync();
        Task<UserDTO?> GetUserByIdAsync(int id);
        Task<User> AddUserAsync(AddUserRequestDTO addUserDTO);
        Task<User?> UpdateUserAsync(int id, AddUserRequestDTO updateUserDTO);
        Task<User?> DeleteUserAsync(int id);
    }
}
