using System.Runtime.Serialization;

namespace MovieReview.Application.Commons.Exceptions
{
    public class MovieNotFoundException : Exception
    {
        public MovieNotFoundException() 
        {

        }

        public MovieNotFoundException(string? message) : base(message)
        {

        }

        public MovieNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {

        }
    }
}
