using MovieReview.Application.DTOs;
using MovieReview.Application.Interfaces;
using MovieReview.Domain.Commons.Enums;
using MovieReview.Domain.Entities;
using MapsterMapper;
using MovieReview.Domain.Interfaces;
using MovieReview.Application.Commons.Exceptions;
using MovieReview.Application.Commons.Constants;

namespace MovieReview.Application.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IMovieRepository _movieRepository;
        private readonly IMapper _mapper;

        public ReviewService(IReviewRepository reviewRepository, IMovieRepository movieRepository, IMapper mapper)
        {
            _reviewRepository = reviewRepository;
            _movieRepository = movieRepository;
            _mapper = mapper;
        }

        public async Task AddReviewAsync(string title, string description, ReviewRate rate, int movieId)
        {
            Movie? movie = await _movieRepository.GetByIdAsync(movieId);

            if (movie == null)
            {
                throw new MovieNotFoundException(MovieConstants.NotFound);
            }

            ReviewDto reviewDto = new ReviewDto()
            {
                Title = title,
                Description = description,
                Rate = rate,
                MovieId = movieId
            };

            Review review = _mapper.Map<Review>(reviewDto);

            await _reviewRepository.AddAsync(review);
            await _reviewRepository.SaveChangesAsync();
        }
    }
}
