using System.Text.Json.Serialization;
using Spotify.Slsk.Integration.Models.Spotify;

namespace Spotify.Slsk.Integration.Models
{

    public class PageWithTracks
    {

        [JsonPropertyName("href")]
        public string? Href { get; set; }

        [JsonPropertyName("items")]
        public List<TrackItem>? Items { get; set; }

        [JsonPropertyName("limit")]
        public int Limit { get; set; }

        [JsonPropertyName("next")]
        public object? Next { get; set; }

        [JsonPropertyName("offset")]
        public int? Offset { get; set; }

        [JsonPropertyName("previous")]
        public string? Previous { get; set; }

        [JsonPropertyName("total")]
        public int? Total { get; set; }
    }

}
