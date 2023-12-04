using Vives.Authentication.Abstractions;

namespace PeopleManager.Ui.Mvc.Stores
{
    public class TokenStore: ITokenStore
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private const string JwtTokenName = "JwtToken";

        public TokenStore(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }
        public string? GetToken()
        {
            string? jwtToken = null;
            _contextAccessor.HttpContext?.Request.Cookies.TryGetValue(JwtTokenName, out jwtToken);
            return jwtToken;
        }

        public void SaveToken(string token)
        {
            ClearToken();
            _contextAccessor.HttpContext?.Response.Cookies.Append(JwtTokenName, token, new CookieOptions{HttpOnly = true});
        }

        public void ClearToken()
        {
            _contextAccessor.HttpContext?.Response.Cookies.Delete(JwtTokenName);
        }
    }
}
