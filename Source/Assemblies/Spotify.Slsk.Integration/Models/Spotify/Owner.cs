﻿using System.Text.Json.Serialization;

namespace Spotify.Slsk.Integration.Models.Spotify
{
    public class Owner
	{

		[JsonPropertyName("display_name")]
		public string? DisplayName { get; set; }

		[JsonPropertyName("external_urls")]
		public ExternalUrls? ExternalUrls { get; set; }

		[JsonPropertyName("href")]
		public string? Href { get; set; }

		[JsonPropertyName("id")]
		public string? Id { get; set; }

		[JsonPropertyName("type")]
		public string? Type { get; set; }

		[JsonPropertyName("uri")]
		public string? Uri { get; set; }
	}


}

