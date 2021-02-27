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
        private Mock<IFileSystemService> _fileSystemService;
        private UserService _userService;
        private List<User> _users;
        private UserTestingHelper _userTestingHelper;
        private User _user;

        [SetUp]
        public void SetUp()
        {
            _appDbContext = new Mock<IAppDbContext>();
            _facebookService = new Mock<IFacebookService>();
            _imageService = new Mock<IImageService>();
            _fileSystemService = new Mock<IFileSystemService>();
            _userTestingHelper = new UserTestingHelper();
            _users = DbSetExtensions.InMemoryUsersData();
            _user = _users.Find(u => u.Id == 1);
            _appDbContext.Setup(u => u.Users).Returns(DbSetExtensions.CreateMockedDbSetAsync(_users));
            _userService = new UserService(
                _appDbContext.Object, 
                _facebookService.Object, 
                _imageService.Object, 
                _fileSystemService.Object);

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
            var result = await _userService.Authenticate(_userTestingHelper.Login("moamer@live.com", "moamer123#"));

            _userTestingHelper.AssertResult(firstName, lastName, email, result);
        }

        [Test]
        [TestCase("moammer@live.com", "moamer123#")]
        public async Task WhenLogin_User_ReturnEmailNotFount(string email, string password)
        {
            var result = await _userService.Authenticate(_userTestingHelper.Login(email, password));

            Assert.IsNull(result);
        }

        [Test]
        [TestCase("moamer@live.com", "moamer1234##")]
        public async Task WhenLogin_User_ReturnPasswordNotMatch(string email, string password)
        {
            var result = await _userService.Authenticate(_userTestingHelper.Login(email, password));

            Assert.IsNull(result);
        }

        [Test]
        [TestCase("Damir","Sauli","damir@live.com")]
        public async Task WhenRegister_User_ReturnUserInfo(string firstName, string lastName, string email)
        {
            var result = await _userService.Registration(_registerRequest);
            
            _appDbContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);

            _userTestingHelper.AssertResult(firstName, lastName, email, result);
        }

        [Test]
        public async Task WhenRequest_UserById_ReturnUser()
        {
            var result = await _userService.GetUserAsync(_user.Id);
            
            Assert.AreEqual(_user, result);
        }

        [Test]
        public async Task WhenUpdate_User_ReturnUpdatedUser()
        {
            const string userValues = "1,MoamerNew,JusupovicNew,moamerNew@live.com,null,newnew123#";
            var userModel = _userTestingHelper.Model(userValues);

            var result = await _userService.UpdateUserAsync(userModel);

            _appDbContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            
            Assert.AreEqual(userModel.Id, result.Id);
            Assert.AreEqual(userModel.FirstName, result.FirstName);
            Assert.AreEqual(userModel.LastName, result.LastName);
            Assert.AreEqual(userModel.Email, result.Email);
        }

        [Test]
        public async Task WhenUpdate_User_ReturnNotFoundById()
        {
            const string wrongUserIdValues = "2,test,test,test@live.com,null,null";
            var userModel = _userTestingHelper.Model(wrongUserIdValues);

            var result = await _userService.UpdateUserAsync(userModel);
            
            Assert.IsNull(result);
        }

        [Test]
        public async Task WhenUpdate_User_ReturnPasswordNotSet()
        {
            const string passwordNotSetValues = "1,test,test,test@live.com,null,null";
            var userModel = _userTestingHelper.Model(passwordNotSetValues);

            var result = await _userService.UpdateUserAsync(userModel);
            _appDbContext.Verify(x=>x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            
            Assert.AreEqual(_user.Password, result.Password);
        }

        [Test]
        public async Task WhenDelete_User_ReturnNoContent()
        {
            await _userService.DeleteUserAsync(_user.Id);

            Assert.AreEqual(0,_users.Count);
            _appDbContext.Verify(o=>o.SaveChangesAsync(CancellationToken.None), Times.Once);
        }
    }
}