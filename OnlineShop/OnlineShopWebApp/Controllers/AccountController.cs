using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using OnlineShopWebApp.Data.Models;
using OnlineShopWebApp.Data.Repository.Users;
using System.Security.Claims;

namespace OnlineShopWebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUsersManager _usersManager;

        public AccountController(IUsersManager usersManager)
        {
            _usersManager = usersManager;
        }

        public IActionResult Authorization() => View();

        [HttpPost]
        public async Task<IActionResult> Authorization(Authorization authorization, string? returnUrl)
        {
            if (!ModelState.IsValid)
                return View(nameof(Authorization));

            var user = _usersManager.TryByGetName(authorization.Login);
            if (user == null)
            {
                ModelState.AddModelError("", "Такого пользователя не существует");
                return View(nameof(Authorization));
            }

            if (user.Password != authorization.Password)
            {
                ModelState.AddModelError("", "Неверный пароль");
                return View(nameof(Authorization));
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, user.Role.Name),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            if (user.Role.Name == "Admin")
                return RedirectToAction("Index", "Order", new { area = "Admin" });

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied() => View();

        public IActionResult Registration() => View();

        [HttpPost]
        public IActionResult Registration(Registration registration)
        {
            if (registration.Login == registration.Password)
                ModelState.AddModelError("", "Логин и пароль не должны совпадать");

            if (_usersManager.TryByGetName(registration.Login) != null)
                ModelState.AddModelError("", "Пользователь с таким email уже существует");

            if (!ModelState.IsValid)
                return View(nameof(Registration));

            _usersManager.Add(new UserAccount
            {
                Name = registration.Login,
                Password = registration.Password,
                RoleId = 1 // User
            });
            return RedirectToAction(nameof(Authorization));
        }
    }
}
