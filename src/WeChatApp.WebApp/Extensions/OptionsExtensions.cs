using WeChatApp.Shared.Options;
using WeChatApp.WebApp.Auth;

namespace WeChatApp.WebApp.Extensions;

/// <summary>
/// </summary>
public static class OptionsExtensions
{
    /// <summary>
    /// 注入设置
    /// </summary>
    /// <param name="services"> </param>
    /// <returns> </returns>
    public static IServiceCollection AddPrivateOptions(this IServiceCollection services)
    {
        IConfiguration? configuration = services.BuildServiceProvider().GetService<IConfiguration>();

        services.Configure<WeChatOptions>(configuration?.GetSection("WeChatOptions"));
        services.Configure<TokenParameter>(configuration?.GetSection("TokenParameter"));
        services.Configure<WsServerOptions>(configuration?.GetSection("WsServer"));
        return services;
    }
}