using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeChatApp.Shared.RequestBody.WebApi
{
    /// <summary>
    /// 
    /// </summary>
    public class BonusPointRecordDtoParameters : ParameterBase
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        /// <value></value>
        public Guid? UserId { get; set; } = Guid.Empty;

        /// <summary>
        /// 部门Id
        /// </summary>
        /// <value></value>
        public Guid? DepartmentId { get; set; } = Guid.Empty;

        /// <summary>
        /// 任务Id
        /// </summary>
        /// <value></value>
        public Guid? WorkTaskId { get; set; } = Guid.Empty;
    }
}