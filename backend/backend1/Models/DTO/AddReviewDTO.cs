namespace backend1.Models.DTO
{
    public class AddReviewDTO
    {
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public int ProductId { get; set; }
        public int UserId { get; set; }
    }
}
