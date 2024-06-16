using MapsterMapper;
using Moq;
using MovieReview.Application.Commons.Constants;
using MovieReview.Application.Commons.Exceptions;
using MovieReview.Application.DTOs;
using MovieReview.Application.Services;
using MovieReview.Domain.Commons;
using MovieReview.Domain.Entities;
using MovieReview.Domain.Interfaces;

namespace MovieReview.Application.Test
{
    public class MovieServiceTest
    {
        [Fact]
        public async Task AddMovieAsync_WithValidData_AddsMovie()
        {
            // Arrange
            string name = "Sample Movie";
            int publicationYear = 2022;
            string directorName = "Sample Director";
            string nation = "Sample Nation";

            var movieRepositoryMock = new Mock<IMovieRepository>();
            var mapperMock = new Mock<IMapper>();
            var movieService = new MovieService(movieRepositoryMock.Object, mapperMock.Object);

            // Simulate mapping from DTO to Movie
            var movieDto = new MovieDto()
            {
                Name = name,
                PublicationYear = publicationYear,
                DirectorName = directorName,
                Nation = nation
            };

            mapperMock.Setup(mapper => mapper.Map<Movie>(movieDto)).Returns(It.IsAny<Movie>());

            // Act
            await movieService.AddMovieAsync(name, publicationYear, directorName, nation);

            // Assert
            movieRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Movie>()), Times.Once);
            movieRepositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task AddMovieAsync_WithInvalidYear_ThrowsException()
        {
            // Arrange
            string name = "Sample Movie";
            //Invalid year
            int publicationYear = DateTime.Now.Year + 1;
            string directorName = "Sample Director";
            string nation = "Sample Nation";

            var movieRepositoryMock = new Mock<IMovieRepository>();
            var mapperMock = new Mock<IMapper>();
            var movieService = new MovieService(movieRepositoryMock.Object, mapperMock.Object);

            // Act and Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
                movieService.AddMovieAsync(name, publicationYear, directorName, nation));

            Assert.Equal(MovieConstants.InvalidYear, exception.Message);
        }

        [Fact]
        public async Task DeleteMovieAsync_WithValidId_DeletesMovie()
        {
            // Arrange
            int movieId = 1;
            var movieRepositoryMock = new Mock<IMovieRepository>();
            var movieService = new MovieService(movieRepositoryMock.Object, null!);

            // Simulate retrieving a movie by ID
            var movie = new Movie { Id = movieId };
            movieRepositoryMock.Setup(repo => repo.GetByIdAsync(movieId)).ReturnsAsync(movie);

            // Act
            await movieService.DeleteMovieAsync(movieId);

            // Assert
            // Verify that Delete and SaveChangesAsync were called on the repository
            movieRepositoryMock.Verify(repo => repo.Delete(movie), Times.Once);
            movieRepositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteMovieAsync_WithInvalidId_ThrowsException()
        {
            // Arrange
            int movieId = 1;
            var movieRepositoryMock = new Mock<IMovieRepository>();
            var movieService = new MovieService(movieRepositoryMock.Object, null!);

            // Simulate that no movie is found with the specified ID
            movieRepositoryMock.Setup(repo => repo.GetByIdAsync(movieId)).ReturnsAsync((Movie)null!);

            // Act and Assert
            var exception = await Assert.ThrowsAsync<MovieNotFoundException>(() =>
                movieService.DeleteMovieAsync(movieId));

            Assert.Equal(MovieConstants.NotFound, exception.Message);
        }

        [Fact]
        public async Task UpdateMovieAsync_WithValidData_UpdatesMovie()
        {
            // Arrange
            int movieId = 1;
            var movieRepositoryMock = new Mock<IMovieRepository>();
            var movieService = new MovieService(movieRepositoryMock.Object, null!);

            // Simulate retrieving a movie by ID
            var movie = new Movie { Id = movieId };
            movieRepositoryMock.Setup(repo => repo.GetByIdAsync(movieId)).ReturnsAsync(movie);

            // Act
            await movieService.UpdateMovieAsync(movieId, "Updated Movie", 2023, "Updated Director", "Updated Nation");

            // Assert
            // Verify that SaveChangesAsync was called on the repository
            movieRepositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);

            // Verify that movie properties were updated as expected
            Assert.Equal("Updated Movie", movie.Name);
            Assert.Equal(2023, movie.PublicationYear);
            Assert.Equal("Updated Director", movie.DirectorName);
            Assert.Equal("Updated Nation", movie.Nation);
        }

        [Fact]
        public async Task UpdateMovieAsync_WithInvalidId_ThrowsException()
        {
            // Arrange
            int movieId = 1;
            var movieRepositoryMock = new Mock<IMovieRepository>();
            var movieService = new MovieService(movieRepositoryMock.Object, null!);

            // Simulate that no movie is found with the specified ID
            movieRepositoryMock.Setup(repo => repo.GetByIdAsync(movieId)).ReturnsAsync((Movie)null!);

            // Act and Assert
            var exception = await Assert.ThrowsAsync<MovieNotFoundException>(() =>
                movieService.UpdateMovieAsync(movieId, "Updated Movie", 2023, "Updated Director", "Updated Nation"));

            Assert.Equal(MovieConstants.NotFound, exception.Message);
        }

        [Fact]
        public async Task UpdateMovieAsync_WithInvalidYear_ThrowsException()
        {
            // Arrange
            int movieId = 1;
            // Invalid year
            int publicationYear = DateTime.Now.Year + 1;
            var movieRepositoryMock = new Mock<IMovieRepository>();
            var movieService = new MovieService(movieRepositoryMock.Object, null!);

            // Simulate retrieving a movie by ID
            var movie = new Movie { Id = movieId };
            movieRepositoryMock.Setup(repo => repo.GetByIdAsync(movieId)).ReturnsAsync(movie);

            // Act and Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
                movieService.UpdateMovieAsync(movieId, "Updated Movie", publicationYear, "Updated Director", "Updated Nation"));

            Assert.Equal(MovieConstants.InvalidYear, exception.Message);
        }

        [Fact]
        public async Task GetByIdAsync_WithValidId_ReturnsMovieDto()
        {
            // Arrange
            int movieId = 1;
            var movieRepositoryMock = new Mock<IMovieRepository>();
            var mapperMock = new Mock<IMapper>();
            var movieService = new MovieService(movieRepositoryMock.Object, mapperMock.Object);

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

            // Simulate mapping from Movie to MovieDto
            var movieDto = new MovieDto
            {
                Name = movie.Name,
                PublicationYear = movie.PublicationYear,
                DirectorName = movie.DirectorName,
                Nation = movie.Nation
            };

            mapperMock.Setup(mapper => mapper.Map<MovieDto>(movie)).Returns(movieDto);

            // Act
            var result = await movieService.GetByIdAsync(movieId);

            // Assert
            Assert.Equal(movieDto, result);
        }

        [Fact]
        public async Task GetByIdAsync_WithInvalidId_ThrowsException()
        {
            // Arrange
            int movieId = 1;
            var movieRepositoryMock = new Mock<IMovieRepository>();
            var mapperMock = new Mock<IMapper>();
            var movieService = new MovieService(movieRepositoryMock.Object, mapperMock.Object);

            // Simulate that no movie is found with the specified ID
            movieRepositoryMock.Setup(repo => repo.GetByIdAsync(movieId)).ReturnsAsync((Movie)null!);

            // Act and Assert
            var exception = await Assert.ThrowsAsync<MovieNotFoundException>(() =>
                movieService.GetByIdAsync(movieId));

            Assert.Equal(MovieConstants.NotFound, exception.Message);
        }

        [Fact]
        public async Task GetAllAsync_WithValidData_ReturnsPaginatedList()
        {
            // Arrange
            int pageNumber = 1;
            int pageSize = 2;

            var movieRepositoryMock = new Mock<IMovieRepository>();
            var mapperMock = new Mock<IMapper>();
            var movieService = new MovieService(movieRepositoryMock.Object, mapperMock.Object);

            var movies = new List<Movie>()
            {
                new Movie { Id = 1, Name = "Movie 1", PublicationYear = 2022, DirectorName = "director", Nation = "nation" },
                new Movie { Id = 2, Name = "Movie 2", PublicationYear = 2023, DirectorName = "director", Nation = "nation" }
            };

            // Simulate retrieving a paginated list of movies
            var paginatedList = new PaginatedList<Movie>(movies, movies.Count, pageNumber, pageSize);

            movieRepositoryMock.Setup(repo => repo.GetAllAsync(pageNumber, pageSize)).ReturnsAsync(paginatedList);

            // Simulate mapping from Movie to MovieDto
            var movieDtos = paginatedList.Items.Select(movie => new MovieDto
            {
                Name = movie.Name,
                PublicationYear = movie.PublicationYear
            }).ToList();

            var paginatedListDto = new PaginatedList<MovieDto>(movieDtos, movieDtos.Count, pageNumber, pageSize);

            mapperMock.Setup(mapper => mapper.Map<PaginatedList<MovieDto>>(paginatedList)).Returns(paginatedListDto);

            // Act
            var result = await movieService.GetAllAsync(pageNumber, pageSize);

            // Assert
            Assert.Equal(paginatedListDto, result);
        }

        [Fact]
        public async Task GetAllAsync_WithEmptyList_ThrowsException()
        {
            // Arrange
            int pageNumber = 1;
            int pageSize = 5;

            var movieRepositoryMock = new Mock<IMovieRepository>();
            var mapperMock = new Mock<IMapper>();
            var movieService = new MovieService(movieRepositoryMock.Object, mapperMock.Object);

            // Simulate an empty list of movies
            var movies = new PaginatedList<Movie>
            {
                Items = new List<Movie>(),
                TotalPages = 0,
            };

            movieRepositoryMock.Setup(repo => repo.GetAllAsync(pageNumber, pageSize)).ReturnsAsync(movies);

            // Act and Assert
            var exception = await Assert.ThrowsAsync<MoviesListEmpty>(() =>
                movieService.GetAllAsync(pageNumber, pageSize));

            Assert.Equal(MovieConstants.Empty, exception.Message);
        }

        [Fact]
        public async Task GetByDirectorNameAsync_WithValidData_ReturnsPaginatedList()
        {
            // Arrange
            int pageNumber = 1;
            int pageSize = 2;
            string directorName = "director";

            var movieRepositoryMock = new Mock<IMovieRepository>();
            var mapperMock = new Mock<IMapper>();
            var movieService = new MovieService(movieRepositoryMock.Object, mapperMock.Object);

            var movies = new List<Movie>()
            {
                new Movie { Id = 1, Name = "Movie 1", PublicationYear = 2022, DirectorName = "director", Nation = "nation" },
                new Movie { Id = 2, Name = "Movie 2", PublicationYear = 2023, DirectorName = "director", Nation = "nation" }
            };

            // Simulate retrieving a paginated list of movies
            var paginatedList = new PaginatedList<Movie>(movies, movies.Count, pageNumber, pageSize);

            movieRepositoryMock.Setup(repo => repo.FilterByDirectorNameAsync(pageNumber, pageSize, directorName)).ReturnsAsync(paginatedList);

            // Simulate mapping from Movie to MovieDto
            var movieDtos = paginatedList.Items.Select(movie => new MovieDto
            {
                Name = movie.Name,
                PublicationYear = movie.PublicationYear
            }).ToList();

            var paginatedListDto = new PaginatedList<MovieDto>(movieDtos, movieDtos.Count, pageNumber, pageSize);

            mapperMock.Setup(mapper => mapper.Map<PaginatedList<MovieDto>>(paginatedList)).Returns(paginatedListDto);

            // Act
            var result = await movieService.GetByDirectorNameAsync(pageNumber, pageSize, directorName);

            // Assert
            Assert.Equal(paginatedListDto, result);
        }

        [Fact]
        public async Task GetByDirectorNameAsync_WithEmptyList_ThrowsException()
        {
            // Arrange
            int pageNumber = 1;
            int pageSize = 5;

            var movieRepositoryMock = new Mock<IMovieRepository>();
            var mapperMock = new Mock<IMapper>();
            var movieService = new MovieService(movieRepositoryMock.Object, mapperMock.Object);

            // Simulate an empty list of movies
            var movies = new PaginatedList<Movie>
            {
                Items = new List<Movie>(),
                TotalPages = 0,
            };

            movieRepositoryMock.Setup(repo => repo.FilterByDirectorNameAsync(pageNumber, pageSize, It.IsAny<string>())).ReturnsAsync(movies);

            // Act and Assert
            var exception = await Assert.ThrowsAsync<MoviesListEmpty>(() =>
                movieService.GetByDirectorNameAsync(pageNumber, pageSize, It.IsAny<string>()));

            Assert.Equal(MovieConstants.Empty, exception.Message);
        }

        [Fact]
        public async Task GetByNameAsync_WithValidData_ReturnsPaginatedList()
        {
            // Arrange
            int pageNumber = 1;
            int pageSize = 2;
            string name = "movie";

            var movieRepositoryMock = new Mock<IMovieRepository>();
            var mapperMock = new Mock<IMapper>();
            var movieService = new MovieService(movieRepositoryMock.Object, mapperMock.Object);

            var movies = new List<Movie>()
            {
                new Movie { Id = 1, Name = "Movie 1", PublicationYear = 2022, DirectorName = "director", Nation = "nation" },
                new Movie { Id = 2, Name = "Movie 2", PublicationYear = 2023, DirectorName = "director", Nation = "nation" }
            };

            // Simulate retrieving a paginated list of movies
            var paginatedList = new PaginatedList<Movie>(movies, movies.Count, pageNumber, pageSize);

            movieRepositoryMock.Setup(repo => repo.FilterByNameAsync(pageNumber, pageSize, name)).ReturnsAsync(paginatedList);

            // Simulate mapping from Movie to MovieDto
            var movieDtos = paginatedList.Items.Select(movie => new MovieDto
            {
                Name = movie.Name,
                PublicationYear = movie.PublicationYear
            }).ToList();

            var paginatedListDto = new PaginatedList<MovieDto>(movieDtos, movieDtos.Count, pageNumber, pageSize);

            mapperMock.Setup(mapper => mapper.Map<PaginatedList<MovieDto>>(paginatedList)).Returns(paginatedListDto);

            // Act
            var result = await movieService.GetByNameAsync(pageNumber, pageSize, name);

            // Assert
            Assert.Equal(paginatedListDto, result);
        }

        [Fact]
        public async Task GetByNameAsync_WithEmptyList_ThrowsException()
        {
            // Arrange
            int pageNumber = 1;
            int pageSize = 5;

            var movieRepositoryMock = new Mock<IMovieRepository>();
            var mapperMock = new Mock<IMapper>();
            var movieService = new MovieService(movieRepositoryMock.Object, mapperMock.Object);

            // Simulate an empty list of movies
            var movies = new PaginatedList<Movie>
            {
                Items = new List<Movie>(),
                TotalPages = 0,
            };

            movieRepositoryMock.Setup(repo => repo.FilterByNameAsync(pageNumber, pageSize, It.IsAny<string>())).ReturnsAsync(movies);

            // Act and Assert
            var exception = await Assert.ThrowsAsync<MoviesListEmpty>(() =>
                movieService.GetByNameAsync(pageNumber, pageSize, It.IsAny<string>()));

            Assert.Equal(MovieConstants.Empty, exception.Message);
        }

        [Fact]
        public async Task GetByNationAsync_WithValidData_ReturnsPaginatedList()
        {
            // Arrange
            int pageNumber = 1;
            int pageSize = 2;
            string nation = "nation";

            var movieRepositoryMock = new Mock<IMovieRepository>();
            var mapperMock = new Mock<IMapper>();
            var movieService = new MovieService(movieRepositoryMock.Object, mapperMock.Object);

            var movies = new List<Movie>()
            {
                new Movie { Id = 1, Name = "Movie 1", PublicationYear = 2022, DirectorName = "director", Nation = "nation" },
                new Movie { Id = 2, Name = "Movie 2", PublicationYear = 2023, DirectorName = "director", Nation = "nation" }
            };

            // Simulate retrieving a paginated list of movies
            var paginatedList = new PaginatedList<Movie>(movies, movies.Count, pageNumber, pageSize);

            movieRepositoryMock.Setup(repo => repo.FilterByNationAsync(pageNumber, pageSize, nation)).ReturnsAsync(paginatedList);

            // Simulate mapping from Movie to MovieDto
            var movieDtos = paginatedList.Items.Select(movie => new MovieDto
            {
                Name = movie.Name,
                PublicationYear = movie.PublicationYear
            }).ToList();

            var paginatedListDto = new PaginatedList<MovieDto>(movieDtos, movieDtos.Count, pageNumber, pageSize);

            mapperMock.Setup(mapper => mapper.Map<PaginatedList<MovieDto>>(paginatedList)).Returns(paginatedListDto);

            // Act
            var result = await movieService.GetByNationAsync(pageNumber, pageSize, nation);

            // Assert
            Assert.Equal(paginatedListDto, result);
        }

        [Fact]
        public async Task GetByNationAsync_WithEmptyList_ThrowsException()
        {
            // Arrange
            int pageNumber = 1;
            int pageSize = 5;

            var movieRepositoryMock = new Mock<IMovieRepository>();
            var mapperMock = new Mock<IMapper>();
            var movieService = new MovieService(movieRepositoryMock.Object, mapperMock.Object);

            // Simulate an empty list of movies
            var movies = new PaginatedList<Movie>
            {
                Items = new List<Movie>(),
                TotalPages = 0,
            };

            movieRepositoryMock.Setup(repo => repo.FilterByNationAsync(pageNumber, pageSize, It.IsAny<string>())).ReturnsAsync(movies);

            // Act and Assert
            var exception = await Assert.ThrowsAsync<MoviesListEmpty>(() =>
                movieService.GetByNationAsync(pageNumber, pageSize, It.IsAny<string>()));

            Assert.Equal(MovieConstants.Empty, exception.Message);
        }

        [Fact]
        public async Task GetByPublicationYearAsync_WithValidData_ReturnsPaginatedList()
        {
            // Arrange
            int pageNumber = 1;
            int pageSize = 2;
            int publicationYear = 2022;

            var movieRepositoryMock = new Mock<IMovieRepository>();
            var mapperMock = new Mock<IMapper>();
            var movieService = new MovieService(movieRepositoryMock.Object, mapperMock.Object);

            var movies = new List<Movie>()
            {
                new Movie { Id = 1, Name = "Movie 1", PublicationYear = 2022, DirectorName = "director", Nation = "nation" },
                new Movie { Id = 2, Name = "Movie 2", PublicationYear = 2022, DirectorName = "director", Nation = "nation" }
            };

            // Simulate retrieving a paginated list of movies
            var paginatedList = new PaginatedList<Movie>(movies, movies.Count, pageNumber, pageSize);

            movieRepositoryMock.Setup(repo => repo.FilterByPublicationYearAsync(pageNumber, pageSize, publicationYear)).ReturnsAsync(paginatedList);

            // Simulate mapping from Movie to MovieDto
            var movieDtos = paginatedList.Items.Select(movie => new MovieDto
            {
                Name = movie.Name,
                PublicationYear = movie.PublicationYear
            }).ToList();

            var paginatedListDto = new PaginatedList<MovieDto>(movieDtos, movieDtos.Count, pageNumber, pageSize);

            mapperMock.Setup(mapper => mapper.Map<PaginatedList<MovieDto>>(paginatedList)).Returns(paginatedListDto);

            // Act
            var result = await movieService.GetByPublicationYearAsync(pageNumber, pageSize, publicationYear);

            // Assert
            Assert.Equal(paginatedListDto, result);
        }

        [Fact]
        public async Task GetByPublicationYearAsync_WithEmptyList_ThrowsException()
        {
            // Arrange
            int pageNumber = 1;
            int pageSize = 5;

            var movieRepositoryMock = new Mock<IMovieRepository>();
            var mapperMock = new Mock<IMapper>();
            var movieService = new MovieService(movieRepositoryMock.Object, mapperMock.Object);

            // Simulate an empty list of movies
            var movies = new PaginatedList<Movie>
            {
                Items = new List<Movie>(),
                TotalPages = 0,
            };

            movieRepositoryMock.Setup(repo => repo.FilterByPublicationYearAsync(pageNumber, pageSize, It.IsAny<int>())).ReturnsAsync(movies);

            // Act and Assert
            var exception = await Assert.ThrowsAsync<MoviesListEmpty>(() =>
                movieService.GetByPublicationYearAsync(pageNumber, pageSize, It.IsAny<int>()));

            Assert.Equal(MovieConstants.Empty, exception.Message);
        }
    }
}