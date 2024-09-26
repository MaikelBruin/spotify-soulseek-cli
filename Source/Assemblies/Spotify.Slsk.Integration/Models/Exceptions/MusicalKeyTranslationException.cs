using System;
namespace Spotify.Slsk.Integration.Models.Exceptions
{
	public class MusicalKeyTranslationException : Exception
	{
        public MusicalKeyTranslationException()
        {
        }

        public MusicalKeyTranslationException(string message)
            : base(message)
        {
        }

        public MusicalKeyTranslationException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}

