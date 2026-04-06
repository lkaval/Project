using Microsoft.AspNetCore.Mvc;
using OnlineShopWebApp.Data.Models;
using OnlineShopWebApp.Data.Repository.Carts;
using OnlineShopWebApp.Data.Repository.Orders;

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

            var existingCart = _cartsRepository.TryGetByUserID(Constants.UserId);
            var order = new Order
            {
                User = user,
                Items = existingCart.Items
            };
            _ordersRepository.Add(order);

            _cartsRepository.Clear(Constants.UserId);
            return View();
        }
    }
}
