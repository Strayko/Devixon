using System;
using System.Threading;
using System.Threading.Tasks;
using DevixonApi.Data.Interfaces;
using DevixonApi.Data.Services;
using DevixonApi.Tests.Extension;
using DevixonApi.Tests.TestHelper;
using Moq;
using NUnit.Framework;

namespace DevixonApi.Tests
{
    [TestFixture]
    public class ImageServiceTests
    {
        [Test]
        public void WhenSet_ImageBase64Format_ReturnTrue()
        {
            var appDbContext = new Mock<IAppDbContext>();
            var imageService = new ImageService(appDbContext.Object);

            var result = imageService.Base64FormatExists(Base64ImageTestHelper.EncodeString);
            
            Assert.AreEqual(true, result);
        }

        [Test]
        public void WhenNotSet_ImageBase64Format_ReturnFalse()
        {
            var appDbContext = new Mock<IAppDbContext>();
            var imageService = new ImageService(appDbContext.Object);

            var result = imageService.Base64FormatExists(Base64ImageTestHelper.FailedString);
            
            Assert.AreEqual(false, result);
        }
    }
}