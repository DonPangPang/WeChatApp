using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChatApp.Shared.FormBody
{
    /// <summary>
    /// 用户更改密码
    /// </summary>
    public class UserChangePasswordDto
    {
        /// <summary>
        /// 旧密码
        /// </summary>
        public string? OldPassword { get; set; } = string.Empty;

        /// <summary>
        /// 新密码
        /// </summary>
        public string? NewPassword { get; set; } = string.Empty;
    }
}