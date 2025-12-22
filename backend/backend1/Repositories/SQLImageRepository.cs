using backend1.Data;
using backend1.Models.Domain;
using backend1.Models.DTO;

namespace backend1.Repositories
{
    public class SQLImageRepository : IImageRepository
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AppDbContext _dbContext;

        public SQLImageRepository(IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor, AppDbContext dbContext)
        {
            _webHostEnvironment = webHostEnvironment;
            _httpContextAccessor = httpContextAccessor;
            _dbContext = dbContext;
        }

        public async Task<Image> UploadImageAsync(ImageDTO request)
        {
            var file = request.File;

            // SỬA 1: Sử dụng WebRootPath (wwwroot) thay vì ContentRootPath
            string webRootPath = _webHostEnvironment.WebRootPath;

            // Nếu WebRootPath null (do chưa cấu hình đúng), fallback về ContentRootPath/wwwroot
            if (string.IsNullOrWhiteSpace(webRootPath))
            {
                webRootPath = Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot");
            }

            // Tạo đường dẫn tới thư mục Images bên trong wwwroot
            var imagesFolderPath = Path.Combine(webRootPath, "Images");

            // SỬA 2: Kiểm tra và tạo thư mục nếu chưa tồn tại (Tránh lỗi DirectoryNotFound)
            if (!Directory.Exists(imagesFolderPath))
            {
                Directory.CreateDirectory(imagesFolderPath);
            }

            var localFilePath = Path.Combine(imagesFolderPath, $"{request.FileName}{Path.GetExtension(file?.FileName)}");

            // Upload file to local folder
            using var stream = new FileStream(localFilePath, FileMode.Create);
            await file.CopyToAsync(stream);

            // ... giữ nguyên phần tạo URL path và lưu DB
            var urlFilePath = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}/Images/{request.FileName}{Path.GetExtension(file.FileName)}";

            var image = new Image
            {
                FileName = request.FileName,
                FilePath = urlFilePath,
                IsThumbnail = request.IsThumbnail,
                ProductId = request.ProductId
            };

            await _dbContext.Images.AddAsync(image);
            await _dbContext.SaveChangesAsync();
            return image;
        }

        public async Task<Image?> DeleteImageAsync(int id)
        {
            var image = await _dbContext.Images.FindAsync(id);
            if (image == null) return null;
            _dbContext.Images.Remove(image);
            await _dbContext.SaveChangesAsync();
            return image;
        }
    }
}