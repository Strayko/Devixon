using System.Threading.Tasks;
using DevixonApi.Data.Responses;

namespace DevixonApi.Data.Interfaces
{
    public interface IFacebookService
    {
        Task<FacebookLoginResponse> GetUserFromFacebookAsync(string facebookToken);
    }
}