using System.Threading.Tasks;

namespace DevixonApi.Data.Interfaces
{
    public interface IFileSystemService
    {
        Task<string> UploadImage(string imageOutput);
    }
}