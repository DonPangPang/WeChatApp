using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeChatApp.Shared.Entity;

namespace WeChatApp.Shared.FormBody
{
    /// <summary>
    /// 积分记录
    /// </summary>
    [AutoMap(typeof(BonusPointRecord), ReverseMap = true)]
    public class BonusPointRecordDto : IDtoBase
    {
        /// <summary>
        /// </summary>
        public Guid Id { get; set; }
    }
}