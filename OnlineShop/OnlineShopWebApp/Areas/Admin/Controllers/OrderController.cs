using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShopWebApp.Areas.Admin.Models;
using OnlineShopWebApp.Data.Models;
using OnlineShopWebApp.Data.Repository.Orders;

namespace OnlineShopWebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {
        private readonly IOrdersRepository _ordersRepository;

        public OrderController(IOrdersRepository ordersRepository)
        {
            _ordersRepository = ordersRepository;
        }

        public IActionResult Index()
        {
            var orders = _ordersRepository.GetAll();
            return View(orders);
        }

        public IActionResult Detail(Guid orderId)
        {
            var order = _ordersRepository.TryGetById(orderId);
            return View(order);
        }

        public IActionResult Edit(Guid orderId)
        {
            var order = _ordersRepository.TryGetById(orderId);
            if (order == null) return NotFound();

            var vm = new OrderEditViewModel
            {
                OrderId = order.Id,
                Status = order.Status,
                DeliveryInfoId = order.DeliveryInfoId,
                RecipientName = order.User?.Name ?? string.Empty,
                Phone = order.User?.Phone ?? string.Empty,
                Address = order.User?.Address ?? string.Empty,
                Email = order.User?.Email ?? string.Empty,
                Items = order.Items.Select(i => new OrderItemEditDto
                {
                    ItemId = i.Id,
                    ProductName = i.Product?.Name ?? $"Товар #{i.ProductId}",
                    UnitPrice = i.Quantity > 0 ? Math.Round(i.Amount / i.Quantity, 2) : i.Amount,
                    Quantity = i.Quantity
                }).ToList()
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(OrderEditViewModel model)
        {
            _ordersRepository.UpdateOrder(model);
            return RedirectToAction(nameof(Index));
        }

        // Оставляем для обратной совместимости со старой кнопкой в Detail
        public IActionResult UpdateOrderStatus(Guid orderId, OrderStatus status)
        {
            _ordersRepository.UpdateOrderStatus(orderId, status);
            return RedirectToAction(nameof(Index));
        }
    }
}
