using Serilog;
using System.Text;

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

        public static string RemoveSpecialCharacters(this string input)
        {
            StringBuilder stringBuilder = new();
            string processed = input.Replace("-", " ").Replace("(", "").Replace(")", "");
            foreach (string queryPart in processed.Split(" "))
            {
                if (!queryPart.HasSpecialChars())
                {
                    stringBuilder.Append(queryPart).Append(' ');
                }
            }

            string result = stringBuilder.ToString();
            if (string.IsNullOrWhiteSpace(result))
            {
                Log.Warning($"Track '{input}' produced an empty query, trying with raw input including special chars...");
                return input;
            }
            else
            {
                return result.Trim();
            }
        }
    }
}

