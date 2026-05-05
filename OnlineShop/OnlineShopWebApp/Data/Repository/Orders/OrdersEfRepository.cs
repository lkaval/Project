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

        public void UpdateOrder(Areas.Admin.Models.OrderEditViewModel model)
        {
            var order = _context.Orders.Include(o => o.Items).FirstOrDefault(o => o.Id == model.OrderId);
            if (order == null) return;

            order.Status = model.Status;

            var delivery = _context.UserDeliveryInfos.Find(model.DeliveryInfoId);
            if (delivery != null)
            {
                delivery.Name = model.RecipientName;
                delivery.Phone = model.Phone;
                delivery.Address = model.Address;
                delivery.Email = model.Email;
            }

            foreach (var dto in model.Items)
            {
                var item = order.Items.FirstOrDefault(i => i.Id == dto.ItemId);
                if (item == null) continue;

                if (dto.Delete)
                {
                    _context.OrderItems.Remove(item);
                }
                else if (dto.Quantity > 0)
                {
                    item.Quantity = dto.Quantity;
                    item.Amount = dto.UnitPrice * dto.Quantity;
                }
            }

            _context.SaveChanges();
        }
    }
}