using MovieReview.Domain.Commons.Constants;
using MovieReview.Domain.Commons.Enums;

namespace MovieReview.Domain.Entities
{
    public class Review
    {
        private int _id;
        private string _title = "";
        private string _description = "";
        private ReviewRate _rate;
        private int _movieId;
        private Movie? movie;

        public Review()
        {
            
        }

        public Review(int id, string title, string description, ReviewRate rate, int movieId)
        {
            _id = id;
            _title = title;
            _description = description;
            _rate = rate;
            _movieId = movieId;
        }

        public int Id
        {
            get { return _id; }
            init
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(ReviewConstants.IdCannotBeNegative);
                }
                _id = value;
            }
        }

        public string Title
        {
            get { return _title; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentNullException(ReviewConstants.TitleCannotBeNullOrWhiteSpace);
                }
                _title = value;
            }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public ReviewRate Rate
        {
            get { return _rate; }
            set
            {
                if (value < ReviewRate.Boring || value > ReviewRate.Good)
                {
                    throw new ArgumentOutOfRangeException(ReviewConstants.RateOutOfRange);
                }
                _rate = value;
            }
        }

        public int MovieId
        {
            get { return _movieId; }
            init
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(ReviewConstants.IdCannotBeNegative);
                }
                _movieId = value;
            }
        }

        public Movie Movie { get => movie; init => movie = value; }
    }
}
