using System.ComponentModel;

namespace WeChatApp.Shared.Enums
{
    /// <summary>
    /// 工作发布类型
    /// </summary>
    public enum WorkPublishType
    {
        /// <summary>
        /// 全局发布
        /// </summary>
        [Description("全局发布")]
        全局发布 = 0,
        /// <summary>
        /// 科室发布
        /// </summary>
        [Description("科室发布")]
        科室发布 = 1,
        /// <summary>
        /// 自定义发布
        /// </summary>
        [Description("自定义发布")]
        自定义发布 = 2,
    }
}