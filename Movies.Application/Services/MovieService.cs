using FluentValidation;
using Movies.Application.Models;
using Movies.Application.Repositories;

namespace Movies.Application.Services;

public class MovieService(IMovieRepository movieRepository, IValidator<Movie> validator)
    : IMovieService
{
    public async Task<bool> CreateAsync(Movie movie, CancellationToken token = default)
    {
        await validator.ValidateAndThrowAsync(movie, token);
        return await movieRepository.CreateAsync(movie);
    }

    public Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default)
    {
        return movieRepository.DeleteAsync(id, token);
    }

    public Task<bool> ExistByIdAsync(Guid id, CancellationToken token = default)
    {
        return movieRepository.ExistsByIdAsync(id, token);
    }

    public Task<IEnumerable<Movie>> GetAllAsync(CancellationToken token = default)
    {
        return movieRepository.GetAllAsync(token);
    }

    public Task<Movie?> GetByIdAsync(Guid id, CancellationToken token = default)
    {
        return movieRepository.GetByIdAsync(id, token);
    }

    public Task<Movie?> GetBySlugAsync(string slug, CancellationToken token = default)
    {
        return movieRepository.GetBySlugAsync(slug, token);
    }

    public async Task<Movie?> UpdateAsync(Movie movie, CancellationToken token = default)
    {
        await validator.ValidateAndThrowAsync(movie, token);
        var movieExists = await movieRepository.ExistsByIdAsync(movie.Id, token);

        if (!movieExists)
        {
            return null;
        }

        await movieRepository.UpdateAsync(movie);
        return movie;
    }
}
