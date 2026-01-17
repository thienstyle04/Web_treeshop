using System.ComponentModel.DataAnnotations;

namespace backend1.Models.DTO
{
    public class LoginRequestDTO
    {
        [Required(ErrorMessage = "Tên đăng nhập là bắt buộc")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
        public string Password { get; set; } = string.Empty;
    }
}
