using backend1.Data;
using backend1.Models.Domain;
using backend1.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace backend1.Repositories
{
    public class SQLReviewRepository : IReviewRepository
    {
        private readonly AppDbContext _dbContext;
        public SQLReviewRepository(AppDbContext dbContext) { _dbContext = dbContext; }

        public async Task<List<ReviewDTO>> GetReviewsByProductIdAsync(int productId)
        {
            var reviews = await _dbContext.Reviews
                .Include(r => r.User)
                .Where(r => r.ProductId == productId)
                .ToListAsync();

            return reviews.Select(r => new ReviewDTO
            {
                Id = r.Id,
                Rating = r.Rating,
                Comment = r.Comment,
                ReviewDate = r.ReviewDate,
                ProductId = r.ProductId,
                UserName = r.User?.FullName ?? "Unknown"
            }).ToList();
        }

        public async Task<Review> AddReviewAsync(AddReviewDTO request)
        {
            var review = new Review
            {
                Rating = request.Rating,
                Comment = request.Comment,
                ProductId = request.ProductId,
                UserId = request.UserId,
                ReviewDate = DateTime.Now
            };
            await _dbContext.Reviews.AddAsync(review);
            await _dbContext.SaveChangesAsync();
            return review;
        }

        public async Task<Review?> DeleteReviewAsync(int id)
        {
            var review = await _dbContext.Reviews.FindAsync(id);
            if (review == null) return null;
            _dbContext.Reviews.Remove(review);
            await _dbContext.SaveChangesAsync();
            return review;
        }
    }
}