using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sat.Recruitment.Api;
using Sat.Recruitment.Api.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sat.Recruitment.Test.UnitTest
{
    [TestClass]
    public class LoadFileServiceTests : TestBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public LoadFileServiceTests()
        {
            string nameDB = Guid.NewGuid().ToString();
            context = CreateContext(nameDB);
            mapper = ConfigureAutoMapper();
        }

        [TestMethod]
        public async Task LoadUsersFromFile_Successful()
        {
            // Arrange
            var fileService = new LoadFileService(new UserService(context), mapper);
            await fileService.LoadUsersFromFileAsync("./Files/Users.txt");

            // Act
            var allUsers = await context.Users.ToListAsync();

            // Assert
            var usersCount = allUsers.Count;
            Assert.AreEqual(2, usersCount);
        }
    }
}
