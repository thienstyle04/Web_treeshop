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
        public decimal? OriginalPrice { get; set; }
        public double? DiscountPercentage { get; set; }
        public bool IsFlashSale { get; set; }
        public DateTime? FlashSaleEndTime { get; set; }

        public int StockQuantity { get; set; }
        public DateTime DateAdded { get; set; }

        // Dữ liệu từ Navigation Properties
        public string? CategoryName { get; set; }
        public int CategoryId { get; set; }
        public List<string> ImageUrls { get; set; } = new List<string>();
        public List<ReviewDTO> Reviews { get; set; } = new List<ReviewDTO>();
    }
}
