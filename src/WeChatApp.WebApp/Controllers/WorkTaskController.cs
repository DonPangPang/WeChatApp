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
using WeChatApp.WebApp.HangfireTasks;
using WeChatApp.WebApp.Services;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
        /// 获取工作任务列表(带分页, 所有数据)
        /// </summary>
        /// <param name="parameter"> </param>
        /// <returns> </returns>
        [HttpGet]
        public async Task<ActionResult> GetWorkTaskListAsync([FromQuery] ParameterBase parameter)
        {
            var res = await _serviceGen.Query<WorkTask>()
                .Include(x => x.Nodes)
                .QueryAsync(parameter);

            return Success("获取成功", res);
        }

        /// <summary>
        /// 工作任务主页
        /// </summary>
        /// <param name="parameters"> </param>
        /// <returns> </returns>
        /// <exception cref="ArgumentNullException"> </exception>
        [HttpGet]
        public async Task<ActionResult> GetWorkTaskIndex([FromQuery] WorkTaskIndexParameters parameters)
        {
            var query = _serviceGen.Query<WorkTask>();

            var pickingTaskCount = await query.Where(x => x.Status == WorkTaskStatus.Publish || x.PickUpUserIds == null || (x.PickUpUserIds.Length < x.MaxPickUpCount * 32 + (x.MaxPickUpCount < 0 ? 0 : x.MaxPickUpCount - 1))).CountAsync();

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

            //var res = await query.QueryAsync(parameters);

            var filterNode = new List<WorkTaskNodeTypes> { WorkTaskNodeTypes.Report, WorkTaskNodeTypes.None };

            var nodeQuery = _serviceGen.Query<WorkTaskNode>();

            var nodes = await nodeQuery.Where(x => !filterNode.Contains(x.Type)).QueryAsync(parameters);

            var workIds = nodes.Select(x => x.WorkTaskId).ToList();

            var works = await _serviceGen.Query<WorkTask>()
                .Where(x => workIds.Contains(x.Id)).AsNoTracking().ToListAsync();

            var result = new List<PorjDyname>();

            foreach (var node in nodes)
            {
                result.Add(new PorjDyname
                {
                    WorkTask = works.Where(x => x.Id == node.WorkTaskId).FirstOrDefault()?.MapTo<WorkTaskDto>(),
                    Node = node,
                });
            }

            return Success("获取成功", new
            {
                pickingTaskCount,
                activeTaskCount,
                totalTaskCount,
                endTaskCount,
                result
            });
        }

        private class PorjDyname
        {
            public WorkTaskDto? WorkTask { get; set; }
            public WorkTaskNode? Node { get; set; }
        }

        /// <summary>
        /// 用户获取可接取的任务
        /// </summary>
        /// <returns> </returns>
        /// <remarks>
        /// 1. 全局发布的任务
        /// 2. 科室任务, 用户属于当前科室
        /// 3. 自定义任务, 自定义任务指定给当前用户
        /// </remarks>
        [HttpGet]
        public async Task<ActionResult> GetPickingWorkTaskListAsync()
        {
            var query = _serviceGen.Query<WorkTask>();

            if (_session.UserInfo is null)
            {
                return Fail("用户信息丢失");
            }

            query = query.Where(x =>
                (x.Status == WorkTaskStatus.Publish || x.Status == WorkTaskStatus.Active) && (x.WorkPublishType == WorkPublishType.全局发布 ||
                (x.WorkPublishType == WorkPublishType.科室发布 && x.DepartmentId == _session.UserInfo.DepartmentId)) &&
                !(x.PickUpUserIds ?? "").Contains(_session.UserInfo.Id.ToString()));

            var pickingTask = await query.Where(x => x.Status == WorkTaskStatus.Publish || x.PickUpUserIds == null || (x.PickUpUserIds.Length < x.MaxPickUpCount * 32 + (x.MaxPickUpCount < 0 ? 0 : x.MaxPickUpCount - 1))).ToListAsync();

            var res = pickingTask.MapTo<WorkTaskDto>();

            return Success(res); ;
        }

        /// <summary>
        /// 获取任务列表(带分页)
        /// </summary>
        /// <param name="parameters"> </param>
        /// <returns> </returns>
        [HttpPost]
        public async Task<ActionResult> GetWorkTasksAsync(WorkTaskDtoGroupParameters parameters)
        {
            var query = _serviceGen.Query<WorkTask>();

            query = _session.UserInfo!.Role switch
            {
                Role.高层管理员 => query,
                Role.中层管理员 => query.Where(x => x.CreateUserId == _session.UserInfo.Id),
                Role.普通成员 => query.Where(x => x.WorkPublishType == WorkPublishType.全局发布 ||
                        (x.WorkPublishType == WorkPublishType.科室发布 && x.DepartmentId == _session.UserInfo.DepartmentId)),
                _ => throw new ArgumentNullException(nameof(_session.UserInfo.Role))
            };

            if (parameters.Status is not null && parameters.Status.Any())
            {
                query = query.Where(x => parameters.Status.Contains(x.Status));
            }

            var res = await query.Include(x => x.Nodes).QueryAsync(parameters);

            var result = res.GroupBy(x => x.Status).Select(x => new
            {
                Status = x.Key,
                Data = x.ToList().MapTo<WorkTaskDto>()
            });

            return Success("获取成功", new
            {
                result
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

            var res = await query
                .Include(x => x.Nodes)!
                .ThenInclude(t => t.Items.Where(j => j.CreateUserId == _session.UserId))
                .Where(x => (x.PickUpUserIds ?? "").Contains(_session.UserId.ToString())).QueryAsync(parameters);

            //var reuslt = await query.Include(x => x.Nodes)!
            //    .ThenInclude(t => t.Items.Where(j => j.CreateUserId == _session.UserId))
            //    .Where(x => (x.PickUpUserIds ?? "")
            //    .Contains(_session.UserId.ToString())).QueryAsync(parameters);

            foreach (var item in res)
            {
                var reportNodeIds = await _serviceGen.Query<WorkTaskNode>()
                    .Where(x => x.Type == WorkTaskNodeTypes.Report)
                    .Where(x => x.WorkTaskId == item.Id).Select(x => x.Id).ToListAsync();
                var reportNodeItemsCount = await _serviceGen.Query<WorkTaskNodeItem>()
                    .Where(x => reportNodeIds.Contains(x.WorkTaskNodeId) && x.CreateUserId == _session.UserId).CountAsync();

                if (reportNodeIds.Count() == reportNodeItemsCount)
                {
                    item.Status = WorkTaskStatus.Finished;
                }

                item.OverProgress = reportNodeIds.Count;
                item.CurrentProgress = reportNodeItemsCount;
            }

            return Success("获取成功", res.MapTo<WorkTaskDto>());
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
                //.Include(x => x.Nodes)
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

            //if (res.IsPublicNodes || res.IsPublic())
            //{
            //    var nodes = await _serviceGen.Query<WorkTaskNode>()
            //        .Where(x => x.WorkTaskId.Equals(id)).ToListAsync();

            //    dto.Nodes = nodes.OrderBy(x => x.Type).ToList();
            //}

            var nodes = await _serviceGen.Query<WorkTaskNode>()
                    .Where(x => x.WorkTaskId.Equals(id)).ToListAsync();

            dto.Nodes = nodes.OrderBy(x => x.Type).ThenBy(x => x.NodeTime).ToList();

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
                   .Where(x => x.WorkTaskId == id)
                   .ToListAsync();

            var dto = res.MapTo<WorkTaskDto>();

            dto.PockUpUsers = users.Select(x => new Shared.Temp.UserItem
            {
                UserId = x.Id,
                Name = x.Name,
                Score = userScore.Where(t => t.PickUpUserId == x.Id).Select(x => x.BonusPoints).FirstOrDefault(0)
            }).ToList();

            //if (res.IsPublicNodes || res.IsPublic())
            //{
            //    var nodes = await _serviceGen.Query<WorkTaskNode>()
            //        .Include(x => x.Items)
            //        .Where(x => x.WorkTaskId.Equals(id)).ToListAsync();

            //    dto.Nodes = nodes.OrderBy(x => x.Type).ToList();
            //}

            var nodes = await _serviceGen.Query<WorkTaskNode>()
                    .Include(x => x.Items)
                    .Where(x => x.WorkTaskId.Equals(id)).ToListAsync();

            dto.Nodes = nodes.OrderBy(x => x.Type).ThenBy(x => x.NodeTime).ToList();

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
                    NodeTime = DateTime.Now,
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

                    case WorkTaskStatus.Publish:
                        node.Title = "已发布";
                        node.Content = $"已发布任务 {entity.Title}";
                        break;

                    default:
                        await _serviceGen.Rollback();
                        throw new ArgumentException("添加任务状态错误");
                }

                node.Create();

                await _serviceGen.Db.AddAsync(node);

                if (entity.Nodes != null)
                {
                    foreach (var item in entity.Nodes)
                    {
                        item.Create();
                    }

                    await _serviceGen.Db.AddRangeAsync(entity.Nodes);
                }

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
        /// 更新任务
        /// </summary>
        /// <param name="dto"> </param>
        /// <returns> </returns>
        [HttpPost]
        public async Task<ActionResult> UpdateWorkTaskAsync(WorkTaskDto dto)
        {
            await _serviceGen.BeginTrans();
            try
            {
                var entity = dto.MapTo<WorkTask>();

                _serviceGen.Db.Update(entity);

                var node = new WorkTaskNode
                {
                    Type = Shared.Enums.WorkTaskNodeTypes.Approval,
                    WorkTaskId = entity.Id,
                    NodeTime = DateTime.Now,
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
                        await _serviceGen.Rollback();
                        throw new ArgumentException("添加任务状态错误");
                }

                node.Create();

                await _serviceGen.Db.AddAsync(node);

                var elderNodes = await _serviceGen.Query<WorkTaskNode>()
                    .Where(x => x.WorkTaskId == entity.Id)
                    .ToListAsync();

                if (entity.Nodes != null)
                {
                    foreach (var item in entity.Nodes)
                    {
                        if (item.Id == Guid.Empty)
                        {
                            item.Create();

                            await _serviceGen.Db.AddAsync(item);
                        }
                        else
                        {
                            var elderNode = elderNodes.Where((x => x.Id == item.Id)).FirstOrDefault();

                            item.Map(elderNode);

                            if (elderNode is not null) _serviceGen.Db.Update(elderNode);
                            else throw new ArgumentNullException($"{item.Title} 任务节点异常");
                        }
                    }

                    foreach (var item in elderNodes.Where(item => !entity.Nodes.Select(x => x.Id).Contains(item.Id)))
                    {
                        _serviceGen.Db.Remove(item);
                    }

                    await _serviceGen.Db.AddRangeAsync(entity.Nodes);
                }

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
        /// <param name="dto"> </param>
        /// <returns> </returns>
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
                    Type = Shared.Enums.WorkTaskNodeTypes.Assigned,
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
        [HttpPost]
        public async Task<IActionResult> PickupWorkTask(PickUpWorkTaskDto dto)
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

            var userIds = string.Join(",", dto.PickUpUserIds);
            task.Status = WorkTaskStatus.Active;
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