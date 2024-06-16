using Microsoft.EntityFrameworkCore;
using MovieReview.Domain.Entities;

namespace MovieReview.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base (options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Movie>()
                .HasMany(m => m.Reviews)
                .WithOne(r => r.Movie)
                .HasForeignKey(r => r.MovieId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        public DbSet<Movie> Movies { get; init; }
        public DbSet<Review> Reviews { get; init; }
    }
}
