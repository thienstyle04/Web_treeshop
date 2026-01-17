namespace backend1.Models.DTO
{
    public class AddDiscountRequestDTO
    {
        public string? Code { get; set; }
        public string? DiscountType { get; set; } // e.g. "Percent", "Fixed"
        public decimal Value { get; set; }
        public decimal MinimumOrderAmount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int? AppliesToProductId { get; set; }
        public int UsageLimit { get; set; }
        public bool IsActive { get; set; }
    }
}