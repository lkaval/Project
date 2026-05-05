using OnlineShopWebApp.Data.Models;

namespace OnlineShopWebApp.Data.Repository.Users
{
    public interface IUsersManager
    {
        void Add(UserAccount user);
        void Update(UserAccount user);
        List<UserAccount> GetAll();
        UserAccount? TryByGetName(string name);
        UserAccount? TryById(int id);
    }
}
