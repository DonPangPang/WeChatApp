using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChatApp.Shared.RequestBody.WebApi
{
    /// <summary>
    /// 工作节点子项请求参数
    /// </summary>
    public class WorkTaskItemDtoParameters : ParameterBase
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public Guid UserId { get; set; } = Guid.Empty;

        /// <summary>
        /// 工作任务Id
        /// </summary>
        public Guid WorkTaskId { get; set; }

        /// <summary>
        /// 子节点Id
        /// </summary>
        public Guid WorkTaskNodeId { get; set; }
    }
}