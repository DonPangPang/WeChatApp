using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeChatApp.Shared.Enums;

namespace WeChatApp.Shared.RequestBody.WebApi
{
    /// <summary>
    /// 工作任务请求参数
    /// </summary>
    public class WorkTaskDtoParameters : ParameterBase
    {
        /// <summary>
        /// 任务状态, 如果不传就是全部状态的任务
        /// </summary>
        /// <value> </value>
        public WorkTaskStatus Status { get; set; } = WorkTaskStatus.None;
    }

    /// <summary>
    /// 工作任务请求参数
    /// </summary>
    public class WorkTaskDtoGroupParameters : ParameterBase
    {
        /// <summary>
        /// 任务状态, 如果不传就是全部状态的任务
        /// </summary>
        /// <value> </value>
        public IEnumerable<WorkTaskStatus> Status { get; set; } = new List<WorkTaskStatus>();
    }

    /// <summary>
    /// 工作任务主页请求参数
    /// </summary>
    public class WorkTaskIndexParameters : ParameterBase
    {
        /// <summary>
        /// 任务状态, 如果不传就是全部状态的任务
        /// </summary>
        /// <value> </value>
        public WorkTaskStatus Status { get; set; } = WorkTaskStatus.None;
    }
}