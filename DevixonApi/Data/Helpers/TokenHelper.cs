using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using DevixonApi.Data.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DevixonApi.Data.Helpers
{
    public class TokenHelper
    {
        // public static string GenerateSecureSecret()
        // {
        //     var hmac = new HMACSHA256();
        //     return Convert.ToBase64String(hmac.Key);
        // }

        public static string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Convert.FromBase64String(JwtCredentialsHelper.Secret);

            var claimsIdentity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim("IsBlocked", user.Blocked.ToString()), 
            });
            
            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Issuer = JwtCredentialsHelper.Issuer,
                Audience = JwtCredentialsHelper.Audience,
                Expires = DateTime.Now.AddMinutes(15),
                SigningCredentials = signingCredentials,
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}