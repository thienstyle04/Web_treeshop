namespace backend1.Models.DTO
{

    // DTO cho việc hiển thị chi tiết hoặc danh sách sản phẩm
    public class ProductDetailsDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? ScientificName { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public DateTime DateAdded { get; set; }

        // Dữ liệu từ Navigation Properties
        public string? CategoryName { get; set; }
        public List<string> ImageUrls { get; set; } = new List<string>();
    }
}
