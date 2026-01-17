using System.ComponentModel.DataAnnotations.Schema;

namespace backend1.Models.Domain
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        public string? OrderStatus { get; set; } // trang thai don hang

        //FK: nguoi dung dat hang
        public int UserId { get; set; }

        //KP: dia chi giao hang
        public int ShippingAddressId { get; set; }
        public ShippingAddress? ShippingAddress { get; set; }
        public string? DiscountCodeUsed { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal DiscountAmount { get; set; }

        // navigation property 
        public User? User { get; set; }
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}