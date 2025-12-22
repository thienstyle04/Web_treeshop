using backend1.Models.Domain;
using backend1.Models.DTO;

namespace backend1.Repositories
{
    public interface IAuthRepository
    {
        Task<User?> RegisterAsync(RegisterRequestDTO request);
        Task<LoginResponseDTO?> LoginAsync(LoginRequestDTO request);
        Task<User?> GetUserByNameAsync(string name);
    }
}
