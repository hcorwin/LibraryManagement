using System.Diagnostics;
using LibraryAPI.Data.Interfaces;
using LibraryAPI.Models;
using LibraryAPI.Models.ModelExtensions;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Modules
{
    public static class BooksModule
    {
        public static void AddBooksEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/books", async (IBooksRepository db) =>
            {
                var books = await db.GetAllBooks();
                return Results.Ok(books);
            });

            app.MapGet("/books{id}", async (IBooksRepository db, int id) =>
            {
                var book = await db.GetBookById(id);
                return Results.Ok(book);
            });

            app.MapPut("/books/add", async (IBooksRepository db, Book book) =>
            {
                await db.AddBook(book);
                await db.SaveChangesAsync();
                return Results.Ok();
            });

            app.MapPut("/books/checkout{userName}", async (IBooksRepository db, Book book, string userName) =>
            {
                await db.CheckoutBook(book, userName);
                await db.SaveChangesAsync();
                return Results.Ok();
            });

            app.MapPut("/books/return{id}", async (IBooksRepository db, int id) =>
            {
                await db.ReturnBook(id);
                await db.SaveChangesAsync();
                return Results.Ok();
            });
        }
    }
}
