using Spotify.Slsk.Integration.Models.Spotify;
using Soulseek;
using File = Soulseek.File;

namespace Spotify.Slsk.Integration.Models
{
    public class TrackToDownload
	{
		public TrackItem? Track { get; set; }
		public string? Query { get; set; }
		public List<SearchResponse>? SearchResponses { get; set; }
		public SearchResponse? SelectedSearchResponse { get; set; }
		public List<File?>? Files { get; set; }
		public File? SelectedFile { get; set; }
		public string? Username { get; set; }
	}
}

