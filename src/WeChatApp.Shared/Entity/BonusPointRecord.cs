using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeChatApp.Shared.Interfaces;

namespace WeChatApp.Shared.Entity
{
    /// <summary>
    /// 积分记录
    /// </summary>
    public class BonusPointRecord : IEntity, ICreator
    {
        /// <summary>
        /// Id
        /// </summary>
        [Key]
        [Required]
        public Guid Id { get; set; }

        /// <summary>
        /// 积分
        /// </summary>
        [Column(TypeName = "decimal(10,2)")]
        public decimal BonusPoints { get; set; } = 0;

        /// <summary>
        /// 积分认领人
        /// </summary>
        [Required]
        public Guid PickUpUserId { get; set; }

        /// <summary>
        /// 积分认领人姓名
        /// </summary>
        [Required]
        public string PickUpUserName { get; set; } = null!;

        /// <summary>
        /// 任务Id
        /// </summary>
        /// <value></value>
        public Guid WorkTaskId { get; set; }

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
        public DateTime CreateTime { get; set; }
    }
}