using Mapster;
using MapsterMapper;
using Moq;
using MovieReview.Application.Commons.Constants;
using MovieReview.Application.Commons.Exceptions;
using MovieReview.Application.DTOs;
using MovieReview.Application.Services;
using MovieReview.Domain.Commons.Enums;
using MovieReview.Domain.Entities;
using MovieReview.Domain.Interfaces;

namespace MovieReview.Application.Test
{
    public class ReviewServiceTest
    {
        [Fact]
        public async Task AddReviewAsync_WithValidData_AddsReview()
        {
            // Arrange
            var title = "Sample Review";
            var description = "This is a great movie!";
            var rate = ReviewRate.Good;
            var movieId = 1;

            var movieRepositoryMock = new Mock<IMovieRepository>();
            var reviewRepositoryMock = new Mock<IReviewRepository>();
            var mapperMock = new Mock<IMapper>();
            var reviewService = new ReviewService(reviewRepositoryMock.Object, movieRepositoryMock.Object, mapperMock.Object);

            // Simulate retrieving a movie by ID
            var movie = new Movie
            {
                Id = movieId,
                Name = "Sample Movie",
                PublicationYear = 2022,
                DirectorName = "Sample Director",
                Nation = "Sample Nation"
            };

            movieRepositoryMock.Setup(repo => repo.GetByIdAsync(movieId)).ReturnsAsync(movie);

            // Simulate mapping from ReviewDto to Review
            var reviewDto = new ReviewDto
            {
                Title = title,
                Description = description,
                Rate = rate,
                MovieId = movieId
            };

            mapperMock.Setup(mapper => mapper.Map<Review>(reviewDto)).Returns(It.IsAny<Review>());

            // Act
            await reviewService.AddReviewAsync(title, description, rate, movieId);

            // Assert
            // Verify that AddAsync, and SaveChangesAsync were called on the repositories
            reviewRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Review>()), Times.Once);
            reviewRepositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task AddReviewAsync_WithInvalidMovieId_ThrowsException()
        {
            // Arrange
            var title = "Sample Review";
            var description = "This is a great movie!";
            var rate = ReviewRate.Good;
            var movieId = 1;

            var movieRepositoryMock = new Mock<IMovieRepository>();
            var reviewRepositoryMock = new Mock<IReviewRepository>();
            var mapperMock = new Mock<IMapper>();
            var reviewService = new ReviewService(reviewRepositoryMock.Object, movieRepositoryMock.Object, mapperMock.Object);

            // Simulate that no movie is found with the specified ID
            movieRepositoryMock.Setup(repo => repo.GetByIdAsync(movieId)).ReturnsAsync((Movie)null!);

            // Act and Assert
            var exception = await Assert.ThrowsAsync<MovieNotFoundException>(() =>
                reviewService.AddReviewAsync(title, description, rate, movieId));

            Assert.Equal(MovieConstants.NotFound, exception.Message);
        }
    }
}