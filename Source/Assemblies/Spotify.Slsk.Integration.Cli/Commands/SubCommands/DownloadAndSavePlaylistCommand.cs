using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Spotify.Slsk.Integration.Models;
using Spotify.Slsk.Integration.Models.enums;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Spotify.Slsk.Integration.Cli.Commands.SubCommands
{
    [Command(Name = "save-playlist", Description = "searches unsaved spotify playlist tracks on soulseek and checks for available high quality mp3s")]
    class DownloadAndSavePlaylistCommand : SpotSeekCommandBase
    {
        [Option(CommandOptionType.SingleValue, ShortName = "i", LongName = "userid", Description = "spotify user id (not email)", ValueName = "spotify user id", ShowInHelpText = true)]
        public string SpotifyUserId { get; set; }

        [Option(CommandOptionType.SingleValue, ShortName = "l", LongName = "playlistid", Description = "spotify playlist id (from link)", ValueName = "spotify playlist id", ShowInHelpText = true)]
        public string SpotifyPlaylistId { get; set; }

        [Option(CommandOptionType.SingleValue, ShortName = "n", LongName = "playlistname", Description = "spotify playlist name", ValueName = "spotify playlist name", ShowInHelpText = true)]
        public string SpotifyPlaylistName { get; set; }

        [Option(CommandOptionType.SingleValue, ShortName = "u", LongName = "ssusername", Description = "soulseek login username", ValueName = "login username", ShowInHelpText = true)]
        public string SSUsername { get; set; }

        [Option(CommandOptionType.SingleValue, ShortName = "p", LongName = "sspassword", Description = "soulseek login password", ValueName = "login password", ShowInHelpText = true)]
        public string SSPassword { get; set; }

        [Option(CommandOptionType.SingleValue, ShortName = "a", LongName = "accesstoken", Description = "spotify access token", ValueName = "access token", ShowInHelpText = true)]
        public string SpotifyAccessToken { get; set; }

        [Option(CommandOptionType.NoValue, ShortName = "g", LongName = "id3tags", Description = "set id3 tags", ValueName = "set id3 tags", ShowInHelpText = true)]
        public bool SetId3Tags { get; set; }

        [Option(CommandOptionType.SingleValue, ShortName = "k", LongName = "keyformat", Description = "desired  format for id3tag 'InitialKey'", ValueName = "desired  format for id3tag 'InitialKey'", ShowInHelpText = true)]
        public string DesiredKeyFormat { get; set; } = MusicalKeyFormat.OpenKey.Value;

        [Option(CommandOptionType.NoValue, ShortName = "s", LongName = "skip-results", Description = "skip tracks that are present in results folder", ValueName = "skip present results", ShowInHelpText = true)]
        public bool SkipPresentResults { get; }

        [Option(CommandOptionType.NoValue, ShortName = "f", LongName = "flac", Description = "allow flac files", ValueName = "allow flac", ShowInHelpText = true)]
        public bool AllowFlac { get; }

        public const int DEFAULT_SEARCH_TIMEOUT = 10;
        [Option(CommandOptionType.SingleValue, ShortName = "t", LongName = "searchtimeout", Description = "max searching time per query", ValueName = "search timeout", ShowInHelpText = true)]
        public int? SearchTimeout { get; set; }


        public DownloadAndSavePlaylistCommand(ILogger<DownloadAndSavePlaylistCommand> logger, IConsole console)
        {
            _logger = logger;
            _console = console;
        }

        protected override async Task<int> OnExecute(CommandLineApplication app)
        {
            if (string.IsNullOrEmpty(SpotifyUserId))
            {
                SpotifyUserId = Prompt.GetString("Spotify user id:", SpotifyUserId);
            }

            if (string.IsNullOrEmpty(SpotifyPlaylistId) && string.IsNullOrEmpty(SpotifyPlaylistName))
            {
                SpotifyPlaylistId = Prompt.GetString("Spotify playlist id (leave blank to use name):", SpotifyPlaylistId);
            }

            if (string.IsNullOrEmpty(SpotifyPlaylistId) && string.IsNullOrEmpty(SpotifyPlaylistName))
            {
                SpotifyPlaylistName = Prompt.GetString("Spotify playlist name:", SpotifyPlaylistName);
            }

            if (string.IsNullOrEmpty(SpotifyAccessToken))
            {
                SpotifyAccessToken = Prompt.GetString("Spotify API access token:", SpotifyAccessToken);
            }

            if (string.IsNullOrEmpty(SSUsername))
            {
                SSUsername = Prompt.GetString("Soulseek user name:", SSUsername);
            }

            if (string.IsNullOrEmpty(SSPassword))
            {
                SSPassword = SecureStringToString(Prompt.GetPasswordAsSecureString("Soulseek password:"));
            }

            SearchTimeout ??= Prompt.GetInt("Max time per search query?", DEFAULT_SEARCH_TIMEOUT);

            try
            {
                await base.OnExecute(app);
                UserProfile userProfile = new()
                {
                    Username = SSUsername,
                    Password = Encrypt(SSPassword),
                };

                if (!Directory.Exists(ProfileFolder))
                {
                    Directory.CreateDirectory(ProfileFolder);
                }

                await File.WriteAllTextAsync($"{ProfileFolder}{Profile}", JsonSerializer.Serialize(userProfile), UTF8Encoding.UTF8);

                Log.Information($"SetId3Tags is set to '{SetId3Tags}'");
                Log.Information($"AllowFlac is set to '{AllowFlac}'");
                Log.Information($"SkipPResults is set to '{SkipPresentResults}'");

                await _downloadService.DownloadUnsavedPlaylistTracksFromUserAsync(SpotifyUserId,
                    SpotifyPlaylistId,
                    SpotifyPlaylistName,
                    SSUsername,
                    SSPassword,
                    SpotifyAccessToken,
                    SetId3Tags,
                    MusicalKeyFormat.from(DesiredKeyFormat),
                    options =>
                {
                    options.AllowFlac = AllowFlac;
                    options.SkipResults = SkipPresentResults;
                    options.SearchTimeout = SearchTimeout.Value;
                });

                return 0;

            }
            catch (Exception ex)
            {
                OnException(ex);
                return 1;
            }
        }
    }
}

