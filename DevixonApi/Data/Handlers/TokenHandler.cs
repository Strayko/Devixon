using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using DevixonApi.Data.Entities;
using DevixonApi.Data.Helpers;
using Microsoft.IdentityModel.Tokens;

namespace DevixonApi.Data.Handlers
{
    public class TokenHandler
    {
        private static JwtSecurityTokenHandler _tokenHandler = new JwtSecurityTokenHandler();
        private static byte[] _key = Bytes();

        public static string GenerateToken(User user)
        {
            var claimsIdentity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim("IsBlocked", user.Blocked.ToString()),
            });
            
            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(_key), SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Issuer = JwtCredentialsHelper.Issuer,
                Audience = JwtCredentialsHelper.Audience,
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = signingCredentials,
            };

            var token = _tokenHandler.CreateToken(tokenDescriptor);
            return _tokenHandler.WriteToken(token);
        }

        public static bool ValidateCurrentToken(Token token)
        {
            var signingKey = new SymmetricSecurityKey(_key);
            
            try
            {
                _tokenHandler.ValidateToken(token.name, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = JwtCredentialsHelper.Issuer,
                    ValidAudience = JwtCredentialsHelper.Audience,
                    IssuerSigningKey = signingKey
                }, out SecurityToken validatedToken);
            }
            catch
            {
                return false;
            }

            return true;
        }
        
        private static byte[] Bytes()
        {
            var key = Convert.FromBase64String(JwtCredentialsHelper.Secret);
            return key;
        }
    }
}
