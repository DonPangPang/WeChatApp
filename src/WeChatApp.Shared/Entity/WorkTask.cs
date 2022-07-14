using System.Security.Cryptography.X509Certificates;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeChatApp.Shared.Enums;
using WeChatApp.Shared.Interfaces;

namespace WeChatApp.Shared.Entity
{
    /// <summary>
    /// 工作任务
    /// </summary>
    public class WorkTask : IEntity, ICreator, IModifyed, IPublic
    {
        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        [Key]
        [Required]
        public Guid Id { get; set; }

        /// <summary>
        /// 部门Id
        /// </summary>
        /// <value></value>
        [Required]
        public Guid DepartmentId { get; set; }

        /// <summary>
        /// 工作标题
        /// </summary>
        [Required]
        public string Title { get; set; } = null!;

        /// <summary>
        /// 工作内容
        /// </summary>
        [Required]
        public string Content { get; set; } = null!;

        /// <summary>
        /// 任务发布类型
        /// </summary>
        /// <value></value>
        public WorkPublishType WorkPublishType { get; set; } = WorkPublishType.科室发布;

        /// <summary>
        /// 最高抢单人数
        /// </summary>
        /// <value></value>
        [Required]
        public int MaxPickUpCount { get; set; } = 0;

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 积分奖励
        /// </summary>
        [Column(TypeName = "decimal(10,2)")]
        public decimal PointsRewards { get; set; } = 0;

        /// <summary>
        /// 积分结算
        /// </summary>
        [Column(TypeName = "decimal(10,2)")]
        public decimal PointsSettlement { get; set; } = 0;

        // /// <summary>
        // /// 接取人Uid
        // /// </summary>
        // public string? PickUpUserUid { get; set; }

        // /// <summary>
        // /// 接取人Id
        // /// </summary>
        // /// <value></value>
        // public Guid? PickUpUserId { get; set; }

        /// <summary>
        /// 接取人Id
        /// </summary>
        /// <value></value>
        public string? PickUpUserIds { get; set; }

        /// <summary>
        /// 接取人姓名
        /// </summary>
        public string? PickUpUserNames { get; set; }

        /// <summary>
        /// 工作任务接取类型
        /// </summary>
        public WorkTaskTypes Type { get; set; } = WorkTaskTypes.PickUp;

        /// <summary>
        /// 子节点
        /// </summary>
        public ICollection<WorkTaskNode>? Nodes { get; set; } = new List<WorkTaskNode>();

        /// <summary>
        /// 工作任务的状态
        /// </summary>
        public WorkTaskStatus Status { get; set; } = WorkTaskStatus.None;

        /// <summary>
        /// 乐观锁
        /// </summary>
        [Timestamp]
        public byte[] Timespan { get; set; } = null!;

        /// <summary>
        /// </summary>
        public string? CreateUserUid { get; set; }

        /// <summary>
        /// </summary>
        public string? CreateUserName { get; set; }

        /// <summary>
        /// </summary>
        public DateTime CreateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// </summary>
        public bool IsPublicNodes { get; set; } = false;

        /// <summary>
        /// </summary>
        public DateTime PublicStartTime { get; set; }

        /// <summary>
        /// </summary>
        public DateTime PublicEndTime { get; set; }

        /// <summary>
        /// </summary>
        public string? ModifyUserUid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public Guid? CreateUserId { get; set; }

        /// <summary>
        /// 修改人Id
        /// </summary>
        /// <value></value>
        public Guid? ModifyUserId { get; set; }

        /// <summary>
        /// </summary>
        public string? ModifyUserName { get; set; }

        /// <summary>
        /// </summary>
        public DateTime ModifyTime { get; set; }
    }
}