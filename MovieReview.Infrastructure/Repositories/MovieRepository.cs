using Microsoft.EntityFrameworkCore;
using MovieReview.Domain.Commons;
using MovieReview.Domain.Entities;
using MovieReview.Domain.Interfaces;
using MovieReview.Infrastructure.Data;
using System;

namespace MovieReview.Infrastructure.Repositories
{
    public class MovieRepository : Repository<Movie>, IMovieRepository
    {
        public MovieRepository(ApplicationDbContext context) : base(context)
        {
            
        }

        public void Delete(Movie movie)
        {
            _context.Remove(movie);
        }

        public async Task<PaginatedList<Movie>> FilterByDirectorNameAsync(int pageNumber, int pageSize, string directorName)
        {
            IQueryable<Movie> query = _context.Movies
                .Where(x => x.DirectorName.ToLower().Contains(directorName.ToLower().Trim()));

            int count = await query.CountAsync();

            List<Movie> movieCollection = await query
                .OrderBy(x => x.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedList<Movie>(movieCollection, count, pageNumber, pageSize);
        }

        public async Task<PaginatedList<Movie>> FilterByNameAsync(int pageNumber, int pageSize, string name)
        {
            IQueryable<Movie> query = _context.Movies
                .Where(x => x.Name.ToLower().Contains(name.ToLower().Trim()));

            int count = await query.CountAsync();

            List<Movie> movieCollection = await query
                .OrderBy(x => x.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedList<Movie>(movieCollection, count, pageNumber, pageSize);
        }

        public async Task<PaginatedList<Movie>> FilterByNationAsync(int pageNumber, int pageSize, string nation)
        {
            IQueryable<Movie> query = _context.Movies
                .Where(x => x.Nation.ToLower() == nation.ToLower().Trim());
            int count = await query.CountAsync();

            List<Movie> movieCollection = await query
                .OrderBy(x => x.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedList<Movie>(movieCollection, count, pageNumber, pageSize);
        }

        public async Task<PaginatedList<Movie>> FilterByPublicationYearAsync(int pageNumber, int pageSize, int publicationYear)
        {
            IQueryable<Movie> query = _context.Movies
                .Where(x => x.PublicationYear == publicationYear);

            int count = await query.CountAsync();

            List<Movie> movieCollection = await query
                .OrderBy(x => x.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedList<Movie>(movieCollection, count, pageNumber, pageSize);
        }

        public async Task<PaginatedList<Movie>> GetAllAsync(int pageNumber, int pageSize)
        {
            int count = await _context.Movies.CountAsync();

            List<Movie> movieCollection = await _context.Movies
                .OrderBy(x => x.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedList<Movie>(movieCollection, count, pageNumber, pageSize);
        }

        public async Task<Movie?> GetByIdAsync(int id)
        {
            return await _context.Movies
                .Include(x => x.Reviews)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}