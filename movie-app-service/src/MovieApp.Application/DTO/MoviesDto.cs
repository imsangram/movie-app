namespace MovieApp.Application.DTO
{
    public class MovieBaseDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public string Poster { get; set; }
        public int Year { get; set; }
    }

    public class MoviesDto
    {
        public IEnumerable<MovieBaseDto> Movies { get; set; }
    }
}
