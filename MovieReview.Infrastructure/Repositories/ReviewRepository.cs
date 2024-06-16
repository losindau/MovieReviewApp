using MovieReview.Domain.Entities;
using MovieReview.Domain.Interfaces;
using MovieReview.Infrastructure.Data;

namespace MovieReview.Infrastructure.Repositories
{
    public class ReviewRepository : Repository<Review>, IReviewRepository
    {
        public ReviewRepository(ApplicationDbContext context) : base(context)
        {

        }
    }
}
