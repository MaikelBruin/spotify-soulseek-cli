using System;
using System.Reflection;
using System.Threading.Tasks;
using Spotify.Slsk.Integration.Cli.Commands.SubCommands;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;

namespace Spotify.Slsk.Integration.Cli.Commands
{

    [Command(Name = "spotseek", OptionsComparison = StringComparison.InvariantCultureIgnoreCase)]
    [VersionOptionFromMember("--version", MemberName = nameof(GetVersion))]
    [Subcommand(
        typeof(TranslateMusicalKeyCommand),
        typeof(DownloadTrackCommand),
        typeof(DownloadAndSavePlaylistCommand),
        typeof(DownloadPlaylistCommand))]
    class SpotseekCommand : SpotSeekCommandBase
    {
        public SpotseekCommand(ILogger<SpotseekCommand> logger, IConsole console)
        {
            _logger = logger;
            _console = console;
        }

        protected override Task<int> OnExecute(CommandLineApplication app)
        {
            // this shows help even if the --help option isn't specified
            app.ShowHelp();
            return Task.FromResult(0);
        }

        private static string GetVersion()
            => typeof(SpotseekCommand).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
    }
}

