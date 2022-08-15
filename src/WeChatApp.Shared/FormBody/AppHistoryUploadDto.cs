using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChatApp.Shared.FormBody
{
    /// <summary>
    /// </summary>
    public class AppHistoryUploadDto
    {
        /// <summary>
        /// </summary>
        public string Version { get; set; } = string.Empty;

        /// <summary>
        /// </summary>
        public string? Descrption { get; set; } = string.Empty;

        /// <summary>
        /// 备注
        /// </summary>
        public string? Remark { get; set; } = string.Empty;

        public string? Path { get; set; } = string.Empty;
    }
}