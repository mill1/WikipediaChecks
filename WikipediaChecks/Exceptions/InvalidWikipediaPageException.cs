using System;

namespace WikipediaChecks.Exceptions
{
    public class InvalidWikipediaPageException : Exception
    {
        public InvalidWikipediaPageException()
        {
        }

        public InvalidWikipediaPageException(string message)
            : base(message)
        {
        }

        public InvalidWikipediaPageException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
