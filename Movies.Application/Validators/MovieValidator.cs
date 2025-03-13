using FluentValidation;
using Movies.Application.Models;
using Movies.Application.Repositories;

namespace Movies.Application.Validators;

public class MovieValidator : AbstractValidator<Movie>
{
    private readonly IMovieRepository _movieRepository;

    public MovieValidator(IMovieRepository movieRepository)
    {
        _movieRepository = movieRepository;
        RuleFor(x => x.Title).NotEmpty().WithMessage("Title is required");
        RuleFor(x => x.Title)
            .MaximumLength(100)
            .WithMessage("Title must not exceed 100 characters");
        RuleFor(x => x.YearOfRelease).NotEmpty().WithMessage("Release date is required");
        RuleFor(x => x.YearOfRelease)
            .LessThanOrEqualTo(DateTime.UtcNow.Year)
            .WithMessage("Release date must be in the past");
        RuleFor(x => x.Genres).NotEmpty().WithMessage("Genre is required");
        RuleFor(x => x.Slug).MustAsync(ValidateSlug).WithMessage("Slug is already in use");
    }

    private async Task<bool> ValidateSlug(
        Movie movie,
        string slug,
        CancellationToken token = default
    )
    {
        var existingMovie = await _movieRepository.GetBySlugAsync(slug);
        if (existingMovie is not null)
        {
            return existingMovie.Id == movie.Id;
        }

        return existingMovie is null;
    }
}
