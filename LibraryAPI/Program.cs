using LibraryAPI.Data;
using LibraryAPI.Data.Interfaces;
using LibraryAPI.Middleware;
using LibraryAPI.Models;
using LibraryAPI.Modules;
using LibraryAPI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IBooksRepository, BooksRepository>();
builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.Decorate<IBooksRepository, CachedBookRepository>();
builder.Services.AddDbContext<ILibraryDbContext, LibraryDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("LibraryManagement")));

builder.Services.AddScoped<IPasswordEncryptionService, PasswordEncryptionService>();
builder.Services.AddTransient<GlobalExceptionHandlingMiddleware>();
builder.Services.AddMemoryCache();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.AddBooksEndpoints();
app.AddUsersEndpoints();

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

app.Run();

// makes Program accessible to tests project
public partial class Program {}
