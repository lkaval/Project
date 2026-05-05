using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        public IActionResult UpdateOrderStatus(Guid orderId, OrderStatus status)
        {
            _ordersRepository.UpdateOrderStatus(orderId, status);
            return RedirectToAction(nameof(Index));
        }


    }
}