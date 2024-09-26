using Xunit;
using Spotify.Slsk.Integration.Services.Download;
using Spotify.Slsk.Integration.Models.enums;

namespace Tests.Integrated;

public class DownloadTests
{
    private DownloadService DownloadService { get; }

    public DownloadTests()
    {
        DownloadService = new();
    }

    [Theory (Skip = "requires auth")]
    [InlineData("sherrif", "someId")]
    public async Task ShouldDownloadAllTracksFromPlaylistInParallel(string userId, string playlistId)
    {
        await DownloadService.DownloadAllPlaylistTracksFromUserAsync(userId, playlistId, null, "someUserNameOrPassword", "someUserNameOrPassword", false, MusicalKeyFormat.OpenKey);
    }
}
