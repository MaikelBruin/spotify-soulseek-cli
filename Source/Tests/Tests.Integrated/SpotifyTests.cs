using Xunit;
using Spotify.Slsk.Integration.Models;
using Spotify.Slsk.Integration.Services.Spotify;
using Spotify.Slsk.Integration.Models.Spotify;

namespace Tests.Integrated;

public class SpotifyTests
{
    private SpotifyClient SpotifyClient { get; } = new();

    public SpotifyTests()
    {
    }

    [Fact(Skip = "Requires auth")]
    public async Task ShouldReturnPageWithTracksFromPlaylist()
    {
        string playlistId = "7dYubYJ9MkFVvCNrSVcuOJ";
        PageWithTracks playlistItems = await SpotifyClient.GetPlaylistItems("someAccessToken", playlistId, 50, 0);
        Assert.NotNull(playlistItems);
        Assert.NotEmpty(playlistItems.Items!);
    }

    [Theory]
    [InlineData("sherrif", "7dYubYJ9MkFVvCNrSVcuOJ")]
    [InlineData("sherrif", "3yacCqGYaaXx7DoTfZal61")]
    public async Task ShouldReturnPageWithTracksFromUserPlaylist(string userId, string playlistId)
    {
        PageWithTracks playlistItems = await SpotifyClient.GetPlaylistItemsFromUser(userId, playlistId, 50, 0);
        Assert.NotEmpty(playlistItems.Items!);
    }

    [Fact(Skip = "Requires auth")]
    public async Task ShouldReturnListOfTracks()
    {
        string playlistId = "7dYubYJ9MkFVvCNrSVcuOJ";
        List<TrackItem> trackItems = await SpotifyClient.GetAllPlaylistTracksAsync("someAccessToken", playlistId);
        Assert.NotEmpty(trackItems);
    }

    [Fact(Skip = "Requires auth")]
    public async Task ShouldReturnListOfUnsavedTracksInPlaylist()
    {
        string playlistId = "7dYubYJ9MkFVvCNrSVcuOJ";
        List<TrackItem> trackItems = await SpotifyClient.GetUnsavedTracksInPlaylist("someAccessToken", playlistId);
        Assert.NotEmpty(trackItems);
    }

    [Fact(Skip = "Requires auth")]
    public async Task ShouldReturnListOfRecentlyAddedTracks()
    {
        string playlistId = "7dYubYJ9MkFVvCNrSVcuOJ";
        DateTime dateOnly = new DateOnly(2021, 1, 2).ToDateTime(TimeOnly.MinValue);
        DateTime laterDate = new DateOnly(2022, 1, 2).ToDateTime(TimeOnly.MinValue);
        int v = dateOnly.CompareTo(laterDate);

        List<TrackItem> trackItems = await SpotifyClient.GetPlayListTracksFromDate("someAccessToken", playlistId, new DateOnly(2022, 4, 1));
        Assert.NotEmpty(trackItems);
    }

    [Fact(Skip = "Requires auth")]
    public async Task ShouldReturnPageWithSavedTrackItems()
    {
        PageWithTracks playlistItems = await SpotifyClient.GetPageWithSavedTracksAsync("someAccessToken", 50, 0);
        Assert.NotNull(playlistItems);
        Assert.NotEmpty(playlistItems.Items!);
    }

    [Fact(Skip = "Requires auth")]
    public async Task ShouldReturnAllSavedTracks()
    {
        List<TrackItem> savedTracks = await SpotifyClient.GetAllSavedTracksAsync("someAccessToken");
        Assert.NotEmpty(savedTracks);
    }
}
