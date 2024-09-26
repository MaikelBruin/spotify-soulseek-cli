namespace Spotify.Slsk.Integration.Extensions
{
    public static class LongExtensions
	{
		public static string ToMB(this long size)
		{
			return $"{size / (double)1000000:N2}MB";
		}
	}
}

