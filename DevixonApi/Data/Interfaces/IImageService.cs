using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DevixonApi.Data.Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DevixonApi.Data.Interfaces
{
    public interface IImageService
    {
        bool Base64FormatExists(string imageOutput);
        Task<string> UploadedImageOnFileSystem(string imageOutput);
        Task<Image> SaveImage(string imageName);
    }
}