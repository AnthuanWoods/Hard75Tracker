using Hard75Tracker;
using Hard75Tracker.Models;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7229/") });
builder.Services.AddScoped<SessionStorageAccessor>();
builder.Services.AddScoped<IUserHttpRepository, UserHttpRepository>();
builder.Services.AddScoped<ICategoryHttpRepository, CategoryHttpRepository>();
builder.Services.AddScoped<ITaskHttpRepository, TaskHttpRepository>();
//builder.Services.AddBootstrapBlazor();
builder.Services.AddBlazorBootstrap();
await builder.Build().RunAsync();
