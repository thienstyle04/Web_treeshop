namespace backend1.Models.DTO
{
    public class ReviewDTO
    {
        public int Id { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime ReviewDate { get; set; }
        public string? UserName { get; set; } // Hiển thị tên người review
        public int ProductId { get; set; }
        public int UserId { get; set; }
    }
}
