using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChatApp.Shared.Options
{
    /// <summary>
    /// WebSocket服务器配置
    /// </summary>
    public class WsServerOptions : IOptions<WsServerOptions>
    {
        /// <summary>
        /// IP
        /// </summary>
        public string IP { get; set; } = string.Empty;

        /// <summary>
        /// 端口
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// </summary>
        public WsServerOptions Value => this;
    }
}