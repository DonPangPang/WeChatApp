using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeChatApp.Shared.Entity;
using WeChatApp.Shared.Enums;

namespace WeChatApp.Shared.FormBody
{
    /// <summary>
    /// </summary>
    [AutoMap(typeof(MessageToast), ReverseMap = true)]
    public class MessageToastDto
    {
        /// <summary>
        /// 消息类型
        /// </summary>
        public MsgToastType Type { get; set; } = MsgToastType.System;

        /// <summary>
        /// 标题
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// 正文
        /// </summary>
        public string? Content { get; set; }
    }
}