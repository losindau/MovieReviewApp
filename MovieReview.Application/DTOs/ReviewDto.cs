using MovieReview.Domain.Commons.Enums;

namespace MovieReview.Application.DTOs
{
    public class ReviewDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public ReviewRate Rate { get; set; }
        public int MovieId { get; set; }

        public override string ToString()
        {
            return $"Id: {Id} Title: {Title} Rate: {Rate} \nDescription: {Description}";
        }
    }
}
