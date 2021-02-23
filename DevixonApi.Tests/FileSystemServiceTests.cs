using System.IO.Abstractions;
using System.Threading;
using System.Threading.Tasks;
using DevixonApi.Data.Services;
using DevixonApi.Tests.TestHelper;
using Moq;
using NUnit.Framework;

namespace DevixonApi.Tests
{
    [TestFixture]
    public class FileSystemServiceTests
    {
        [Test]
        public async Task WhenUpload_ImageOnFileSystem_ReturnImageName()
        {
            var mockFile = new Mock<IFileSystem>();
            mockFile.Setup(f => f.File.WriteAllBytesAsync(It.IsAny<string>(), It.IsAny<byte[]>(), CancellationToken.None))
                .Returns(Task.CompletedTask);
            
            var fileSystemService = new FileSystemService(mockFile.Object);
            var result = await fileSystemService.UploadImage(Base64ImageTestingHelper.EncodeString);
            
            mockFile.Verify(f=>f.File.WriteAllBytesAsync(It.IsAny<string>(), It.IsAny<byte[]>(), CancellationToken.None), Times.Once);
            Assert.AreEqual("test.jpeg", result);
        }
    }
}