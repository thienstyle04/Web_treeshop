namespace backend1.Models.Domain
{
    public class ShoppingCartItem
    {
        public int Id { get; set; }
        public int UserId { get; set; } // id nguoi dung
        public int ProductId { get; set; } // id san pham
        public int Quantity { get; set; } // so luong
        public DateTime DateCreated { get; set; } // ngay tao

        // Navigation Property
        public Product? Product { get; set; }
        public User? User { get; set; }
    }
}
