using OnlineShopWebApp.Areas.Admin.Models;
using Microsoft.EntityFrameworkCore;
using OnlineShopWebApp.Areas.Admin.Models;

namespace OnlineShopWebApp.Data.Repository.Roles
{
    public class RolesEfRepository : IRolesRepository
    {
        private readonly AppDbContext _context;
        public RolesEfRepository(AppDbContext context) => _context = context;

        public void Add(Role role) { _context.Roles.Add(role); _context.SaveChanges(); }
        public List<Role> GetAll() => _context.Roles.AsNoTracking().ToList();
        public Role TryGetByName(string name) => _context.Roles.AsNoTracking().FirstOrDefault(r => r.Name == name);
        public void Remove(string name)
        {
            var role = _context.Roles.FirstOrDefault(r => r.Name == name);
            if (role != null) { _context.Roles.Remove(role); _context.SaveChanges(); }
        }
    }
}