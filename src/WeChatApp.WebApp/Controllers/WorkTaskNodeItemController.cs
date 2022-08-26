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

            if (!parameters.WorkTaskNodeId.IsEmpty())
            {
                query = query.Where(x => x.WorkTaskNodeId == parameters.WorkTaskNodeId);
            }

            var res = await query.QueryAsync(parameters);

            return Success("查询成功", res);
        }

        /// <summary>
        /// 用户单个任务的每个节点的提交记录
        /// </summary>
        /// <remarks> 用户单个任务的每个节点的提交记录 </remarks>
        /// <param name="parameters"> </param>
        /// <returns> </returns>
        [HttpGet]
        public async Task<ActionResult> GetTaskItemsByUser([FromQuery] WorkTaskWithItemsDtoParameters parameters)
        {
            var query = _serviceGen.Query<WorkTaskNodeItem>()
                .GroupJoin(_serviceGen.Query<WorkTaskNode>(), item => item.WorkTaskNodeId, node => node.Id, (item, grouping) => new { item, grouping })
                .SelectMany(@t => @t.grouping.DefaultIfEmpty(), (@t, node) => new { @t.item, node });

            if (!parameters.WorkTaskId.IsEmpty())
            {
                query = query.Where(x => x.item.WorkTaskId == parameters.WorkTaskId);
            }

            if (!parameters.UserId.IsEmpty())
            {
                query = query.Where(x => x.item.CreateUserId == parameters.UserId);
            }

            var result = await query.OrderByDescending(x => x.item.CreateTime).AsNoTracking().ToListAsync();

            return Success("查询成功", result);
        }
    }
}