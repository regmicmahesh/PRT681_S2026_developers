using System.ComponentModel.DataAnnotations;

namespace TheaterAdmin.Models
{
    public class Movie
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Director { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [Display(Name = "Contact Email")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Release Date")]
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }

        [Required]
        public MovieLanguage Language { get; set; }

        [Required]
        [Display(Name = "Category")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a category.")]
        public int CategoryId { get; set; }
        public Category? Category { get; set; } = null!;
    }
}