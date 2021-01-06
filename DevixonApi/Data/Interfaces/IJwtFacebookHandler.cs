using DevixonApi.Data.Responses;

namespace DevixonApi.Data.Interfaces
{
    public interface IJwtFacebookHandler
    {
        FacebookTokenResponse CreateAccessToken(string userId, string email);
        FacebookTokenResponse CreateRefreshToken(string userId);
    }
}