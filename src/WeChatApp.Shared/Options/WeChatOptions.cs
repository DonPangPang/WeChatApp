using Microsoft.Extensions.Options;

namespace WeChatApp.Shared.Options;

/// <summary>
/// 微信基础设置
/// </summary>
public class WeChatOptions:IOptions<WeChatOptions>
{
    /// <summary>
    /// 企业ID
    /// </summary>
    public string CorpId { get; set; } = null!;

    /// <summary>
    /// 应用ID
    /// </summary>
    public string AgentId { get; set; } = null!;
    
    /// <summary>
    /// 密钥
    /// </summary>
    public string Secret { get; set; } = null!;
    
    public WeChatOptions Value => this;
}