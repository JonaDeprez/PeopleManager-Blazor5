using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using PeopleManager.Dto.Requests;
using PeopleManager.Dto.Results;
using PeopleManager.RestApi.Settings;

namespace PeopleManager.RestApi.Authentication
{
    public class IdentityService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtSettings _jwtSettings;

        public IdentityService(
            UserManager<IdentityUser> userManager,
            JwtSettings jwtSettings)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings;
        }

        public async Task<AuthenticationResult> SignInAsync(SignInRequest request)
        {
            //Check user
            var user = await _userManager.FindByEmailAsync(request.Username);
            if (user is null)
            {
                return new AuthenticationResult
                {
                    Errors = new List<string>
                    {
                        "User/Pass combination is wrong."
                    }
                };
            }

            //Check password
            var isPasswordVerified = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!isPasswordVerified)
            {
                return new AuthenticationResult
                {
                    Errors = new List<string>
                    {
                        "User/Pass combination is wrong."
                    }
                };
            }

            //Generate Token
            return GenerateJwtToken(user);
        }

        public async Task<AuthenticationResult> RegisterAsync(RegisterRequest request)
        {
            //Check unique user
            var user = await _userManager.FindByEmailAsync(request.Username);
            if (user is not null)
            {
                return new AuthenticationResult
                {
                    Errors = new List<string>
                    {
                        "User already exists."
                    }
                };
            }

            //Create user & generate password
            var identityUser = new IdentityUser(request.Username);
            identityUser.Email = request.Username;
            var registerResult = await _userManager.CreateAsync(identityUser, request.Password);
            if (!registerResult.Succeeded)
            {
                return new AuthenticationResult
                {
                    Errors = registerResult.Errors.Select(e => e.Description).ToList()
                };
            }

            //Generate Token
            return GenerateJwtToken(identityUser);
        }

        public AuthenticationResult GenerateJwtToken(IdentityUser user)
        {
            if (string.IsNullOrWhiteSpace(user.Email))
            {
                return new AuthenticationResult { Errors = new List<string> { "Unable to generate token" } };
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Name, user.Email),
                    new Claim("id", user.Id)
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new AuthenticationResult
            {
                Token = tokenHandler.WriteToken(token)
            };
        }
    }
}
