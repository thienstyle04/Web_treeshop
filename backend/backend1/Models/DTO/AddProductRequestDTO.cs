using System.ComponentModel.DataAnnotations;

namespace backend1.Models.DTO
{
    public class AddProductRequestDTO
    {
        [Required(ErrorMessage = "Tên sản phẩm là bắt buộc")]
        [MinLength(3, ErrorMessage = "Tên sản phẩm phải có ít nhất 3 ký tự")]
        public string? Name { get; set; }

        public string? ScientificName { get; set; }
        public string? Description { get; set; }

        [Required(ErrorMessage = "Giá là bắt buộc")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Giá phải lớn hơn 0")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Số lượng tồn kho là bắt buộc")]
        [Range(0, int.MaxValue, ErrorMessage = "Số lượng tồn kho không hợp lệ")]
        public int StockQuantity { get; set; }

        // Mặc định có thể là DateTime.Now, nhưng vẫn để cho phép client gửi lên
        public DateTime DateAdded { get; set; }

        [Required(ErrorMessage = "Phải cung cấp CategoryId")]
        public int CategoryId { get; set; }

        // Optional: URL ảnh sản phẩm
        public string? ImageUrl { get; set; }
    }
}
