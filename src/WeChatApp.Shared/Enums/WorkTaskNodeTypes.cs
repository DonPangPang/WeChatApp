using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChatApp.Shared.Enums
{
    /// <summary>
    /// 节点的类型
    /// </summary>
    public enum WorkTaskNodeTypes
    {
        /// <summary>
        /// 无
        /// </summary>
        [Description("无")]
        None = 0,

        /// <summary>
        /// 发布
        /// </summary>
        [Description("发布")]
        Publish = 100,

        /// <summary>
        /// 审批
        /// </summary>
        [Description("审批")]
        Approval = 101,

        /// <summary>
        /// 接取
        /// </summary>
        [Description("接取")]
        PickUp = 200,

        /// <summary>
        /// 分配, 指定
        /// </summary>
        [Description("分配")]
        Assigned = 101,

        /// <summary>
        /// 汇报
        /// </summary>
        [Description("汇报")]
        Report = 201,

        /// <summary>
        /// 修改
        /// </summary>
        [Description("修改")]
        Modify = 103,

        /// <summary>
        /// 完成
        /// </summary>
        [Description("完成")]
        End = 209,

        /// <summary>
        /// 结束
        /// </summary>
        [Description("结束")]
        Close = 109
    }
}