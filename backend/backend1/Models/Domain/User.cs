namespace backend1.Models.Domain
{
    public class User
    {
        public int Id { get; set; } // Khóa chính (PK), kiểu INT
        public string? Name { get; set; } // Tên đăng nhập (Username)
        public string? FullName { get; set; }
        public string? Description { get; set; }
        public string? Role { get; set; } // Phân quyền (e.g., "Admin", "Customer")
        public string? PasswordHash { get; set; } // Cần thiết để lưu mật khẩu băm

        // Navigation Properties
        public List<Order> Orders { get; set; } = new List<Order>();
        public List<Review> Reviews { get; set; } = new List<Review>();
        public List<ShippingAddress> ShippingAddresses { get; set; } = new List<ShippingAddress>();
        public List<ShoppingCartItem> ShoppingCartItems { get; set; } = new List<ShoppingCartItem>();
    }
}
