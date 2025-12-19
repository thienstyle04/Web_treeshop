namespace backend1.Models.Domain
{
    public class Category
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? UrlHandler { get; set; } // tao url thân thiện
        // navigation property
        public string? Description { get; set; }
        public List<Product> Products { get; set; } = new List<Product>();
    }
}

