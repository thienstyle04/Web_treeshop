namespace backend1.Models.DTO
{
    public class ImageDTO
    {
        public IFormFile? File { get; set; } // File ảnh upload
        public string? FileName { get; set; }
        public bool IsThumbnail { get; set; } = false;
        public int ProductId { get; set; }
    }
}
