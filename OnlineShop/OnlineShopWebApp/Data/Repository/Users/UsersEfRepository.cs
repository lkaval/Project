using Microsoft.EntityFrameworkCore;
using OnlineShopWebApp.Data.Models;

namespace OnlineShopWebApp.Data.Repository.Users
{
    public class UsersEfRepository : IUsersManager
    {
        private readonly AppDbContext _context;

        public UsersEfRepository(AppDbContext context) => _context = context;

        public void Add(UserAccount user)
        {
            _context.UserAccounts.Add(user);
            _context.SaveChanges();
        }

        public void Update(UserAccount user)
        {
            var existing = _context.UserAccounts.Find(user.Id);
            if (existing == null) return;
            existing.Name = user.Name;
            existing.RoleId = user.RoleId;
            if (!string.IsNullOrWhiteSpace(user.Password))
                existing.Password = user.Password;
            _context.SaveChanges();
        }

        public List<UserAccount> GetAll() =>
            _context.UserAccounts.Include(u => u.Role).AsNoTracking().ToList();

        public UserAccount? TryByGetName(string name) =>
            _context.UserAccounts.Include(u => u.Role).AsNoTracking()
                    .FirstOrDefault(u => u.Name == name);

        public UserAccount? TryById(int id) =>
            _context.UserAccounts.Include(u => u.Role).AsNoTracking()
                    .FirstOrDefault(u => u.Id == id);
    }
}
