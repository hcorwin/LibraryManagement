using FluentAssertions;
using LibraryAPI.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using Newtonsoft.Json;

namespace LibraryAPITests
{
    [TestClass]
    public class LibraryApiTests
    {
        private const string connectionString = "Server=DESKTOP-3HA7IL6;Database=LibraryManagement;Trusted_Connection=SSPI;Encrypt=false;TrustServerCertificate=true";
        private static DbContextOptionsBuilder<LibraryDbContext> optionsBuilder = new DbContextOptionsBuilder<LibraryDbContext>();
        private static LibraryDbContext _dbContext;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            optionsBuilder.UseSqlServer(connectionString);
            _dbContext = new LibraryDbContext(optionsBuilder.Options);
        }

        [ClassCleanup]
        public static async Task ClassCleanup()
        {
            var book = await _dbContext.Books.FirstOrDefaultAsync(x => x.Title == "TestTitle");
            if (book != null) _dbContext.Books.Remove(book);
            await _dbContext.SaveChangesAsync();
            await _dbContext.DisposeAsync();
        }

        [TestMethod]
        public async Task GetBooks_ReturnsOK()
        {
            await using var app = new WebApplicationFactory<Program>();
            using var client = app.CreateClient();

            var result = await client.GetFromJsonAsync<List<Book>>("/books");

            result!.Count.Should().BeGreaterThan(0);
        }

        [TestMethod]
        public async Task GetBooksById_ReturnsOK()
        {
            await using var app = new WebApplicationFactory<Program>();
            using var client = app.CreateClient();
            const int id = 1;

            var result = await client.GetFromJsonAsync<Book>($"/books{id}");

            result!.Id.Should().Be(1);
        }

        [TestMethod]
        public async Task AddBook_ReturnsOk()
        {
            await using var app = new WebApplicationFactory<Program>();
            using var client = app.CreateClient();

            var result = await client.PutAsJsonAsync("/books/add", new Book
            {
                Title = "TestTitle",
                Author = "HoldenC",
                Available = true
            });

            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task CheckoutBook_ReturnsOK()
        {
            await using var app = new WebApplicationFactory<Program>();
            using var client = app.CreateClient();
            const string userName = "testuser";

            var result = await client.PutAsJsonAsync($"/books/checkout{userName}", new Book
            {
                Id = 1
            });


            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task ReturnBook_ReturnsOK()
        {
            await using var app = new WebApplicationFactory<Program>();
            using var client = app.CreateClient();
            const int id = 1;
            var content = new StringContent(id.ToString(), Encoding.UTF8, "application/json");

            var result = await client.PutAsync($"/books/return{id}", content);

            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
