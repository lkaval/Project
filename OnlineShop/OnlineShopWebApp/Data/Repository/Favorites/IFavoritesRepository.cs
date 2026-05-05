using OnlineShopWebApp.Data.Models;

namespace OnlineShopWebApp.Data.Repository.Favorites
{
    public interface IFavoritesRepository
    {
        List<Favorite> GetByUserId(string userId);
        bool Exists(string userId, int rawgGameId);
        void Add(Favorite favorite);
        void Remove(string userId, int rawgGameId);
    }
}
