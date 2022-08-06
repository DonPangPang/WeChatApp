using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace WeChatApp.Shared.Options
{
    /// <summary>
    /// 数据库设置
    /// </summary>
    public class DbOptions : IOptions<DbOptions>
    {
        /// <summary>
        /// </summary>
        /// <value> </value>
        public IEnumerable<DbSetting> DbSettings { get; set; } = null!;

        /// <summary>
        /// </summary>
        public DbOptions Value => this;
    }

    /// <summary>
    /// </summary>
    public class DbSetting
    {
        /// <summary>
        /// 数据库类型
        /// </summary>
        /// <value> </value>
        public string DbType { get; set; } = "SqlServer";

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public string ConnectionString { get; set; } = string.Empty;

        /// <summary>
        /// 是否启用
        /// </summary>
        /// <value> </value>
        public bool IsEnable { get; set; } = false;
    }
}