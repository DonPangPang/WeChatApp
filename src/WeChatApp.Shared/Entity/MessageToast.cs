using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeChatApp.Shared.Enums;
using WeChatApp.Shared.Interfaces;

namespace WeChatApp.Shared.Entity
{
    /// <summary>
    /// 消息
    /// </summary>
    public class MessageToast : IEntity
    {
        /// <summary>
        /// </summary>
        public Guid Id { get; set; }

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

        /// <summary>
        /// 是否已经发送
        /// </summary>
        public bool IsPush { get; set; } = false;

        /// <summary>
        /// 用户Id
        /// </summary>
        public Guid UserId { get; set; }
    }
}