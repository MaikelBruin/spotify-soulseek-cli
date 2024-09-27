using Spotify.Slsk.Integration.Models.Spotify;
using System.Text.Json;
using static Spotify.Slsk.Integration.Constants.Constants;
using System.Net.Http.Headers;
using System.Net;
using System.Reflection;
using Spotify.Slsk.Integration.Models;

namespace Spotify.Slsk.Integration.Services.Spotify
{
    public class SpotifyClient
    {
        private HttpClient HttpClient { get; } = new();

        public SpotifyClient()
        {
        }

        public async Task<List<PlaylistItem>> GetAllPlaylistsFromUser(string userId, string spotifyAccessToken)
        {
            List<PlaylistItem> result = new();
            int offset = 0;
            int limit = 50;


            PageWithPlaylists pageWithPlaylists = await GetPlaylistsFromUser(userId, limit, offset, spotifyAccessToken);
            int total = pageWithPlaylists.Total;

            int pages = total / limit;
            if (total % limit != 0)
            {
                pages++;
            }

            for (int i = 0; i < pages; i++)
            {
                foreach (PlaylistItem item in pageWithPlaylists.Items!)
                {
                    result.Add(item);
                }
                offset += limit;
                pageWithPlaylists = await GetPlaylistsFromUser(userId, limit, offset, spotifyAccessToken);
            }

            return result;
        }



        public async Task SaveTrackAsync(string accessToken, TrackItem track)
        {
            string endpoint = $"me/tracks";
            string queryParams = $"?ids={track.Track!.Id}";

            HttpRequestMessage request = new(HttpMethod.Put, $"{SPOTIFY_BASE_URL}{endpoint}{queryParams}");
            _ = await GetResponseAsync(request, accessToken);
        }

        public async Task<List<TrackItem>> GetAllSavedTracksAsync(string accessToken)
        {
            List<TrackItem> result = new();
            int offset = 0;
            int limit = 50;


            PageWithTracks pageWithTracks = await GetPageWithSavedTracksAsync(accessToken, limit, offset);
            int total = pageWithTracks.Total!.Value;

            int pages = total / limit;
            if (total % limit != 0)
            {
                pages++;
            }

            for (int i = 0; i < pages; i++)
            {
                foreach (TrackItem item in pageWithTracks.Items!)
                {
                    result.Insert(0, item);
                }
                offset += limit;
                pageWithTracks = await GetPageWithSavedTracksAsync(accessToken, limit, offset);
            }

            return result;
        }

        public async Task<List<TrackItem>> GetAllPlaylistTracksFromUser(string userId, string playlistId, string accessToken)
        {
            List<TrackItem> result = new();
            int offset = 0;
            int limit = 50;


            PageWithTracks pageWithTracks = await GetPlaylistItemsFromUser(userId, playlistId, limit, offset, accessToken);
            int total = pageWithTracks.Total!.Value;

            int pages = total / limit;
            if (total % limit != 0)
            {
                pages++;
            }

            for (int i = 0; i < pages; i++)
            {
                foreach (TrackItem item in pageWithTracks.Items!)
                {
                    result.Add(item);
                }
                offset += limit;
                pageWithTracks = await GetPlaylistItemsFromUser(userId, playlistId, limit, offset, accessToken);
            }

            return result;
        }

        public async Task<List<TrackItem>> GetAllPlaylistTracksAsync(string accessToken, string playlistId)
        {
            List<TrackItem> result = new();
            int offset = 0;
            int limit = 50;


            PageWithTracks pageWithTracks = await GetPlaylistItems(accessToken, playlistId, limit, offset);
            int total = pageWithTracks.Total!.Value;

            int pages = total / limit;
            if (total % limit != 0)
            {
                pages++;
            }

            for (int i = 0; i < pages; i++)
            {
                foreach (TrackItem item in pageWithTracks.Items!)
                {
                    result.Insert(0, item);
                }
                offset += limit;
                pageWithTracks = await GetPlaylistItems(accessToken, playlistId, limit, offset);
            }

            return result;
        }

        public async Task<List<TrackItem>> GetUnsavedTracksInPlaylist(string accessToken, string playlistId)
        {
            List<TrackItem> playlistTracks = await GetAllPlaylistTracksAsync(accessToken, playlistId);
            List<TrackItem> savedTracks = await new SpotifyClient().GetAllSavedTracksAsync(accessToken);

            List<TrackItem> unsavedTracksInPlaylist = new();
            foreach (TrackItem playlistTrack in playlistTracks)
            {
                TrackItem? saved = savedTracks.FirstOrDefault(savedTrack => savedTrack.Track!.Id == playlistTrack.Track!.Id);
                if (saved == null)
                {
                    unsavedTracksInPlaylist.Add(playlistTrack);
                }
            }
            return unsavedTracksInPlaylist;
        }

        public async Task<List<TrackItem>> GetPlayListTracksFromDate(string accessToken, string playlistId, DateOnly fromDate)
        {
            List<TrackItem> allPlaylistTracks = await GetAllPlaylistTracksAsync(accessToken, playlistId);
            DateTime dateTime = fromDate.ToDateTime(TimeOnly.MinValue);
            List<TrackItem> recentTrackItems = allPlaylistTracks
                .Where(track => track.AddedAt.CompareTo(dateTime) >= 0)
                .ToList();
            return recentTrackItems;
        }

        public async Task<PlaylistItem> GetPlaylistFromUserByName(string userId, string playlistName, string spotifyAccessToken)
        {
            List<PlaylistItem> playlistItems = await GetAllPlaylistsFromUser(userId, spotifyAccessToken);
            PlaylistItem? playlistItem = playlistItems.FirstOrDefault(playlist => playlist.Name == playlistName);
            return playlistItem
                ?? throw new Exception($"Playlist with name '{playlistName}' not found for user '{userId}'");
        }

        public async Task<PageWithTracks> GetPageWithSavedTracksAsync(string accessToken, int limit, int offset)
        {
            string endpoint = $"me/tracks";
            string queryParams = $"?limit={limit}&offset={offset}";

            HttpRequestMessage request = new(HttpMethod.Get, $"{SPOTIFY_BASE_URL}{endpoint}{queryParams}");
            HttpResponseMessage response = await GetResponseAsync(request, accessToken);

            return await DeserializeResponseAsync<PageWithTracks>(response);
        }

        public async Task<PageWithPlaylists> GetPlaylistsFromUser(string userId, int limit, int offset, string spotifyAccessToken)
        {
            string endpoint = $"users/{userId}/playlists";
            string queryParams = $"?limit={limit}&offset={offset}";

            HttpRequestMessage request = new(HttpMethod.Get, $"{SPOTIFY_BASE_URL}{endpoint}{queryParams}");
            HttpResponseMessage response = await GetResponseAsync(request, spotifyAccessToken);

            return await DeserializeResponseAsync<PageWithPlaylists>(response);
        }

        public async Task<AudioFeatures> GetTrackAudioFeatures(string trackId, string accessToken)
        {
            string endpoint = $"audio-features/{trackId}";

            HttpRequestMessage request = new(HttpMethod.Get, $"{SPOTIFY_BASE_URL}{endpoint}");
            HttpResponseMessage response = await GetResponseAsync(request, accessToken);

            return await DeserializeResponseAsync<AudioFeatures>(response);
        }

        public async Task<PlaylistItem> GetPlaylistFromUser(string userId, string playlistId, string accessToken)
        {
            string endpoint = $"users/{userId}/playlists/{playlistId}";

            HttpRequestMessage request = new(HttpMethod.Get, $"{SPOTIFY_BASE_URL}{endpoint}");
            HttpResponseMessage response = await GetResponseAsync(request, accessToken);

            return await DeserializeResponseAsync<PlaylistItem>(response);
        }

        public async Task<PageWithTracks> GetPlaylistItemsFromUser(string userId, string playlistId, int limit, int offset, string accessToken)
        {
            string endpoint = $"users/{userId}/playlists/{playlistId}/tracks";
            string queryParams = $"?limit={limit}&offset={offset}";

            HttpRequestMessage request = new(HttpMethod.Get, $"{SPOTIFY_BASE_URL}{endpoint}{queryParams}");
            HttpResponseMessage response = await GetResponseAsync(request, accessToken);

            return await DeserializeResponseAsync<PageWithTracks>(response);
        }

        public async Task<PageWithTracks> GetPlaylistItems(string accessToken, string playlistId, int limit, int offset)
        {
            string endpoint = $"playlists/{playlistId}/tracks";
            string queryParams = $"?limit={limit}&offset={offset}";

            HttpRequestMessage request = new(HttpMethod.Get, $"{SPOTIFY_BASE_URL}{endpoint}{queryParams}");
            HttpResponseMessage response = await GetResponseAsync(request, accessToken);

            return await DeserializeResponseAsync<PageWithTracks>(response);
        }

        private async Task<HttpResponseMessage> GetResponseAsync(HttpRequestMessage request, string? accessToken = null)
        {
            if (accessToken != null)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }

            HttpResponseMessage response = await HttpClient.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new Exception("Unauthorized, please provide an access token using -a or use another command that does not require authentication");
            }

            return response;
        }

        private static async Task<TResponse> DeserializeResponseAsync<TResponse>(HttpResponseMessage httpResponseMessage)
        {
            string responseString = await httpResponseMessage.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TResponse>(responseString)!;
        }
    }
}

