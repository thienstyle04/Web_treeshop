namespace backend1.Models.DTO
{
    public class CreateOrderRequestDTO
    {
        public int UserId { get; set; }
        public int ShippingAddressId { get; set; }
        public string? DiscountCodeUsed { get; set; }
        public decimal DiscountAmount { get; set; }
        public List<CreateOrderItemDTO>? Items { get; set; }
    }
}
