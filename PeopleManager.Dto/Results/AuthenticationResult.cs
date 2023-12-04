namespace PeopleManager.Dto.Results
{
    public class AuthenticationResult
    {
        public string? Token { get; set; }
        public IList<string> Errors { get; set; } = new List<string>();
    }
}
