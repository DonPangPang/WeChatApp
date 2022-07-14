using System;
using System.Collections.Generic;
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
        高层管理员 = 2,

        /// <summary>
        /// 中层管理员
        /// </summary>
        中层管理员 = 1,

        /// <summary>
        /// 普通成员
        /// </summary>
        普通成员 = 0,
    }
}