using System.Text;
using Spotify.Slsk.Integration.Extensions;
using Spotify.Slsk.Integration.Models;
using Spotify.Slsk.Integration.Models.enums;
using Spotify.Slsk.Integration.Models.Spotify;
using Spotify.Slsk.Integration.Services.Id3Tag;
using Spotify.Slsk.Integration.Services.SoulSeek;
using Spotify.Slsk.Integration.Services.Spotify;
using Serilog;
using Soulseek;

namespace Spotify.Slsk.Integration.Services.Download
{
    public class DownloadService
    {
        private SpotifyClient SpotifyClient { get; } = new();
        private SoulseekClient SoulseekClient { get; }

        public DownloadService()
        {
            SoulseekClient = SoulseekService.GetClient();
        }

        public async Task DownloadAllPlaylistTracksFromUserAsync(string spotifyUserId, string? spotifyPlaylistId, string? spotifyPlaylistName,
            string ssUsername, string ssPassword, string spotifyAccessToken, bool setId3Tags, MusicalKeyFormat musicalKeyFormat, Action<SoulseekOptions>? soulseekOptionsAction = null)
        {
            SoulseekOptions options = new();
            soulseekOptionsAction?.Invoke(options);

            await SoulseekService.ConnectAndLoginAsync(SoulseekClient, ssUsername, ssPassword);
            PlaylistItem playlistItem;
            if (spotifyPlaylistId == null)
            {
                spotifyPlaylistName = spotifyPlaylistName
                    ?? throw new Exception("Please provide playlist name or Id");
                Log.Warning($"Playlist Id not present, searching for playlist '{spotifyPlaylistName}' of user '{spotifyUserId}'...");
                playlistItem = await SpotifyClient.GetPlaylistFromUserByName(spotifyUserId, spotifyPlaylistName, spotifyAccessToken);
                spotifyPlaylistId = playlistItem.Id;
            }
            else
            {
                playlistItem = await SpotifyClient.GetPlaylistFromUser(spotifyUserId, spotifyPlaylistId, spotifyAccessToken);
            }

            List<TrackItem> trackItems = await SpotifyClient.GetAllPlaylistTracksFromUser(spotifyUserId, spotifyPlaylistId!, spotifyAccessToken);
            List<TrackToDownload> tracksToDownload = new();
            foreach (TrackItem trackItem in trackItems)
            {
                tracksToDownload.Add(new()
                {
                    Track = trackItem,
                    Query = GetQueryForTrack(trackItem)
                });
            }

            string playlistName = playlistItem.Name!;
            await DownloadTracksInParallelAsync(ssUsername, ssPassword, spotifyAccessToken, tracksToDownload, playlistName, options, setId3Tags, musicalKeyFormat, save: false);
        }

        public async Task DownloadUnsavedPlaylistTracksFromUserAsync(string spotifyUserId, string? spotifyPlaylistId, string? spotifyPlaylistName,
            string ssUsername, string ssPassword, string spotifyAccessToken, bool setId3Tags, MusicalKeyFormat musicalKeyFormat, Action<SoulseekOptions>? soulseekOptionsAction = null)
        {
            SoulseekOptions options = new();
            soulseekOptionsAction?.Invoke(options);

            await SoulseekService.ConnectAndLoginAsync(SoulseekClient, ssUsername, ssPassword);
            PlaylistItem playlistItem = new();
            if (spotifyPlaylistId == null)
            {
                spotifyPlaylistName = spotifyPlaylistName
                    ?? throw new Exception("Please provide playlist name or Id");
                Log.Warning($"Playlist Id not present, searching for playlist '{spotifyPlaylistName}' of user '{spotifyUserId}'...");
                playlistItem = await SpotifyClient.GetPlaylistFromUserByName(spotifyUserId, spotifyPlaylistName, spotifyAccessToken);
                spotifyPlaylistId = playlistItem.Id;
            }

            List<TrackItem> trackItems = await SpotifyClient.GetUnsavedTracksInPlaylist(spotifyAccessToken, spotifyPlaylistId!);
            List<TrackToDownload> tracksToDownload = new();

            foreach (TrackItem trackItem in trackItems)
            {
                tracksToDownload.Add(new TrackToDownload()
                {
                    Track = trackItem,
                    Query = GetQueryForTrack(trackItem)
                });
            }

            string playlistName = playlistItem.Name!;
            Log.Information($"Attempting to download '{tracksToDownload.Count}' files...");
            await DownloadTracksInParallelAsync(ssUsername, ssPassword, spotifyAccessToken, tracksToDownload, playlistName, options, setId3Tags, musicalKeyFormat, save: true);
        }

        private async Task DownloadTracksInParallelAsync(string ssUsername, string ssPassword, string spotifyAccessToken, List<TrackToDownload> tracksToDownload, string playlistName,
            SoulseekOptions options, bool setId3Tags, MusicalKeyFormat musicalKeyFormat, bool save = true)
        {
            SemaphoreSlim semaphoreSlim = new(5);
            IEnumerable<Task> tasks = tracksToDownload.Select(async trackToDownload =>
            {
                await semaphoreSlim.WaitAsync();
                try
                {
                    await DownloadSpotifyTrackAsync(ssUsername, ssPassword, spotifyAccessToken, playlistName, trackToDownload, options, setId3Tags, musicalKeyFormat, save);
                    Log.Information($"Downloads remaining: '{tracksToDownload.Count - (tracksToDownload.IndexOf(trackToDownload) + 1)}'");
                }
                finally
                {
                    semaphoreSlim.Release();
                }
            });

            await Task.WhenAll(tasks);
        }

        private async Task<bool> DownloadSpotifyTrackAsync(string ssUsername, string ssPassword, string spotifyAccessToken, string playlistName,
            TrackToDownload trackToDownload, SoulseekOptions soulseekOptions, bool setId3Tags, MusicalKeyFormat musicalKeyFormat, bool save = false)
        {
            SoulseekResult result = new();
            try
            {
                result = await SoulseekService.GetTrackAsync(SoulseekClient, trackToDownload, ssUsername, ssPassword, soulseekOptions, playlistName);
            }
            catch (Exception e)
            {
                Log.Error($"Something went wrong downloading '{trackToDownload.Query}', stacktrace:");
                Log.Error($"{e.StackTrace}");
            }

            if (result.Success && save)
            {
                Log.Information($"Download successful, saving track in spotify...");
                await SpotifyClient.SaveTrackAsync(spotifyAccessToken, trackToDownload.Track!);
            }

            if (result.Success && setId3Tags)
            {
                Log.Information($"Setting ID3 tags of downloaded file...");
                AudioFeatures audioFeatures = await SpotifyClient.GetTrackAudioFeatures(trackToDownload.Track!.Track!.Id!, spotifyAccessToken);
                Id3TagService.SetId3Tags(trackToDownload.Track, audioFeatures, musicalKeyFormat, result.FilePath!);
            };

            return result.Success;
        }

        public static string GetQueryForTrack(TrackItem trackItem)
        {
            string queryRaw = $"{trackItem.Track!.Name} {trackItem.Track.Artists![0].Name} {trackItem.Track.Album!.Name}";
            return queryRaw.RemoveSpecialCharacters();
        }
    }
}

