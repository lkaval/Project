using OnlineShopWebApp.Data.Models;

public interface IUsersManager
{
    void Add(UserAccount user);
    List<UserAccount> GetAll();
    UserAccount TryByGetName(string name);
}