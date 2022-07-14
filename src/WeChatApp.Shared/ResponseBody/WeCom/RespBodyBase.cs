namespace WeChatApp.Shared.ResponseBody.WeCom;

/// <summary>
/// 企业微信Api
/// </summary>
public class RespBodyBase
{
    /// <summary>
    /// 状态码
    /// </summary>
    public int errcode { get; set; }

    /// <summary>
    /// 错误信息
    /// </summary>
    public string errmsg { get; set; } = string.Empty;
}