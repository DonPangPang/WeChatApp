using Microsoft.AspNetCore.Authorization;
using WeChatApp.Shared.Options;
using WeChatApp.WebApp.Auth;
using WeChatApp.WebApp.Filters;
using WeChatApp.WebApp.Services;
using WeChatApp.WebApp.WebSocket;

namespace WeChatApp.WebApp.Extensions;

/// <summary>
/// </summary>
public static class ServicesExtensions
{
    /// <summary>
    /// 注入服务
    /// </summary>
    /// <param name="services"> </param>
    /// <returns> </returns>
    public static IServiceCollection AddPrivateServices(this IServiceCollection services)
    {
        services.AddScoped<IWeComServices, WeComServices>();
        services.AddScoped<IServiceGen, ServiceGen>();
        services.AddScoped<Session>();

        services.AddScoped<IAuthorizationHandler, PermissionHandler>();
        services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();

        services.AddHostedService<WsMessageService>();
        services.AddSingleton<IWsSessionManager, WsSessionManager>();

        services.AddScoped<IMessageToastService, MessageToastService>();

        return services;
    }
}