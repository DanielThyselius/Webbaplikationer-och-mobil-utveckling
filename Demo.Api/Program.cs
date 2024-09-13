using Demo.Api;
using DemoModels;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerUI;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new()
            {
                Title = "Vårt fina Demo Api",
                Version = "v1",
                Description = "Ett fint Api",
                Contact = new() { Name = "Daniel", Email = "..." }
            });
        });

        // Add our services to the container.
        builder.Services.AddSingleton<IBookService, BookService>();

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("allowAll", policy => policy
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod());
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.DocExpansion(DocExpansion.List);
                options.DefaultModelsExpandDepth(-1);
            });
            //app.UseCors("allowAll");
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.MapGet("/", () => Results.File("./local-test-page.html", "text/html"));

        // Map endopoints
        app.MapGet("/books", GetAllBooks)
            .WithSummary("Hämtar alla böcker")
            .WithDescription("Hämtar alla böcker som sagt...")
            .WithTags("Böcker")
            .Produces<Book>(StatusCodes.Status200OK);
            
        app.MapGet("/books/{id}", GetBookById);
        app.MapPut("/books/{id}", UpdateBook);
        app.MapPost("/books", CreateBook);


        app.Run();
    }



    // Handlers

    static IResult CreateBook(Book book, IBookService bookService)
    {
        // TODO: Flytta detta till en egen metod
        // Problem med formulär? Lägg till [FromForm] i parametern ex: ([FromForm] Book book)
        bookService.Add(book);
        return Results.Created($"/books/{book.Id}", book);
    }

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

    private static IResult GetBookById(int id, IBookService bookService)
    {

        var book = bookService.GetBookById(id);

        if (book is null)
        {
            return Results.NotFound();
        }

        return Results.Ok(book);
    }

    private static IResult GetAllBooks(IBookService bookService)
    {
        return Results.Ok(bookService.GetBooks());
    }
}