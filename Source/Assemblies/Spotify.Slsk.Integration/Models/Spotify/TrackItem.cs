using System.Text.Json.Serialization;

namespace Spotify.Slsk.Integration.Models.Spotify
{
    public class TrackItem
    {

        [JsonPropertyName("added_at")]
        public DateTime AddedAt { get; set; }

        [JsonPropertyName("added_by")]
        public AddedBy? AddedBy { get; set; }

        [JsonPropertyName("is_local")]
        public bool IsLocal { get; set; }

        [JsonPropertyName("primary_color")]
        public object? PrimaryColor { get; set; }

        [JsonPropertyName("track")]
        public Track? Track { get; set; }

        [JsonPropertyName("video_thumbnail")]
        public VideoThumbnail? VideoThumbnail { get; set; }
    }

}
