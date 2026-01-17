using System.ComponentModel.DataAnnotations.Schema;

namespace backend1.Models.Domain
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int Quantity { get; set; }

        // ✅ SỬA: Đưa lên trên
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }

        // Foreign Key
        public int ProductId { get; set; }
        public int OrderId { get; set; }

        // Navigation Property
        public Product? Product { get; set; }
        public Order? Order { get; set; }
    }
}