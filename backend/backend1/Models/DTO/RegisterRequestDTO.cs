using System.ComponentModel.DataAnnotations;

namespace backend1.Models.DTO
{
    public class RegisterRequestDTO
    {
        [Required(ErrorMessage = "Tên đăng nhập là bắt buộc")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Họ tên là bắt buộc")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
        [MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự")]
        public string Password { get; set; } = string.Empty;

        public string? Description { get; set; }
    }
}
