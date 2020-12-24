using System.Threading.Tasks;
using DevixonApi.Data.Entities;
using DevixonApi.Data.Requests;
using DevixonApi.Data.Responses;

namespace DevixonApi.Data.Interfaces
{
    public interface IUserService
    {
        Task<LoggedUserResponse> Authenticate(LoginRequest loginRequest);
        Task<LoggedUserResponse> Registration(RegisterRequest registerRequest);
        Task<User> GetUserAsync(int userId);
        bool ValidateUser(Token token);
    }
}