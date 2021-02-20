using System;
using System.IO.Abstractions;
using System.Threading;
using System.Threading.Tasks;
using DevixonApi.Data.Entities;
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

            var result = imageService.Base64FormatExists(Base64ImageTestingHelper.EncodeString);
            
            Assert.AreEqual(true, result);
        }

        [Test]
        public void WhenNotSet_ImageBase64Format_ReturnFalse()
        {
            var appDbContext = new Mock<IAppDbContext>();
            var imageService = new ImageService(appDbContext.Object);

            var result = imageService.Base64FormatExists(Base64ImageTestingHelper.FailedString);
            
            Assert.AreEqual(false, result);
        }

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

        [Test]
        public async Task WhenSave_Image_ReturnImage()
        {
            var appDbContext = new Mock<IAppDbContext>();
            var images = DbSetExtensions.InMemoryImagesData();
            appDbContext.Setup(i => i.Images).Returns(DbSetExtensions.CreateMockedDbSetAsync(images));

            var imageService = new ImageService(appDbContext.Object);

            var result = await imageService.SaveImage("test.jpeg");
            
            appDbContext.Verify(i=>i.Images.AddAsync(It.IsAny<Image>(), CancellationToken.None), Times.Once);
            appDbContext.Verify(i=>i.SaveChangesAsync(CancellationToken.None), Times.Once);
            
            var expected = images.Find(i => i.Name == "test.jpeg");
            Assert.AreEqual(expected, result);
        }
    }
}