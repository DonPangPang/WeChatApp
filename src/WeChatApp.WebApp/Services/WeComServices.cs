using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using WeChatApp.EzHttpClient;
using WeChatApp.Shared.GlobalVars;
using WeChatApp.Shared.Options;
using WeChatApp.Shared.RequestBody.WeCom;
using WeChatApp.Shared.ResponseBody.WeCom;
using WeChatApp.WebApp.Extensions;

namespace WeChatApp.WebApp.Services;

/// <summary>
/// </summary>
public class WeComServices : IWeComServices
{
    private readonly IMemoryCache _cache;
    private readonly WeChatOptions _weChatOptions;

    /// <summary>
    /// </summary>
    /// <param name="weChatOptions"> </param>
    /// <param name="cache">         </param>
    public WeComServices(
        IOptions<WeChatOptions> weChatOptions,
        IMemoryCache cache)
    {
        _cache = cache;
        _weChatOptions = weChatOptions.Value;
    }

    /// <summary>
    /// </summary>
    /// <returns> </returns>
    public async Task<RespAccessTokenBody> GetAccessTokenAsync()
    {
        var res = await HttpClientWrapper
            .Create()
            .Url($"https://qyapi.weixin.qq.com/cgi-bin/gettoken?corpid={_weChatOptions.CorpId}&corpsecret={_weChatOptions.Secret}")
            .GetAsync<RespAccessTokenBody>();

        if (res.errcode == 0) _cache.Set(GlobalVars.AccessTokenKey, res.access_token, TimeSpan.FromSeconds(res.expires_in));

        return res;
    }

    /// <summary>
    /// </summary>
    /// <param name="parameters"> </param>
    /// <returns> </returns>
    /// <exception cref="Exception"> </exception>
    public async Task<RespUserInfoBody> GetUserInfoAsync(GetUserInfoRequestParameters parameters)
    {
        if (_cache.TryGetValue(GlobalVars.AccessTokenKey, out string access_token))
        {
            var user = await HttpClientWrapper
                .Create()
                .Url($"https://qyapi.weixin.qq.com/cgi-bin/user/getuserinfo?access_token={access_token}&code={parameters.code}&debug=1")
                .GetAsync<RespUserBody>();

            var res = await HttpClientWrapper
                .Create()
                .Url($"https://qyapi.weixin.qq.com/cgi-bin/user/get?access_token={access_token}&userid={user.UserId}")
                .GetAsync<RespUserInfoBody>();

            return res;
        }
        else
        {
            throw new Exception("AccessToken获取失败");
        }
    }
}