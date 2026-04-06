namespace OnlineShopWebApp.Data.Models
{
    public class Cart
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public List<CartItem> Items { get; set; } = new List<CartItem>();
        public decimal TotalAmount
        {
            get
            {
                return Items?.Sum(item => item.Amount) ?? 0;
            }
        }
        public decimal Quantity
        {
            get
            {
                return Items?.Sum(item => item.Quantity) ?? 0;
            }
        }
    }

}
