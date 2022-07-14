using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeChatApp.Shared.Entity;
using WeChatApp.Shared.Extensions;
using WeChatApp.Shared.FormBody;
using WeChatApp.Shared.RequestBody.WebApi;
using WeChatApp.WebApp.Extensions;
using WeChatApp.WebApp.Filters;
using WeChatApp.WebApp.Services;

namespace WeChatApp.WebApp.Controllers
{
    /// <summary>
    /// 积分记录
    /// </summary>
    public class BonusPointRecordController : ApiController<BonusPointRecord, BonusPointRecordDto>
    {
        private readonly IServiceGen _serviceGen;
        private readonly Session _session;

        /// <summary>
        /// </summary>
        /// <param name="serviceGen"> </param>
        /// <param name="session">    </param>
        public BonusPointRecordController(IServiceGen serviceGen, Session session) : base(serviceGen)
        {
            _serviceGen = serviceGen;
            _session = session;
        }

        /// <summary>
        /// 获取积分记录(带分页)
        /// </summary>
        /// <param name="parameters"> </param>
        /// <returns> </returns>
        [HttpGet]
        public async Task<ActionResult> GetBonusPointRecordsAsync([FromQuery] BonusPointRecordDtoParameters parameters)
        {
            var query = _serviceGen.Query<BonusPointRecord>();

            if (!parameters.UserId.IsEmpty())
            {
                query = query.Where(x => x.PickUpUserId == parameters.UserId);
            }

            if (!parameters.Q.IsEmpty())
            {
                query = query.Where(x => x.PickUpUserName.Contains(parameters.Q ?? ""));
            }

            if (!parameters.DepartmentId.IsEmpty())
            {
                var users = await _serviceGen.Query<User>().Where(x => x.DepartmentId == parameters.DepartmentId).Select(x => x.Id).ToListAsync();

                query = query.Where(x => users.Contains(x.PickUpUserId));
            }

            if (!parameters.WorkTaskId.IsEmpty())
            {
                query = query.Where(x => x.WorkTaskId == parameters.WorkTaskId);
            }

            var res = await query.QueryAsync(parameters);

            return Success("获取成功", res);
        }

        /// <summary>
        /// 获取排名
        /// </summary>
        /// <returns> </returns>
        [HttpGet]
        public async Task<ActionResult> GetRankings()
        {
            var globalRank = await _serviceGen.Query<BonusPointRecord>()
                .GroupBy(x => x.PickUpUserName)
                .Select(x => new { Name = x.Key, Score = x.Sum(t => t.BonusPoints) })
                .OrderByDescending(x => x.Score).ToListAsync();

            var userIds = await _serviceGen.Query<User>().Where(x => x.DepartmentId == _session.UserInfo!.DepartmentId).Select(x => x.Id).ToListAsync();

            var deptRank = await _serviceGen.Query<BonusPointRecord>()
                .Where(x => userIds.Contains(x.PickUpUserId))
                .GroupBy(x => x.PickUpUserName)
                .Select(x => new { Name = x.Key, Score = x.Sum(t => t.BonusPoints) })
                .OrderByDescending(x => x.Score).ToListAsync();

            return Success("获取成功", new
            {
                globalRank,
                deptRank
            });
        }
    }
}