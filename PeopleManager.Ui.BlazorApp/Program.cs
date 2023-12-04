using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using PeopleManager.Sdk.Extensions;
using PeopleManager.Ui.BlazorApp;
using PeopleManager.Ui.BlazorApp.Stores;
using Vives.Authentication.Abstractions;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddScoped<ITokenStore, TokenStore>();
builder.Services.AddApi("https://localhost:7134");

await builder.Build().RunAsync();
