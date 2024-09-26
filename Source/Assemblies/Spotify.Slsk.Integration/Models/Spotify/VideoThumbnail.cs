using System.Text.Json.Serialization;

namespace Spotify.Slsk.Integration.Models.Spotify
{
    public class VideoThumbnail
    {

        [JsonPropertyName("url")]
        public object? Url { get; set; }
    }

}
