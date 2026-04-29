using OnlineShopWebApp.Areas.Admin.Models;

namespace OnlineShopWebApp.Data.Repository.Roles
{
    public interface IRolesRepository
    {
        List<Role> GetAll();
        Role? TryGetByName(string name); // ← добавили ?
        void Add(Role role);
        void Remove(string name);
    }
}