using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using PeopleManager.Sdk.Handlers;
using Vives.Authentication.Abstractions;

namespace PeopleManager.Sdk.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApi(this IServiceCollection services, string apiUrl)
        {
            services.AddScoped<AuthorizationHttpHandler>();
            services.AddHttpClient("PeopleManagerApi", httpClient =>
            {
                httpClient.BaseAddress = new Uri(apiUrl);
            }).AddHttpMessageHandler<AuthorizationHttpHandler>();

            services.AddScoped<PersonApiService>();
            services.AddScoped<VehicleApiService>();
            services.AddScoped<IdentityApiService>();

            return services;
        }
    }
}
