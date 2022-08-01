using System.ComponentModel;

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
    Easy,
    /// <summary>
    /// 一般
    /// </summary>
    [Description("一般")]
    Normal,
    /// <summary>
    /// 困难
    /// </summary>
    [Description("困难")]
    Hard,
}