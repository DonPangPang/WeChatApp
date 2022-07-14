using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using WeChatApp.Client;
using WeChatApp.Client.Apis;
using WeChatApp.Client.Data;
using WeChatApp.Client.Services;
using WeChatApp.Shared.Options;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddMasaBlazor();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.Configuration["BaseUrl"]!.ToString()) });
//builder.Services.AddScoped<IHttpClientFactory>();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddSingleton<IMemoryDataService, MemoryDataService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.AddApis();

await builder.Build().RunAsync();