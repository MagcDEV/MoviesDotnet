using Microsoft.AspNetCore.Mvc;
using Movies.Api.Mapping;
using Movies.Application.Services;
using Movies.Contracts.Requests;

namespace Movies.Api.Controllers;

[ApiController]
public class MovieControllers(IMovieService movieService) : ControllerBase
{
    [HttpPost(ApiEndpoints.Movies.Create)]
    public async Task<IActionResult> Create(
        [FromBody] CreateMovieRequest request,
        CancellationToken token
    )
    {
        var movie = request.MapToMovie();
        await movieService.CreateAsync(movie, token);
        return CreatedAtAction(nameof(Get), new { idOrSlug = movie.Id }, movie.MapToResponse());
    }

    [HttpGet(ApiEndpoints.Movies.Get)]
    public async Task<IActionResult> Get([FromRoute] string idOrSlug, CancellationToken token)
    {
        var movie = Guid.TryParse(idOrSlug, out var id)
            ? await movieService.GetByIdAsync(id, token)
            : await movieService.GetBySlugAsync(idOrSlug);
        if (movie is null)
        {
            return NotFound();
        }

        return Ok(movie.MapToResponse());
    }

    [HttpGet(ApiEndpoints.Movies.GetAll)]
    public async Task<IActionResult> GetAll(CancellationToken token)
    {
        var movies = await movieService.GetAllAsync(token);
        return Ok(movies.MapToResponse());
    }

    [HttpPut(ApiEndpoints.Movies.Update)]
    public async Task<IActionResult> Update(
        [FromRoute] Guid id,
        [FromBody] UpdateMovieRequest request,
        CancellationToken token
    )
    {
        var movie = request.MapToMovie(id);
        var updatedMovie = await movieService.UpdateAsync(movie, token);

        if (updatedMovie is null)
        {
            return NotFound();
        }

        var response = updatedMovie.MapToResponse();

        return Ok(response);
    }

    [HttpDelete(ApiEndpoints.Movies.Delete)]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken token)
    {
        var isDeleted = await movieService.DeleteByIdAsync(id, token);

        if (!isDeleted)
        {
            return NotFound();
        }

        return Ok();
    }
}
