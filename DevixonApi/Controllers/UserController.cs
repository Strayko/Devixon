using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using DevixonApi.Data.Interfaces;
using DevixonApi.Data.Models;
using DevixonApi.Data.Requests;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
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
            if (loginRequest == null || string.IsNullOrEmpty(loginRequest.Email) || string.IsNullOrEmpty(loginRequest.Password))
            {
                return BadRequest("Missing login details");
            }

            var loginResponse = await _userService.Authenticate(loginRequest);

            if (loginResponse == null)
            {
                return BadRequest("Invalid credentials");
            }

            return Ok(loginResponse);
        }
        
        [HttpPost]
        [AllowAnonymous]
        [Route("register")]
        public async Task<IActionResult> Register(RegisterRequest registerRequest)
        {
            if (ModelState.IsValid)
            {
                var registerResponse = await _userService.Registration(registerRequest);
                return Ok(registerResponse);
            }

            return BadRequest();
        }

        [HttpGet]
        [Route("details")]
        public async Task<ActionResult<UserModel>> Details()
        {

            var userId = HttpContext.User.Claims.First().Value;
            if (userId == null)
                return BadRequest("User ID not found or token expiry.");
            
            var user = await _userService.GetUserAsync(Int32.Parse(userId));

            if (user == null)
                return NotFound();

            return _mapper.Map<UserModel>(user);
        }
    }
}