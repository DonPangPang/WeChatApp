using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WeChatApp.Shared;
using WeChatApp.Shared.Extensions;
using WeChatApp.Shared.GlobalVars;
using WeChatApp.WebApp.Auth;

namespace WeChatApp.WebApp.Extensions
{
    /// <summary>
    /// 认证扩展
    /// </summary>
    public static class AuthExtensions
    {
        /// <summary>
        /// 添加认证
        /// </summary>
        /// <param name="services"> </param>
        /// <returns> </returns>
        public static IServiceCollection AddAuthSetup(this IServiceCollection services)
        {
            IConfiguration? configuration = services.BuildServiceProvider().GetService<IConfiguration>();

            var para = configuration!.GetSection("TokenParameter").Get<PermissionRequirement>();

            services.AddAuthorization(opts =>
            {
                opts.AddPolicy(GlobalVars.Permission, policy =>
                {
                    policy.Requirements.Add(new PermissionRequirement());
                });
            });

            services.AddAuthentication(opts =>
            {
                opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opts.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer("Bearer", options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidAudience = para.Audience,
                        ValidIssuer = para.Issuer,
                        IssuerSigningKey =
                            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(para.Secret))
                    };

                    options.SaveToken = true;

                    options.Events = new JwtBearerEvents
                    {
                        //此处为权限验证失败后触发的事件
                        OnChallenge = context =>
                        {
                            //此处代码为终止.Net Core默认的返回类型和数据结果，这个很重要哦，必须
                            context.HandleResponse();
                            //自定义自己想要返回的数据结果，我这里要返回的是Json对象，通过引用Newtonsoft.Json库进行转换
                            //自定义返回的数据类型
                            context.Response.ContentType = "application/json";
                            //自定义返回状态码，默认为401 我这里改成 200
                            context.Response.StatusCode = StatusCodes.Status200OK;
                            //context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            //输出Json数据结果
                            var result = new WcResponse
                            {
                                Code = WcStatus.TokenExpired,
                                Message = "Token校验失败"
                            }.ToJson();
                            context.Response.WriteAsync(result);
                            return Task.FromResult(0);
                        }
                    };
                }
            );

            return services;
        }
    }
}