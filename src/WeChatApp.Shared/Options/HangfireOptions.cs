using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChatApp.Shared.Options
{
    /// <summary>
    /// Hangfire设置
    /// </summary>
    public class HangfireOptions : IOptions<HangfireOptions>
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; } = null!;

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; } = null!;

        /// <summary>
        /// </summary>
        public HangfireOptions Value => this;
    }
}