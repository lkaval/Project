using OnlineShopWebApp.Data.Models;

namespace OnlineShopWebApp.Data.Repository.Carts
{
    public interface ICartsRepository
    {
        void Add(Models.Product product, string userId);
        void Clear(string userId);
        void DecreaseAmount(int productId, string userId);
        Cart? TryGetByUserID(string userId); // ← добавили ?
    }
}