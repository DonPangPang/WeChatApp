using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeChatApp.Shared.Enums;
using WeChatApp.Shared.Interfaces;

namespace WeChatApp.Shared.Entity
{
    /// <summary>
    /// 工作任务节点
    /// </summary>
    public class WorkTaskNode : IEntity, ICreator
    {
        /// <summary>
        /// Id
        /// </summary>
        [Key]
        [Required]
        public Guid Id { get; set; }

        /// <summary>
        /// 工作任务的Id
        /// </summary>
        public Guid WorkTaskId { get; set; }

        /// <summary>
        /// 节点类型
        /// </summary>
        [Required]
        public WorkTaskNodeTypes Type { get; set; } = WorkTaskNodeTypes.None;

        /// <summary>
        /// 标题
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string? Content { get; set; }

        /// <summary>
        /// 图片
        /// </summary>
        /// <value></value>
        public string? ImageSources { get; set; }

        /// <summary>
        /// 子节点
        /// </summary>
        /// <value></value>
        public ICollection<WorkTaskNodeItem>? Items { get; set; }

        /// <summary>
        /// 节点时间
        /// </summary>
        /// <value></value>
        public DateTime? NodeTime { get; set; }

        /// <summary>
        /// </summary>
        public string? CreateUserUid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public Guid? CreateUserId { get; set; }

        /// <summary>
        /// </summary>
        public string? CreateUserName { get; set; }

        /// <summary>
        /// </summary>
        public DateTime CreateTime { get; set; } = DateTime.Now;
    }
}