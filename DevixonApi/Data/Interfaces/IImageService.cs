using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DevixonApi.Data.Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DevixonApi.Data.Interfaces
{
    public interface IImageService
    {
        Match Base64FormatExists(string imageOutput);
        Task<Image> UploadedImage(string imageOutput);
    }
}