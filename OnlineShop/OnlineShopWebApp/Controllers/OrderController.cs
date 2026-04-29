using Microsoft.AspNetCore.Mvc;
using OnlineShopWebApp.Data.Models;
using OnlineShopWebApp.Data.Repository.Carts;
using OnlineShopWebApp.Data.Repository.Orders;
using System.Linq;

namespace OnlineShopWebApp.Controllers
{
    public class OrderController : Controller
    {
        private readonly ICartsRepository _cartsRepository;
        private readonly IOrdersRepository _ordersRepository;

        public OrderController(ICartsRepository cartsRepository, IOrdersRepository ordersRepository)
        {
            _cartsRepository = cartsRepository;
            _ordersRepository = ordersRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Buy(UserDeliveryInfo user)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", user);
            }

            // 1. Получаем корзину с проверкой на null
            var existingCart = _cartsRepository.TryGetByUserID(Constants.UserId);
            if (existingCart == null || !existingCart.Items.Any())
            {
                ModelState.AddModelError("", "Корзина пуста или не найдена.");
                return View("Index", user);
            }

            // 2. Маппинг CartItem → OrderItem (разные таблицы!)
            var orderItems = existingCart.Items.Select(ci => new OrderItem
            {
                Id = Guid.NewGuid(),
                ProductId = ci.ProductId,
                Quantity = ci.Quantity,
                Amount = ci.Amount // Фиксируем цену на момент покупки
            }).ToList();

            // 3. Создаём заказ
            var order = new Order
            {
                User = user,      // EF Core автоматически сохранит DeliveryInfo и проставит FK
                Items = orderItems
            };

            // 4. Сохраняем заказ в БД
            _ordersRepository.Add(order);

            // 5. Очищаем корзину пользователя
            _cartsRepository.Clear(Constants.UserId);

            // 6. Перенаправляем на страницу успеха (или оставьте View(), если у вас есть Success.cshtml)
            return View("Success");
        }
    }
}