using FluentAssertions;
using LibraryAPI.Data;
using LibraryAPI.Data.Interfaces;
using LibraryAPI.Models;
using LibraryAPITests.TestDataHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.EntityFrameworkCore;

namespace LibraryAPITests
{
    [TestClass]
    public class BooksRepositoryTests
    {
        [TestMethod]
        public async Task GetAllBooks_ReturnsList()
        {
            var dbContextMock = new Mock<ILibraryDbContext>();
            dbContextMock.Setup(x => x.Books)
                .ReturnsDbSet(BookTestDataHelper.GetBookList());

            var repository = new BooksRepository(dbContextMock.Object);
            var allBooks = await repository.GetAllBooks();

            allBooks.Count.Should().BeGreaterThan(0);
        }

        [TestMethod]
        public async Task Exists_ReturnsTrueForExistingBook()
        {
            var dbContextMock = new Mock<ILibraryDbContext>();
            dbContextMock.Setup(x => x.Books)
                .ReturnsDbSet(BookTestDataHelper.GetBookList());

            var repository = new BooksRepository(dbContextMock.Object);
            var doesBookExist = await repository.Exists(1);

            doesBookExist.Should().BeTrue();
        }

        [TestMethod]
        public async Task Exists_ReturnsFalseForNotExistingBook()
        {
            var dbContextMock = new Mock<ILibraryDbContext>();
            dbContextMock.Setup(x => x.Books)
                .ReturnsDbSet(BookTestDataHelper.GetBookList());

            var repository = new BooksRepository(dbContextMock.Object);
            var doesBookExist = await repository.Exists(4);

            doesBookExist.Should().BeFalse();
        }

        [TestMethod]
        public async Task GetBookById_ReturnsBook()
        {
            var dbContextMock = new Mock<ILibraryDbContext>();
            dbContextMock.Setup(x => x.Books)
                .ReturnsDbSet(BookTestDataHelper.GetBookList());

            var repository = new BooksRepository(dbContextMock.Object);
            var book = await repository.GetBookById(1);

            book.Should().NotBeNull();
        }

        [ExpectedException(typeof(KeyNotFoundException), "Id not found")]
        [TestMethod]
        public async Task GetBooksById_ThrowsException()
        {
            var dbContextMock = new Mock<ILibraryDbContext>();
            dbContextMock.Setup(x => x.Books)
                .ReturnsDbSet(BookTestDataHelper.GetBookList());

            var repository = new BooksRepository(dbContextMock.Object);
            await repository.GetBookById(4);
        }

        [TestMethod]
        public async Task AddBook_Adds()
        {
            var dbContextMock = new Mock<ILibraryDbContext>();
            dbContextMock.Setup(x => x.Books)
                .ReturnsDbSet(BookTestDataHelper.GetBookList());
            var newBook = new Book
            {
                Title = "add test",
                Author = "test author",
                Available = true
            };

            var repository = new BooksRepository(dbContextMock.Object);
            await repository.AddBook(newBook);

            dbContextMock.Verify(x => x.Books.AddAsync(It.IsAny<Book>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [ExpectedException(typeof(ArgumentException), "Book already exists")]
        [TestMethod]
        public async Task AddBooks_ThrowsExpception()
        {
            var dbContextMock = new Mock<ILibraryDbContext>();
            dbContextMock.Setup(x => x.Books)
                .ReturnsDbSet(BookTestDataHelper.GetBookList());
            var newBook = new Book
            {
                Id = 1,
                Title = "add test",
                Author = "test author",
                Available = true
            };

            var repository = new BooksRepository(dbContextMock.Object);
            await repository.AddBook(newBook);
        }

        [TestMethod]
        public async Task CheckoutBook_ChecksOut()
        {
            var dbContextMock = new Mock<ILibraryDbContext>();
            dbContextMock.Setup(x => x.Books)
                .ReturnsDbSet(BookTestDataHelper.GetBookList());

            var bookToCheckout = new Book
            {
                Id = 1,
                Title = "test",
                Author = "test",
                LastCheckedOutDate = DateTime.Now.AddDays(10),
                LastReturnedDate = DateTime.Now.AddMonths(-1),
                CheckedOutBy = null,
                Available = true
            };

            var repository = new BooksRepository(dbContextMock.Object);
            await repository.CheckoutBook(bookToCheckout, "test");
            await dbContextMock.Object.SaveChangesAsync();

            var book = dbContextMock.Object.Books.First(x => x.Id == 1);
            book.Available.Should().BeFalse();
        }

        [ExpectedException(typeof(ArgumentException), "Book isn't available")]
        [TestMethod]
        public async Task CheckoutBook_NotAvailable()
        {
            var dbContextMock = new Mock<ILibraryDbContext>();
            dbContextMock.Setup(x => x.Books)
                .ReturnsDbSet(BookTestDataHelper.GetBookList());

            var bookToCheckout = new Book
            {
                Id = 2,
                Title = "test",
                Author = "test",
                LastCheckedOutDate = DateTime.Now.AddDays(10),
                LastReturnedDate = DateTime.Now.AddMonths(-1),
                CheckedOutBy = null,
                Available = true
            };

            var repository = new BooksRepository(dbContextMock.Object);
            await repository.CheckoutBook(bookToCheckout, "test");
        }

        [TestMethod]
        public async Task ReturnBook_Returns()
        {
            var dbContextMock = new Mock<ILibraryDbContext>();
            dbContextMock.Setup(x => x.Books)
                .ReturnsDbSet(BookTestDataHelper.GetBookList());

            var repository = new BooksRepository(dbContextMock.Object);
            await repository.ReturnBook(2);
            await dbContextMock.Object.SaveChangesAsync();

            var book = dbContextMock.Object.Books.First(x => x.Id == 2);
            book.Available.Should().BeTrue();
        }
    }
}
