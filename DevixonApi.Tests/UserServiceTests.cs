using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DevixonApi.Data;
using DevixonApi.Data.Entities;
using DevixonApi.Data.Interfaces;
using DevixonApi.Data.Managers;
using DevixonApi.Data.Requests;
using DevixonApi.Data.Responses;
using DevixonApi.Data.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;
using NSubstitute;
using NUnit.Framework;

namespace DevixonApi.Tests
{
    [TestFixture]
    public class UserServiceTests
    {
        private LoginRequest _loginRequest;
        private Mock<IAppDbContext> _appDbContext;
        private Mock<IFacebookService> _facebookService;
        private Mock<IImageService> _imageService;
        private UserService _userService;
        private List<User> _users;

        [SetUp]
        public void SetUp()
        {
            _appDbContext = new Mock<IAppDbContext>();
            _facebookService = new Mock<IFacebookService>();
            _imageService = new Mock<IImageService>();
            _users = DbSetExtensions.InMemoryUsersData();
            _appDbContext.Setup(u => u.Users).Returns(DbSetExtensions.CreateMockedDbSetAsync(_users));
            _userService = new UserService(_appDbContext.Object, _facebookService.Object, _imageService.Object);

            _loginRequest = new LoginRequest
            {
                Email = "moamer@live.com",
                Password = "moamer123#"
            };
        }

        [Test]
        public void WhenLogin_User_ReturnUserInfo()
        {
            var result = _userService.Authenticate(_loginRequest);
            
            Assert.AreEqual("Moamer", result.Result.FirstName);
            Assert.AreEqual("Jusupovic", result.Result.LastName);
            Assert.AreEqual("moamer@live.com", result.Result.Email);
            Assert.IsNotEmpty(result.Result.Token);
        }

        [Test]
        public void WhenLogin_User_ReturnNotFount()
        {
            var result = _userService.Authenticate(new LoginRequest
            {
                Email = "moammer@live.com",
                Password = "moamer123#"
            });

            Assert.IsNull(result.Result);
        }

        [Test]
        public void WhenLogin_User_ReturnPasswordNotMatch()
        {
            var result = _userService.Authenticate(new LoginRequest
            {
                Email = "moamer@live.com",
                Password = "moamer1234##"
            });

            Assert.IsNull(result.Result);
        }

        [Test]
        public void WhenRegister_User_ReturnUserInfo()
        {
            var users = new List<User>();
            IEnumerable<EntityEntry> entries = new List<EntityEntry>();
            
            var registerRequest = new RegisterRequest
            {
                FirstName = "Damir",
                LastName = "Sauli",
                Email = "damir@live.com",
                Password = "damir123#"
            };
            
            var appDbContext = new Mock<IAppDbContext>();
            var facebookService = new Mock<IFacebookService>();
            var imageService = new Mock<IImageService>();
            // var mockChangeTracker = new Mock<ChangeTracker>(MockBehavior.Strict, appDbContext.Object);
            //
            // mockChangeTracker.Setup(c => c.Entries()).Returns(entries);
            // appDbContext.Setup(m => m.ChangeTracker).Returns(mockChangeTracker.Object);

            appDbContext.Setup(s => s.Users).Returns(DbSetExtensions.CreateMockedDbSetAsync(users));
            appDbContext.Setup(p => p.SaveChangesAsync(CancellationToken.None)).Returns(Task.FromResult(1));

            var userService = new UserService(appDbContext.Object, facebookService.Object, imageService.Object);
            userService.Registration(registerRequest);
            
            appDbContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            
        }
    }
}