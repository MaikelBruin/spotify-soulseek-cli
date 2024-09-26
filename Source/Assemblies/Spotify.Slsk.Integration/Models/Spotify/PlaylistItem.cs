using System.Text.Json.Serialization;

namespace Spotify.Slsk.Integration.Models.Spotify
{
    public class PlaylistItem
	{

		[JsonPropertyName("collaborative")]
		public bool Collaborative { get; set; }

		[JsonPropertyName("description")]
		public string? Description { get; set; }

		[JsonPropertyName("external_urls")]
		public ExternalUrls? ExternalUrls { get; set; }

		[JsonPropertyName("href")]
		public string? Href { get; set; }

		[JsonPropertyName("id")]
		public string? Id { get; set; }

		[JsonPropertyName("images")]
		public List<Image>? Images { get; set; }

		[JsonPropertyName("name")]
		public string? Name { get; set; }

		[JsonPropertyName("owner")]
		public Owner? Owner { get; set; }

		[JsonPropertyName("primary_color")]
		public object? PrimaryColor { get; set; }

		[JsonPropertyName("public")]
		public bool Public { get; set; }

		[JsonPropertyName("snapshot_id")]
		public string? SnapshotId { get; set; }

		[JsonPropertyName("tracks")]
		public PlaylistTracks? Tracks { get; set; }

		[JsonPropertyName("type")]
		public string? Type { get; set; }

		[JsonPropertyName("uri")]
		public string? Uri { get; set; }
	}


}

