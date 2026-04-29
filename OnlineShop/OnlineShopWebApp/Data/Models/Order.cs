using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace OnlineShopWebApp.Data.Models
{
    public class Order
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        // 🔹 Foreign Key для связи с UserDeliveryInfo
        public int DeliveryInfoId { get; set; }

        // 🔹 Вернули имя "User", чтобы ваши контроллеры не падали
        [ForeignKey(nameof(DeliveryInfoId))]
        public UserDeliveryInfo User { get; set; } = new();

        // 🔹 Теперь тут OrderItem, а не CartItem
        public List<OrderItem> Items { get; set; } = new();

        public OrderStatus Status { get; set; } = OrderStatus.Created;
        public DateTime CreatedDateTime { get; set; } = DateTime.UtcNow;

        public decimal TotalAmount => Items?.Sum(i => i.Amount) ?? 0;
    }
}