namespace OnlineShopWebApp.Data.Models
{
    public class UserAccount
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}