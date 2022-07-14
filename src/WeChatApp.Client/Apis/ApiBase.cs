using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace WeChatApp.Client.Apis;

public static class ApiBase
{
    public static string BaseUrl { get; private set; } = null!;

    private static Dictionary<RequestApis, string> Dictionary = new Dictionary<RequestApis, string>()
    {
        [RequestApis.获取登陆人信息] = "api/User/GetUserInfo",
        [RequestApis.获取Token] = "api/Test/GetToken"
    };

    public static void AddApis(this WebAssemblyHostBuilder builder)
    {
        BaseUrl = builder.Configuration["BaseUrl"] ?? throw new ArgumentNullException("请配置基础基础Url");
    }

    public static string Get(this RequestApis apis)
    {
        Dictionary.TryGetValue(apis, out var result);
        return result ?? "";
    }
}

public enum RequestApis
{
    获取UserCode,
    获取登陆人信息,
    获取Token
}