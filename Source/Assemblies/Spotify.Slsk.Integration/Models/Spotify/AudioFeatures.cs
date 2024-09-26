using System.Text.Json.Serialization;

namespace Spotify.Slsk.Integration.Models.Spotify
{
    public class AudioFeatures
	{
        [JsonPropertyName("danceability")]
        public double Danceability { get; set; }

        [JsonPropertyName("energy")]
        public double Energy { get; set; }

        [JsonPropertyName("key")]
        public int Key { get; set; }

        [JsonPropertyName("loudness")]
        public double Loudness { get; set; }

        [JsonPropertyName("mode")]
        public int Mode { get; set; }

        [JsonPropertyName("speechiness")]
        public double Speechiness { get; set; }

        [JsonPropertyName("acousticness")]
        public double Acousticness { get; set; }

        [JsonPropertyName("instrumentalness")]
        public double Instrumentalness { get; set; }

        [JsonPropertyName("liveness")]
        public double Liveness { get; set; }

        [JsonPropertyName("valence")]
        public double Valence { get; set; }

        [JsonPropertyName("tempo")]
        public double Tempo { get; set; }

        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("uri")]
        public string? Uri { get; set; }

        [JsonPropertyName("track_href")]
        public string? TrackHref { get; set; }

        [JsonPropertyName("analysis_url")]
        public string? AnalysisUrl { get; set; }

        [JsonPropertyName("duration_ms")]
        public int DurationMs { get; set; }

        [JsonPropertyName("time_signature")]
        public int TimeSignature { get; set; }
    }
}

