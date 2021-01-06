using System.Threading.Tasks;
using DevixonApi.Data.Interfaces;
using DevixonApi.Data.Requests;
using DevixonApi.Data.Services;
using Microsoft.AspNetCore.Mvc;

namespace DevixonApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FacebookController : ControllerBase
    {
        private readonly IAccountFacebookService _accountFacebookService;


        public FacebookController(IAccountFacebookService accountFacebookService)
        {
            _accountFacebookService = accountFacebookService;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> FacebookLoginAsync([FromBody] FacebookLoginRequest facebookLoginRequest)
        {
            var authorizationTokens = await _accountFacebookService.FacebookLoginAsync(facebookLoginRequest);
            if (authorizationTokens == null)
            {
                return BadRequest(new {errors = "Invalid token!"});
            }

            return Ok(authorizationTokens);
        }
    }
}