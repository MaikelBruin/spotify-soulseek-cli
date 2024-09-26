using System;
using System.Threading.Tasks;
using Spotify.Slsk.Integration.Services.Id3Tag;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Spotify.Slsk.Integration.Cli.Commands.SubCommands
{
    [Command(Name = "translate-to-open-key", Description = "Translates the musical key id3tag for a given folder to Open Key format.")]
    class TranslateMusicalKeyCommand : SpotSeekCommandBase
    {
        [Option(CommandOptionType.SingleValue, ShortName = "f", LongName = "folder", Description = "folder to translate", ValueName = "folder to translate", ShowInHelpText = true)]
        public string InputFolder { get; set; }

        public TranslateMusicalKeyCommand(ILogger<TranslateMusicalKeyCommand> logger, IConsole console)
        {
            _logger = logger;
            _console = console;
        }

        protected override async Task<int> OnExecute(CommandLineApplication app)
        {
            if (string.IsNullOrEmpty(InputFolder))
            {
                InputFolder = Prompt.GetString("Input folder (absolute path):", InputFolder);
            }

            try
            {
                await base.OnExecute(app);
                Log.Information($"Folder is set to '{InputFolder}'");
                Log.Information($"Attempting to translate 'InitialKey' id3tag to OpenKey format...");
                Id3TagService.TranslateMusicalKeyForFilesInFolder(InputFolder);
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

