using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using DevixonApi.Data.Interfaces;
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

        public UserController(IUserService userService)
        {
            _userService = userService;
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

            var loginResponse = await _userService.Login(loginRequest);

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
                var registerResponse = await _userService.Register(registerRequest);
                return Ok(registerResponse);
            }

            return BadRequest();
        }

        [HttpGet]
        [Route("details")]
        public async Task<IActionResult> Details()
        {
            
            //
            // var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:5001/api/user/details");
            // request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            // HttpResponseMessage response = await HttpClient.;
            //
            // if (response.StatusCode != HttpStatusCode.OK)
            // {
            //     return Content(response.ToString());
            // }

            return Content($"test");
        }
    }
}