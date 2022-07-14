using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeChatApp.Shared.Entity;

namespace WeChatApp.Shared.Temp
{
    /// <summary>
    /// </summary>
    public class MessageToastTemp
    {
        /// <summary>
        /// 发送给用户的Id
        /// </summary>
        public IEnumerable<Guid>? UserIds { get; set; }

        /// <summary>
        /// 发送给用户的消息
        /// </summary>
        public MessageToast? MessageToast { get; set; }
    }
}