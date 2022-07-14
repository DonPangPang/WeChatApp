using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChatApp.Shared.Temp
{
    /// <summary>
    /// 任务节点子项用户
    /// </summary>
    public class UserItem
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// 用户姓名
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 分数
        /// </summary>
        public decimal Score { get; set; } = 0;
    }
}