namespace OnlineShopWebApp.Services
{
    public interface IRawgService
    {
        Task<RawgGamesResponse> GetGamesAsync(int page = 1, string? search = null, string? genre = null);
        Task<RawgGame?> GetGameAsync(int id);
        Task<List<RawgGenre>> GetGenresAsync();
    }
}
