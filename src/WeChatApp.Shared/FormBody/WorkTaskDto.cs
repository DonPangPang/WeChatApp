using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeChatApp.Shared.Entity;
using WeChatApp.Shared.Enums;
using WeChatApp.Shared.Temp;
using WeChatApp.Shared.Extensions;

namespace WeChatApp.Shared.FormBody
{
    /// <summary>
    /// 工作任务
    /// </summary>
    [AutoMap(typeof(WorkTask), ReverseMap = true)]
    public class WorkTaskDto : IDtoBase
    {
        /// <summary>
        /// </summary>
        /// <value> </value>
        public Guid Id { get; set; }

        /// <summary>
        /// 部门Id
        /// </summary>
        /// <value> </value>
        public Guid DepartmentId { get; set; }

        /// <summary>
        /// 工作标题
        /// </summary>
        public string Title { get; set; } = null!;

        /// <summary>
        /// 工作内容
        /// </summary>
        [Required]
        public string Content { get; set; } = null!;

        /// <summary>
        /// 最高抢单人数
        /// </summary>
        /// <value> </value>
        public int MaxPickUpCount { get; set; } = 0;

        /// <summary>
        /// 任务发布类型
        /// </summary>
        /// <value> </value>
        public WorkPublishType WorkPublishType { get; set; } = WorkPublishType.科室发布;

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
        public decimal PointsRewards { get; set; } = 0;

        /// <summary>
        /// 积分结算
        /// </summary>
        public decimal PointsSettlement { get; set; } = 0;

        /// <summary>
        /// 接取人Id
        /// </summary>
        /// <value> </value>
        public string? PickUpUserIds { get; set; }

        /// <summary>
        /// 接取人姓名
        /// </summary>
        public string? PickUpUserNames { get; set; }

        /// <summary>
        /// 接取人列表
        /// </summary>
        /// <value> </value>
        public ICollection<UserItem>? PockUpUsers { get; set; }

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
        /// </summary>
        /// <value> </value>
        public Guid? CreateUserId { get; set; }

        /// <summary>
        /// 修改人Id
        /// </summary>
        /// <value> </value>
        public Guid? ModifyUserId { get; set; }

        /// <summary>
        /// </summary>
        public string? ModifyUserName { get; set; }

        /// <summary>
        /// </summary>
        public DateTime ModifyTime { get; set; }

        /// <summary>
        /// 接取人数
        /// </summary>
        public int PickCount => PickUpUserIds.IsEmpty() ? 0 : PickUpUserIds!.Split(",").Length;
    }
}