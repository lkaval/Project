using OnlineShopWebApp.Data.Models;

namespace OnlineShopWebApp.Data.Repository.Orders
{
    public interface IOrdersRepository
    {
        void Add(Order order);
        List<Order> GetAll();
        Order TryGetById(Guid id);
        void UpdateOrderStatus(Guid orderId, OrderStatus newStatus);
    }
}