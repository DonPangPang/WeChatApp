using System.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeChatApp.Shared.Attributes;

namespace WeChatApp.Shared.Extensions
{
    /// <summary>
    /// 通用扩展
    /// </summary>
    public static class UtilExtensions
    {
        /// <summary>
        /// 获取颜色
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static string GetColor(this Enum e)
        {
            var type = e.GetType();
            var field = type.GetField(e.ToString());
            var attributes = field!.GetCustomAttributes(typeof(ShowColorAttribute), false);
            if (attributes.Length > 0)
            {
                return ((ShowColorAttribute)attributes[0]).Color ?? "grey";
            }
            return "grey";
        }
    }
}