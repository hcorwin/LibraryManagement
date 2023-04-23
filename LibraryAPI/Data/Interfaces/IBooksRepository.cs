using LibraryAPI.Models;

namespace LibraryAPI.Data.Interfaces;

public interface IBooksRepository
{
    Task<List<Book>> GetAllBooks(CancellationToken cancellationToken = default);
    Task<bool> Exists(int id, CancellationToken cancellationToken = default);
    Task<Book> GetBookById(int id, CancellationToken cancellationToken = default);
    Task AddBook(Book book, CancellationToken cancellationToken = default);
    Task CheckoutBook(Book book, string username, CancellationToken cancellationToken = default);
    Task ReturnBook(int id, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}