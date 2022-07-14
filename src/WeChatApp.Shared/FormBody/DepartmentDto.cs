using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using WeChatApp.Shared.Entity;

namespace WeChatApp.Shared.FormBody
{
    /// <summary>
    /// 部门
    /// </summary>
    [AutoMap(typeof(Department), ReverseMap = true)]
    public class DepartmentDto : IDtoBase
    {
        /// <summary>
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string DepartmentName { get; set; } = null!;

        /// <summary>
        /// 部门树形结构
        /// </summary>
        public string? DepartmentIdTree { get; set; }

        /// <summary>
        /// 父部门Id
        /// </summary>
        public Guid? ParentId { get; set; }

        /// <summary>
        /// 子部门
        /// </summary>
        public IEnumerable<DepartmentDto>? Children { get; set; }
    }
}