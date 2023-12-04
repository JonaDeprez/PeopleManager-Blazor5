using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PeopleManager.Dto.Requests;
using PeopleManager.RestApi.Authentication;

namespace PeopleManager.RestApi.Controllers
{
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly IdentityService _identityService;

        public IdentityController(IdentityService identityService)
        {
            _identityService = identityService;
        }

        [HttpPost("api/identity/sign-in")]
        public async Task<IActionResult> SignIn([FromBody]SignInRequest request)
        {
            var result = await _identityService.SignInAsync(request);
            return Ok(result);
        }

        [HttpPost("api/identity/register")]
        public async Task<IActionResult> Register([FromBody]RegisterRequest request)
        {
            var result = await _identityService.RegisterAsync(request);
            return Ok(result);
        }
    }
}
