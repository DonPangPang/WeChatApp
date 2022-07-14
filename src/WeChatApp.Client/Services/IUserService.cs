using System.Net.Http.Json;
using WeChatApp.Client.Apis;
using WeChatApp.EzHttpClient;
using WeChatApp.Shared;
using WeChatApp.Shared.RequestBody.WeCom;

namespace WeChatApp.Client.Services;

public interface IUserService
{
    Task<WcResponse> GetUserInfoAsync(GetUserInfoRequestParameters parameters);
}

public class UserService : IUserService
{
    private readonly HttpClient _client;

    public UserService(HttpClient client)
    {
        _client = client;
    }

    public async Task<WcResponse> GetUserInfoAsync(GetUserInfoRequestParameters parameters)
    {
        var res = await _client.GetFromJsonAsync<WcResponse>($"{RequestApis.获取登陆人信息.Get()}?code={parameters.code}");

        return res;
    }
}