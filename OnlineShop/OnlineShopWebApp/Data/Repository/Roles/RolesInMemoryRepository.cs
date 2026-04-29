using OnlineShopWebApp.Areas.Admin.Models;

namespace OnlineShopWebApp.Data.Repository.Roles
{
    public class RolesInMemoryRepository : IRolesRepository
    {
        private readonly List<Role> _roles = new List<Role>();
        public void Add(Role role)
        {
            _roles.Add(role);
        }
        public List<Role> GetAll()
        {
            return _roles;
        }
        public Role TryGetByName(string name)
        {
            return _roles.FirstOrDefault(x => x.Name == name);
        }
        public void Remove(string name)
        {
            _roles.RemoveAll(x => x.Name == name);
        }
    }
}