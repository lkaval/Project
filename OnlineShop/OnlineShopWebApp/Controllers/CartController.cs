using Microsoft.AspNetCore.Mvc;
using OnlineShopWebApp.Data.Models;
using OnlineShopWebApp.Data.Repository.Carts;
using OnlineShopWebApp.Data.Repository.Products;

public class CartController : Controller
{
    private readonly IProductsRepository _productsRepository;
    private readonly ICartsRepository _cartsRepository;

    public CartController(Cart cart, IProductsRepository productsRepository, ICartsRepository cartsRepository)
    {
        _productsRepository = productsRepository;
        _cartsRepository = cartsRepository;
    }

    public IActionResult Index()
    {
        var cart = _cartsRepository.TryGetByUserID(Constants.UserId);
        return View(cart);
    }

    public IActionResult Add(int productId)
    {
        var product = _productsRepository.TryGetById(productId);
        _cartsRepository.Add(product, Constants.UserId);
        return RedirectToAction("Index");
    }
    public IActionResult DecreaseAmount(int productId)
    {
        _cartsRepository.DecreaseAmount(productId, Constants.UserId);
        return RedirectToAction("Index");
    }
    public IActionResult Clear()
    {
        _cartsRepository.Clear(Constants.UserId);

        return RedirectToAction("Index");
    }
}