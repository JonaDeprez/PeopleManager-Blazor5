using Vives.Authentication.Abstractions;

namespace PeopleManager.Ui.BlazorApp.Stores
{
    public class TokenStore:ITokenStore
    {
        public string? GetToken()
        {
            return string.Empty;
        }

        public void SaveToken(string token)
        {
            
        }

        public void ClearToken()
        {
            
        }
    }
}
