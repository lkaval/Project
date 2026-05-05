using OnlineShopWebApp.Data.Models;

namespace OnlineShopWebApp.Services
{
    public interface IEmailService
    {
        Task SendOrderKeysAsync(string toEmail, string customerName, Guid orderId, List<(string ProductName, string Key)> keys);
    }
}
