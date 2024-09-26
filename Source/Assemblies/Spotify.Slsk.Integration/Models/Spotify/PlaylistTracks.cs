using System.Text.Json.Serialization;

namespace Spotify.Slsk.Integration.Models.Spotify
{
    public class PlaylistTracks
	{

		[JsonPropertyName("href")]
		public string? Href { get; set; }

		[JsonPropertyName("total")]
		public int Total { get; set; }
	}


}

