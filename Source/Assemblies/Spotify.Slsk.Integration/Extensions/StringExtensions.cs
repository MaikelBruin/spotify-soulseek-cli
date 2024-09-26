namespace Spotify.Slsk.Integration.Extensions
{
    public static class StringExtensions
	{
		public static string ToLocalOSPath(this string path)
		{
			return path.Replace('\\', Path.DirectorySeparatorChar).Replace('/', Path.DirectorySeparatorChar);
		}

        public static bool HasSpecialChars(this string inputString)
        {
            return inputString.Any(ch => !Char.IsLetterOrDigit(ch));
        }
    }
}

