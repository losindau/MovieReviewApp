using MovieReview.Application.DTOs;
using MovieReview.ConsoleApp.Views;
using MovieReview.Domain.Commons;

namespace MovieReview.ConsoleApp.Commons.Helpers
{
    public static class PaginationHelper
    {
        public static async Task NavigatePagination<T>(Func<int, int, string, Task<PaginatedList<T>>> func , int pageNumber, int pageSize, string str)
        {
            ConsoleKeyInfo choice = new ConsoleKeyInfo();

            while (choice.Key != ConsoleKey.B)
            {
                Console.Clear();

                var paginatedList = await func(pageNumber, pageSize, str);

                foreach (var item in paginatedList.Items)
                {
                    Console.WriteLine(item);
                }

                Menu.PrintSubPaginationMenu(paginatedList);

                choice = Console.ReadKey();

                switch (choice.Key)
                {
                    case ConsoleKey.N:

                        if (paginatedList.HasNextPage)
                        {
                            pageNumber++;
                        }

                        break;

                    case ConsoleKey.P:

                        if (paginatedList.HasPreviousPage)
                        {
                            pageNumber--;
                        }

                        break;

                    default:
                        break;
                }
            }
        }

        public static async Task NavigatePagination<T>(Func<int, int, int, Task<PaginatedList<T>>> func, int pageNumber, int pageSize, int num)
        {
            ConsoleKeyInfo choice = new ConsoleKeyInfo();

            while (choice.Key != ConsoleKey.B)
            {
                Console.Clear();

                var paginatedList = await func(pageNumber, pageSize, num);

                foreach (var item in paginatedList.Items)
                {
                    Console.WriteLine(item);
                }

                Menu.PrintSubPaginationMenu(paginatedList);

                choice = Console.ReadKey();

                switch (choice.Key)
                {
                    case ConsoleKey.N:

                        if (paginatedList.HasNextPage)
                        {
                            pageNumber++;
                        }

                        break;

                    case ConsoleKey.P:

                        if (paginatedList.HasPreviousPage)
                        {
                            pageNumber--;
                        }

                        break;

                    default:
                        break;
                }
            }
        }
    }
}
