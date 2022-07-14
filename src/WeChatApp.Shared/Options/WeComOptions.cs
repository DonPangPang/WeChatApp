using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace WeChatApp.Shared.Options
{
    /// <summary>
    /// 企业微信端的配置
    /// </summary>
    public class WeComOptions : IOptions<WeComOptions>
    {
        /// <summary>
        /// 企业Id
        /// </summary>
        public string? CorpId { get; set; }

        /// <summary>
        /// 回调Url
        /// </summary>
        public string? RedirectUri { get; set; }

        public WeComOptions Value => this;
    }
}