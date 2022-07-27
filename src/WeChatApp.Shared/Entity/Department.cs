using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using WeChatApp.Shared.Interfaces;

namespace WeChatApp.Shared.Entity
{
    /// <summary>
    /// 部门
    /// </summary>
    public class Department : IEntity, ICreator, ITree<Department>
    {
        /// <summary>
        /// </summary>
        [Required]
        public Guid Id { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        [Required]
        public string DepartmentName { get; set; } = null!;

        /// <summary>
        /// 部门树形结构
        /// </summary>
        public string? TreeIds { get; set; }

        /// <summary>
        /// 父部门Id
        /// </summary>
        public Guid? ParentId { get; set; }

        /// <summary>
        /// 父级部门
        /// </summary>
        public Department? Parent { get; set; }

        /// <summary>
        /// 部门
        /// </summary>
        public virtual ICollection<Department>? Children { get; set; }

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

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        [NotMapped]
        public int Level { get; set; } = 0;
    }
}