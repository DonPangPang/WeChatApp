using Microsoft.OpenApi.Models;
using System.Reflection;
using WeChatApp.WebApp.Filters;

namespace WeChatApp.WebApp.Extensions
{
    /// <summary>
    /// Swagger扩展
    /// </summary>
    public static class SwaggerExtensions
    {
        /// <summary>
        /// 添加swagger
        /// </summary>
        /// <param name="services"> </param>
        /// <returns> </returns>
        public static IServiceCollection AddSwaggerSetup(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Api Document", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Description = "在下框中输入请求头中需要添加Jwt授权Token：Bearer Token",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });

                c.DocumentFilter<SwaggerEnumFilter>();

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });

                var xmlFile = $"app-doc.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                var sharedXml = Path.Combine(AppContext.BaseDirectory, "shared-doc.xml");

                //... and tell Swagger to use those XML comments.
                c.IncludeXmlComments(xmlPath, true);
                c.IncludeXmlComments(sharedXml, true);
            });

            return services;
        }
    }
}