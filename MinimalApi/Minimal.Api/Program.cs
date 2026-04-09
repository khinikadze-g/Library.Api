using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Minimal.Api.Data;
using Minimal.Api.Models;
using Minimal.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApiDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("MinimalApiConnectionString")));

builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("books", async (Book book, IBookService bookService, IValidator<Book> validator) =>
{
    var validationResult = await validator.ValidateAsync(book);
    if (!validationResult.IsValid)
    {
        return Results.BadRequest(validationResult.Errors);
    }
    var created = await bookService.CreateAsync(book);
    if (!created)
    {
        return Results.BadRequest(new List<ValidationFailure>{new ("Isbn", "a book with this Isbn already exists")});
    }
    return Results.Created($"/books/{book.Isbn}", book);
}).WithName("CreateBook")
    .Accepts<Book>("application/json")
    .Produces<Book>(201)
    .Produces<IEnumerable<ValidationFailure>>(400)
    .WithTags("Books");

app.MapGet("books", async (IBookService bookservice) =>
{
    var books = await bookservice.GetAllAsync();
    return Results.Ok(books);
}).Produces<Book>(200)
    .WithTags("Books");

app.MapGet("/books/{isbn}", async (string isbn, IBookService bookservice) =>
{
    var book = await bookservice.GetByIsbnAsync(isbn);
    return book is not null ? Results.Ok(book) : Results.NotFound();
}).Produces<Book>(200)
    .Produces(404)
    .WithTags("Books");

app.MapPut("/books/{isbn}", async (string isbn, Book book, IBookService bookservice) =>
{
    var updatedBook = await bookservice.UpdateAsync(isbn, book);
    return updatedBook is not null ? Results.Ok() : Results.NotFound();
}).Accepts<Book>("application/json")
    .Produces<Book>(204)
    .Produces<IEnumerable<ValidationFailure>>(400)
    .WithTags("Books");

app.MapDelete("/books/{isbn}", async (string isbn, IBookService bookservice) =>
{
    return await bookservice.DeleteAsync(isbn);
}).Produces<Book>(200)
    .Produces(404)
    .WithTags("Books");



app.Run();

