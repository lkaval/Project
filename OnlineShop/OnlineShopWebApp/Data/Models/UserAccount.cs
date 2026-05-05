using OnlineShopWebApp.Areas.Admin.Models;

namespace OnlineShopWebApp.Data.Models
{
    public class UserAccount
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int RoleId { get; set; } = 1;
        public Role Role { get; set; } = null!;
    }
}
