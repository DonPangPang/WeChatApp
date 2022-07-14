using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using WeChatApp.AdminClient.Auth;
using WeChatApp.AdminClient.Services;
using WeChatApp.Shared.FormBody;

namespace WeChatApp.AdminClient.Extensions
{
    public static class ClientServicesExtenions
    {
        public static IServiceCollection AddClientServices(this IServiceCollection services)
        {
            services.AddSingleton<IHttpFunc, HttpFunc>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<IWorkTaskService, WorkTaskService>();

            services.AddSingleton<Session>();


            services.AddBlazoredLocalStorage();
            services.AddAuthorizationCore();
            services.AddScoped<JwtAuthProvider>();
            services.AddScoped<AuthenticationStateProvider, JwtAuthProvider>(
                provider => provider.GetRequiredService<JwtAuthProvider>()
            );
            services.AddScoped<ILoginService, JwtAuthProvider>(provider => provider.GetRequiredService<JwtAuthProvider>());
            return services;
        }
    }

    /// <summary>
    /// 用户会话
    /// </summary>
    public class Session
    {
        /// <summary>
        /// Uid
        /// </summary>
        public string? Uid { get; set; }

        /// <summary>
        /// 用户信息
        /// </summary>
        public UserDto? UserInfo { get; set; }
    }
}