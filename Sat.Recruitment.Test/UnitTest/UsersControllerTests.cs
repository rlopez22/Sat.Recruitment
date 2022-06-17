using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sat.Recruitment.Api;
using Sat.Recruitment.Api.Controllers;
using Sat.Recruitment.Api.DTOs;
using Sat.Recruitment.Api.Entities;
using Sat.Recruitment.Api.Services;
using System;
using System.Threading.Tasks;

namespace Sat.Recruitment.Test.UnitTest
{
    [TestClass]
    public class UsersControllerTests : TestBase
    {
        private readonly IUserService userService;
        private readonly IMapper mapper;
        public UsersControllerTests()
        {
            string nameDB = Guid.NewGuid().ToString();
            ApplicationDbContext context = CreateContext(nameDB);
            mapper = ConfigureAutoMapper();
            userService = new UserService(context);
        }

        [TestMethod]
        public async Task GetAllUsers_IsCorrect()
        {
            // Arrange
            User user1 = new User()
            {
                Name = "Juan",
                Email = "juan@gmail.com",
                Address = "Av. Velazquez",
                Phone = "+349 1122354215",
                Type = UserTypes.Normal,
                Money = 124
            };

            User user2 = new User()
            {
                Name = "Rafa",
                Email = "rafa@gmail.com",
                Address = "Av. Sol",
                Phone = "+349 1122354216",
                Type = UserTypes.SuperUser,
                Money = 124
            };

            await userService.AddUser(user1);
            await userService.AddUser(user2);

            var controller = new UsersController(userService, mapper);

            // Act
            var allUsers = await controller.Get();

            // Assert
            var usersCount = allUsers.Count;
            Assert.AreEqual(2, usersCount);
        }

        [TestMethod]
        public async Task CreateUser_Ok()
        {
            // Arrange
            var user = new UserCreationDTO()
            {
                Name = "Rafa",
                Email = "rafa@gmail.com",
                Address = "Av. Sol",
                Phone = "+349 1122354216",
                Type = UserTypes.SuperUser,
                Money = 124
            };

            var controller = new UsersController(userService, mapper);

            // Act
            var response = await controller.Post(user);

            // Assert
            var result = response as CreatedAtRouteResult;
            Assert.AreEqual(201, result.StatusCode);
        }

        [TestMethod]
        public async Task CreateUser_BadRequest()
        {
            // Arrange
            var user = new UserCreationDTO()
            {
                Name = "Rafa",
            };

            var controller = new UsersController(userService, mapper);

            // Act
            var response = await controller.Post(user);

            // Assert
            var result = response as BadRequestObjectResult;
            Assert.AreEqual(400, result.StatusCode);
        }

        [TestMethod]
        public async Task CreateUser_IsDuplicated()
        {
            // Arrange
            User user1 = new User()
            {
                Name = "Juan",
                Email = "juan@gmail.com",
                Address = "Av. Velazquez",
                Phone = "+349 1122354215",
                Type = UserTypes.Normal,
                Money = 124
            };

            UserCreationDTO user2 = new UserCreationDTO()
            {
                Name = "Rafa",
                Email = "juan@gmail.com",
                Address = "Av. Sol",
                Phone = "+349 1122354216",
                Type = UserTypes.SuperUser,
                Money = 124
            };

            await userService.AddUser(user1);

            var controller = new UsersController(userService, mapper);

            // Act
            var response = await controller.Post(user2);

            // Assert
            var result = response as BadRequestObjectResult;
            Assert.AreEqual(400, result.StatusCode);
            Assert.AreEqual("The user is duplicated.", result.Value);
        }

        [TestMethod]
        public async Task CreateUser_PremiumGiftIsCorrect()
        {
            // Arrange
            var user = new UserCreationDTO()
            {
                Name = "Rafa",
                Email = "raf.ael+lop.ez@gmail.com",
                Address = "Av. Sol",
                Phone = "+34 91122354216",
                Type = UserTypes.Premium,
                Money = 1000
            };

            var controller = new UsersController(userService, mapper);

            // Act
            var response = await controller.Post(user);

            // Assert
            var result = response as CreatedAtRouteResult;
            UserDTO userCreated = result.Value as UserDTO;
            Assert.AreEqual(3000, userCreated.Money);
        }
    }
}
