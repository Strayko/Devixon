using System;
using System.Threading.Tasks;
using DevixonApi.Data.Entities;
using DevixonApi.Data.Interfaces;
using DevixonApi.Data.Requests;
using DevixonApi.Data.Responses;
using Microsoft.EntityFrameworkCore;

namespace DevixonApi.Data.Services
{
    public class AccountFacebookService : IAccountFacebookService
    {
        private readonly AppDbContext _appDbContext;
        private readonly IFacebookService _facebookService;
        private readonly IJwtFacebookHandler _jwtFacebookHandler;

        public AccountFacebookService(AppDbContext appDbContext,IFacebookService facebookService, IJwtFacebookHandler jwtFacebookHandler)
        {
            _appDbContext = appDbContext;
            _facebookService = facebookService;
            _jwtFacebookHandler = jwtFacebookHandler;
        }
        
        public async Task<AuthorizationFacebookTokenResponse> FacebookLoginAsync(
            FacebookLoginRequest facebookLoginRequest)
        {
            if (string.IsNullOrEmpty(facebookLoginRequest.FacebookToken)) 
                throw new Exception("Token is null or empty");

            var facebookUser = await _facebookService.GetUserFromFacebookAsync(facebookLoginRequest.FacebookToken);
            // var user = _appDbContext
            // var domainUser = await _appDbContext.Users.SingleOrDefaultAsync(user => user.Email == facebookUser.Email);

            return await CreateAccessTokens(facebookUser);
        }

        private async Task<AuthorizationFacebookTokenResponse> CreateAccessTokens(FacebookLoginResponse user)
        {
            var accessToken = _jwtFacebookHandler.CreateAccessToken(user.Id, user.Email);
            var refreshToken = _jwtFacebookHandler.CreateRefreshToken(user.Id);

            return new AuthorizationFacebookTokenResponse {AccessToken = accessToken, RefreshToken = refreshToken};
        }
    }
}