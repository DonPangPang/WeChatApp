using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeChatApp.Shared.Entity;

namespace WeChatApp.Shared.FormBody
{
    /// <summary>
    /// 工作节点子项
    /// </summary>
    [AutoMap(typeof(WorkTaskNodeItem), ReverseMap = true)]
    public class WorkTaskNodeItemDto : IDtoBase
    {
        /// <summary>
        /// Id
        /// </summary>
        /// <value> </value>
        public Guid Id { get; set; }

        /// <summary>
        /// 任务Id
        /// </summary>
        /// <value> </value>
        public Guid WorkTaskId { get; set; }

        /// <summary>
        /// 工作任务节点Id
        /// </summary>
        /// <value> </value>
        public Guid WorkTaskNodeId { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        /// <value> </value>
        public string? Title { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        /// <value> </value>
        public string? Content { get; set; }

        /// <summary>
        /// 图片路径
        /// </summary>
        /// <value> </value>
        public string? ImageSources { get; set; }

        /// <summary>
        /// </summary>
        /// <value> </value>
        public string? CreateUserUid { get; set; }

        /// <summary>
        /// </summary>
        /// <value> </value>
        public Guid? CreateUserId { get; set; }

        /// <summary>
        /// </summary>
        /// <value> </value>
        public string? CreateUserName { get; set; }

        /// <summary>
        /// </summary>
        /// <value> </value>
        public DateTime CreateTime { get; set; }
    }
}