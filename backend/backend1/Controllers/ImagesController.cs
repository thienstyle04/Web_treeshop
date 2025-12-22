using backend1.Models.DTO;
using backend1.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository _imageRepository;
        public ImagesController(IImageRepository imageRepository) { _imageRepository = imageRepository; }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] ImageDTO request)
        {
            if (request.File == null || request.File.Length == 0) return BadRequest("File is empty");
            var image = await _imageRepository.UploadImageAsync(request);
            return Ok(image);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var image = await _imageRepository.DeleteImageAsync(id);
            return image == null ? NotFound() : Ok(image);
        }
    }
}