using System.IO.Abstractions;
using System.Threading;
using System.Threading.Tasks;
using DevixonApi.Data.Interfaces;
using DevixonApi.Data.Services;
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
            var mockFile = new Mock<IFile>();
            mockFile.Setup(s=>s.WriteAllBytesAsync(It.IsAny<string>(), It.IsAny<byte[]>(), CancellationToken.None))
                .Returns(Task.CompletedTask);
            
            var imageService = new ImageService(appDbContext.Object, mockFile.Object);

            var result = imageService.Base64FormatExists(Base64ImageTestHelper.EncodeString);
            
            Assert.AreEqual(true, result);
        }

        [Test]
        public void WhenNotSet_ImageBase64Format_ReturnFalse()
        {
            var appDbContext = new Mock<IAppDbContext>();
            var mockFile = new Mock<IFile>();
            mockFile.Setup(s=>s.WriteAllBytesAsync(It.IsAny<string>(), It.IsAny<byte[]>(), CancellationToken.None))
                .Returns(Task.CompletedTask);
            
            var imageService = new ImageService(appDbContext.Object, mockFile.Object);

            var result = imageService.Base64FormatExists(Base64ImageTestHelper.FailedString);
            
            Assert.AreEqual(false, result);
        }

        [Test]
        public async Task WhenUpload_ImageOnFileSystem_ReturnImageName()
        {
            var appDbContext = new Mock<IAppDbContext>();
            var mockFile = new Mock<IFile>();
            mockFile.Setup(f => f.WriteAllBytesAsync(It.IsAny<string>(), It.IsAny<byte[]>(), CancellationToken.None))
                .Returns(Task.CompletedTask);

            var imageService = new ImageService(appDbContext.Object, mockFile.Object);
            var result = await imageService.UploadedImageOnFileSystem(Base64ImageTestHelper.EncodeString);
            
            mockFile.Verify(f=>f.WriteAllBytesAsync(It.IsAny<string>(), It.IsAny<byte[]>(), CancellationToken.None), Times.Once);
            
            Assert.AreEqual("test.jpeg", result);
        }
    }
}