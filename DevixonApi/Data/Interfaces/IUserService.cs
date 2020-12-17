using System.Threading.Tasks;
using DevixonApi.Data.Entities;
using DevixonApi.Data.Requests;
using DevixonApi.Data.Responses;

namespace DevixonApi.Data.Interfaces
{
    public interface IUserService
    {
        Task<LoginResponse> Authenticate(LoginRequest loginRequest);
        Task<RegisterResponse> Registration(RegisterRequest registerRequest);
        Task<User> GetUserAsync(int userId);
    }
}