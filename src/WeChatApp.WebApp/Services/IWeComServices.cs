using WeChatApp.Shared.RequestBody.WeCom;
using WeChatApp.Shared.ResponseBody.WeCom;

namespace WeChatApp.WebApp.Services;

/// <summary>
/// 微信接口
/// </summary>
public interface IWeComServices
{
    /// <summary>
    /// 获取AccessToken
    /// </summary>
    /// <returns> </returns>
    Task<RespAccessTokenBody> GetAccessTokenAsync();

    /// <summary>
    /// 获取用户信息
    /// </summary>
    /// <param name="parameters"> </param>
    /// <returns> </returns>
    Task<RespUserInfoBody> GetUserInfoAsync(GetUserInfoRequestParameters parameters);
}