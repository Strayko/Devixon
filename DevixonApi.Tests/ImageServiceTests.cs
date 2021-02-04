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
        public async Task WhenUpload_Image_ReturnImage()
        {
            var images = DbSetExtensions.InMemoryImagesData();
            var appDbContext = new Mock<IAppDbContext>();
            
            appDbContext.Setup(i => i.Images).Returns(DbSetExtensions.CreateMockedDbSetAsync(images));

            var imageService = new ImageService(appDbContext.Object);
            var result = await imageService.UploadedImage(Base64ImageTestHelper.EncodeString);
            Console.WriteLine(result);
            Console.WriteLine(result);
        }
    }
}