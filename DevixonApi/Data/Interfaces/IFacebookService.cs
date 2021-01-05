using System.Threading.Tasks;
using DevixonApi.Data.Responses;

namespace DevixonApi.Data.Interfaces
{
    public interface IFacebookService
    {
        Task<FacebookLoginResponse> GetUserFromFacebookAsync(string facebookToken);
        Task<T> GetAsync<T>(string accessToken, string endpoint, string args = null);
    }
}