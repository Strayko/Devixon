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
using DevixonApi.Data.Models;
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
        private RegisterRequest _registerRequest;
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

            _registerRequest = new RegisterRequest
            {
                FirstName = "Damir",
                LastName = "Sauli",
                Email = "damir@live.com",
                Password = "damir123#"
            };
        }

        [Test]
        public async Task WhenLogin_User_ReturnUserInfo()
        {
            var result = await _userService.Authenticate(_loginRequest);
            
            Assert.AreEqual("Moamer", result.FirstName);
            Assert.AreEqual("Jusupovic", result.LastName);
            Assert.AreEqual("moamer@live.com", result.Email);
            Assert.IsNotEmpty(result.Token);
        }

        [Test]
        public async Task WhenLogin_User_ReturnNotFount()
        {
            var result = await _userService.Authenticate(new LoginRequest
            {
                Email = "moammer@live.com",
                Password = "moamer123#"
            });

            Assert.IsNull(result);
        }

        [Test]
        public async Task WhenLogin_User_ReturnPasswordNotMatch()
        {
            var result = await _userService.Authenticate(new LoginRequest
            {
                Email = "moamer@live.com",
                Password = "moamer1234##"
            });

            Assert.IsNull(result);
        }

        [Test]
        public async Task WhenRegister_User_ReturnUserInfo()
        {
            var result = await _userService.Registration(_registerRequest);
            
            _appDbContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            
            Assert.AreEqual("Damir", result.FirstName);
            Assert.AreEqual("Sauli", result.LastName);
            Assert.AreEqual("damir@live.com", result.Email);
            Assert.IsNotEmpty(result.Token);
        }

        [Test]
        public async Task WhenRequest_UserById_ReturnUser()
        {
            var user = _users.Find(u=>u.Id == 1);
            
            var result = await _userService.GetUserAsync(user.Id);
            
            Assert.AreEqual(user, result);
        }

        [Test]
        public async Task WhenUpdate_User_ReturnUpdatedUser()
        {
            var userModel = new UserModel
            {
                Id = 1,
                FirstName = "MoamerNew",
                LastName = "JusupovicNew",
                Email = "moamerNew@live.com",
                SetImage = null,
                Password = null
            };

            var result = await _userService.UpdateUserAsync(userModel);
            
            _appDbContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            
            Assert.AreEqual(userModel.Id, result.Id);
            Assert.AreEqual(userModel.FirstName, result.FirstName);
            Assert.AreEqual(userModel.LastName, result.LastName);
            Assert.AreEqual(userModel.Email, result.Email);
        }
    }
}