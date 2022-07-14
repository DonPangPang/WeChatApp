using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChatApp.Shared.FormBody
{
    /// <summary>
    /// Dto
    /// </summary>
    public interface IDtoBase
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }
    }
}