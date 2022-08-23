using System.Linq;
using System.Reflection.Metadata;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using WeChatApp.Shared.Entity;
using WeChatApp.Shared.Extensions;
using WeChatApp.Shared.FormBody;
using WeChatApp.Shared.RequestBody.WebApi;
using WeChatApp.WebApp.Extensions;
using WeChatApp.WebApp.Filters;
using WeChatApp.WebApp.Services;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
        /// 获取积分记录
        /// </summary>
        /// <param name="parameters"> </param>
        /// <returns> </returns>
        [HttpGet]
        public async Task<ActionResult> GetBonusPointRecordsAsync([FromQuery] BonusPointRecordDtoParameters parameters)
        {
            //var query = _serviceGen.Query<BonusPointRecord>();

            var query = _serviceGen.Query<BonusPointRecord>()
                .Join(_serviceGen.Query<WorkTask>(), record => record.WorkTaskId, task => task.Id,
                    (record, task) => new { record, task })
                .Where(@t => @t.record.PickUpUserId == parameters.UserId);

            if (!parameters.UserId.IsEmpty())
            {
                query = query.Where(@x => @x.record.PickUpUserId == parameters.UserId);
            }

            if (!parameters.Q.IsEmpty())
            {
                query = query.Where(@x => @x.record.PickUpUserName.Contains(parameters.Q ?? ""));
            }

            if (!parameters.DepartmentId.IsEmpty())
            {
                var users = await _serviceGen.Query<User>().Where(x => x.DepartmentId == parameters.DepartmentId).Select(x => x.Id).ToListAsync();

                query = query.Where(@x => users.Contains(@x.record.PickUpUserId));
            }

            if (!parameters.WorkTaskId.IsEmpty())
            {
                query = query.Where(@x => @x.record.WorkTaskId == parameters.WorkTaskId);
            }

            if (!parameters.StartTime.IsEmpty() && !parameters.EndTime.IsEmpty())
            {
                query = query.Where(@x =>
                    @x.record.CreateTime >= parameters.StartTime && @x.record.CreateTime <= parameters.EndTime);
            }

            var res = await query.Select(@x => new BonusPointRecordDto()
            {
                Id = @x.record.Id,
                BonusPoints = @x.record.BonusPoints,
                PickUpUserId = @x.record.PickUpUserId,
                PickUpUserName = @x.record.PickUpUserName,
                WorkTaskId = @x.record.WorkTaskId,
                CreateUserUid = @x.record.CreateUserUid,
                CreateUserId = @x.record.CreateUserId,
                CreateUserName = @x.record.CreateUserName,
                CreateTime = @x.record.CreateTime,
                WorkTaskName = @x.task.Title
            }).ToListAsync();

            return Success("获取成功", res);
        }

        /// <summary>
        /// 获取排名
        /// </summary>
        /// <returns> </returns>
        [HttpPost]
        public async Task<ActionResult> GetRankings(List<Guid> departments)
        {
            var globalRank = await _serviceGen.Query<User>()
                .Join(_serviceGen.Query<BonusPointRecord>(), user => user.Id, bpRecord => bpRecord.PickUpUserId, (user, bpRecord) => new { user, bpRecord })
                .GroupBy(@x => new { @x.user.Id, @x.user.Name })
                .Select(x => new { Name = x.Key.Name, Score = x.Any() ? x.Sum(x => x.bpRecord.BonusPoints) : 0 })
                .OrderByDescending(x => x.Score)
                .ToListAsync();

            //var globalRank = await _serviceGen.Query<BonusPointRecord>()
            //    .GroupBy(x => x.PickUpUserName)
            //    .Select(x => new { Name = x.Key, Score = x.Sum(t => t.BonusPoints) })
            //    .OrderByDescending(x => x.Score).ToListAsync();

            var user_query = _serviceGen.Query<User>();

            var deptId = departments.FirstOrDefault();
            if (deptId != Guid.Empty)
            {
                var depts = string.Join(",", await _serviceGen.Query<Department>().Where(x => x.TreeIds!.Contains(deptId.ToString())).Select(x => x.TreeIds).ToListAsync());
                user_query = user_query.Where(x => depts.Contains(x.DepartmentId.ToString() ?? ""));
            }
            else
            {
                user_query = user_query.Where(x => x.DepartmentId == _session.UserInfo!.DepartmentId);
            }

            var userIds = await user_query.Select(x => x.Id).ToListAsync();

            var deptRank = await _serviceGen.Query<User>()
                .Join(_serviceGen.Query<BonusPointRecord>(), user => user.Id, bpRecord => bpRecord.PickUpUserId, (user, bpRecord) => new { user, bpRecord })
                .Where(@x => userIds.Contains(@x.bpRecord.PickUpUserId))
                .GroupBy(@x => new { @x.user.Id, @x.user.Name })
                .Select(x => new { Name = x.Key.Name, Score = x.Any() ? x.Sum(x => x.bpRecord.BonusPoints) : 0 })
                .OrderByDescending(x => x.Score)
                .ToListAsync();

            //var deptRank = await _serviceGen.Query<BonusPointRecord>()
            //    .Where(x => userIds.Contains(x.PickUpUserId))
            //    .GroupBy(x => x.PickUpUserName)
            //    .Select(x => new { Name = x.Key, Score = x.Sum(t => t.BonusPoints) })
            //    .OrderByDescending(x => x.Score).ToListAsync();

            return Success("获取成功", new
            {
                globalRank,
                deptRank
            });
        }
    }
}