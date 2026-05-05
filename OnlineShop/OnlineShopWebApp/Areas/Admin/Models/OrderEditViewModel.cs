using OnlineShopWebApp.Data.Models;

namespace OnlineShopWebApp.Areas.Admin.Models
{
    public class OrderEditViewModel
    {
        public Guid OrderId { get; set; }
        public OrderStatus Status { get; set; }

        public int DeliveryInfoId { get; set; }
        public string RecipientName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public List<OrderItemEditDto> Items { get; set; } = new();
    }

    public class OrderItemEditDto
    {
        public Guid ItemId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public bool Delete { get; set; }
    }
}
