using AIInterview.Blazor;
using AIInterview.Blazor.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("https://localhost:7296/")
});

builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<ApiService>();
await builder.Build().RunAsync();