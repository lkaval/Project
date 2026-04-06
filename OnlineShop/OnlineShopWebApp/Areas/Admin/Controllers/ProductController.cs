using Microsoft.AspNetCore.Mvc;
using OnlineShopWebApp.Data.Models;
using OnlineShopWebApp.Data.Repository.Products;

namespace OnlineShopWebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IProductsRepository _productsRepository;

        public ProductController(IProductsRepository productsRepository)
        {
            _productsRepository = productsRepository;

        }

        public IActionResult Index()
        {
            var products = _productsRepository.GetAll();
            return View(products);
        }
        public IActionResult Add()
        {
            return View();

        }
        [HttpPost]
        public IActionResult Add(Product product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }

            _productsRepository.Add(product);

            return RedirectToAction(nameof(Index));

        }
        public IActionResult Edit(int productId)
        {
            var product = _productsRepository.TryGetById(productId);
            return View(product);

        }
        [HttpPost]
        public IActionResult Edit(Product product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }

            _productsRepository.Update(product);

            return RedirectToAction(nameof(Index));

        }

    }
}