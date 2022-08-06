using AutoMapper;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeChatApp.Shared.Entity;

namespace WeChatApp.Shared.FormBody
{
    /// <summary>
    /// 积分记录
    /// </summary>
    [AutoMap(typeof(BonusPointRecord), ReverseMap = true)]
    public class BonusPointRecordDto : IDtoBase
    {
        /// <summary>
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 积分
        /// </summary>
        public decimal BonusPoints { get; set; } = 0;

        /// <summary>
        /// 积分认领人
        /// </summary>
        public Guid PickUpUserId { get; set; }

        /// <summary>
        /// 积分认领人姓名
        /// </summary>
        public string PickUpUserName { get; set; } = null!;

        /// <summary>
        /// 任务Id
        /// </summary>
        /// <value> </value>
        public Guid WorkTaskId { get; set; }

        /// <summary>
        /// </summary>
        public string? CreateUserUid { get; set; }

        /// <summary>
        /// </summary>
        /// <value> </value>
        public Guid? CreateUserId { get; set; }

        /// <summary>
        /// </summary>
        public string? CreateUserName { get; set; }

        /// <summary>
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 任务名称
        /// </summary>
        public string? WorkTaskName { get; set; } = string.Empty;
    }
}