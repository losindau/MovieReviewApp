using MovieReview.Domain.Commons;
using MovieReview.Domain.Entities;

namespace MovieReview.Domain.Interfaces
{
    public interface IMovieRepository : IRepository<Movie>
    {
        Task<PaginatedList<Movie>> GetAllAsync(int pageNumber, int pageSize);
        Task<PaginatedList<Movie>> FilterByNameAsync(int pageNumber, int pageSize, string name);
        Task<PaginatedList<Movie>> FilterByPublicationYearAsync(int pageNumber, int pageSize, int publicationYear);
        Task<PaginatedList<Movie>> FilterByDirectorNameAsync(int pageNumber, int pageSize, string directorName);
        Task<PaginatedList<Movie>> FilterByNationAsync(int pageNumber, int pageSize, string nation);
        Task<Movie?> GetByIdAsync(int id);
        void Delete(Movie movie);
    }
}
