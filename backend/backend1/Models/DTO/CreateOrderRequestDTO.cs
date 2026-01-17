namespace backend1.Models.DTO
{
    public class CreateOrderRequestDTO
    {
        public int UserId { get; set; }
        public int? ShippingAddressId { get; set; } // Optional, if null will use inline address
        
        // Inline shipping address (used when ShippingAddressId is null)
        public string? RecipientName { get; set; }
        public string? Phone { get; set; }
        public string? StreetAddress { get; set; }
        public string? City { get; set; }
        
        public string? DiscountCodeUsed { get; set; }
        public decimal DiscountAmount { get; set; }
        public List<CreateOrderItemDTO>? Items { get; set; }
    }
}
