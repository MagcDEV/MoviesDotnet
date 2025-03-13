using System.Text.RegularExpressions;

namespace Movies.Application.Models;

public partial class Movie
{
    public required Guid Id { get; init; }

    public required string Title { get; set; }

    public string Slug => GenerateSlug(Title);

    public required int YearOfRelease { get; set; }

    public required List<string> Genres { get; init; } = new();

    private string GenerateSlug(string title)
    {
        var slugTitle = SlugRegex()
            .Replace(title, string.Empty)
            .ToLower()
            .Replace(" ", "-");
        return $"{slugTitle}-{YearOfRelease}";
    }

    [GeneratedRegex("[^0-9A-Za-z _-]", RegexOptions.NonBacktracking, 10)]
    private static partial Regex SlugRegex();
}
