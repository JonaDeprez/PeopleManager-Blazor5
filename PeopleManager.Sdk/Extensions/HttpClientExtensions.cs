using System.Net.Http.Headers;

namespace PeopleManager.Sdk.Extensions
{
    public static class HttpClientExtensions
    {
        public static HttpClient AddAuthorization(this HttpClient client, string? token)
        {
            client.DefaultRequestHeaders.AddAuthorization(token);
            return client;
        }

        public static HttpRequestMessage AddAuthorization(this HttpRequestMessage request, string? token)
        {
            request.Headers.AddAuthorization(token);
            return request;
        }

        public static HttpRequestHeaders AddAuthorization(this HttpRequestHeaders headers, string? token)
        {
            var headerName = "Authorization";
            if (headers.Contains(headerName))
            {
                headers.Remove(headerName);
            }

            if (!string.IsNullOrWhiteSpace(token))
            {
                headers.Add(headerName, $"Bearer {token}");
            }

            return headers;
        }
    }
}
