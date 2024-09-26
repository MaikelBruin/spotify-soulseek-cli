using System;
namespace Spotify.Slsk.Integration.Models.enums
{
    public class MusicalKeyFormat
    {
        private const string TONAL_KEY = "TONAL";
        private const string OPEN_KEY_KEY = "OPENKEY";
        public static readonly MusicalKeyFormat Tonal = new MusicalKeyFormat(TONAL_KEY);
        public static readonly MusicalKeyFormat OpenKey = new MusicalKeyFormat(OPEN_KEY_KEY);

        public string Value { get; private set; }

        private MusicalKeyFormat(string value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value;
        }

        public override bool Equals(object? obj)
        {
            if (obj is MusicalKeyFormat other)
            {
                return Value.Equals(other.Value);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public static implicit operator string(MusicalKeyFormat keyFormat)
        {
            return keyFormat.Value;
        }

        public static MusicalKeyFormat from(string keyFormatString)
        {
            return (keyFormatString.ToUpper()) switch
            {
                TONAL_KEY => Tonal,
                OPEN_KEY_KEY => OpenKey,
                _ => throw new Exception($"Could not translate key format '{keyFormatString}'")
            };
        }
    }
}

