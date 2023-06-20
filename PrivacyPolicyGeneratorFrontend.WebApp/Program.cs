using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using PrivacyPolicyGeneratorFrontend.WebApp;
using PrivacyPolicyGeneratorFrontend.WebApp.Shared.Extensions;
using PrivacyPolicyGeneratorFrontend.WebService.Interfaces;
using PrivacyPolicyGeneratorFrontend.WebService.Services;
using Radzen;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
var _serverUrl = builder.Configuration["ServerUrl"];

builder.Services.AddLocalization();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<ITelebirrPaymentService, TelebirrPaymentService>();
 
builder.Services.AddScoped(sp => new HttpClient {
    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddSingleton(new HttpClient
{
    BaseAddress = new Uri(_serverUrl)
});

builder.Services.AddHttpClient<IClient, Client>(client => client.BaseAddress = new Uri(_serverUrl));


var host = builder.Build();
await host.SetDefaultCulture();

await host.RunAsync();  
