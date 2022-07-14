using Newtonsoft.Json.Serialization;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Pang.AutoMapperMiddleware;
using WeChatApp.Shared.Extensions;
using WeChatApp.Shared.GlobalVars;
using WeChatApp.WebApp.Auth;
using WeChatApp.WebApp.Extensions;
using WeChatApp.WebApp.Filters;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(opts =>
{
    opts.Filters.Add<AsyncAccessTokenFilter>();
    opts.Filters.Add<AsyncExceptionFilter>();
})
.AddNewtonsoftJson(setup =>
{
    setup.SerializerSettings.ContractResolver
        = new CamelCasePropertyNamesContractResolver();
    setup.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Serialize;
})
/*添加XML*/.AddXmlDataContractSerializerFormatters()
.ConfigureApiBehaviorOptions(setup =>
{
    setup.InvalidModelStateResponseFactory = context =>
    {
        var problemDetails = new ValidationProblemDetails(context.ModelState)
        {
            Type = "http://www.baidu.com",
            Title = "有错误",
            Status = StatusCodes.Status422UnprocessableEntity,
            Detail = "请看详细信息",
            Instance = context.HttpContext.Request.Path
        };

        problemDetails.Extensions.Add("traceId", context.HttpContext.TraceIdentifier);

        return new UnprocessableEntityObjectResult(problemDetails)
        {
            ContentTypes = { "application/problem+json" }
        };
    };
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// 添加设置
builder.Services.AddSwaggerSetup();
builder.Services.AddPrivateOptions();
builder.Services.AddEzHttpClient();
builder.Services.AddPrivateServices();
builder.Services.AddMemoryCache();
builder.Services.AddAuthSetup();
builder.Services.AddCorsSetup();
builder.Services.AddAutoMapper();
builder.Services.AddSqlServer();
builder.Services.AddHangfireSupport();

builder.WebHost.UseUrls("http://*:10500/");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}

app.UseStaticFiles();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHangfire();
app.UseAutoMapperMiddleware();
//app.UseHttpsRedirection();
app.UseCors(GlobalVars.Cors);
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();