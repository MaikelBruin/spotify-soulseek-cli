using System.Text.Json.Serialization;

namespace Spotify.Slsk.Integration.Models.Spotify
{
    public class ExternalUrls
    {

        [JsonPropertyName("spotify")]
        public string? Spotify { get; set; }
    }

}
