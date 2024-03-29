using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using WeChatApp.AdminClient;
using WeChatApp.AdminClient.Data;
using WeChatApp.AdminClient.Extensions;
using WeChatApp.Shared.GlobalVars;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddMasaBlazor();
// builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new
// Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddHttpClient(ApiVars.ApiBase, x =>
{
    //x.BaseAddress = new Uri("http://127.0.0.1:8014");
    x.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
});

builder.Services.AddSingleton<WeatherForecastService>();

builder.Services.AddClientServices();

await builder.Build().RunAsync();