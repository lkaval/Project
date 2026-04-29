namespace OnlineShopWebApp.Data.Models
{
    public class Cart
    {
        public Guid Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public List<CartItem> Items { get; set; } = new();

        public decimal TotalAmount => Items?.Sum(i => i.Amount) ?? 0;
        public decimal Quantity => Items?.Sum(i => i.Quantity) ?? 0;
    }
}