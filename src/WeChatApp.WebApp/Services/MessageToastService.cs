using Microsoft.EntityFrameworkCore;
using Pang.AutoMapperMiddleware;
using WeChatApp.Shared.Entity;
using WeChatApp.Shared.Enums;
using WeChatApp.Shared.Extensions;
using WeChatApp.Shared.FormBody;
using WeChatApp.Shared.Temp;
using WeChatApp.WebApp.Extensions;
using WeChatApp.WebApp.WebSocket;

namespace WeChatApp.WebApp.Services
{
    public class MessageToastService : IMessageToastService
    {
        private readonly IWsSessionManager _wsSessionManager;
        private readonly IServiceGen _service;

        public MessageToastService(IWsSessionManager wsSessionManager, IServiceGen service)
        {
            _wsSessionManager = wsSessionManager;
            _service = service;
        }

        public async Task ReSendMessageAsync()
        {
            var messages = await _service.Query<MessageToast>().Where(x => !x.IsPush).GroupBy(x => x.UserId).ToListAsync();

            foreach (var msg in messages)
            {
                await SendMessageAsync(msg.Key, msg.ToList());
            }
        }

        public async Task ReSendMessageAsync(Guid id)
        {
            var messages = await _service.Query<MessageToast>().Where(x => !x.IsPush && x.UserId == id).ToListAsync();

            await SendMessageAsync(id, messages);
        }

        private async Task SendMessageAsync(Guid userId, IEnumerable<MessageToast> messageToasts)
        {
            var session = _wsSessionManager.TryGetValue(userId);

            if (session is null)
            {
                return;
            }

            foreach (var message in messageToasts)
            {
                if (session.State == SuperSocket.SessionState.Connected)
                {
                    var msg = message.MapTo<MessageToastDto>().ToJson();

                    await session.SendAsync(msg);
                    _service.Db.Remove(msg);
                }
            }

            await _service.SaveAsync();
        }

        public async Task SendMessageAsync(MessageToastTemp messageTemp)
        {
            if (messageTemp.UserIds!.IsEmpty())
            {
                throw new ArgumentNullException("推送消息无法找到消息人");
            }

            if (messageTemp.MessageToast is null)
            {
                throw new ArgumentNullException("发送的消息不能为空");
            }

            var sessions = _wsSessionManager.GetAllSessions().Where(x => messageTemp.UserIds!.Contains(x.Key)).Select(x => x.Value);

            var message = messageTemp.MessageToast.MapTo<MessageToastDto>().ToJson();
            foreach (var session in sessions)
            {
                if (session.State == SuperSocket.SessionState.Connected)
                {
                    await session.SendAsync(message);
                }
                else
                {
                    messageTemp.MessageToast.UserId = session.UserId;
                    messageTemp.MessageToast.Create();
                    await _service.Db.AddAsync(messageTemp.MessageToast);
                }
            }

            await _service.SaveAsync();

            var unSendUserIds = _wsSessionManager.GetAllSessions().Where(x => !messageTemp.UserIds!.Contains(x.Key)).Select(x => x.Key);

            foreach (var item in unSendUserIds)
            {
                messageTemp.MessageToast.UserId = item;
                messageTemp.MessageToast.Create();
                await _service.Db.AddAsync(messageTemp.MessageToast);
            }

            await _service.SaveAsync();
        }

        public async Task SendMessageAsync(WorkTask entity, bool pick = false)
        {
            if (pick)
            {
                var user = await _service.Query<User>().Where(x => x.Id == entity.CreateUserId).FirstOrDefaultAsync();

                await SendMessageAsync(new MessageToastTemp
                {
                    UserIds = new List<Guid>() { user!.Id },
                    MessageToast = new MessageToast
                    {
                        Title = "任务已被接取",
                        Content = $"任务: {entity.Title} 被接取"
                    }
                });
            }

            if (entity.Status == WorkTaskStatus.PendingReview)
            {
                var users = await _service.Query<User>().Where(x => x.Role == Role.高层管理员).ToListAsync();

                await SendMessageAsync(new Shared.Temp.MessageToastTemp
                {
                    UserIds = users.Select(x => x.Id),
                    MessageToast = new MessageToast
                    {
                        Title = "待审核",
                        Content = $"任务: {entity.Title} 处于待审核状态"
                    }
                });
            }

            if (entity.Status == WorkTaskStatus.PendingPublish)
            {
                var users = await _service.Query<User>().Where(x => x.Role == Role.中层管理员).ToListAsync();

                await SendMessageAsync(new Shared.Temp.MessageToastTemp
                {
                    UserIds = users.Select(x => x.Id),
                    MessageToast = new MessageToast
                    {
                        Title = "待发布",
                        Content = $"任务: {entity.Title} 处于待发布状态"
                    }
                });
            }

            if (entity.Status == WorkTaskStatus.Overrule)
            {
                var user = await _service.Query<User>().Where(x => x.Id == entity.CreateUserId).FirstOrDefaultAsync();

                await SendMessageAsync(new MessageToastTemp
                {
                    UserIds = new List<Guid>() { user!.Id },
                    MessageToast = new MessageToast
                    {
                        Title = "被驳回",
                        Content = $"任务: {entity.Title} 处于被驳回状态"
                    }
                });
            }

            if (entity.Status == WorkTaskStatus.Active)
            {
                if (entity.WorkPublishType == WorkPublishType.全局发布)
                {
                    var users = await _service.Query<User>().Where(x => x.Role == Role.普通成员).ToListAsync();

                    await SendMessageAsync(new Shared.Temp.MessageToastTemp
                    {
                        UserIds = users.Select(x => x.Id),
                        MessageToast = new MessageToast
                        {
                            Title = "待接取",
                            Content = $"任务: {entity.Title} 处于待接取状态"
                        }
                    });
                }

                if (entity.WorkPublishType == WorkPublishType.科室发布)
                {
                    var users = await _service.Query<User>().Where(x => x.Role == Role.普通成员 && x.DepartmentId == entity.DepartmentId).ToListAsync();

                    await SendMessageAsync(new Shared.Temp.MessageToastTemp
                    {
                        UserIds = users.Select(x => x.Id),
                        MessageToast = new MessageToast
                        {
                            Title = "待接取",
                            Content = $"任务: {entity.Title} 处于待接取状态"
                        }
                    });
                }

                if (entity.WorkPublishType == WorkPublishType.自定义发布)
                {
                    var users = await _service.Query<User>().Where(x => x.Role == Role.普通成员 && (entity.PickUpUserIds ?? "").Contains(x.Id.ToString())).ToListAsync();

                    await SendMessageAsync(new Shared.Temp.MessageToastTemp
                    {
                        UserIds = users.Select(x => x.Id),
                        MessageToast = new MessageToast
                        {
                            Title = "待接取",
                            Content = $"任务: {entity.Title} 处于待接取状态"
                        }
                    });
                }
            }
        }
    }
}