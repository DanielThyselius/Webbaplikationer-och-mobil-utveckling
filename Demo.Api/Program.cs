using Demo.Api;
using Demo.Api.Endpoints;
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

        app.MapBookEndpoints();
        app.MapAuthorEndpoints();

        app.Run();
    }



}