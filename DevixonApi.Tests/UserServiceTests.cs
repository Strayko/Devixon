using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DevixonApi.Controllers;
using DevixonApi.Data.Entities;
using DevixonApi.Data.Interfaces;
using DevixonApi.Data.Requests;
using DevixonApi.Data.Responses;
using DevixonApi.Data.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;

namespace DevixonApi.Tests
{
    [TestFixture]
    public class UserServiceTests
    {
        private LoginRequest loginRequest = new LoginRequest
        {
            Email = "moamer@live.com",
            Password = "moamer123#"
        };

        [Test]
        public void WhenLogin_User_ReturnUserInfo()
        {
            var appDbContext = new Mock<IAppDbContext>();
            var facebookService = new Mock<IFacebookService>();
            var imageService = new Mock<IImageService>();
            var dbSetFaker = new DbSetFaker();
            var data = dbSetFaker.GetUser();
            var mockSet = new Mock<DbSet<User>>();
            dbSetFaker.ProvideQuerableDbSet(mockSet, data);
            appDbContext.Setup(u => u.Users).Returns(mockSet.Object);
            var userService = new UserService(appDbContext.Object, facebookService.Object, imageService.Object);
            
            Task<LoggedUserResponse> user = userService.Authenticate(loginRequest);

            Assert.AreEqual("Moamer", user.Result.FirstName);
            Assert.AreEqual("Jusupovic", user.Result.LastName);
            Assert.AreEqual("moamer@live.com", user.Result.Email);
            Assert.IsNotEmpty(user.Result.Token);
        }
    }
}