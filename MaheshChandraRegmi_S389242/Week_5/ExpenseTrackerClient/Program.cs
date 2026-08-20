using ExpenseTrackerClient;
using ExpenseTrackerClient.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddSingleton<AuthState>();
builder.Services.AddScoped<ExpenseApi>();

// week 4 API — run ExpenseTrackerAuth first
builder.Services.AddScoped(_ => new HttpClient
{
    BaseAddress = new Uri("http://localhost:5041/")
});

await builder.Build().RunAsync();
