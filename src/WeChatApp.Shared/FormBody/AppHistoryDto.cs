using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeChatApp.Shared.Entity;
using WeChatApp.Shared.Interfaces;

namespace WeChatApp.Shared.FormBody
{
    /// <summary>
    /// </summary>
    [AutoMap(typeof(AppHistory), ReverseMap = true)]
    public class AppHistoryDto : IDtoBase
    {
        /// <summary>
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string? Remark { get; set; } = string.Empty;

        /// <summary>
        /// </summary>
        public string? ConfigValue { get; set; }

        /// <summary>
        /// </summary>
        public string? Version { get; set; }

        /// <summary>
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// 路径
        /// </summary>
        public string? Source { get; set; }

        /// <summary>
        /// </summary>
        public string? CreateUserUid { get; set; }

        /// <summary>
        /// </summary>
        public Guid? CreateUserId { get; set; }

        /// <summary>
        /// </summary>
        public string? CreateUserName { get; set; }

        /// <summary>
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// </summary>
        public string? ModifyUserUid { get; set; }

        /// <summary>
        /// </summary>
        public Guid? ModifyUserId { get; set; }

        /// <summary>
        /// </summary>
        public string? ModifyUserName { get; set; }

        /// <summary>
        /// </summary>
        public DateTime ModifyTime { get; set; }
    }
}