using System.Net.Http.Json;
using PeopleManager.Dto.Requests;
using PeopleManager.Dto.Results;

namespace PeopleManager.Sdk
{
    public class IdentityApiService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public IdentityApiService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<AuthenticationResult> SignInAsync(SignInRequest request)
        {
            var httpClient = _httpClientFactory.CreateClient("PeopleManagerApi");
            var route = "/api/identity/sign-in";
            var httpResponse = await httpClient.PostAsJsonAsync(route, request);

            httpResponse.EnsureSuccessStatusCode();

            var result = await httpResponse.Content.ReadFromJsonAsync<AuthenticationResult>();

            if (result is null)
            {
                return new AuthenticationResult { Errors = { "Something went wrong." } };
            }

            return result;
        }

        public async Task<AuthenticationResult> RegisterAsync(RegisterRequest request)
        {
            var httpClient = _httpClientFactory.CreateClient("PeopleManagerApi");
            var route = "/api/identity/register";
            var httpResponse = await httpClient.PostAsJsonAsync(route, request);

            httpResponse.EnsureSuccessStatusCode();

            var result = await httpResponse.Content.ReadFromJsonAsync<AuthenticationResult>();
            if (result is null)
            {
                return new AuthenticationResult { Errors = { "Something went wrong." } };
            }

            return result;
        }

    }
}
