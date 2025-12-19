namespace backend1.Models.DTO
{
    public class AddToCartRequestDTO
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
