using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pang.AutoMapperMiddleware;
using System.Runtime.CompilerServices;
using WeChatApp.Shared.Entity;
using WeChatApp.Shared.Enums;
using WeChatApp.Shared.Extensions;
using WeChatApp.Shared.FormBody;
using WeChatApp.Shared.Interfaces;
using WeChatApp.Shared.RequestBody.WebApi;
using WeChatApp.WebApp.Extensions;
using WeChatApp.WebApp.Filters;
using WeChatApp.WebApp.Services;

namespace WeChatApp.WebApp.Controllers
{
    /// <summary>
    /// 工作任务
    /// </summary>
    public class WorkTaskController : ApiController<WorkTask, WorkTaskDto>
    {
        private readonly IServiceGen _serviceGen;
        private readonly Session _session;
        private readonly IMessageToastService _messageToastService;

        /// <summary>
        /// </summary>
        /// <param name="serviceGen">          </param>
        /// <param name="session">             </param>
        /// <param name="messageToastService"> </param>
        public WorkTaskController(IServiceGen serviceGen, Session session, IMessageToastService messageToastService) : base(serviceGen)
        {
            _serviceGen = serviceGen;
            _session = session;
            _messageToastService = messageToastService;
        }

        /// <summary>
        /// 获取任务列表(带分页)
        /// </summary>
        /// <param name="parameters"> </param>
        /// <returns> </returns>
        [HttpGet]
        public async Task<ActionResult> GetWorkTasksAsync([FromQuery] WorkTaskDtoParameters parameters)
        {
            var query = _serviceGen.Query<WorkTask>();

            var activeTaskCount = await query.Where(x => x.Status == WorkTaskStatus.Active).CountAsync();

            var totalTaskCount = await query.CountAsync();

            var endTaskCount = await query.Where(x => x.Status == WorkTaskStatus.End).CountAsync();

            query = _session.UserInfo!.Role switch
            {
                Role.高层管理员 => query,
                Role.中层管理员 => query.Where(x => x.CreateUserId == _session.UserInfo.Id),
                Role.普通成员 => query.Where(x => x.WorkPublishType == WorkPublishType.全局发布 ||
                        (x.WorkPublishType == WorkPublishType.科室发布 && x.DepartmentId == _session.UserInfo.DepartmentId)),
                _ => throw new ArgumentNullException(nameof(_session.UserInfo.Role))
            };

            if (parameters.Status != WorkTaskStatus.None)
            {
                query = query.Where(x => x.Status == parameters.Status);
            }

            var res = await query.QueryAsync(parameters);

            return Success("获取成功", new
            {
                activeTaskCount,
                totalTaskCount,
                endTaskCount,
                res
            });
        }

        /// <summary>
        /// 获取我的任务
        /// </summary>
        /// <returns> </returns>
        [HttpGet]
        public async Task<ActionResult> GetMyWorkTasksAsync([FromQuery] WorkTaskDtoParameters parameters)
        {
            var query = _serviceGen.Query<WorkTask>();

            query = query.Where(x => (x.PickUpUserIds ?? "").Contains(_session.UserInfo!.Id.ToString()));

            if (parameters.Status != WorkTaskStatus.None)
            {
                query = query.Where(x => x.Status == parameters.Status);
            }

            var res = await query.Where(x => x.PickUpUserIds.Contains(_session.UserId.ToString())).QueryAsync(parameters);

            foreach (var item in res)
            {
                var reportNodeIds = await _serviceGen.Query<WorkTaskNode>().Where(x => x.WorkTaskId == item.Id).Select(x => x.Id).ToListAsync();
                var reportNodeItemsCount = await _serviceGen.Query<WorkTaskNodeItem>()
                    .Where(x => reportNodeIds.Contains(x.WorkTaskNodeId) && x.CreateUserId == _session.UserId).CountAsync();

                if (reportNodeIds.Count() == reportNodeItemsCount)
                {
                    item.Status = WorkTaskStatus.Finished;
                }
            }

            return Success("获取成功", res);
        }

        /// <summary>
        /// 获取工作任务
        /// </summary>
        /// <param name="id"> </param>
        /// <returns> </returns>
        [HttpGet]
        public async Task<ActionResult> GetWorkTaskAsync(Guid id)
        {
            var res = await _serviceGen.Query<WorkTask>()
                .Where(x => x.Id.Equals(id))
                .FirstOrDefaultAsync();

            if (res is null) return Fail("未找到该任务");

            var users = await _serviceGen.Query<User>()
                .Where(x => (res.PickUpUserIds ?? "").Contains(x.Id.ToString())).ToListAsync();

            var userScore = await _serviceGen.Query<BonusPointRecord>()
                   .Where(x => x.WorkTaskId == res.Id)
                   .ToListAsync();

            var dto = res.MapTo<WorkTaskDto>();

            dto.PockUpUsers = users.Select(x => new Shared.Temp.UserItem
            {
                UserId = x.Id,
                Name = x.Name,
                Score = userScore.Where(x => x.PickUpUserId == x.Id).Select(x => x.BonusPoints).FirstOrDefault(0)
            }).ToList();

            if (res is null) return Fail("没有找到该数据");

            if (res is not null) return Success("获取成功", dto);

            return Fail("获取失败");
        }

        /// <summary>
        /// 获取工作任务带任务节点
        /// </summary>
        /// <param name="id"> </param>
        /// <returns> </returns>
        [HttpGet]
        public async Task<ActionResult> GetWorkTaskWithNodesAsync(Guid id)
        {
            var res = await _serviceGen.Query<WorkTask>()
                .Where(x => x.Id.Equals(id))
                .FirstOrDefaultAsync();

            if (res is null) return Fail("没有找到该数据");

            var users = await _serviceGen.Query<User>()
                .Where(x => (res.PickUpUserIds ?? "").Contains(x.Id.ToString())).ToListAsync();

            var userScore = await _serviceGen.Query<BonusPointRecord>()
                   .Where(x => x.WorkTaskId == res.Id)
                   .ToListAsync();

            var dto = res.MapTo<WorkTaskDto>();

            dto.PockUpUsers = users.Select(x => new Shared.Temp.UserItem
            {
                UserId = x.Id,
                Name = x.Name,
                Score = userScore.Where(x => x.PickUpUserId == x.Id).Select(x => x.BonusPoints).FirstOrDefault(0)
            }).ToList();

            if (res.IsPublicNodes || res.IsPublic())
            {
                var nodes = await _serviceGen.Query<WorkTaskNode>()
                    .Where(x => x.WorkTaskId.Equals(id)).ToListAsync();

                dto.Nodes = nodes.OrderBy(x => x.Type).ToList();
            }

            return Success("获取成功", dto);
        }

        /// <summary>
        /// 获取工作任务带任务节点, 以及任务节点的任务人的提交信息
        /// </summary>
        /// <param name="id"> </param>
        /// <returns> </returns>
        [HttpGet]
        public async Task<ActionResult> GetWorkTaskWithNodesAndItemsAsync(Guid id)
        {
            var res = await _serviceGen.Query<WorkTask>()
                .Where(x => x.Id.Equals(id))
                .FirstOrDefaultAsync();

            if (res is null) return Fail("没有找到该数据");

            var users = await _serviceGen.Query<User>()
                .Where(x => (res.PickUpUserIds ?? "").Contains(x.Id.ToString())).ToListAsync();

            var userScore = await _serviceGen.Query<BonusPointRecord>()
                   .Where(x => x.WorkTaskId == res.Id)
                   .ToListAsync();

            var dto = res.MapTo<WorkTaskDto>();

            dto.PockUpUsers = users.Select(x => new Shared.Temp.UserItem
            {
                UserId = x.Id,
                Name = x.Name,
                Score = userScore.Where(x => x.PickUpUserId == x.Id).Select(x => x.BonusPoints).FirstOrDefault(0)
            }).ToList();

            if (res.IsPublicNodes || res.IsPublic())
            {
                var nodes = await _serviceGen.Query<WorkTaskNode>()
                    .Include(x => x.Items)
                    .Where(x => x.WorkTaskId.Equals(id)).ToListAsync();

                dto.Nodes = nodes.OrderBy(x => x.Type).ToList();
            }

            return Success("获取成功", dto);
        }

        /// <summary>
        /// 创建任务
        /// </summary>
        /// <param name="dto"> </param>
        /// <returns> </returns>
        [HttpPost]
        public async Task<ActionResult> CreateWorkTaskAsync(WorkTaskDto dto)
        {
            await _serviceGen.BeginTrans();
            try
            {
                var entity = dto.MapTo<WorkTask>();
                entity.Create();

                await _serviceGen.Db.AddAsync(entity);

                var node = new WorkTaskNode
                {
                    Type = Shared.Enums.WorkTaskNodeTypes.Approval,
                    WorkTaskId = entity.Id,
                };

                switch (entity.Status)
                {
                    case WorkTaskStatus.PendingReview:
                        node.Title = "待审核";
                        node.Content = $"待审核任务 {entity.Title}";
                        break;

                    case WorkTaskStatus.PendingPublish:
                        node.Title = "待发布";
                        node.Content = $"待发布任务 {entity.Title}";
                        break;

                    case WorkTaskStatus.Active:
                        node.Title = "已发布";
                        node.Content = $"已发布任务 {entity.Title}";
                        break;

                    default:
                        throw new ArgumentException("添加任务状态错误");
                }

                node.Create();

                await _serviceGen.Db.AddAsync(node);

                await _serviceGen.SaveAsync();

                await _serviceGen.CommitTrans();

                _ = Task.Run(async () =>
                {
                    await _messageToastService.SendMessageAsync(entity);
                });

                return Success("创建成功");
            }
            catch
            {
                await _serviceGen.Rollback();
                throw;
            }
        }

        /// <summary>
        /// 创建高层指派给中层的任务
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> CreateAssignWorkTaskAsync(WorkTaskDto dto)
        {
            await _serviceGen.BeginTrans();
            try
            {
                var entity = dto.MapTo<WorkTask>();
                entity.Create();

                await _serviceGen.Db.AddAsync(entity);

                var node = new WorkTaskNode
                {
                    Type = Shared.Enums.WorkTaskNodeTypes.Approval,
                    WorkTaskId = entity.Id,
                };

                switch (entity.Status)
                {
                    case WorkTaskStatus.Assign:
                        node.Title = "指派任务";
                        node.Content = $"待指派任务 {entity.Title}";
                        break;

                    default:
                        throw new ArgumentException("添加任务状态错误");
                }

                node.Create();

                await _serviceGen.Db.AddAsync(node);

                await _serviceGen.SaveAsync();

                await _serviceGen.CommitTrans();

                _ = Task.Run(async () =>
                {
                    await _messageToastService.SendMessageAsync(entity);
                });

                return Success("创建成功");
            }
            catch
            {
                await _serviceGen.Rollback();
                throw;
            }
        }

        /// <summary>
        /// 审批任务
        /// </summary>
        /// <returns> </returns>
        [HttpPost]
        public async Task<ActionResult> ApprovalTask(WorkTaskApprovalDto dto)
        {
            await _serviceGen.BeginTrans();

            try
            {
                var task = await _serviceGen.Query<WorkTask>().Where(x => x.Id.Equals(dto.WorkTaskId)).FirstOrDefaultAsync();

                if (task is null) return Fail("没有找到该数据");

                if (task.Status != WorkTaskStatus.PendingReview) return Fail("该任务不是待审核状态");

                var node = new WorkTaskNode
                {
                    Type = Shared.Enums.WorkTaskNodeTypes.Approval,
                    WorkTaskId = task.Id,
                };

                if (dto.ApprovalResult)
                {
                    task.Status = WorkTaskStatus.PendingPublish;

                    node.Title = "待发布";
                    node.Content = $"待发布任务 {task.Title}";
                }
                else
                {
                    task.Status = WorkTaskStatus.Overrule;

                    node.Title = "已驳回";
                    node.Content = $"已驳回任务 {task.Title}";
                }

                _serviceGen.Db.Update(task);

                node.Create();

                await _serviceGen.Db.AddAsync(node);

                await _serviceGen.SaveAsync();

                await _serviceGen.CommitTrans();

                _ = Task.Run(async () => await _messageToastService.SendMessageAsync(task));

                return Success("操作成功");
            }
            catch
            {
                await _serviceGen.Rollback();
                throw;
            }
        }

        /// <summary>
        /// 发布任务
        /// </summary>
        /// <returns> </returns>
        [HttpGet]
        public async Task<IActionResult> PublishWorkTask(Guid id)
        {
            if (id == Guid.Empty) return Fail("参数错误");

            var task = await _serviceGen.Query<WorkTask>().Where(x => x.Id.Equals(id)).FirstOrDefaultAsync();

            if (task is null) return Fail("没有找到该任务");

            if (task.Status != WorkTaskStatus.PendingPublish || task.Status != WorkTaskStatus.Assign) return Fail("该任务不是待发布状态");

            task.Status = WorkTaskStatus.Active;

            _serviceGen.Db.Update(task);

            var res = await _serviceGen.SaveAsync();

            if (res)
            {
                _ = Task.Run(async () => await _messageToastService.SendMessageAsync(task));
                return Success("发布成功");
            }

            return Fail("发布失败");
        }

        /// <summary>
        /// 抢单/指定
        /// </summary>
        /// <returns> </returns>
        [HttpGet]
        public async Task<IActionResult> PickupWorkTask([FromBody] PickUpWorkTaskDto dto)
        {
            if (dto is null) return Fail("参数错误");

            if (dto.WorkTaskId == Guid.Empty) return Fail("需要指定任务");

            if (dto.PickUpUserIds.IsEmpty()) return Fail("需要指定抢单人");

            var task = await _serviceGen.Query<WorkTask>()
                .Where(x => x.Id.Equals(dto.WorkTaskId)).FirstOrDefaultAsync();

            if (task is null) return Fail("没有找到该任务");

            var pickUpCount = await _serviceGen.Query<WorkTaskNodeItem>()
                .Where(x => x.WorkTaskId == dto.WorkTaskId).CountAsync();

            if (pickUpCount >= task.MaxPickUpCount)
            {
                return Fail("到达抢单上限");
            }

            var userIds = string.Join(",", dto);

            task.PickUpUserIds = task.PickUpUserIds + "," + userIds;

            _serviceGen.Db.Update(task);

            var res = await _serviceGen.SaveAsync();

            if (res)
            {
                _ = Task.Run(async () => await _messageToastService.SendMessageAsync(task, true));
                return Success("抢单成功");
            }

            return Fail("抢单失败");
        }
    }
}