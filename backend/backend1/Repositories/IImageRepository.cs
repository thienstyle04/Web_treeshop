using backend1.Models.Domain;
using backend1.Models.DTO;

namespace backend1.Repositories
{
    public interface IImageRepository
    {
        Task<Image> UploadImageAsync(ImageDTO request);
        Task<Image?> DeleteImageAsync(int id);
    }
}