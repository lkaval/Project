using Microsoft.AspNetCore.Mvc;
using OnlineShopWebApp.Data.Models;

namespace OnlineShopWebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUsersManager _usersManager;

        public AccountController(IUsersManager usersManager)
        {
            _usersManager = usersManager;
        }

        public IActionResult Authorization()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Authorization(Authorization authorization)
        {
            if (!ModelState.IsValid)
                return View(nameof(Authorization));

            var userAccount = _usersManager.TryByGetName(authorization.Login);
            if (userAccount == null)
            {
                ModelState.AddModelError("", "Такого пользователя не сущетсвует");
                return View(nameof(Authorization));
            }

            if (userAccount.Password != authorization.Password)
            {
                ModelState.AddModelError("", "Неверный пароль");
                return View(nameof(Authorization));

            }
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registration(Registration registration)
        {
            if (registration.Login == registration.Password)
            {
                ModelState.AddModelError("", "Логин и пароль не должны совпадать");
            }
            if (!ModelState.IsValid)
            {
                return View(nameof(Registration));
            }

            _usersManager.Add(new UserAccount
            {
                Name = registration.Login,
                Password = registration.Password,
            });
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}