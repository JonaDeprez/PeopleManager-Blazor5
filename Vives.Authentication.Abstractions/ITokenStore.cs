namespace Vives.Authentication.Abstractions
{
    public interface ITokenStore
    {
        string? GetToken();
        void SaveToken(string token);
        void ClearToken();
    }
}
