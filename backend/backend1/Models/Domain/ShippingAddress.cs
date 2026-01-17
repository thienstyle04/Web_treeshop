namespace backend1.Models.Domain
{
    public class ShippingAddress
    {
        public int Id { get; set; }
        public int UserId { get; set; } // id nguoi dung
        public string? RecipientName { get; set; } // ten nguoi nhan
        public string? StreetAddress { get; set; } // dia chi
        public string? City { get; set; } // thanh pho
        public string? Phone { get; set; } // so dien thoai
        public bool IsDefault { get; set; } // dia chi mac dinh

        // navigation property
        public User? User { get; set; }
        public List<Order> Orders { get; set; } = new List<Order>();
    }
}
