using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChatApp.Shared.Enums
{
    /// <summary>
    /// 
    /// </summary>
    public enum WorkTaskTypes
    {
        /// <summary>
        /// 无
        /// </summary>
        [Description("无")]
        None = 0,

        /// <summary>
        /// 接取
        /// </summary>
        [Description("接取")]
        PickUp = 1,

        /// <summary>
        /// 指定
        /// </summary>
        [Description("指定")]
        Assigned = 2
    }
}