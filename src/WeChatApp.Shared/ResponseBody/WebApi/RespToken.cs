using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeChatApp.Shared.FormBody;

namespace WeChatApp.Shared.ResponseBody.WebApi
{
    /// <summary>
    /// Token返回
    /// </summary>
    public class RespToken
    {
        /// <summary>
        /// </summary>
        public string Token { get; set; } = string.Empty;

        /// <summary>
        /// </summary>
        public string RefreshToken { get; set; } = string.Empty;

        /// <summary>
        /// </summary>
        public UserDto User { get; set; }
    }
}