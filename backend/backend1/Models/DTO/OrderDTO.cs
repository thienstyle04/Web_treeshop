namespace backend1.Models.DTO
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string? OrderStatus { get; set; }
        public int UserId { get; set; }
        public string? RecipientName { get; set; } // Lấy từ ShippingAddress
        public List<OrderDetailItemDTO> OrderItems { get; set; }
    }
}
