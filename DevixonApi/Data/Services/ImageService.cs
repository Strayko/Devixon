using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DevixonApi.Data.Entities;
using DevixonApi.Data.Interfaces;
using DevixonApi.Data.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DevixonApi.Data.Services
{
    public class ImageService : IImageService
    {
        private readonly IAppDbContext _appDbContext;

        public ImageService(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        
        public Match Base64FormatExists(string imageOutput)
        {
            var base64Data = imageOutput.Substring(0, imageOutput.IndexOf(",", StringComparison.Ordinal));
            var encodedFormat = Regex.Match(base64Data, @"\b(base64)\b");
            return encodedFormat;
        }

        public async Task<EntityEntry<Image>> UploadedImage(string imageOutput)
        {
            var base64Image = imageOutput.Substring(imageOutput.IndexOf(",", StringComparison.Ordinal) + 1);
            var base64Data = imageOutput.Substring(0, imageOutput.IndexOf(",", StringComparison.Ordinal));

            var imageFormat = Regex.Match(base64Data, @"\b(jpeg|png|jpg)\b");

            var folderName = Path.Combine("Resources", "Images");
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            var imageName = $"test.{imageFormat.Value}";

            var imgPath = Path.Combine(pathToSave, imageName);
            var imageBytes = Convert.FromBase64String(base64Image);
            await File.WriteAllBytesAsync(imgPath, imageBytes);

            var uploadedImage = await _appDbContext.Images.AddAsync(
                new Image
                {
                    Name = imageName,
                    CreatedAt = DateTime.Now
                });

            await _appDbContext.SaveChangesAsync();
            return uploadedImage;
        }
    }
}