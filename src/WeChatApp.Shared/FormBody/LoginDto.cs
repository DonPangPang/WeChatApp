using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeChatApp.Shared.Enums;

namespace WeChatApp.Shared.FormBody
{
    /// <summary>
    /// 登录
    /// </summary>
    public class LoginDto
    {
        /// <summary>
        /// 账号
        /// </summary>
        [Required(ErrorMessage = "账号不能为空")]
        public string UserName { get; set; } = null!;

        /// <summary>
        /// 密码
        /// </summary>
        [Required(ErrorMessage = "密码不能为空")]
        [MinLength(6, ErrorMessage = "最小6字符长度")]
        public string Password { get; set; } = null!;

        /// <summary>
        /// 访问端
        /// </summary>
        public ClientUA ClientUA { get; set; } = ClientUA.Mobile;
    }
}