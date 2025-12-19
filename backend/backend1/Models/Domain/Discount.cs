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

        public DateTime ExpiryDate { get; set; }
        public bool IsActive { get; set; }
    }
}