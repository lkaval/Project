namespace OnlineShopWebApp.Data.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        public UserDeliveryInfo User { get; set; }
        public List<CartItem> Items { get; set; }

        public OrderStatus Status { get; set; }
        public DateTime CreatedDateTime { get; set; }   
        public Order()
        {
            Id = Guid.NewGuid();
            Status = OrderStatus.Created;  
            CreatedDateTime = DateTime.Now;
        }
        public decimal TotalAmount
        {
            get
            {
                return Items?.Sum(item => item.Amount) ?? 0;
            }
        }
    }
}
