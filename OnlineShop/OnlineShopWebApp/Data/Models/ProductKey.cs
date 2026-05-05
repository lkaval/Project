namespace OnlineShopWebApp.Data.Models
{
    public class ProductKey
    {
        public int Id { get; set; }
        public string Key { get; set; } = string.Empty;
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
        public Guid OrderId { get; set; }
    }
}
