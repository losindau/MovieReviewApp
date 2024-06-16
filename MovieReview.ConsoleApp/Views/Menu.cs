using MovieReview.ConsoleApp.Commons.Constants;
using MovieReview.Domain.Commons;

namespace MovieReview.ConsoleApp.Views
{
    public static class Menu
    {
        public static void PrintMainMenu()
        {
            Console.WriteLine(MenuConstants.MainMenu);
            Console.WriteLine(MenuConstants.PrintAllMovies, 1);
            Console.WriteLine(MenuConstants.AddMovie, 2);
            Console.WriteLine(MenuConstants.FilterMovies, 3);
            Console.WriteLine(MenuConstants.Exit, 4);
            Console.Write(MenuConstants.InputChoice);
        }

        public static void PrintMovieMenu()
        {
            Console.WriteLine(MenuConstants.UpdateMovie, 1);
            Console.WriteLine(MenuConstants.DeleteMovie, 2);
            Console.WriteLine(MenuConstants.AddReview, 3);
            Console.WriteLine(MenuConstants.GetDetail, 4);
            Console.WriteLine(MenuConstants.Exit, 5);
            Console.Write(MenuConstants.InputChoice);
        }

        public static void PrintFilterMenu()
        {
            Console.WriteLine(MenuConstants.FilterByName, 1);
            Console.WriteLine(MenuConstants.FilterByPublicationYear, 2);
            Console.WriteLine(MenuConstants.FilterByDirectorName, 3);
            Console.WriteLine(MenuConstants.FilterByNation, 4);
            Console.WriteLine(MenuConstants.Exit, 5);
            Console.Write(MenuConstants.InputChoice);
        }

        public static void PrintPaginationMenu <T> (PaginatedList<T> paginatedList)
        {
            Console.WriteLine();
            Console.WriteLine(MenuConstants.PaginationMenu);

            if (paginatedList.HasNextPage)
            {
                Console.WriteLine(MenuConstants.NextPage);
            }

            if (paginatedList.HasPreviousPage)
            {
                Console.WriteLine(MenuConstants.PreviousPage);
            }

            Console.WriteLine(MenuConstants.BackMenu);
            Console.WriteLine(MenuConstants.SubChoice);
            Console.Write(MenuConstants.InputPaginationChoice);
        }

        public static void PrintSubPaginationMenu<T>(PaginatedList<T> paginatedList)
        {
            Console.WriteLine();
            Console.WriteLine(MenuConstants.PaginationMenu);

            if (paginatedList.HasNextPage)
            {
                Console.WriteLine(MenuConstants.NextPage);
            }

            if (paginatedList.HasPreviousPage)
            {
                Console.WriteLine(MenuConstants.PreviousPage);
            }

            Console.WriteLine(MenuConstants.BackMenu);
            Console.WriteLine(MenuConstants.SubChoice);
            Console.Write(MenuConstants.InputSubPaginationChoice);
        }
    }
}