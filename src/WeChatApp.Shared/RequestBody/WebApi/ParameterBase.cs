using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeChatApp.Shared.Interfaces;

namespace WeChatApp.Shared.RequestBody.WebApi
{
    /// <summary>
    /// 参数基类
    /// </summary>
    public class ParameterBase : IPaging, ISorting, IParameterBase
    {
        /// <summary>
        /// 模糊查询
        /// </summary>
        public string? Q { get; set; }

        /// <summary>
        /// 页码
        /// </summary>
        public int Page { get; set; } = 1;

        /// <summary>
        /// 每页大小
        /// </summary>
        public int PageSize { get; set; } = 50;

        /// <summary>
        /// 排序, 例如按姓名倒序,年龄正序: OrderBy=Name desc,Age
        /// </summary>
        public string? OrderBy { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }
    }
}