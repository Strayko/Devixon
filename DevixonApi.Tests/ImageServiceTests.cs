using System.Collections.Generic;
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
        private Mock<IAppDbContext> _appDbContext;
        private ImageService _imageService;
        private List<Image> _images;

        [SetUp]
        public void SetUp()
        {
            _appDbContext = new Mock<IAppDbContext>();
            _imageService = new ImageService(_appDbContext.Object);
            _images = DbSetExtensions.InMemoryImagesData();
            _appDbContext.Setup(i => i.Images).Returns(DbSetExtensions.CreateMockedDbSetAsync(_images));
        }
        
        [Test]
        public void WhenSet_ImageBase64Format_ReturnTrue()
        {
            var result = _imageService.Base64FormatExists(Base64ImageTestingHelper.EncodeString);
            
            Assert.AreEqual(true, result);
        }

        [Test]
        public void WhenNotSet_ImageBase64Format_ReturnFalse()
        {
            var result = _imageService.Base64FormatExists(Base64ImageTestingHelper.FailedString);
            
            Assert.AreEqual(false, result);
        }

        [Test]
        public async Task WhenSave_Image_ReturnImage()
        {
            var result = await _imageService.SaveImage("test.jpeg");
            
            _appDbContext.Verify(i=>i.Images.AddAsync(It.IsAny<Image>(), CancellationToken.None), Times.Once);
            _appDbContext.Verify(i=>i.SaveChangesAsync(CancellationToken.None), Times.Once);
            
            var expected = _images.Find(i => i.Name == "test.jpeg");
            Assert.AreEqual(expected, result);
        }
    }
}