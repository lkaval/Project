using System.Text.Json.Serialization;

namespace OnlineShopWebApp.Services
{
    public class RawgGamesResponse
    {
        public int Count { get; set; }
        public string? Next { get; set; }
        public List<RawgGame> Results { get; set; } = new();
    }

    public class RawgGame
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;

        [JsonPropertyName("background_image")]
        public string? BackgroundImage { get; set; }

        public double Rating { get; set; }

        [JsonPropertyName("ratings_count")]
        public int RatingsCount { get; set; }

        public string? Released { get; set; }

        public List<RawgGenre> Genres { get; set; } = new();

        [JsonPropertyName("description_raw")]
        public string? DescriptionRaw { get; set; }
    }

    public class RawgGenre
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;

        [JsonPropertyName("games_count")]
        public int GamesCount { get; set; }

        [JsonPropertyName("image_background")]
        public string? ImageBackground { get; set; }
    }

    public class RawgGenresResponse
    {
        public List<RawgGenre> Results { get; set; } = new();
    }
}
