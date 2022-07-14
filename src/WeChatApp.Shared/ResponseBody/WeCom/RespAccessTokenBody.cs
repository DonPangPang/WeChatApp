using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChatApp.Shared.ResponseBody.WeCom
{
    /// <summary>
    /// </summary>
    public class RespAccessTokenBody : RespBodyBase
    {
        /// <summary>
        /// access_token
        /// </summary>
        public string access_token { get; set; } = null!;

        /// <summary>
        /// 过期时间
        /// </summary>
        public int expires_in { get; set; }
    }
}