using MovieReview.Application.DTOs;
using MovieReview.Domain.Commons;

namespace MovieReview.Application.Interfaces
{
    public interface IMovieService
    {
        public Task AddMovieAsync(string name, int publicationYear, string directionName, string nation);
        public Task DeleteMovieAsync(int id);
        public Task UpdateMovieAsync(int id, string name, int publicationYear, string directorName, string nation);
        public Task<PaginatedList<MovieDto>> GetAllAsync(int pageNumber, int pageSize);
        public Task<PaginatedList<MovieDto>> GetByDirectorNameAsync(int pageNumber, int pageSize, string directorName);
        public Task<PaginatedList<MovieDto>> GetByNameAsync(int pageNumber, int pageSize, string name);
        public Task<PaginatedList<MovieDto>> GetByNationAsync(int pageNumber, int pageSize, string nation);
        public Task<PaginatedList<MovieDto>> GetByPublicationYearAsync(int pageNumber, int pageSize, int publicationYear);
        public Task<MovieDto> GetByIdAsync(int id);
    }
}
