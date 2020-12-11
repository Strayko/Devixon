using System.Threading.Tasks;
using DevixonApi.Data.Interfaces;
using DevixonApi.Data.Requests;
using Microsoft.AspNetCore.Mvc;

namespace DevixonApi.Controllers
{
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
    }
}