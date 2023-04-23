using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    public class UsersRepositoryTests
    {
        [TestMethod]
        public async Task Exists_ReturnsTrue()
        {
            var dbContextMock = new Mock<ILibraryDbContext>();
            dbContextMock.Setup(x => x.Users)
                .ReturnsDbSet(UserTestDataHelper.GetUserList());

            var repository = new UsersRepository(dbContextMock.Object);
            var exists = await repository.Exists("test");

            exists.Should().BeTrue();
        }

        [TestMethod]
        public async Task Exists_ReturnsFalse()
        {
            var dbContextMock = new Mock<ILibraryDbContext>();
            dbContextMock.Setup(x => x.Users)
                .ReturnsDbSet(UserTestDataHelper.GetUserList());

            var repository = new UsersRepository(dbContextMock.Object);
            var exists = await repository.Exists("testingthis");

            exists.Should().BeFalse();
        }

        [TestMethod]
        public async Task AddUser_Adds()
        {
            var dbContextMock = new Mock<ILibraryDbContext>();
            dbContextMock.Setup(x => x.Users)
                .ReturnsDbSet(UserTestDataHelper.GetUserList());

            var newUser = new User()
            {
                Id = 15,
                Admin = false,
                Password = "hi",
                Username = "hello",
                Salt = "goodbye"
            };

            var repository = new UsersRepository(dbContextMock.Object);
            await repository.AddUser(newUser);

            dbContextMock.Verify(x => x.Users.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [ExpectedException(typeof(ArgumentException), "User Already Exists")]
        [TestMethod]
        public async Task AddUser_ExceptionThrown()
        {
            var dbContextMock = new Mock<ILibraryDbContext>();
            dbContextMock.Setup(x => x.Users)
                .ReturnsDbSet(UserTestDataHelper.GetUserList());

            var newUser = new User()
            {
                Id = 1,
                Admin = false,
                Password = "hi",
                Username = "test",
                Salt = "goodbye"
            };

            var repository = new UsersRepository(dbContextMock.Object);
            await repository.AddUser(newUser);
        }

        [TestMethod]
        public async Task GetUserByUsername_ReturnsUser()
        {
            var dbContextMock = new Mock<ILibraryDbContext>();
            dbContextMock.Setup(x => x.Users)
                .ReturnsDbSet(UserTestDataHelper.GetUserList());

            var repository = new UsersRepository(dbContextMock.Object);
            var user = await repository.GetUserByUsername("test");

            user.Should().NotBeNull();
        }

        [ExpectedException(typeof(KeyNotFoundException), "User Not Found")]
        [TestMethod]
        public async Task GetUserByUsername_ThrowsException()
        {
            var dbContextMock = new Mock<ILibraryDbContext>();
            dbContextMock.Setup(x => x.Users)
                .ReturnsDbSet(UserTestDataHelper.GetUserList());

            var repository = new UsersRepository(dbContextMock.Object);
            var user = await repository.GetUserByUsername("throw exception");
        }
    }
}
