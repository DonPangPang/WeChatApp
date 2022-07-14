namespace WeChatApp.Shared.RequestBody.WeCom;

/// <summary>
/// 
/// </summary>
public class GetUserInfoRequestParameters : IRequestParameter
{
    /// <summary>
    /// 
    /// </summary>
    /// <value></value>
    public string code { get; set; } = string.Empty;
}