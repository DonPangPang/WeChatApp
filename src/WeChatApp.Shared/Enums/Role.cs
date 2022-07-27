using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChatApp.Shared.Enums
{
    /// <summary>
    /// 角色
    /// </summary>
    public enum Role
    {
        /// <summary>
        /// 高层管理员
        /// </summary>
        [Description("高层管理员")]
        高层管理员 = 2,

        /// <summary>
        /// 中层管理员
        /// </summary>
        [Description("中层管理员")]
        中层管理员 = 1,

        /// <summary>
        /// 普通成员
        /// </summary>
        [Description("普通成员")]
        普通成员 = 0,
    }
}