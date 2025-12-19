namespace backend1.Models.DTO
{
    public class AddDiscountRequestDTO
    {
        public string? Code { get; set; }
        public string? DiscountType { get; set; } // e.g. "Percent", "Fixed"
        public decimal Value { get; set; }
        public decimal MinimumOrderAmount { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool IsActive { get; set; }
    }
}