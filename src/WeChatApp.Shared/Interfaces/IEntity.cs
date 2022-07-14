using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeChatApp.Shared.Interfaces
{
    /// <summary>
    /// 实体
    /// </summary>
    public interface IEntity
    {
        public Guid Id { get; set; }
    }
}