using Microsoft.AspNetCore.Mvc;
using OnlineShopWebApp.Data.Models;
using OnlineShopWebApp.Data.Repository.Carts;

namespace OnlineShopWebApp.Views.Shared.Components.CartViewComponents
{
    public class CartViewComponent : ViewComponent
    {
        private readonly ICartsRepository _cartRepository;

        public CartViewComponent(ICartsRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public IViewComponentResult Invoke()
        {
            var cart = _cartRepository.TryGetByUserID(Constants.UserId);

            if (cart == null)
            {
                return Content(string.Empty);
            }
            var productCounts = cart.Quantity;
            return View("Cart",productCounts);
        }
    }
}
