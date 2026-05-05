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

    private string GetUserId()
    {
        if (User.Identity?.IsAuthenticated == true)
            return User.Identity.Name!;

        var anonId = HttpContext.Session.GetString("AnonCartId");
        if (string.IsNullOrEmpty(anonId))
        {
            anonId = Guid.NewGuid().ToString();
            HttpContext.Session.SetString("AnonCartId", anonId);
        }
        return anonId;
    }

    public IActionResult Index()
    {
        var cart = _cartsRepository.TryGetByUserID(GetUserId());
        return View(cart);
    }

    public IActionResult Add(int productId)
    {
        var product = _productsRepository.TryGetById(productId);
        _cartsRepository.Add(product, GetUserId());
        return RedirectToAction("Index");
    }

    public IActionResult DecreaseAmount(int productId)
    {
        _cartsRepository.DecreaseAmount(productId, GetUserId());
        return RedirectToAction("Index");
    }

    public IActionResult Clear()
    {
        _cartsRepository.Clear(GetUserId());
        return RedirectToAction("Index");
    }
}
