using AutoMapper;
using books_API;
using books_API.Data;
using books_API.Models;
using books_API.Models.DTO;
using books_API.Repository;
using books_API.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using System.Net;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IBookRepository, BookRepository>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAutoMapper(typeof(MappingConfig));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/api/book", async (IBookRepository _bookRepo) => {
    APIResponse response = new();
    response.Result = await _bookRepo.GetAllAsync();
    response.IsSuccess = true;
    response.StatusCode=System.Net.HttpStatusCode.OK;

    return Results.Ok(response);
}).WithName("GetBooks").Produces<APIResponse>(200);

app.MapGet("/api/book/{id:int}", async (IBookRepository _bookRepo, int id) =>
{
    APIResponse response = new();
    response.Result = await _bookRepo.GetAsync(id);
    response.IsSuccess = true;
    response.StatusCode = System.Net.HttpStatusCode.OK;

    return Results.Ok(response);
}).WithName("GetBook").Produces<APIResponse>(200);

app.MapPost("/api/book", async (IBookRepository _bookRepo, IMapper _mapper, [FromBody] BookCreateDTO book_DTO) =>
{
    APIResponse response = new() { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest };
   

    if (string.IsNullOrEmpty(book_DTO.Name))
    {
        response.ErrorMessages.Add("Invalid Id or Book Name");
        return Results.BadRequest(response);
    }

    if(_bookRepo.GetAsync(book_DTO.Name.ToLower()) != null)
    {
        response.ErrorMessages.Add("Book Name already Exists");
        return Results.BadRequest(response);
    }

    Book book = _mapper.Map<Book>(book_DTO);

    await _bookRepo.CreateAsync(book);
    await _bookRepo.SaveAsync();

    response.Result = book;
    response.IsSuccess = true;
    response.StatusCode = HttpStatusCode.Created;

    return Results.Ok(response);


}).WithName("CreateBook").Accepts<Book>("application/json").Produces<APIResponse>(201).Produces(400);

app.MapDelete("/api/book/{id:int}", async (IBookRepository _bookRepo, int id) =>
{
    APIResponse response = new() { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest };

    Book bookFromStore = await _bookRepo.GetAsync(id);

    if (bookFromStore != null)
    {
        await _bookRepo.RemoveAsync(bookFromStore);
        await _bookRepo.SaveAsync();
        response.IsSuccess = true;
        response.StatusCode = HttpStatusCode.NoContent;
        return Results.Ok(response);
    }
    else
    {
        response.ErrorMessages.Add("Invalid Id");
        return Results.BadRequest(response);
    }
});


app.UseHttpsRedirection();

app.Run();
