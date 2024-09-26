using Spotify.Slsk.Integration.Models.enums;
using Spotify.Slsk.Integration.Models.Spotify;
using Spotify.Slsk.Integration.Services.Id3Tag;
using Xunit;

namespace Tests.Integrated
{
    public class Id3TagTests
    {
        readonly string PathToTrackItem = $"..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}Resources{Path.DirectorySeparatorChar}sample-track.json";
        readonly string PathToSampleMp3 = $"..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}Resources{Path.DirectorySeparatorChar}sample-3s.mp3";
        readonly string PathToTrackItemAudioFeatures = $"..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}Resources{Path.DirectorySeparatorChar}sample-audio-features.json";


        [Fact]
        public void ShouldSetId3Tags()
        {
            TrackItem trackItem = new();
            using (StreamReader r = new(PathToTrackItem))
            {
                string json = r.ReadToEnd();
                trackItem.Track = System.Text.Json.JsonSerializer.Deserialize<Track>(json)!;
            }

            Id3TagService.SetId3Tags(trackItem, PathToSampleMp3);
        }

        [Fact]
        public void ShouldSetId3TagsWithAudioFeatures()
        {
            TrackItem trackItem = new();
            AudioFeatures audioFeatures = new();
            using (StreamReader r = new(PathToTrackItem))
            {
                string json = r.ReadToEnd();
                trackItem.Track = System.Text.Json.JsonSerializer.Deserialize<Track>(json)!;
            }

            using (StreamReader r = new(PathToTrackItemAudioFeatures))
            {
                string json = r.ReadToEnd();
                audioFeatures = System.Text.Json.JsonSerializer.Deserialize<AudioFeatures>(json)!;
            }

            Id3TagService.SetId3Tags(trackItem, audioFeatures, MusicalKeyFormat.OpenKey, PathToSampleMp3);
        }

    }
}
