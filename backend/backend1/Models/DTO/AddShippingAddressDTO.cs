namespace backend1.Models.DTO
{
    public class AddShippingAddressDTO
    {
        public int UserId { get; set; }
        public string? RecipientName { get; set; }
        public string? StreetAddress { get; set; }
        public string? City { get; set; }
        public string? Phone { get; set; }
        public bool IsDefault { get; set; }
    }
}
