using System.ComponentModel.DataAnnotations;

namespace MvcMusic.Models
{
    public class Music
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Artist { get; set; }
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }
        public string? Genre { get; set; }
        public decimal Price { get; set; }
    }
}
