namespace backend1.Models.DTO
{
    public class AddUserRequestDTO
    {
        public string? Name { get; set; }
        public string? FullName { get; set; }
        public string? Password { get; set; } // Nhận password thô để xử lý
        public string? Description { get; set; }
        public string? Role { get; set; } = "Customer";
    }
}
