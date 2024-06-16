namespace MovieReview.Application.DTOs
{
    public class MovieDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public int PublicationYear { get; set; }
        public string DirectorName { get; set; } = "";
        public string Nation { get; set; } = "";
        public ICollection<ReviewDto> Reviews { get; set; } = new List<ReviewDto>();

        public override string ToString()
        {
            return $"{Id} {Name} {PublicationYear} {DirectorName} {Nation}";
        }
    }
}