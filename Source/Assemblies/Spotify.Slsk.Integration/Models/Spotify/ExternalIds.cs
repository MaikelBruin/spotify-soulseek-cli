using System.Text.Json.Serialization;

namespace Spotify.Slsk.Integration.Models.Spotify
{
    public class ExternalIds
    {

        [JsonPropertyName("isrc")]
        public string? Isrc { get; set; }
    }

}
