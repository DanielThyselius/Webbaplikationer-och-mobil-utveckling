using DemoModels;
using Microsoft.AspNetCore.Mvc;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

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
            app.UseSwaggerUI();
            app.UseCors("allowAll");
        }

        app.UseHttpsRedirection();

        // This is where the magic happens
        var books = Book.GetSampleData();

        // Get all books
        app.MapGet("/books", () => Results.Ok(books));
        // Get a specific book by id
        app.MapGet("/books/{id}", (int id) =>
        {
            var book = books.FirstOrDefault(b => b.Id == id);
            if (book is null)
            {
                return Results.NotFound();
            }

            return Results.Ok(book);
        });

        // Problem med formulär? Lägg till [FromForm] i parametern ex: ([FromForm] Book book)
        app.MapPost("/books", (Book book) =>
        {
            book.Id = books.Max(b => b.Id) + 1;
            books.Add(book);
            return Results.Created($"/books/{book.Id}", book);
        });

        app.Run();
    }
}