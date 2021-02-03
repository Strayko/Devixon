using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DevixonApi.Data.Entities;
using DevixonApi.Data.Helpers;
using DevixonApi.Data.Interfaces;
using DevixonApi.Data.Managers;
using DevixonApi.Data.Models;
using DevixonApi.Data.Requests;
using DevixonApi.Data.Responses;
using Microsoft.EntityFrameworkCore;

namespace DevixonApi.Data.Services
{
    public class UserService : IUserService
    {
        private readonly IAppDbContext _appDbContext;
        private readonly IFacebookService _facebookService;
        private readonly IImageService _imageService;

        public UserService(IAppDbContext appDbContext, IFacebookService facebookService, IImageService imageService)
        {
            _appDbContext = appDbContext;
            _facebookService = facebookService;
            _imageService = imageService;
        }

        public async Task<LoggedUserResponse> Authenticate(LoginRequest loginRequest)
        {
            var user = await _appDbContext.Users.FirstOrDefaultAsync(u => u.Email == loginRequest.Email);
            if (user == null) return null;
            
            var passwordHash = HashingHelper.HashUsingPbkdf2(loginRequest.Password, user.PasswordSalt);
            if (user.Password != passwordHash) return null;

            var token = JwtAuthManager.GenerateToken(user);

            return LoggedUser(user, token);
        }

        public async Task<LoggedUserResponse> Registration(RegisterRequest registerRequest)
        {
            var base64Encode = PasswordHelper.EncodeAndHash(registerRequest.Password, out var passwordHash);

            var user = await CreateUser(registerRequest, passwordHash, base64Encode);
            await _appDbContext.SaveChangesAsync(CancellationToken.None);
            
            var token = JwtAuthManager.GenerateToken(user);

            return LoggedUser(user, token);
        }

        public async Task<User> GetUserAsync(int userId)
        {
            var user = _appDbContext.Users.Where(u => u.Id == userId)
                .Include(i=> i.Image);
            
            return await user.FirstOrDefaultAsync();
        }

        public async Task<User> UpdateUserAsync(UserModel userModel)
        {
            var user = _appDbContext.Users.SingleOrDefault(u => u.Id == userModel.Id);
            if (user == null) return null;

            user.FirstName = userModel.FirstName;
            user.LastName = userModel.LastName;
            user.Email = userModel.Email;
            
            var base64EncodeFormat = _imageService.Base64FormatExists(userModel.SetImage);
            if (base64EncodeFormat != null)
            {
                var uploadedImage = await _imageService.UploadedImage(userModel.SetImage);
                user.ImageId = uploadedImage.Id;
            }
            
            if (!string.IsNullOrEmpty(userModel.Password))
            {
                var base64Encode = PasswordHelper.EncodeAndHash(userModel.Password, out var passwordHash);
                
                user.Password = passwordHash;
                user.PasswordSalt = base64Encode;
            }
            
            await _appDbContext.SaveChangesAsync(CancellationToken.None);
            var getUpdatedUser = GetUserAsync(userModel.Id);

            return await getUpdatedUser;
        }

        public async Task<LoggedUserResponse> FacebookLoginAsync(FacebookLoginRequest facebookLoginRequest)
        {
            if (string.IsNullOrEmpty(facebookLoginRequest.FacebookToken)) 
                throw new Exception("Token is null or empty");

            var facebookUser = await _facebookService.GetUserFromFacebookAsync(facebookLoginRequest.FacebookToken);

            var applicationUser = await _appDbContext.Users.SingleOrDefaultAsync(user => user.Email == facebookUser.Email);

            string token;
            if (applicationUser == null)
            {
                var user = await CreateFacebookUser(facebookUser);
                await _appDbContext.SaveChangesAsync(CancellationToken.None);

                token = await Task.Run(() => JwtAuthManager.GenerateToken(user));

                return LoggedUser(user, token);
            }

            token = await Task.Run(() => JwtAuthManager.GenerateToken(applicationUser));
            
            return LoggedUser(applicationUser, token);
        }
        
        public bool ValidateToken(Token token)
        {
            var userToken = JwtAuthManager.ValidateCurrentToken(token);
            return userToken;
        }

        private async Task<User> CreateFacebookUser(FacebookLoginResponse facebookUser)
        {
            var user = new User
            {
                FirstName = facebookUser.FirstName,
                LastName = facebookUser.LastName,
                Email = facebookUser.Email,
                Password = null,
                PasswordSalt = null,
                FacebookUser = true,
                Active = true,
                Blocked = false,
                ImageId = null,
                CreatedAt = DateTime.Now
            };
            await _appDbContext.Users.AddAsync(user, CancellationToken.None);

            return user;
        }

        private static LoggedUserResponse LoggedUser(User user, string token)
        {
            return new LoggedUserResponse()
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Token = token
            };
        }
        
        private async Task<User> CreateUser(RegisterRequest registerRequest, string passwordHash, string base64Encode)
        {
            var user = new User
            {
                FirstName = registerRequest.FirstName,
                LastName = registerRequest.LastName,
                Email = registerRequest.Email,
                Password = passwordHash,
                PasswordSalt = base64Encode,
                FacebookUser = false,
                Active = true,
                Blocked = false,
                ImageId = null,
                CreatedAt = DateTime.Now
            };
            
            await _appDbContext.Users.AddAsync(user, CancellationToken.None);
            
            return user;
        }
    }
}