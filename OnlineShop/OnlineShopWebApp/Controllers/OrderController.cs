using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShopWebApp.Data;
using OnlineShopWebApp.Data.Models;
using OnlineShopWebApp.Data.Repository.Carts;
using OnlineShopWebApp.Data.Repository.Orders;
using OnlineShopWebApp.Services;
using System.Text.Json;

namespace OnlineShopWebApp.Controllers
{
    [Authorize]
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
        public IActionResult ConfirmDelivery(UserDeliveryInfo user)
        {
            if (!ModelState.IsValid)
                return View("Index", user);

            var cart = _cartsRepository.TryGetByUserID(User.Identity!.Name!);
            if (cart == null || !cart.Items.Any())
            {
                ModelState.AddModelError("", "Корзина пуста или не найдена.");
                return View("Index", user);
            }

            HttpContext.Session.SetString("DeliveryInfo", JsonSerializer.Serialize(user));

            return RedirectToAction(nameof(Payment));
        }

        public IActionResult Payment()
        {
            var json = HttpContext.Session.GetString("DeliveryInfo");
            if (json == null)
                return RedirectToAction(nameof(Index));

            var cart = _cartsRepository.TryGetByUserID(User.Identity!.Name!);
            if (cart == null || !cart.Items.Any())
                return RedirectToAction("Index", "Cart");

            ViewBag.Total = cart.Items.Sum(i => i.Amount);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Pay()
        {
            var json = HttpContext.Session.GetString("DeliveryInfo");
            if (json == null)
                return RedirectToAction(nameof(Index));

            var user = JsonSerializer.Deserialize<UserDeliveryInfo>(json)!;
            user.Id = 0;

            var existingCart = _cartsRepository.TryGetByUserID(User.Identity!.Name!);
            if (existingCart == null || !existingCart.Items.Any())
                return RedirectToAction("Index", "Cart");

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

            _cartsRepository.Clear(User.Identity!.Name!);
            HttpContext.Session.Remove("DeliveryInfo");

            _ = _emailService.SendOrderKeysAsync(user.Email, user.Name, order.Id, keysForEmail)
                             .ContinueWith(t => { }, TaskContinuationOptions.OnlyOnFaulted);

            TempData["OrderEmail"] = user.Email;
            return View("Buy");
        }
    }
}
