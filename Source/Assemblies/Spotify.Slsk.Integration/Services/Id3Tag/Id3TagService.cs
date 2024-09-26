using System.Runtime.InteropServices;
using Spotify.Slsk.Integration.Models.enums;
using Spotify.Slsk.Integration.Models.Spotify;
using Spotify.Slsk.Integration.Services.Id3Tag.Translators;
using Serilog;

namespace Spotify.Slsk.Integration.Services.Id3Tag
{
    public class Id3TagService
    {
        public static void SetId3Tags(TrackItem trackItem, String path)
        {
            TagLib.File file = TagLib.File.Create(path);
            file.Tag.Title = trackItem.Track!.Name;
            file.Tag.Performers = trackItem.Track!.Artists!.Select(s => s.Name).ToArray();
            file.Tag.AlbumArtists = trackItem.Track!.Album!.Artists!.Select(s => s.Name).ToArray();
            file.Tag.Album = trackItem.Track!.Album!.Name;
            file.Tag.Year = uint.Parse(trackItem.Track.Album.ReleaseDate![..4]);
            file.Save();
        }

        public static void SetId3Tags(TrackItem trackItem, AudioFeatures audioFeatures, MusicalKeyFormat musicalKeyFormat, String path)
        {
            SetId3Tags(trackItem, path);
            TagLib.File file = TagLib.File.Create(path);
            file.Tag.BeatsPerMinute = ((uint)audioFeatures.Tempo);
            string? currentKeyString = file.Tag.InitialKey;
            OpenKey? currentKey = currentKeyString == null ? null : OpenKey.From(currentKeyString);

            switch (musicalKeyFormat)
            {
                case var _ when musicalKeyFormat.Equals(MusicalKeyFormat.OpenKey):
                    if (!string.IsNullOrEmpty(currentKeyString) && currentKey == null)
                    {
                        string? openKeyString = MusicalKeyTranslator.TranslateKeyToOpenKeyFormat(currentKeyString);
                        currentKey = openKeyString == null ? null : OpenKey.From(openKeyString);
                    }

                    OpenKey? openKey = MusicalKeyTranslator.TranslatePitchKeyToOpenKeyFormat(audioFeatures.Key);
                    if (currentKey == null && openKey != null)
                    {
                        file.Tag.InitialKey = openKey.Value;
                    }

                    break;
                case var _ when musicalKeyFormat.Equals(MusicalKeyFormat.Tonal):
                    file.Tag.InitialKey = MusicalKeyTranslator.TranslatePitchKeyToTonalFormat(audioFeatures.Key);
                    break;
            }

            file.Save();
        }

        public static void TranslateMusicalKeyForFilesInFolder(string folderPath)
        {
            folderPath = Path.GetFullPath(folderPath);
            var mp3Files = Directory.GetFiles(folderPath, "*.mp3", SearchOption.AllDirectories);

            foreach (var file in mp3Files)
            {
                string prettyFileName;
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    prettyFileName = file.Split(@"\")[^1];
                } else
                {
                    prettyFileName = file.Split(@"/")[^1];
                }

                try
                {
                    var tfile = TagLib.File.Create(file);
                    string? currentKeyString = tfile.Tag.InitialKey;
                    OpenKey? currentKey = currentKeyString == null ? null : OpenKey.From(currentKeyString);

                    if (!string.IsNullOrEmpty(currentKeyString) && currentKey == null)
                    {
                        string? translatedKey = MusicalKeyTranslator.TranslateKeyToOpenKeyFormat(currentKeyString);
                        if (!string.IsNullOrEmpty(translatedKey))
                        {
                            tfile.Tag.InitialKey = translatedKey;
                        }

                        tfile.Save();
                        Log.Information($"Updated InitialKey for '{prettyFileName}' from '{currentKeyString}' to {translatedKey}.");
                    }
                    else if (string.IsNullOrEmpty(currentKeyString))
                    {
                        Log.Warning($"No InitialKey tag found for '{prettyFileName}'.");
                    }
                    else if (currentKey != null)
                    {
                        Log.Information($"OpenKey already set to '{currentKey}' for '{prettyFileName}'.");
                    }
                    else
                    {
                        Log.Error("Unexpected code block hit!");
                    }
                }
                catch (Exception ex)
                {
                    Log.Error($"Error processing file {prettyFileName}: {ex.Message}");
                    continue;
                }
            }
        }
    }
}
