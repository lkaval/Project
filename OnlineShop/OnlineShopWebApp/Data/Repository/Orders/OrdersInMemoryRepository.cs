using OnlineShopWebApp.Areas.Admin.Models;
using OnlineShopWebApp.Data.Models;

namespace OnlineShopWebApp.Data.Repository.Orders
{
    public class OrdersInMemoryRepository : IOrdersRepository
    {
        private List<Order> orders = new List<Order>();



        public void Add(Order order)
        {
            orders.Add(order);
        }
        public List<Order> GetAll()
        {
            return orders;
        }

        public Order TryGetById(Guid id)
        {
            return orders.FirstOrDefault(x => x.Id == id);
        }

        public void UpdateOrderStatus(Guid orderId, OrderStatus newStatus)
        {
            var order = TryGetById(orderId);
            if (order != null)
                order.Status = newStatus;
        }

        public void UpdateOrder(OrderEditViewModel model)
        {
            var order = TryGetById(model.OrderId);
            if (order == null) return;
            order.Status = model.Status;
        }
    }
}