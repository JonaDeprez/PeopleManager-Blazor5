using System.IdentityModel.Tokens.Jwt;
using System.Security;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using PeopleManager.Dto.Requests;
using PeopleManager.Sdk;
using PeopleManager.Ui.Mvc.Models;
using PeopleManager.Ui.Mvc.Stores;

namespace PeopleManager.Ui.Mvc.Controllers
{
    public class IdentityController : Controller
    {
        private readonly IdentityApiService _identityApiService;
        private readonly TokenStore _tokenStore;

        public IdentityController(
            IdentityApiService identityApiService,
            TokenStore tokenStore)
        {
            _identityApiService = identityApiService;
            _tokenStore = tokenStore;
        }

        [HttpGet]
        public async Task<IActionResult> SignIn(string returnUrl = "/")
        {
            ViewBag.ReturnUrl = returnUrl;

            await InternalSignOut();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn(SignInRequest request, string returnUrl = "/")
        {
            ViewBag.ReturnUrl = returnUrl;

            if (!ModelState.IsValid)
            {
                return View();
            }

            var result = await _identityApiService.SignInAsync(request);
            if (result.Errors.Any() || string.IsNullOrWhiteSpace(result.Token))
            {
                ModelState.AddModelError("", "Something went wrong");
                return View();
            }

            await InternalSignIn(result.Token);
            return LocalRedirect(returnUrl);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignOut(string returnUrl = "/")
        {
            await InternalSignOut();
            return LocalRedirect(returnUrl);
        }

        [HttpGet]
        public async Task<IActionResult> Register(string returnUrl = "/")
        {
            ViewBag.ReturnUrl = returnUrl;

            await InternalSignOut();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel viewModel, string returnUrl = "/")
        {
            ViewBag.ReturnUrl = returnUrl;

            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var registerRequest = new RegisterRequest
            {
                Username = viewModel.Username,
                Password = viewModel.Password
            };

            var result = await _identityApiService.RegisterAsync(registerRequest);
            if (result.Errors.Any() || string.IsNullOrWhiteSpace(result.Token))
            {
                ModelState.AddModelError("", "Something went wrong");
                return View(viewModel);
            }

            await InternalSignIn(result.Token);
            return LocalRedirect(returnUrl);
        }

        private async Task InternalSignOut()
        {
            await HttpContext.SignOutAsync();
            _tokenStore.ClearToken();
        }

        private async Task InternalSignIn(string token)
        {
            _tokenStore.SaveToken(token);
            var claimsPrincipal = GetClaimsPrincipal(token);
            await HttpContext.SignInAsync(claimsPrincipal);
        }

        private ClaimsPrincipal GetClaimsPrincipal(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            if (jwtToken is null)
            {
                throw new SecurityException("Token could not be converted");
            }

            var nameClaim = jwtToken.Claims.FirstOrDefault(c => c.Type.ToLower() == "name");
            var claims = jwtToken.Claims.ToList();

            if (nameClaim is not null)
            {
                claims.Add(new Claim(ClaimTypes.Name, nameClaim.Value));
            }

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            
            return new ClaimsPrincipal(claimsIdentity);
        }
    }
}
