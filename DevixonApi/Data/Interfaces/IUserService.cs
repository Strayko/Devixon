using System.Threading.Tasks;
using DevixonApi.Data.Entities;
using DevixonApi.Data.Models;
using DevixonApi.Data.Requests;
using DevixonApi.Data.Responses;

namespace DevixonApi.Data.Interfaces
{
    public interface IUserService
    {
        Task<LoggedUserResponse> Authenticate(LoginRequest loginRequest);
        Task<LoggedUserResponse> Registration(RegisterRequest registerRequest);
        Task<LoggedUserResponse> FacebookLoginAsync(FacebookLoginRequest facebookLoginRequest);
        Task<User> UpdateUserAsync(UserModel userModel);
        Task<int?> DeleteUserAsync(int userId);
        Task<User> GetUserAsync(int userId);
        bool ValidateToken(Token token);
    }
}