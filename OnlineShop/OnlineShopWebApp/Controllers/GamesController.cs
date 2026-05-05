using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShopWebApp.Data.Models;
using OnlineShopWebApp.Data.Repository.Favorites;
using OnlineShopWebApp.Services;

namespace OnlineShopWebApp.Controllers
{
    public class GamesController : Controller
    {
        private readonly IRawgService _rawg;
        private readonly IFavoritesRepository _favorites;

        public GamesController(IRawgService rawg, IFavoritesRepository favorites)
        {
            _rawg = rawg;
            _favorites = favorites;
        }

        public async Task<IActionResult> Index(string? q = null, int page = 1, string? genre = null)
        {
            var response = await _rawg.GetGamesAsync(page, q, genre);
            var genres = await _rawg.GetGenresAsync();

            HashSet<int> favoriteIds = new();
            if (User.Identity?.IsAuthenticated == true)
                favoriteIds = _favorites.GetByUserId(User.Identity.Name!)
                                        .Select(f => f.RawgGameId).ToHashSet();

            var vm = new GamesIndexViewModel
            {
                Games = response.Results.Select(g => new GameCardViewModel
                {
                    Game = g,
                    IsFavorite = favoriteIds.Contains(g.Id)
                }).ToList(),
                Query = q,
                Page = page,
                TotalPages = response.Count > 0 ? (int)Math.Ceiling(response.Count / 20.0) : 1,
                Genres = genres,
                SelectedGenre = genre
            };

            return View(vm);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var game = await _rawg.GetGameAsync(id);
            if (game == null)
                return NotFound();

            bool isFavorite = User.Identity?.IsAuthenticated == true
                && _favorites.Exists(User.Identity.Name!, id);

            ViewBag.IsFavorite = isFavorite;
            return View(game);
        }

        [Authorize]
        public IActionResult Favorites()
        {
            var list = _favorites.GetByUserId(User.Identity!.Name!);
            return View(list);
        }

        [Authorize]
        [HttpPost]
        public IActionResult ToggleFavorite(int gameId, string gameName, string? imageUrl, string returnUrl = "/Games")
        {
            var userId = User.Identity!.Name!;
            if (_favorites.Exists(userId, gameId))
                _favorites.Remove(userId, gameId);
            else
                _favorites.Add(new Favorite
                {
                    UserId = userId,
                    RawgGameId = gameId,
                    GameName = gameName,
                    GameImageUrl = imageUrl
                });

            return Redirect(returnUrl);
        }
    }

    public class GamesIndexViewModel
    {
        public List<GameCardViewModel> Games { get; set; } = new();
        public string? Query { get; set; }
        public int Page { get; set; } = 1;
        public int TotalPages { get; set; } = 1;
        public List<RawgGenre> Genres { get; set; } = new();
        public string? SelectedGenre { get; set; }
    }

    public class GameCardViewModel
    {
        public RawgGame Game { get; set; } = null!;
        public bool IsFavorite { get; set; }
    }
}
