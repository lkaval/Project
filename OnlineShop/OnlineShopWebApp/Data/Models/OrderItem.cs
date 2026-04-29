namespace OnlineShopWebApp.Data.Models
{
    public class OrderItem
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public Order Order { get; set; } = null!;
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
        public int Quantity { get; set; }
        // 💡 Для заказов цену лучше фиксировать в момент покупки, 
        // чтобы изменения цены товара в каталоге не меняли историю заказов.
        public decimal Amount { get; set; }
    }
}