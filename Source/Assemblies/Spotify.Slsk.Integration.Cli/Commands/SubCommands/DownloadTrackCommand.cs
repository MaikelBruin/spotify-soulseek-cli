using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Spotify.Slsk.Integration.Models;
using Spotify.Slsk.Integration.Services.SoulSeek;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;

namespace Spotify.Slsk.Integration.Cli.Commands.SubCommands
{
    [Command(Name = "download-track", Description = "searches track on soulseek and checks for available high quality mp3s")]
    class DownloadTrackCommand: SpotSeekCommandBase
    {
        [Option(CommandOptionType.SingleValue, ShortName = "u", LongName = "ssusername", Description = "soulseek login username", ValueName = "login username", ShowInHelpText = true)]
        public string SSUsername { get; set; }

        [Option(CommandOptionType.SingleValue, ShortName = "p", LongName = "sspassword", Description = "soulseek login password", ValueName = "login password", ShowInHelpText = true)]
        public string SSPassword { get; set; }

        [Option(CommandOptionType.SingleValue, ShortName = "q", LongName = "query", Description = "search query for soulseek", ValueName = "search query", ShowInHelpText = true)]
        public string SearchQuery { get; set; }

        public DownloadTrackCommand(ILogger<DownloadTrackCommand> logger, IConsole console)
        {
            _logger = logger;
            _console = console;
        }

        protected override async Task<int> OnExecute(CommandLineApplication app)
        {
            if (string.IsNullOrEmpty(SSUsername))
            {
                SSUsername = Prompt.GetString("Soulseek user name:", SSUsername);
            }

            if (string.IsNullOrEmpty(SSPassword))
            {
                SSPassword = SecureStringToString(Prompt.GetPasswordAsSecureString("Soulseek password:"));
            }

            if (string.IsNullOrEmpty(SearchQuery))
            {
                SearchQuery = Prompt.GetString("Soulseek search query:", SearchQuery);
            }

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

                TrackToDownload trackToDownload = new()
                {
                    Query = SearchQuery
                };

                SoulseekResult result = await SoulseekService.GetTrackAsync(_soulseekClient, trackToDownload, SSUsername, SSPassword, new SoulseekOptions());
                bool success = result.Success;

                if (success)
                {
                    return 0;
                }
                else
                {
                    _logger.LogError($"Failed downloading track using query '{SearchQuery}'");
                    return 2;
                }
            }
            catch (Exception ex)
            {
                OnException(ex);
                return 1;
            }
        }
    }
}

