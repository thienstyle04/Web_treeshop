using System.ComponentModel.DataAnnotations.Schema;

namespace backend1.Models.Domain
{
    public class Image
    {
        public int Id { get; set; }
        public string? FileName { get; set; }
        public string? FilePath { get; set; }  // URL truy cập công khai
        public bool IsThumbnail { get; set; } // ảnh đại diện
        public int ProductId { get; set; }
        public Product? Product { get; set; }

        // dung cho upload ảnh nhưng không thay đổi database
        [NotMapped]
        public IFormFile? File { get; set; }
    }
}
