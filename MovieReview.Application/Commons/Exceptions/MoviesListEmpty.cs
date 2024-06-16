using System.Runtime.Serialization;

namespace MovieReview.Application.Commons.Exceptions
{
    public class MoviesListEmpty : Exception
    {
        public MoviesListEmpty()
        {

        }

        public MoviesListEmpty(string? message) : base(message)
        {

        }

        public MoviesListEmpty(string? message, Exception? innerException) : base(message, innerException)
        {

        }
    }
}
