using System.ComponentModel;
using WeChatApp.Shared.Attributes;

/// <summary>
/// 性别
/// </summary>
public enum Gender
{
    /// <summary>
    /// 
    /// </summary>
    [ShowColor(Color = "#EC407A")]
    [Description("女")]
    女 = 0,
    /// <summary>
    /// 
    /// </summary>
    [ShowColor(Color = "#42A5F5")]
    [Description("男")]
    男 = 1
}