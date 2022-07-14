using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeChatApp.Shared.Attributes
{
    /// <summary>
    /// 枚举颜色
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class ShowColorAttribute : Attribute
    {
        /// <summary>
        /// 颜色
        /// </summary>
        /// <value></value>
        public string? Color { get; set; } = "grey";
    }
}