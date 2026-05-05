using Microsoft.AspNetCore.Mvc;
using OnlineShopWebApp.Data;
using OnlineShopWebApp.Data.Models;
using OnlineShopWebApp.Data.Repository.Carts;
using OnlineShopWebApp.Data.Repository.Orders;
using OnlineShopWebApp.Services;

namespace OnlineShopWebApp.Controllers
{
    public class OrderController : Controller
    {
        private readonly ICartsRepository _cartsRepository;
        private readonly IOrdersRepository _ordersRepository;
        private readonly IEmailService _emailService;
        private readonly AppDbContext _context;

        public OrderController(
            ICartsRepository cartsRepository,
            IOrdersRepository ordersRepository,
            IEmailService emailService,
            AppDbContext context)
        {
            _cartsRepository = cartsRepository;
            _ordersRepository = ordersRepository;
            _emailService = emailService;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Buy(UserDeliveryInfo user)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", user);
            }

            var existingCart = _cartsRepository.TryGetByUserID(Constants.UserId);
            if (existingCart == null || !existingCart.Items.Any())
            {
                ModelState.AddModelError("", "Корзина пуста или не найдена.");
                return View("Index", user);
            }

            var orderItems = existingCart.Items.Select(ci => new OrderItem
            {
                Id = Guid.NewGuid(),
                ProductId = ci.ProductId,
                Quantity = ci.Quantity,
                Amount = ci.Amount
            }).ToList();

            var order = new Order
            {
                User = user,
                Items = orderItems
            };

            _ordersRepository.Add(order);

            // Генерируем ключ для каждой единицы каждого товара
            var keyRecords = new List<ProductKey>();
            var keysForEmail = new List<(string ProductName, string Key)>();

            foreach (var cartItem in existingCart.Items)
            {
                for (int i = 0; i < cartItem.Quantity; i++)
                {
                    var keyValue = EmailService.GenerateKey();
                    keyRecords.Add(new ProductKey
                    {
                        Key = keyValue,
                        ProductId = cartItem.ProductId,
                        OrderId = order.Id
                    });
                    keysForEmail.Add((cartItem.Product.Name, keyValue));
                }
            }

            _context.ProductKeys.AddRange(keyRecords);
            await _context.SaveChangesAsync();

            _cartsRepository.Clear(Constants.UserId);

            // Отправляем email в фоне — не блокируем ответ при ошибке SMTP
            _ = _emailService.SendOrderKeysAsync(user.Email, user.Name, order.Id, keysForEmail)
                             .ContinueWith(t => { /* ошибка логируется Serilog через middleware */ },
                                           TaskContinuationOptions.OnlyOnFaulted);

            TempData["OrderEmail"] = user.Email;
            return View("Buy");
        }
    }
}
