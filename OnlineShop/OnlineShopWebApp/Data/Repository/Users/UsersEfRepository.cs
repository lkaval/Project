using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using OnlineShopWebApp.Data.Models;

namespace OnlineShopWebApp.Data.Repository.Users
{
    public class UsersEfRepository : IUsersManager
    {
        private readonly AppDbContext _context;

        public UsersEfRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Add(UserAccount user)
        {
            _context.UserAccounts.Add(user);
            _context.SaveChanges();
        }

        public List<UserAccount> GetAll()
        {
            return _context.UserAccounts.AsNoTracking().ToList();
        }

        public UserAccount? TryByGetName(string name)
        {
            return _context.UserAccounts.AsNoTracking().FirstOrDefault(u => u.Name == name);
        }
    }
}