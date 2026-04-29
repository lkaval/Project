using System.Collections.Generic;
using OnlineShopWebApp.Data.Models;

namespace OnlineShopWebApp.Data.Repository.Users
{
    public interface IUsersManager
    {
        void Add(UserAccount user);
        List<UserAccount> GetAll();
        UserAccount? TryByGetName(string name);
    }
}