namespace Spotify.Slsk.Integration.Extensions
{
    public static class DoubleExtensions
	{
		public static string ToMB(this double size)
		{
			return $"{size / 1000000:N2}MB";
		}
	}
}

