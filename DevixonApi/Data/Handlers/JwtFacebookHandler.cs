using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AutoMapper.Configuration;
using DevixonApi.Data.Helpers;
using DevixonApi.Data.Interfaces;
using DevixonApi.Data.Responses;
using Microsoft.IdentityModel.Tokens;

namespace DevixonApi.Data.Handlers
{
    public class JwtFacebookHandler : IJwtFacebookHandler
    {
        private readonly IConfiguration _configuration;
        private static byte[] _key = Bytes();

        public JwtFacebookHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public JwtFacebookHandler()
        {
            
        }

        public FacebookTokenResponse CreateAccessToken(string userId, string email)
        {
            var now = DateTime.UtcNow;
            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, userId.ToString()),
                new Claim(ClaimTypes.Email, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, now.ToShortTimeString())
            };

            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(_key), SecurityAlgorithms.HmacSha256Signature);

            var expiry = now.AddMinutes(180);
            var jwt = CreateSecurityToken(claims, now, expiry, signingCredentials);
            var token = new JwtSecurityTokenHandler().WriteToken(jwt);

            return CreateTokenResource(token, expiry.ToShortTimeString());
        }

        public FacebookTokenResponse CreateRefreshToken(string userId)
        {
            var now = DateTime.UtcNow;
            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, now.ToShortTimeString())
            };
            
            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(_key), SecurityAlgorithms.HmacSha256Signature);
            var expiry = now.AddMinutes(180);
            var jwt = CreateSecurityToken(claims, now, expiry, signingCredentials);
            var token = new JwtSecurityTokenHandler().WriteToken(jwt);

            return CreateTokenResource(token, expiry.ToShortTimeString());
        }

        private JwtSecurityToken CreateSecurityToken(IEnumerable<Claim> claims, DateTime now, DateTime expiry, SigningCredentials credentials)
            => new JwtSecurityToken(claims: claims, notBefore: now, expires: expiry, signingCredentials: credentials);

        private static FacebookTokenResponse CreateTokenResource(string token, string expiry)
            => new FacebookTokenResponse {Token = token, Expiry = expiry};
        
        private static byte[] Bytes()
        {
            var key = Convert.FromBase64String(JwtCredentialsHelper.Secret);
            return key;
        }
    }
}