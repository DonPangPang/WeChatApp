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
    /// 用户表
    /// </summary>
    public class User : IEntity
    {
        /// <summary>
        /// Id
        /// </summary>
        [Key]
        [Required]
        public Guid Id { get; set; }

        /// <summary>
        /// UserId
        /// </summary>
        [Required]
        public string Uid { get; set; } = null!;

        /// <summary>
        /// 密码
        /// </summary>
        public string? Password { get; set; } = "123456";

        /// <summary>
        /// 姓名
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        /// <summary>
        /// 邮箱
        /// </summary>
        [MaxLength(100)]
        public string? Email { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        /// <value></value>
        public Gender Gender { get; set; } = Gender.男;

        /// <summary>
        /// 电话
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string Tel { get; set; } = null!;

        /// <summary>
        /// 角色
        /// </summary>
        [Required]
        public Role Role { get; set; } = Role.普通成员;

        /// <summary>
        /// 是否为超级管理员
        /// </summary>
        public bool IsSuper { get; set; } = false;

        ///// <summary>
        ///// 部门
        ///// </summary>
        //[NotMapped]
        //public int[]? Department { get; set; }

        /// <summary>
        /// 部门
        /// </summary>
        [NotMapped]
        public int[]? WeComDepartment { get; set; }

        /// <summary>
        /// 部门
        /// </summary>
        [Required]
        public Guid? DepartmentId { get; set; }
    }
}