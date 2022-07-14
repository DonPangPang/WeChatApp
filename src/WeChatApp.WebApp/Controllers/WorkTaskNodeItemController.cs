using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeChatApp.Shared.Entity;
using WeChatApp.Shared.Extensions;
using WeChatApp.Shared.FormBody;
using WeChatApp.Shared.RequestBody.WebApi;
using WeChatApp.WebApp.Extensions;
using WeChatApp.WebApp.Services;

namespace WeChatApp.WebApp.Controllers
{
    /// <summary>
    /// 工作节点子项
    /// </summary>
    public class WorkTaskNodeItemController : ApiController<WorkTaskNodeItem, WorkTaskNodeItemDto>
    {
        private readonly IServiceGen _serviceGen;

        /// <summary>
        /// </summary>
        /// <param name="serviceGen"> </param>
        /// <returns> </returns>
        public WorkTaskNodeItemController(IServiceGen serviceGen) : base(serviceGen)
        {
            _serviceGen = serviceGen;
        }

        /// <summary>
        /// 获取工作节点的子项
        /// </summary>
        /// <returns> </returns>
        /// <exception cref="ArgumentNullException"> </exception>
        [HttpGet]
        public async Task<ActionResult> GetWorkTaskItems([FromQuery] WorkTaskItemDtoParameters parameters)
        {
            var query = _serviceGen.Query<WorkTaskNodeItem>();

            if (!parameters.UserId.IsEmpty())
            {
                query = query.Where(x => x.CreateUserId == parameters.UserId);
            }

            if (!parameters.WorkTaskId.IsEmpty())
            {
                query = query.Where(x => x.WorkTaskId == parameters.WorkTaskId);
            }

            if (parameters.WorkTaskNodeId.IsEmpty())
            {
                query = query.Where(x => x.WorkTaskNodeId == parameters.WorkTaskNodeId);
            }

            var res = await query.QueryAsync(parameters);

            return Success("查询成功", res);
        }
    }
}