using MovieReview.Application.DTOs;
using MovieReview.Application.Interfaces;
using MovieReview.ConsoleApp.Commons.Constants;
using MovieReview.ConsoleApp.Commons.Helpers;
using MovieReview.ConsoleApp.Views;
using MovieReview.Domain.Commons;
using MovieReview.Domain.Commons.Enums;

namespace MovieReview.ConsoleApp
{
    public class MovieReviewApp
    {
        private readonly IMovieService _movieService;
        private readonly IReviewService _reviewService;
        private readonly int pageSize = 5;

        public MovieReviewApp(IMovieService movieService, IReviewService reviewService)
        {
            _movieService = movieService;
            _reviewService = reviewService;
        }

        public async Task RunAsync()
        {
            ConsoleKeyInfo choice = new ConsoleKeyInfo();

            while (choice.Key != ConsoleKey.D4 && choice.Key != ConsoleKey.NumPad4)
            {
                Console.Clear();
                Menu.PrintMainMenu();

                choice = Console.ReadKey();

                switch (choice.Key)
                {
                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1:

                        try
                        {
                            await PrintAllMovies();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                            Console.ReadKey();
                        }

                        break;

                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:

                        try
                        {
                            Console.WriteLine();
                            await AddMovie();

                            Console.ReadKey();
                            Console.WriteLine(MenuConstants.Success);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                            Console.ReadKey();
                        }

                        break;

                    case ConsoleKey.D3:
                    case ConsoleKey.NumPad3:

                        try
                        {
                            await PrintFilterSubFunctions();
                        }
                        catch (Exception e)
                        {

                            Console.WriteLine(e.Message);
                            Console.ReadKey();
                        }

                        break;

                    default:
                        break;
                }
            }
        }

        public async Task PrintAllMovies()
        {
            int pageNumber = 1;
            ConsoleKeyInfo choice = new ConsoleKeyInfo();

            while (choice.Key != ConsoleKey.B)
            {
                Console.Clear();

                PaginatedList<MovieDto> movies = await _movieService.GetAllAsync(pageNumber, pageSize);

                foreach (MovieDto item in movies.Items)
                {
                    Console.WriteLine(item);
                }

                Menu.PrintPaginationMenu(movies);

                choice = Console.ReadKey();

                switch (choice.Key)
                {
                    case ConsoleKey.N:

                        if (movies.HasNextPage)
                        {
                            pageNumber++;
                        }

                        break;

                    case ConsoleKey.P:

                        if (movies.HasPreviousPage)
                        {
                            pageNumber--;
                        }

                        break;

                    case ConsoleKey.I:

                        await PrintMovieSubFunctions(movies);

                        break;

                    default:
                        break;
                }
            }
        }

        public async Task PrintMovieSubFunctions(PaginatedList<MovieDto> movies)
        {
            foreach (MovieDto item in movies.Items)
            {
                Console.WriteLine(item);
            }

            ConsoleKeyInfo choice = new ConsoleKeyInfo();

            while (choice.Key != ConsoleKey.D5 && choice.Key != ConsoleKey.NumPad5)
            {
                Console.Clear();

                Menu.PrintMovieMenu();

                choice = Console.ReadKey();

                switch (choice.Key)
                {
                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1:

                        try
                        {
                            Console.WriteLine();
                            await UpdateMovie();
                            Console.ReadKey();
                            Console.WriteLine(MenuConstants.Success);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                            Console.ReadKey();
                        }

                        break;

                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:

                        try
                        {
                            Console.WriteLine();
                            await DeleteMovie();
                            Console.ReadKey();
                            Console.WriteLine(MenuConstants.Success);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                            Console.ReadKey();
                        }

                        break;

                    case ConsoleKey.D3:
                    case ConsoleKey.NumPad3:

                        try
                        {
                            Console.WriteLine();
                            await AddReview();
                            Console.ReadKey();
                            Console.WriteLine(MenuConstants.Success);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                            Console.ReadKey();
                        }

                        break;

                    case ConsoleKey.D4:
                    case ConsoleKey.NumPad4:

                        try
                        {
                            Console.WriteLine();
                            await GetDetailMovie();
                            Console.ReadKey();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                            Console.ReadKey();
                        }

                        break;

                    default:
                        break;
                }
            }
        }

        public async Task AddMovie()
        {
            Console.WriteLine(MenuConstants.AddMenu);

            Console.WriteLine("Input movie infomation (name, publication year, director name, nation):");
            string[] movieInfo = Console.ReadLine()!.Split(',', StringSplitOptions.TrimEntries);
            int publicationYear;

            if (movieInfo.Length < 4)
            {
                throw new ArgumentException(ErrorConstants.AllFieldMustBeEntered);
            }

            if (string.IsNullOrWhiteSpace(movieInfo[0]) || string.IsNullOrWhiteSpace(movieInfo[2]) || string.IsNullOrWhiteSpace(movieInfo[3]))
            {
                throw new ArgumentException(ErrorConstants.FieldMustBeEntered);
            }

            if (!int.TryParse(movieInfo[1], out publicationYear))
            {
                throw new ArgumentException(ErrorConstants.FieldMustBeNumber);
            }

            await _movieService.AddMovieAsync(movieInfo[0], publicationYear, movieInfo[2], movieInfo[3]);
        }

        public async Task UpdateMovie()
        {
            Console.WriteLine(MenuConstants.UpdateMenu);

            Console.WriteLine("Input movie infomation (id, name, publication year, director name, nation):");
            string[] movieInfo = Console.ReadLine()!.Split(',', StringSplitOptions.TrimEntries);
            int id;
            int publicationYear;

            if (movieInfo.Length < 5)
            {
                throw new ArgumentException(ErrorConstants.AllFieldMustBeEntered);
            }

            if (string.IsNullOrWhiteSpace(movieInfo[1]) || string.IsNullOrWhiteSpace(movieInfo[3]) || string.IsNullOrWhiteSpace(movieInfo[4]))
            {
                throw new ArgumentException(ErrorConstants.FieldMustBeEntered);
            }

            if (!int.TryParse(movieInfo[0], out id) || !int.TryParse(movieInfo[2], out publicationYear))
            {
                throw new ArgumentException(ErrorConstants.FieldMustBeNumber);
            }

            await _movieService.UpdateMovieAsync(id, movieInfo[1], publicationYear, movieInfo[3], movieInfo[4]);
        }

        public async Task DeleteMovie()
        {
            Console.WriteLine(MenuConstants.DeleteMenu);

            Console.Write("Input movie id: ");
            string inputId = Console.ReadLine()!;
            int id;

            if (!int.TryParse(inputId, out id))
            {
                throw new ArgumentException(ErrorConstants.FieldMustBeNumber);
            }

            await _movieService.DeleteMovieAsync(id);
        }

        public async Task AddReview()
        {
            Console.WriteLine(MenuConstants.AddMenu);

            Console.WriteLine("Input review infomation (title, description, rate, movie id):");
            string[] reviewInfo = Console.ReadLine()!.Split(',', StringSplitOptions.TrimEntries);
            int rateNum;
            int movieId;

            if (reviewInfo.Length < 4)
            {
                throw new ArgumentException(ErrorConstants.AllFieldMustBeEntered);
            }

            if (string.IsNullOrWhiteSpace(reviewInfo[0]) || string.IsNullOrWhiteSpace(reviewInfo[1]) || string.IsNullOrWhiteSpace(reviewInfo[2]))
            {
                throw new ArgumentException(ErrorConstants.FieldMustBeEntered);
            }

            if (!int.TryParse(reviewInfo[2], out rateNum) || !int.TryParse(reviewInfo[3], out movieId))
            {
                throw new ArgumentException(ErrorConstants.FieldMustBeNumber);
            }

            ReviewRate rate = (ReviewRate)rateNum;

            await _reviewService.AddReviewAsync(reviewInfo[0], reviewInfo[1], rate, movieId);
        }

        public async Task GetDetailMovie()
        {
            Console.Write("Input movie id: ");
            string inputId = Console.ReadLine()!;
            int id;

            if (!int.TryParse(inputId, out id))
            {
                throw new ArgumentException(ErrorConstants.FieldMustBeNumber);
            }

            MovieDto movie = await _movieService.GetByIdAsync(id);

            Console.WriteLine(movie);

            foreach (ReviewDto item in movie.Reviews)
            {
                Console.WriteLine(item);
            }
        }

        public async Task PrintFilterSubFunctions()
        {
            ConsoleKeyInfo choice = new ConsoleKeyInfo();

            while (choice.Key != ConsoleKey.D5 && choice.Key != ConsoleKey.NumPad5)
            {
                Console.Clear();

                Menu.PrintFilterMenu();

                choice = Console.ReadKey();

                switch (choice.Key)
                {
                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1:

                        try
                        {
                            Console.WriteLine();
                            await GetMoviesByName();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                            Console.ReadKey();
                        }

                        break;

                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:

                        try
                        {
                            Console.WriteLine();
                            await GetMoviesByPublicationYear();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                            Console.ReadKey();
                        }

                        break;

                    case ConsoleKey.D3:
                    case ConsoleKey.NumPad3:

                        try
                        {
                            Console.WriteLine();
                            await GetMoviesByDirectorName();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                            Console.ReadKey();
                        }

                        break;

                    case ConsoleKey.D4:
                    case ConsoleKey.NumPad4:

                        try
                        {
                            Console.WriteLine();
                            await GetMoviesByNation();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                            Console.ReadKey();
                        }

                        break;

                    default:
                        break;
                }
            }
        }

        public async Task GetMoviesByName()
        {
            Console.Write("Input name: ");
            string name = Console.ReadLine()!;
            int pageNumber = 1;

            await PaginationHelper.NavigatePagination(_movieService.GetByNameAsync, pageNumber, pageSize, name);
        }

        public async Task GetMoviesByPublicationYear()
        {
            Console.Write("Input publication year: ");
            string inputPublicationYear = Console.ReadLine()!;
            int publicationYear;

            if (!int.TryParse(inputPublicationYear, out publicationYear))
            {
                throw new ArgumentException(ErrorConstants.FieldMustBeNumber);
            }

            int pageNumber = 1;

            await PaginationHelper.NavigatePagination(_movieService.GetByPublicationYearAsync, pageNumber, pageSize, publicationYear);
        }

        public async Task GetMoviesByDirectorName()
        {
            Console.Write("Input director name: ");
            string directorName = Console.ReadLine()!;
            int pageNumber = 1;

            await PaginationHelper.NavigatePagination(_movieService.GetByDirectorNameAsync, pageNumber, pageSize, directorName);
        }

        public async Task GetMoviesByNation()
        {
            Console.Write("Input nation: ");
            string nation = Console.ReadLine()!;
            int pageNumber = 1;

            await PaginationHelper.NavigatePagination(_movieService.GetByNationAsync, pageNumber, pageSize, nation);
        }
    }
}
