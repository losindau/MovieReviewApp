using MovieReview.Domain.Commons.Constants;

namespace MovieReview.Domain.Entities
{
    public class Movie
    {
        private int _id;
        private string _name;
        private int _publicationYear;
        private string _directorName;
        private string _nation;
        private ICollection<Review> _reviews = new List<Review>();

        public Movie()
        {
            
        }

        public Movie(int id, string name, int publicationYear, string directorName, string nation)
        {
            _id = id;
            _name = name;
            _publicationYear = publicationYear;
            _directorName = directorName;
            _nation = nation;
        }

        public int Id
        {
            get { return _id; }
            init
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(MovieConstants.IdCannotBeNegative);
                }
                _id = value;
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                if (String.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentNullException(MovieConstants.NameCannotBeNullOrWhiteSpace);
                }
                _name = value;
            }
        }

        public int PublicationYear
        {
            get { return _publicationYear; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(MovieConstants.PublicationYearCannotBeNegative);
                }
                _publicationYear = value;
            }
        }

        public string DirectorName
        {
            get { return _directorName; }
            set
            {
                if (String.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentNullException(MovieConstants.DirectorNameCannotBeNullOrWhiteSpace);
                }
                _directorName = value;
            }
        }

        public string Nation
        {
            get { return _nation; }
            set
            {
                if (String.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentNullException(MovieConstants.NationCannotBeNullOrWhiteSpace);
                }
                _nation = value;
            }
        }

        public ICollection<Review> Reviews { get => _reviews; set => _reviews = value; }
    }
}
