using System;
using System.Data;
using System.IO;
using System.IO.Abstractions;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using DevixonApi.Data.Entities;
using DevixonApi.Data.Interfaces;

namespace DevixonApi.Data.Services
{
    public class ImageService : IImageService
    {
        private readonly IAppDbContext _appDbContext;

        public ImageService(IAppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public bool Base64FormatExists(string imageOutput)
        {
            if (!imageOutput.Contains(",")) return false;
            
            var base64Data = imageOutput.Substring(0, imageOutput.IndexOf(",", StringComparison.Ordinal));
            var encodedFormat = Regex.Match(base64Data, @"\b(base64)\b");
            return encodedFormat.Success;
        }

        public async Task<Image> SaveImage(string imageName)
        {
            var image = new Image
            {
                Name = imageName,
                CreatedAt = DateTime.Now
            };

            await _appDbContext.Images.AddAsync(image, CancellationToken.None);
            await _appDbContext.SaveChangesAsync(CancellationToken.None);

            return image;
        }
    }
}