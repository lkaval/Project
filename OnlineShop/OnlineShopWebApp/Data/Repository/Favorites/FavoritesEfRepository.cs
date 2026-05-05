using OnlineShopWebApp.Data.Models;

namespace OnlineShopWebApp.Data.Repository.Favorites
{
    public class FavoritesEfRepository : IFavoritesRepository
    {
        private readonly AppDbContext _context;
        public FavoritesEfRepository(AppDbContext context) => _context = context;

        public List<Favorite> GetByUserId(string userId) =>
            _context.Favorites.Where(f => f.UserId == userId)
                              .OrderByDescending(f => f.AddedAt)
                              .ToList();

        public bool Exists(string userId, int rawgGameId) =>
            _context.Favorites.Any(f => f.UserId == userId && f.RawgGameId == rawgGameId);

        public void Add(Favorite favorite)
        {
            _context.Favorites.Add(favorite);
            _context.SaveChanges();
        }

        public void Remove(string userId, int rawgGameId)
        {
            var fav = _context.Favorites.FirstOrDefault(f => f.UserId == userId && f.RawgGameId == rawgGameId);
            if (fav != null)
            {
                _context.Favorites.Remove(fav);
                _context.SaveChanges();
            }
        }
    }
}
