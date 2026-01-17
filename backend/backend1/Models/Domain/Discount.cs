using System.ComponentModel.DataAnnotations.Schema;

namespace backend1.Models.Domain
{
    public class Discount
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public string? DiscountType { get; set; }

        // ✅ SỬA: Đưa lên trên
        [Column(TypeName = "decimal(18,2)")]
        public decimal Value { get; set; }

        // ✅ SỬA: Đưa lên trên
        [Column(TypeName = "decimal(18,2)")]
        public decimal MinimumOrderAmount { get; set; }

        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime EndDate { get; set; }

        // Null -> Applies to order total
        // Not Null -> Applies to specific product automatically (Flash Sale)
        public int? AppliesToProductId { get; set; }

        public int UsageLimit { get; set; } // 0 = Unlimited
        public int UsedCount { get; set; }

        public bool IsActive { get; set; }
    }
}