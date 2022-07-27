using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChatApp.Shared.Enums
{
    /// <summary>
    /// 工作状态
    /// </summary>
    public enum WorkTaskStatus
    {
        /// <summary>
        /// 无状态
        /// </summary>
        [Description("无状态")]
        None = 0,

        /// <summary>
        /// 待审核
        /// </summary>
        [Description("待审核")]
        PendingReview = 11,

        /// <summary>
        /// 待发布
        /// </summary>
        [Description("待发布")]
        PendingPublish = 12,

        /// <summary>
        /// 指派任务
        /// </summary>
        [Description("指派任务")]
        Assign = 122,

        /// <summary>
        /// 驳回
        /// </summary>
        [Description("驳回")]
        Overrule = 13,

        /// <summary>
        /// 发布
        /// </summary>
        [Description("发布")]
        Publish = 21,

        /// <summary>
        /// 进行中
        /// </summary>
        [Description("进行中")]
        Active = 22,

        /// <summary>
        /// 已完成
        /// </summary>
        [Description("已完成")]
        Finished = 23,

        /// <summary>
        /// 结束
        /// </summary>
        [Description("结束")]
        End = 23,

        /// <summary>
        /// 评分
        /// </summary>
        [Description("评分")]
        Grade = 24,
    }
}