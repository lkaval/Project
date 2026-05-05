using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShopWebApp.Data.Models;
using OnlineShopWebApp.Data.Repository.Roles;
using OnlineShopWebApp.Data.Repository.Users;

namespace OnlineShopWebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly IUsersManager _usersManager;
        private readonly IRolesRepository _rolesRepository;

        public UserController(IUsersManager usersManager, IRolesRepository rolesRepository)
        {
            _usersManager = usersManager;
            _rolesRepository = rolesRepository;
        }

        public IActionResult Index()
        {
            var users = _usersManager.GetAll();
            return View(users);
        }

        public IActionResult Edit(int id)
        {
            var user = _usersManager.TryById(id);
            if (user == null) return NotFound();
            ViewBag.Roles = _rolesRepository.GetAll();
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(UserAccount user)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Roles = _rolesRepository.GetAll();
                return View(user);
            }
            _usersManager.Update(user);
            return RedirectToAction(nameof(Index));
        }
    }
}
