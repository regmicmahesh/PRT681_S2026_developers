using System.ComponentModel.DataAnnotations;
using week1HelloWorldMVC.Models;

namespace week1HelloWorldMVC.Tests;

public class MovieValidationTests
{
    [Fact]
    public void ValidMovie_PassesValidation()
    {
        var results = Validate(CreateValidMovie());

        Assert.Empty(results);
    }

    [Theory]
    [InlineData("")]
    [InlineData("AB")]
    public void ShortOrMissingTitle_FailsValidation(string title)
    {
        var movie = CreateValidMovie();
        movie.Title = title;

        var results = Validate(movie);

        Assert.Contains(
            results,
            result => result.MemberNames.Contains(nameof(Movie.Title))
        );
    }

    [Fact]
    public void FutureReleaseDate_FailsValidation()
    {
        var movie = CreateValidMovie();
        movie.ReleaseDate = DateTime.Today.AddDays(1);

        var results = Validate(movie);

        Assert.Contains(
            results,
            result => result.MemberNames.Contains(nameof(Movie.ReleaseDate))
        );
    }

    [Fact]
    public void UnsupportedRating_FailsValidation()
    {
        var movie = CreateValidMovie();
        movie.Rating = "Unrated";

        var results = Validate(movie);

        Assert.Contains(
            results,
            result => result.MemberNames.Contains(nameof(Movie.Rating))
        );
    }

    [Fact]
    public void PriceOutsideAllowedRange_FailsValidation()
    {
        var movie = CreateValidMovie();
        movie.Price = 0;

        var results = Validate(movie);

        Assert.Contains(
            results,
            result => result.MemberNames.Contains(nameof(Movie.Price))
        );
    }

    private static Movie CreateValidMovie()
    {
        return new Movie
        {
            Title = "Valid Movie",
            ReleaseDate = DateTime.Today,
            Genre = "Drama",
            Price = 10.00m,
            Rating = "PG",
        };
    }

    private static IReadOnlyList<ValidationResult> Validate(Movie movie)
    {
        var results = new List<ValidationResult>();
        Validator.TryValidateObject(
            movie,
            new ValidationContext(movie),
            results,
            validateAllProperties: true
        );

        return results;
    }
}
