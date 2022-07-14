using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeChatApp.Shared.FormBody
{
    /// <summary>
    /// 工作任务审批参数
    /// </summary>
    public class WorkTaskApprovalDto
    {
        /// <summary>
        /// 工作任务Id
        /// </summary>
        /// <value></value>
        public Guid WorkTaskId { get; set; } = Guid.Empty;

        /// <summary>
        /// 审批结果
        /// </summary>
        /// <value></value>
        public bool ApprovalResult { get; set; } = false;

        /// <summary>
        /// 审批意见
        /// </summary>
        /// <value></value>
        public string ApprovalOpinion { get; set; } = null!;
    }
}