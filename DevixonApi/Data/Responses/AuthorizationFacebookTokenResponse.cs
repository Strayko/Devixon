namespace DevixonApi.Data.Responses
{
    public class AuthorizationFacebookTokenResponse
    {
        public FacebookTokenResponse AccessToken { get; set; }
        public FacebookTokenResponse RefreshToken { get; set; }
    }
}