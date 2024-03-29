﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeChatApp.Shared.Enums;
using AutoMapper;
using WeChatApp.Shared.Entity;

namespace WeChatApp.Shared.FormBody
{
    /// <summary>
    /// 用户
    /// </summary>
    [AutoMap(typeof(User), ReverseMap = true)]
    public class UserDto : IDtoBase
    {
        /// <summary>
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// UserId
        /// </summary>
        [Required(ErrorMessage = "UserId不能为空")]
        public string Uid { get; set; } = null!;

        /// <summary>
        /// 姓名
        /// </summary>
        [Required(ErrorMessage = "姓名不能为空")]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        /// <summary>
        /// 性别
        /// </summary>
        public Gender Gender { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [MaxLength(100)]
        public string? Email { get; set; }

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
        public int[]? WeComDepartment { get; set; }

        /// <summary>
        /// 部门
        /// </summary>
        public Guid? DepartmentId { get; set; }
        
        /// <summary>
        /// 部门名称
        /// </summary>
        public string DepartmentName { get; set; } = string.Empty;

        /// <summary>
        /// 积分
        /// </summary>
        public decimal Score { get; set; } = 0;

        /// <summary>
        /// 全局排名
        /// </summary>
        public int GlobalRank { get; set; } = 0;
        
        /// <summary>
        /// 部门排名
        /// </summary>
        public int DepartmentRank { get; set; } = 0;
    }
}