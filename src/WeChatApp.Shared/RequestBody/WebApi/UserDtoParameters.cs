using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeChatApp.Shared.RequestBody.WebApi
{
    /// <summary>
    /// 
    /// </summary>
    public class UserDtoParameters : ParameterBase
    {
        /// <summary>
        /// 部门Id
        /// </summary>
        /// <value></value>
        public Guid DepartmentId { get; set; }
    }
}