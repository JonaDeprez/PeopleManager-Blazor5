using PeopleManager.Sdk.Extensions;
using Vives.Authentication.Abstractions;

namespace PeopleManager.Sdk.Handlers
{
    public class AuthorizationHttpHandler(ITokenStore tokenStore) : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = tokenStore.GetToken();

            request.AddAuthorization(token);

            return base.SendAsync(request, cancellationToken);
        }
    }
}
