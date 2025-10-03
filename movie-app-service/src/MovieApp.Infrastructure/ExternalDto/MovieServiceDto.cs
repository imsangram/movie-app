namespace MovieApp.Infrastructure.ExternalDto
{
    public class MovieServiceDto
    {
        public List<MovieBaseServiceDto> Movies { get; set; }
    }
    public class MovieBaseServiceDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public int Year { get; set; }
        public string Poster { get; set; }
    }
}
