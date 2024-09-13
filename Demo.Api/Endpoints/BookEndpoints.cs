using DemoModels;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Demo.Api.Endpoints;

public static class BookEndpoints
{
    private const string _tag = "Böcker";
    private const string _route = "books";
    private const string _contentType = "application/json";
    public static void MapBookEndpoints(this IEndpointRouteBuilder app)
    {
        // Map endopoints
        app.MapGet(_route, GetAllBooks)
            .WithOpenApi()
            .WithSummary("Hämtar alla böcker")
            .WithDescription("Hämtar alla böcker som sagt...")
            .WithTags(_tag)
            .Produces<BookResponse>(StatusCodes.Status200OK);

        app.MapPost(_route, CreateBook)
            .WithOpenApi()
            .WithSummary("Skapar en ny bok")
            .WithDescription("Skapar en ny bok som sagt...")
            .WithTags(_tag);

        app.MapGet($"{_route}/{{id}}", GetBookById)
            .WithOpenApi()
            .WithSummary("Hämtar en bok")
            .WithDescription("Hämtar en bok som sagt...")
            .WithTags(_tag);
        // Behövs inte när vi använder TypedResults
        //.Produces<DetailedBookResponse>(StatusCodes.Status200OK)
        //.Produces(StatusCodes.Status404NotFound);

        app.MapPut($"{_route}/{{id}}", UpdateBook);
    }

    // Request/Response models
    public record BookResponse(string Title, string Review);
    public record DetailedBookResponse(string Title, string Author, string Review, string Description);
    public record CreateBookRequest(string Title, string Author, string Description);


    // Handlers
    static Created<DetailedBookResponse> CreateBook(CreateBookRequest request, IBookService bookService)
    {
        var book = new Book() {
            Title = request.Title,
            Author = request.Author,
            Description = request.Description,
            Review = "No review yet"
        };

        var response = new DetailedBookResponse(book.Title, book.Author, book.Review, book.Description);

        bookService.Add(book);
        return TypedResults.Created($"/books/{book.Id}", response);
    }

    // Todo: Implement TypedResult i UpdateBook
    static IResult UpdateBook(int id, Book book, IBookService bookService)
    {
        var existingBook = bookService.GetBookById(id);

        if (existingBook is null)
        {
            return Results.NotFound();
        }

        bookService.Update(book);
        return Results.Ok(book);
    }

    // Vi kan explicit definera vår(a) response(s) med hjälp av TypedResults
    private static Results<Ok<DetailedBookResponse>, NotFound> GetBookById(int id, IBookService bookService)
    {
        var book = bookService.GetBookById(id);
        if (book is null)
            return TypedResults.NotFound();

        // Map book to response (could be done with AutoMapper)
        var response = new DetailedBookResponse(book.Title, book.Author, book.Review, book.Description);

        return TypedResults.Ok(response);
    }

    private static IResult GetAllBooks(IBookService bookService)
    {
        return Results.Ok(bookService.GetBooks().Select(x => new BookResponse( x.Title, x.Review)
        ));
    }
}
