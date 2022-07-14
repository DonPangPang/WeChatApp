using WeChatApp.Shared.Entity;
using WeChatApp.Shared.Temp;

namespace WeChatApp.WebApp.Services
{
    /// <summary>
    /// 消息推送服务
    /// </summary>
    public interface IMessageToastService
    {
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="messageTemp"> </param>
        /// <returns> </returns>
        Task SendMessageAsync(MessageToastTemp messageTemp);

        /// <summary>
        /// 重发消息
        /// </summary>
        /// <param name="id"> </param>
        /// <returns> </returns>
        Task ReSendMessageAsync(Guid id);

        /// <summary>
        /// 重新发送消息
        /// </summary>
        /// <returns> </returns>
        Task ReSendMessageAsync();

        Task SendMessageAsync(WorkTask entity, bool pick = false);
    }
}