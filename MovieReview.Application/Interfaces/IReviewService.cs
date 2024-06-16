using MovieReview.Domain.Commons.Enums;

namespace MovieReview.Application.Interfaces
{
    public interface IReviewService
    {
        public Task AddReviewAsync(string title, string description, ReviewRate rate, int movieId);
    }
}
