using Spotify.Slsk.Integration.Models.enums;
using Spotify.Slsk.Integration.Models.Exceptions;

namespace Spotify.Slsk.Integration.Services.Id3Tag.Translators
{
    public class MusicalKeyTranslator
    {
        //with help of https://en.wikipedia.org/wiki/Pitch_class
        public static string TranslatePitchKeyToTonalFormat(int key)
        {
            return (key) switch
            {
                0 => "C",
                1 => "C-sharp",
                2 => "D",
                3 => "D-sharp",
                4 => "E",
                5 => "F",
                6 => "F-sharp",
                7 => "G",
                8 => "G-sharp",
                9 => "A",
                10 => "A-sharp",
                11 => "B",
                -1 => "",
                _ => "",
            };
        }

        //with help of https://getsongkey.com/tools/notation-converter
        public static OpenKey? TranslatePitchKeyToOpenKeyFormat(int key)
        {
            return (key) switch
            {
                0 => OpenKey._01d,
                1 => OpenKey._08d,
                2 => OpenKey._03d,
                3 => OpenKey._10d,
                4 => OpenKey._05d,
                5 => OpenKey._12d,
                6 => OpenKey._07d,
                7 => OpenKey._02d,
                8 => OpenKey._09d,
                9 => OpenKey._04d,
                10 => OpenKey._11d,
                11 => OpenKey._06d,
                -1 => null,
                _ => null,
            };
        }

        // with help from https://getsongkey.com/tools/notation-converter
        public static string? TranslateKeyToOpenKeyFormat(string tonalKey)
        {
            return (tonalKey.ToLower()) switch
            {
                //tonal
                "c" => OpenKey._01d,
                "cm" => OpenKey._10m,
                "c-sharp" => OpenKey._08d,
                "c#" => OpenKey._08d,
                "c#m" => OpenKey._05m,
                "d" => OpenKey._03d,
                "dm" => OpenKey._12m,
                "d-sharp" => OpenKey._10d,
                "d#" => OpenKey._10d,
                "d#m" => OpenKey._07m,
                "e" => OpenKey._05d,
                "em" => OpenKey._02m,
                "f" => OpenKey._12d,
                "fm" => OpenKey._09m,
                "f-sharp" => OpenKey._07d,
                "f#" => OpenKey._07d,
                "f#m" => OpenKey._04m,
                "g" => OpenKey._02d,
                "gm" => OpenKey._11m,
                "g-sharp" => OpenKey._09d,
                "g#" => OpenKey._09d,
                "g#m" => OpenKey._06m,
                "a" => OpenKey._04d,
                "am" => OpenKey._01m,
                "a-sharp" => OpenKey._11d,
                "a#" => OpenKey._11d,
                "a#m" => OpenKey._08m,
                "b" => OpenKey._06d,
                "bm" => OpenKey._03m,
                //camelot
                "10a" => OpenKey._03m,
                "10b" => OpenKey._03d,
                "11a" => OpenKey._04m,
                "11b" => OpenKey._04d,
                "12a" => OpenKey._05m,
                "12b" => OpenKey._05d,
                "1a" => OpenKey._06m,
                "01a" => OpenKey._06m,
                "1b" => OpenKey._06d,
                "01b" => OpenKey._06d,
                "2a" => OpenKey._07m,
                "02a" => OpenKey._07m,
                "2b" => OpenKey._07d,
                "02b" => OpenKey._07d,
                "3a" => OpenKey._08m,
                "03a" => OpenKey._08m,
                "3b" => OpenKey._08d,
                "03b" => OpenKey._08d,
                "4a" => OpenKey._09m,
                "04a" => OpenKey._09m,
                "4b" => OpenKey._09d,
                "04b" => OpenKey._09d,
                "5a" => OpenKey._10m,
                "05a" => OpenKey._10m,
                "5b" => OpenKey._10d,
                "05b" => OpenKey._10d,
                "6a" => OpenKey._11m,
                "06a" => OpenKey._11m,
                "6b" => OpenKey._11d,
                "06b" => OpenKey._11d,
                "7a" => OpenKey._12m,
                "07a" => OpenKey._12m,
                "7b" => OpenKey._12d,
                "07b" => OpenKey._12d,
                "8a" => OpenKey._01m,
                "08a" => OpenKey._01m,
                "8b" => OpenKey._01d,
                "08b" => OpenKey._01d,
                "9a" => OpenKey._02m,
                "09a" => OpenKey._02m,
                "9b" => OpenKey._02d,
                "09b" => OpenKey._02d,

                //openkey without leading zero
                "1d" => OpenKey._01d,
                "1m" => OpenKey._01m,
                "2d" => OpenKey._02d,
                "2m" => OpenKey._02m,
                "3d" => OpenKey._03d,
                "3m" => OpenKey._03m,
                "4d" => OpenKey._04d,
                "4m" => OpenKey._04m,
                "5d" => OpenKey._05d,
                "5m" => OpenKey._05m,
                "6d" => OpenKey._06d,
                "6m" => OpenKey._06m,
                "7d" => OpenKey._07d,
                "7m" => OpenKey._07m,
                "8d" => OpenKey._08d,
                "8m" => OpenKey._08m,
                "9d" => OpenKey._09d,
                "9m" => OpenKey._09m,

                //other
                "" => "",
                _ => null
            } ;
        }
    }
}
