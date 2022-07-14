using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChatApp.Shared.Enums
{
    /// <summary>
    /// </summary>
    public enum MsgToastType
    {
        /// <summary>
        /// 系统消息
        /// </summary>
        [Description("系统消息")]
        System = 0,

        /// <summary>
        /// 通知
        /// </summary>
        [Description("通知")]
        Notice,

        /// <summary>
        /// 推送
        /// </summary>
        [Description("推送")]
        Push,
    }
}