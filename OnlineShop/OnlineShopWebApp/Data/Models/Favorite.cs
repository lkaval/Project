namespace OnlineShopWebApp.Data.Models
{
    public class Favorite
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public int RawgGameId { get; set; }
        public string GameName { get; set; } = string.Empty;
        public string? GameImageUrl { get; set; }
        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
    }
}
