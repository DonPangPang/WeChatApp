using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeChatApp.Shared.FormBody
{
    /// <summary>
    /// 
    /// </summary>
    public class PickUpWorkTaskDto
    {
        /// <summary>
        /// 任务Id
        /// </summary>
        /// <value></value>
        public Guid WorkTaskId { get; set; }

        /// <summary>
        /// 抢单人的Id
        /// </summary>
        /// <value></value>
        [Required]
        public IEnumerable<Guid> PickUpUserIds { get; set; } = null!;
    }
}