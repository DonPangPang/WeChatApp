using WeChatApp.Shared.RequestBody;

namespace WeChatApp.WebApp.Extensions;

/// <summary>
/// </summary>
public static class HttpClientExtensions
{
    /// <summary>
    /// 添加HttpClient的支持
    /// </summary>
    /// <param name="services"> </param>
    /// <returns> </returns>
    public static IServiceCollection AddEzHttpClient(this IServiceCollection services)
    {
        services.AddHttpClient();
        //services.AddScoped<IHttpClientFactory>();

        return services;
    }
}