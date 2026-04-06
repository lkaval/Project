using Microsoft.AspNetCore.Mvc;
using OnlineShopWebApp.Data.Repository.Products;

namespace OnlineShopWebApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductsRepository _productRepository;

        public ProductController(IProductsRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public IActionResult Index()
        {
            var products = _productRepository.GetAll();
            return View(products);
        }

        public IActionResult Details(int id)
        {
            var product = _productRepository.TryGetById(id);

            return View(product);
        }

    }
}