using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DevixonApi.Data.Entities;
using DevixonApi.Data.Interfaces;
using DevixonApi.Data.Requests;
using DevixonApi.Data.Services;
using DevixonApi.Tests.Extension;
using DevixonApi.Tests.TestHelper;
using Moq;
using NUnit.Framework;

namespace DevixonApi.Tests
{
    [TestFixture]
    public class UserServiceTests
    {
        private RegisterRequest _registerRequest;
        private Mock<IAppDbContext> _appDbContext;
        private Mock<IFacebookService> _facebookService;
        private Mock<IImageService> _imageService;
        private UserService _userService;
        private List<User> _users;
        private UserHelper _userHelper;

        [SetUp]
        public void SetUp()
        {
            _appDbContext = new Mock<IAppDbContext>();
            _facebookService = new Mock<IFacebookService>();
            _imageService = new Mock<IImageService>();
            _userHelper = new UserHelper();
            _users = DbSetExtensions.InMemoryUsersData();
            _appDbContext.Setup(u => u.Users).Returns(DbSetExtensions.CreateMockedDbSetAsync(_users));
            _userService = new UserService(_appDbContext.Object, _facebookService.Object, _imageService.Object);

            _registerRequest = new RegisterRequest
            {
                FirstName = "Damir",
                LastName = "Sauli",
                Email = "damir@live.com",
                Password = "damir123#"
            };
        }

        [Test]
        [TestCase("Moamer","Jusupovic","moamer@live.com")]
        public async Task WhenLogin_User_ReturnUserInfo(string firstName, string lastName, string email)
        {
            var result = await _userService.Authenticate(_userHelper.Login("moamer@live.com", "moamer123#"));

            _userHelper.AssertResult(firstName, lastName, email, result);
        }

        [Test]
        [TestCase("moammer@live.com", "moamer123#")]
        public async Task WhenLogin_User_ReturnEmailNotFount(string email, string password)
        {
            var result = await _userService.Authenticate(_userHelper.Login(email, password));

            Assert.IsNull(result);
        }

        [Test]
        [TestCase("moamer@live.com", "moamer1234##")]
        public async Task WhenLogin_User_ReturnPasswordNotMatch(string email, string password)
        {
            var result = await _userService.Authenticate(_userHelper.Login(email, password));

            Assert.IsNull(result);
        }

        [Test]
        [TestCase("Damir","Sauli","damir@live.com")]
        public async Task WhenRegister_User_ReturnUserInfo(string firstName, string lastName, string email)
        {
            var result = await _userService.Registration(_registerRequest);
            
            _appDbContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);

            _userHelper.AssertResult(firstName, lastName, email, result);
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
            const string userValues = "1,MoamerNew,JusupovicNew,moamerNew@live.com,null,newnew123#";
            var userModel = _userHelper.Model(userValues);

            var result = await _userService.UpdateUserAsync(userModel);

            _appDbContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            
            Assert.AreEqual(userModel.Id, result.Id);
            Assert.AreEqual(userModel.FirstName, result.FirstName);
            Assert.AreEqual(userModel.LastName, result.LastName);
            Assert.AreEqual(userModel.Email, result.Email);
        }

        [Test]
        public async Task WhenUpdate_User_NotFoundById()
        {
            const string wrongUserIdValues = "2,test,test,test@live.com,null,null";
            var userModel = _userHelper.Model(wrongUserIdValues);

            var result = await _userService.UpdateUserAsync(userModel);
            
            Assert.IsNull(result);
        }

        [Test]
        public async Task WhenUpdate_User_PasswordNotSet()
        {
            var user = _users.Find(u => u.Id == 1);
            
            const string passwordNotSetValues = "1,test,test,test@live.com,null,null";
            var userModel = _userHelper.Model(passwordNotSetValues);

            var result = await _userService.UpdateUserAsync(userModel);
            _appDbContext.Verify(x=>x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            
            Assert.AreEqual(user.Password, result.Password);
        }
    }
}