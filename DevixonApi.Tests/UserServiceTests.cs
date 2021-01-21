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
        private LoginRequest _loginRequest;
        private Mock<IAppDbContext> _appDbContext;
        private Mock<IFacebookService> _facebookService;
        private Mock<IImageService> _imageService;
        private DbSetFaker _dbSetFaker;
        private IQueryable<User> _data;
        private Mock<DbSet<User>> _mockSet;
        private UserService _userService;

        [SetUp]
        public void SetUp()
        {
            _appDbContext = new Mock<IAppDbContext>();
            _facebookService = new Mock<IFacebookService>();
            _imageService = new Mock<IImageService>();
            _dbSetFaker = new DbSetFaker();
            _data = _dbSetFaker.GetUser();
            _mockSet = new Mock<DbSet<User>>();
            _dbSetFaker.ProvideQuerableDbData(_mockSet, _data);
            _appDbContext.Setup(u => u.Users).Returns(_mockSet.Object);
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
            Task<LoggedUserResponse> user = _userService.Authenticate(_loginRequest);
            
            Assert.AreEqual("Moamer", user.Result.FirstName);
            Assert.AreEqual("Jusupovic", user.Result.LastName);
            Assert.AreEqual("moamer@live.com", user.Result.Email);
            Assert.IsNotEmpty(user.Result.Token);
        }

        [Test]
        public void WhenLogin_User_ReturnNotFount()
        {
            Task<LoggedUserResponse> user = _userService.Authenticate(new LoginRequest
            {
                Email = "moammer@live.com",
                Password = "moamer123#"
            });
            
            Assert.IsNull(user.Result);
        }

        [Test]
        public void WhenLogin_User_ReturnPasswordNotMatch()
        {
            Task<LoggedUserResponse> user = _userService.Authenticate(new LoginRequest
            {
                Email = "moamer@live.com",
                Password = "moamer1234##"
            });
            
            Assert.IsNull(user.Result);
        }

        [Test]
        public void WhenRegister_User_ReturnUserInfo()
        {
            List<User> users = new List<User>();

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
            var dbSetFaker = new DbSetFaker();
            appDbContext.Setup(u => u.Users).Returns(dbSetFaker.GetQueryableMockDbSet<User>(users));
            appDbContext.Setup(p => p.SaveChangesAsync()).ReturnsAsync(1);

            var userService = new UserService(appDbContext.Object, facebookService.Object, imageService.Object);
            
            var createUser = userService.Registration(registerRequest);
            Console.WriteLine(createUser);
            Console.WriteLine(createUser);
        }
    }
}