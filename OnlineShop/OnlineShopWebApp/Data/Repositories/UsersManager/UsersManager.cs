using OnlineShopWebApp.Data.Models;

namespace OnlineShopWebApp.Data.Repository.UsersManager
{
    public class UsersManager : IUsersManager
    {
        private readonly List<UserAccount> _users = new List<UserAccount>();
        public List<UserAccount> GetAll()
        {
            return _users;
        }

        public void Add(UserAccount user)
        {
            _users.Add(user);
        }
        public UserAccount TryByGetName(string name)
        {
            return _users.FirstOrDefault(x => x.Name == name);
        }
    }
}
