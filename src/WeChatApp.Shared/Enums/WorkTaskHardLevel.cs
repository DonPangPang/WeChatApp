using System.ComponentModel;
using WeChatApp.Shared.Attributes;

namespace WeChatApp.Shared.Enums;

/// <summary>
/// 工作任务难易度
/// </summary>
public enum WorkTaskHardLevel
{
    /// <summary>
    /// 简单
    /// </summary>
    [Description("简单")]
    [ShowColor(Color = "#689F38")]
    Easy,
    /// <summary>
    /// 一般
    /// </summary>
    [Description("一般")]
    [ShowColor(Color = "#FFEE58")]
    Normal,
    /// <summary>
    /// 困难
    /// </summary>
    [Description("困难")]
    [ShowColor(Color = "#F4511E")]
    Hard,
}