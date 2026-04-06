using Microsoft.AspNetCore.Mvc;
using OnlineShopWebApp.Areas.Admin.Models;
using OnlineShopWebApp.Data.Repository.Roles;

namespace OnlineShopWebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RoleController : Controller
    {
        private readonly IRolesRepository _rolesRepository;
        public RoleController(IRolesRepository rolesRepository)
        {
            _rolesRepository = rolesRepository;
        }
        public IActionResult Index()
        {
            var roles = _rolesRepository.GetAll();
            return View(roles);
        }
        public IActionResult Remove(string roleName)
        {
            _rolesRepository.Remove(roleName);
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Add()
        {
            return View();

        }
        [HttpPost]
        public IActionResult Add(Role role)
        {
            if (_rolesRepository.TryGetByName(role.Name) != null)
            {
                ModelState.AddModelError("", "Такая роль уже существует");
            }
            if (ModelState.IsValid)
            {
                _rolesRepository.Add(role);
                return RedirectToAction(nameof(Index));
            }
            return View(role);

        }
    }
}