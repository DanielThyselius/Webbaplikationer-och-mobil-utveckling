
using DemoModels;

namespace Demo.Api.Endpoints;

public static class AuthorEndpoints
{
    public static void MapAuthorEndpoints(this IEndpointRouteBuilder app)
    {
        // Map endopoints
        app.MapGet("/authors", GetAllAuthors)
            .WithOpenApi()
            .WithSummary("Hämtar alla författare")
            .WithDescription("Hämtar alla författare som sagt...")
            .WithTags("Författare")
            .Produces<List<Author>>(StatusCodes.Status200OK);
    }

    private static IResult GetAllAuthors()
    {
        var authors = new List<Author>
        {
            new Author { Name = "F. Scott Fitzgerald", Email = ""}
        };
        return Results.Ok(authors);
    }
}
