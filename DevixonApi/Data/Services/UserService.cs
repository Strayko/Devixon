using System;
using System.Linq;
using System.Threading.Tasks;
using DevixonApi.Data.Entities;
using DevixonApi.Data.Helpers;
using DevixonApi.Data.Interfaces;
using DevixonApi.Data.Requests;
using DevixonApi.Data.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DevixonApi.Data.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _appDbContext;

        public UserService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<LoggedUserResponse> Authenticate(LoginRequest loginRequest)
        {
            var user = _appDbContext.Users.SingleOrDefault(user => user.Email == loginRequest.Email);
            if (user == null) return null;
            
            var passwordHash = HashingHelper.HashUsingPbkdf2(loginRequest.Password, user.PasswordSalt);
            if (user.Password != passwordHash) return null;

            var token = await Task.Run(() => TokenHelper.GenerateToken(user));

            return LoggedUser(user, token);
        }

        public async Task<LoggedUserResponse> Registration(RegisterRequest registerRequest)
        {
            var base64Encode = Base64EncodeHelper.Generate(registerRequest.Password);
            var passwordHash = HashingHelper.HashUsingPbkdf2(registerRequest.Password, base64Encode);

            var user = CreateUser(registerRequest, passwordHash, base64Encode);
            await _appDbContext.SaveChangesAsync();
            
            var token = await Task.Run(() => TokenHelper.GenerateToken(user.Entity));

            return LoggedUser(user.Entity, token);
        }

        public async Task<User> GetUserAsync(int userId)
        {
            IQueryable<User> user = _appDbContext.Users.Where(u => u.Id == userId);
            return await user.FirstOrDefaultAsync();
        }

        public bool ValidateUser(Token token)
        {
            var userToken = TokenHelper.ValidateCurrentToken(token);
            return userToken;
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
                FirstName = registerRequest.FirstName,
                LastName = registerRequest.LastName,
                Email = registerRequest.Email,
                Password = passwordHash,
                PasswordSalt = base64Encode,
                Active = true,
                Blocked = false,
                TS = DateTime.Now
            });
            return user;
        }
    }
}