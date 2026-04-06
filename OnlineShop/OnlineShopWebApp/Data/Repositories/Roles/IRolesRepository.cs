using OnlineShopWebApp.Areas.Admin.Models;

namespace OnlineShopWebApp.Data.Repository.Roles
{
    public interface IRolesRepository
    {
        List<Role> GetAll();
        Role TryGetByName(string Name);
        void Add(Role Role);
        void Remove(string Name);
    }
}