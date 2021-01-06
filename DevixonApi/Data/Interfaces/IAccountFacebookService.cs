using System.Threading.Tasks;
using DevixonApi.Data.Entities;
using DevixonApi.Data.Requests;
using DevixonApi.Data.Responses;

namespace DevixonApi.Data.Interfaces
{
    public interface IAccountFacebookService
    {
        Task<AuthorizationFacebookTokenResponse> FacebookLoginAsync(FacebookLoginRequest facebookLoginRequest);
    }
}