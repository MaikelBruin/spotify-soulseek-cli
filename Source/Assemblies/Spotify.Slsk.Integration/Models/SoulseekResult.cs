using System.Text.Json.Serialization;

namespace Spotify.Slsk.Integration.Models
{
    public class SoulseekResult
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; } = false;

        [JsonPropertyName("filePath")]
        public string? FilePath { get; set; }
    }
}
