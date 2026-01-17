using System.ComponentModel.DataAnnotations.Schema;

namespace backend1.Models.Domain
{
    public class Product
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? ScientificName { get; set; } // ten khoa hoc
        public string? Description { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }// ton kho
        public DateTime DateAdded { get; set; }
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        // navigation property // thuoc tinh dieu huong
        public List<Image> Images { get; set; } = new List<Image>();
        public List<Review> reviews { get; set; } = new List<Review>();

        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
