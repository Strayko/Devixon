using System;
using System.IO;
using System.IO.Abstractions;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using DevixonApi.Data.Interfaces;

namespace DevixonApi.Data.Services
{
    public class FileSystemService : IFileSystemService
    {
        private readonly IFileSystem _fileSystem;

        public FileSystemService(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public async Task<string> UploadImage(string imageOutput)
        {
            var base64Image = imageOutput.Substring(imageOutput.IndexOf(",", StringComparison.Ordinal) + 1);
            var base64Data = imageOutput.Substring(0, imageOutput.IndexOf(",", StringComparison.Ordinal));

            var imageFormat = Regex.Match(base64Data, @"\b(jpeg|png|jpg)\b");

            var folderName = Path.Combine("Resources", "Images");
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            var imageName = $"test.{imageFormat.Value}";

            var imgPath = Path.Combine(pathToSave, imageName);
            var imageBytes = Convert.FromBase64String(base64Image);
            await _fileSystem.File.WriteAllBytesAsync(imgPath, imageBytes, CancellationToken.None);

            return imageName;
        }
    }
}