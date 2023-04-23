using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryAPI.Models;

namespace LibraryAPITests.TestDataHelper
{
    public static class UserTestDataHelper
    {
        public static List<User> GetUserList() =>
            new()
            {
                new User
                {
                    Id = 1,
                    Admin = false,
                    Username = "test",
                    Password = "unit",
                    Salt = "testing"
                },
                new User
                {
                    Id = 2,
                    Admin = false,
                    Username = "test1",
                    Password = "unit1",
                    Salt = "testing1"
                },
            };
    }
}
