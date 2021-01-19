using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using DevixonApi.Data.Entities;
using DevixonApi.Data.Interfaces;
using DevixonApi.Data.Models;
using DevixonApi.Data.Requests;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DevixonApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }
        
        [HttpPost]
        [AllowAnonymous]
        [Route("login")]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            if (!ModelState.IsValid) return BadRequest();
            
            var loginResponse = await _userService.Authenticate(loginRequest);
            if (loginResponse != null)
            {
                return Ok(loginResponse);
            }
                
            return Unauthorized(new {errors = "Invalid Credentials"});

        }
        
        [HttpPost]
        [AllowAnonymous]
        [Route("register")]
        public async Task<IActionResult> Register(RegisterRequest registerRequest)
        {
            if (!ModelState.IsValid) return BadRequest();
            
            var registerResponse = await _userService.Registration(registerRequest);
            return Ok(registerResponse);

        }

        [HttpGet]
        [Route("details")]
        public async Task<ActionResult<UserModel>> Details()
        {
            var userId = HttpContext.User.Claims.First().Value;
            if (userId == null) return BadRequest(new {errors = "User ID not found."});
            
            var user = await _userService.GetUserAsync(int.Parse(userId));
            if (user == null) return NotFound();

            return _mapper.Map<UserModel>(user);
        }

        [HttpPut, DisableRequestSizeLimit]
        [Route("update")]
        public async Task<ActionResult<UserModel>> Update(UserModel userModel)
        {
            if (!ModelState.IsValid) return BadRequest();

            var user = await _userService.UpdateUserAsync(userModel);
            
            return _mapper.Map<UserModel>(user);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("validate")]
        public IActionResult Validate(Token token)
        {
            var validate = _userService.ValidateToken(token);
            if (validate) return Ok(new {success = "Token is valid."});

            return BadRequest(new {errors = "Token has expired."});
        }
    }
}