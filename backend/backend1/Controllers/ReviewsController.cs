using backend1.Models.DTO;
using backend1.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewRepository _reviewRepository;
        public ReviewsController(IReviewRepository reviewRepository) { _reviewRepository = reviewRepository; }

        [HttpGet("product/{productId:int}")]
        public async Task<IActionResult> GetByProduct(int productId)
        {
            return Ok(await _reviewRepository.GetReviewsByProductIdAsync(productId));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddReviewDTO request)
        {
            var review = await _reviewRepository.AddReviewAsync(request);
            return Ok(review);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] AddReviewDTO request)
        {
            var review = await _reviewRepository.UpdateReviewAsync(id, request);
            return review == null ? NotFound() : Ok(review);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var review = await _reviewRepository.DeleteReviewAsync(id);
            return review == null ? NotFound() : Ok(review);
        }
    }
}