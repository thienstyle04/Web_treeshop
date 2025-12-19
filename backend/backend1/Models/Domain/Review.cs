namespace backend1.Models.Domain
{
    public class Review
    {
        public int Id { get; set; }
        public int Rating { get; set; } // Đánh giá sao (1-5)
        public string? Comment { get; set; } // Bình luận của khách hàng
        public DateTime ReviewDate { get; set; } // Ngày tạo đánh giá

        // Foreign Keys
        public int ProductId { get; set; }
        public int UserId { get; set; }
        // Navigation Properties
        public Product? Product { get; set; }
        public User? User { get; set; }
    }
}
