using backend1.Models.Domain;
using backend1.Models.DTO;

namespace backend1.Repositories
{
    public interface IReviewRepository
    {
        Task<List<ReviewDTO>> GetReviewsByProductIdAsync(int productId);
        Task<Review> AddReviewAsync(AddReviewDTO addReviewDTO);
        Task<Review?> UpdateReviewAsync(int id, AddReviewDTO request);
        Task<Review?> DeleteReviewAsync(int id);
    }
}