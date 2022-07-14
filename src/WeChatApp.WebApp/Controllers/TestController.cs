using Microsoft.AspNetCore.Mvc;
using Pang.AutoMapperMiddleware;
using WeChatApp.Shared.Entity;
using WeChatApp.Shared.FormBody;
using WeChatApp.WebApp.Services;

namespace WeChatApp.WebApp.Controllers;

/// <summary>
/// 测试接口
/// </summary>
public class TestController : ApiController
{
    private readonly IWeComServices _weComServices;
    private readonly IMessageToastService _messageToastService;

    /// <summary>
    /// </summary>
    /// <param name="weComServices">       </param>
    /// <param name="messageToastService"> </param>
    public TestController(IWeComServices weComServices, IMessageToastService messageToastService)
    {
        _weComServices = weComServices;
        _messageToastService = messageToastService;
    }

    /// <summary>
    /// [WeCom]获取AccessToken
    /// </summary>
    /// <returns> </returns>
    [HttpGet]
    public async Task<ActionResult> GetTokenAsync()
    {
        var res = await _weComServices.GetAccessTokenAsync();

        if (res.errcode != 0)
        {
            return Fail("获取access_token失败");
        }

        return Success(res);
    }

    public class SendMessageDto
    {
        /// <summary>
        /// UserId集合
        /// </summary>
        public IEnumerable<Guid> UserIds { get; set; } = new List<Guid>();

        /// <summary>
        /// 消息
        /// </summary>
        public MessageToastDto Message { get; set; } = new();
    }

    /// <summary>
    /// 测试发送消息
    /// </summary>
    /// <param name="dto"> </param>
    /// <returns> </returns>
    [HttpPost]
    public async Task<ActionResult> SendMessage(SendMessageDto dto)
    {
        var message = dto.Message.MapTo<MessageToast>();
        await _messageToastService.SendMessageAsync(new Shared.Temp.MessageToastTemp
        {
            UserIds = dto.UserIds,
            MessageToast = message
        });

        return Success("调用成功");
    }
}