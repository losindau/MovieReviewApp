using MapsterMapper;
using MovieReview.Application.Commons.Constants;
using MovieReview.Application.Commons.Exceptions;
using MovieReview.Application.DTOs;
using MovieReview.Application.Interfaces;
using MovieReview.Domain.Commons;
using MovieReview.Domain.Entities;
using MovieReview.Domain.Interfaces;

namespace MovieReview.Application.Services
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IMapper _mapper;

        public MovieService(IMovieRepository movieRepository, IMapper mapper)
        {
            _movieRepository = movieRepository;
            _mapper = mapper;
        }

        public async Task AddMovieAsync(string name, int publicationYear, string directionName, string nation)
        {
            if (publicationYear > DateTime.Now.Year)
            {
                throw new ArgumentException(MovieConstants.InvalidYear);
            }

            MovieDto movieDto = new MovieDto()
            {
                Name = name,
                PublicationYear = publicationYear,
                DirectorName = directionName,
                Nation = nation
            };

            Movie movie = _mapper.Map<Movie>(movieDto);

            await _movieRepository.AddAsync(movie);
            await _movieRepository.SaveChangesAsync();
        }

        public async Task DeleteMovieAsync(int id)
        {
            Movie? movie = await _movieRepository.GetByIdAsync(id);

            if (movie == null)
            {
                throw new MovieNotFoundException(MovieConstants.NotFound);
            }

            _movieRepository.Delete(movie);
            await _movieRepository.SaveChangesAsync();
        }

        public async Task UpdateMovieAsync(int id, string name, int publicationYear, string directorName, string nation)
        {
            Movie? movie = await _movieRepository.GetByIdAsync(id);

            if (movie == null)
            {
                throw new MovieNotFoundException(MovieConstants.NotFound);
            }

            if (publicationYear > DateTime.Now.Year)
            {
                throw new ArgumentException(MovieConstants.InvalidYear);
            }

            movie.Name = name;
            movie.PublicationYear = publicationYear;
            movie.DirectorName = directorName;
            movie.Nation = nation;

            await _movieRepository.SaveChangesAsync();
        }

        public async Task<MovieDto> GetByIdAsync(int id)
        {
            Movie? movie = await _movieRepository.GetByIdAsync(id);

            if (movie == null)
            {
                throw new MovieNotFoundException(MovieConstants.NotFound);
            }

            return _mapper.Map<MovieDto>(movie);
        }

        public async Task<PaginatedList<MovieDto>> GetAllAsync(int pageNumber, int pageSize)
        {
            PaginatedList<Movie> movies = await _movieRepository.GetAllAsync(pageNumber, pageSize);

            if (movies.Items.Count < 1)
            {
                throw new MoviesListEmpty(MovieConstants.Empty);
            }

            return _mapper.Map<PaginatedList<MovieDto>>(movies);
        }

        public async Task<PaginatedList<MovieDto>> GetByDirectorNameAsync(int pageNumber, int pageSize, string directorName)
        {
            PaginatedList<Movie> movies = await _movieRepository.FilterByDirectorNameAsync(pageNumber, pageSize, directorName);

            if (movies.Items.Count < 1)
            {
                throw new MoviesListEmpty(MovieConstants.Empty);
            }

            return _mapper.Map<PaginatedList<MovieDto>>(movies);
        }

        public async Task<PaginatedList<MovieDto>> GetByNameAsync(int pageNumber, int pageSize, string name)
        {
            PaginatedList<Movie> movies = await _movieRepository.FilterByNameAsync(pageNumber, pageSize, name);

            if (movies.Items.Count < 1)
            {
                throw new MoviesListEmpty(MovieConstants.Empty);
            }

            return _mapper.Map<PaginatedList<MovieDto>>(movies);
        }

        public async Task<PaginatedList<MovieDto>> GetByNationAsync(int pageNumber, int pageSize, string nation)
        {
            PaginatedList<Movie> movies = await _movieRepository.FilterByNationAsync(pageNumber, pageSize, nation);

            if (movies.Items.Count < 1)
            {
                throw new MoviesListEmpty(MovieConstants.Empty);
            }

            return _mapper.Map<PaginatedList<MovieDto>>(movies);
        }

        public async Task<PaginatedList<MovieDto>> GetByPublicationYearAsync(int pageNumber, int pageSize, int publicationYear)
        {
            PaginatedList<Movie> movies = await _movieRepository.FilterByPublicationYearAsync(pageNumber, pageSize, publicationYear);

            if (movies.Items.Count < 1)
            {
                throw new MoviesListEmpty(MovieConstants.Empty);
            }

            return _mapper.Map<PaginatedList<MovieDto>>(movies);
        }
    }
}