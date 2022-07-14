using WeChatApp.Shared.GlobalVars;

namespace WeChatApp.WebApp.Extensions
{
    /// <summary>
    /// 跨域扩展
    /// </summary>
    public static class CorsExtensions
    {
        /// <summary>
        /// 添加扩展配置
        /// </summary>
        /// <param name="services"> </param>
        /// <returns> </returns>
        public static IServiceCollection AddCorsSetup(this IServiceCollection services)
        {
            services.AddCors(opt =>
            {
                opt.AddPolicy(GlobalVars.Cors, policyBuilder =>
                {
                    policyBuilder.AllowCredentials()
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                        .SetIsOriginAllowed(_ => true);
                });
            });

            return services;
        }
    }
}