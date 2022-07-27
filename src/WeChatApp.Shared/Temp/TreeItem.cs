using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeChatApp.Shared.Interfaces;

namespace WeChatApp.Shared.Temp
{
    /// <summary>
    /// 
    /// </summary>
    public class TreeItem : ITree<TreeItem>
    {
        /// <summary>
        /// Id
        /// </summary>
        /// <value></value>
        public Guid Id { get; set; } = Guid.Empty;
        /// <summary>
        /// 名称
        /// </summary>
        /// <value></value>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public Guid? ParentId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public ICollection<TreeItem>? Children { get; set; }

        /// <summary>
        /// 树状结构
        /// </summary>
        /// <value></value>
        public string? TreeIds { get; set; }

        /// <summary>
        /// 节点类型
        /// </summary>
        /// <value></value>
        public TreeItemType Type { get; set; } = TreeItemType.Department;
        /// <summary>
        /// 
        /// </summary>
        /// <value></value>
        public int Level { get; set; } = 0;
    }

    /// <summary>
    /// 
    /// </summary>
    public enum TreeItemType
    {
        /// <summary>
        /// 部门
        /// </summary>
        Department,
        /// <summary>
        /// 用户
        /// </summary>
        User
    }
}