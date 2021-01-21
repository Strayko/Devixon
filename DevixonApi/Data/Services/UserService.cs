using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DevixonApi.Data.Entities;
using DevixonApi.Data.Helpers;
using DevixonApi.Data.Interfaces;
using DevixonApi.Data.Managers;
using DevixonApi.Data.Models;
using DevixonApi.Data.Requests;
using DevixonApi.Data.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

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
            var user = _appDbContext.Users.SingleOrDefault(user => user.Email == loginRequest.Email);
            if (user == null) return null;
            
            var passwordHash = HashingHelper.HashUsingPbkdf2(loginRequest.Password, user.PasswordSalt);
            if (user.Password != passwordHash) return null;

            var token = await Task.Run(() => JwtAuthManager.GenerateToken(user));

            return LoggedUser(user, token);
        }

        public async Task<LoggedUserResponse> Registration(RegisterRequest registerRequest)
        {
            var base64Encode = PasswordHelper.EncodeAndHash(registerRequest.Password, out var passwordHash);

            var user = CreateUser(registerRequest, passwordHash, base64Encode);
            await _appDbContext.SaveChangesAsync();
            
            var token = await Task.Run(() => JwtAuthManager.GenerateToken(user.Entity));

            return LoggedUser(user.Entity, token);
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
            if (base64EncodeFormat.Success)
            {
                var uploadedImage = await _imageService.UploadedImage(userModel.SetImage);
                user.ImageId = uploadedImage.Entity.Id;
            }
            
            if (!string.IsNullOrEmpty(userModel.Password))
            {
                var base64Encode = PasswordHelper.EncodeAndHash(userModel.Password, out var passwordHash);
                
                user.Password = passwordHash;
                user.PasswordSalt = base64Encode;
            }
            
            await _appDbContext.SaveChangesAsync();
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
                await _appDbContext.SaveChangesAsync();

                token = await Task.Run(() => JwtAuthManager.GenerateToken(user.Entity));

                return LoggedUser(user.Entity, token);
            }

            token = await Task.Run(() => JwtAuthManager.GenerateToken(applicationUser));
            
            return LoggedUser(applicationUser, token);
        }
        
        public bool ValidateToken(Token token)
        {
            var userToken = JwtAuthManager.ValidateCurrentToken(token);
            return userToken;
        }

        private async Task<EntityEntry<User>> CreateFacebookUser(FacebookLoginResponse facebookUser)
        {
            var user = await _appDbContext.Users.AddAsync(new User
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
            });
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
        
        private EntityEntry<User> CreateUser(RegisterRequest registerRequest, string passwordHash, string base64Encode)
        {
            var user = _appDbContext.Users.Add(new User
            {
                Id = 2,
                FirstName = registerRequest.FirstName,
                LastName = registerRequest.LastName,
                Email = registerRequest.Email,
                Password = passwordHash,
                PasswordSalt = base64Encode,
                FacebookUser = false,
                Active = true,
                Blocked = false,
                ImageId = 2,
                CreatedAt = DateTime.Now
            });
            return user;
        }
    }
}