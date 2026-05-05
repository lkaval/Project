using Microsoft.EntityFrameworkCore;
using OnlineShopWebApp.Data.Models;

namespace OnlineShopWebApp.Data.Repository.Orders
{
    public class OrdersEfRepository : IOrdersRepository
    {
        private readonly AppDbContext _context;
        public OrdersEfRepository(AppDbContext context) => _context = context;

        public void Add(Order order) { _context.Orders.Add(order); _context.SaveChanges(); }

        public List<Order> GetAll() =>
            _context.Orders
                    .Include(o => o.Items)
                    .Include(o => o.User)
                    .AsNoTracking()
                    .ToList();

        public Order TryGetById(Guid id) =>
            _context.Orders
                    .Include(o => o.Items).ThenInclude(i => i.Product)
                    .Include(o => o.User)
                    .AsNoTracking()
                    .FirstOrDefault(o => o.Id == id);

        public void UpdateOrderStatus(Guid orderId, OrderStatus newStatus)
        {
            var order = _context.Orders.Find(orderId);
            if (order != null) { order.Status = newStatus; _context.SaveChanges(); }
        }
    }
}