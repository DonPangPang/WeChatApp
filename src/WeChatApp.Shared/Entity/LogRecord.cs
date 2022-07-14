using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeChatApp.Shared.Interfaces;

namespace WeChatApp.Shared.Entity
{
    /// <summary>
    /// 日志
    /// </summary>
    public class LogRecord : IEntity
    {
        /// <summary>
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 日志级别
        /// </summary>
        public LogLevel LogLevel { get; set; } = LogLevel.Warning;

        /// <summary>
        /// API, Db...
        /// </summary>
        public LogType Type { get; set; } = LogType.None;

        /// <summary>
        /// 模块
        /// </summary>
        public string? Module { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string? Content { get; set; }

        /// <summary>
        /// 日志时间
        /// </summary>
        public DateTime LogTime { get; set; } = DateTime.Now;
    }

    /// <summary>
    /// 日志类型
    /// </summary>
    public enum LogType
    {
        /// <summary>
        /// </summary>
        None,

        /// <summary>
        /// </summary>
        Api,

        /// <summary>
        /// </summary>
        Database,

        /// <summary>
        /// </summary>
        Task,
    }
}