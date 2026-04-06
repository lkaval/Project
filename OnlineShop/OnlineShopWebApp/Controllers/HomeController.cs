using Microsoft.AspNetCore.Mvc;
using OnlineShopWebApp.Data.Repository.Carts;
using OnlineShopWebApp.Data.Repository.Products;

namespace OnlineShopWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductsRepository _productRepository;
        private readonly ICartsRepository _cartRepository;

        public HomeController(IProductsRepository productRepository, ICartsRepository cartRepository)
        {
            _productRepository = productRepository;
            _cartRepository = cartRepository;
        }

        public IActionResult Index()
        {
            var products = _productRepository.GetAll();

            return View(products);
        }
    }
}