using Microsoft.Extensions.Caching.Memory;
using System.Net.Http.Json;
using System.Text.Json;

namespace OnlineShopWebApp.Services
{
    public class RawgService : IRawgService
    {
        private readonly HttpClient _http;
        private readonly IMemoryCache _cache;
        private readonly string _apiKey;

        private static readonly JsonSerializerOptions _json = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public RawgService(HttpClient http, IMemoryCache cache, IConfiguration config)
        {
            _http = http;
            _cache = cache;
            _apiKey = config["RawgSettings:ApiKey"] ?? string.Empty;
        }

        public async Task<RawgGamesResponse> GetGamesAsync(int page = 1, string? search = null, string? genre = null)
        {
            var cacheKey = $"games_p{page}_s{search}_g{genre}";
            if (_cache.TryGetValue(cacheKey, out RawgGamesResponse? cached))
                return cached!;

            if (string.IsNullOrEmpty(_apiKey))
                return new RawgGamesResponse();

            var url = $"games?key={_apiKey}&page={page}&page_size=20&ordering=-rating";
            if (!string.IsNullOrWhiteSpace(search))
                url += $"&search={Uri.EscapeDataString(search)}";
            if (!string.IsNullOrWhiteSpace(genre))
                url += $"&genres={Uri.EscapeDataString(genre)}";

            try
            {
                var result = await _http.GetFromJsonAsync<RawgGamesResponse>(url, _json)
                             ?? new RawgGamesResponse();
                _cache.Set(cacheKey, result, TimeSpan.FromMinutes(5));
                return result;
            }
            catch
            {
                return new RawgGamesResponse();
            }
        }

        public async Task<RawgGame?> GetGameAsync(int id)
        {
            var cacheKey = $"game_{id}";
            if (_cache.TryGetValue(cacheKey, out RawgGame? cached))
                return cached;

            if (string.IsNullOrEmpty(_apiKey))
                return null;

            try
            {
                var result = await _http.GetFromJsonAsync<RawgGame>($"games/{id}?key={_apiKey}", _json);
                _cache.Set(cacheKey, result, TimeSpan.FromMinutes(30));
                return result;
            }
            catch
            {
                return null;
            }
        }

        public async Task<List<RawgGenre>> GetGenresAsync()
        {
            const string cacheKey = "genres";
            if (_cache.TryGetValue(cacheKey, out List<RawgGenre>? cached))
                return cached!;

            if (string.IsNullOrEmpty(_apiKey))
                return new List<RawgGenre>();

            try
            {
                var result = await _http.GetFromJsonAsync<RawgGenresResponse>($"genres?key={_apiKey}", _json);
                var genres = result?.Results ?? new List<RawgGenre>();
                _cache.Set(cacheKey, genres, TimeSpan.FromHours(1));
                return genres;
            }
            catch
            {
                return new List<RawgGenre>();
            }
        }
    }
}
