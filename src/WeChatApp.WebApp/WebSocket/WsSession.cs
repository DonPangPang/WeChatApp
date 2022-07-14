using SuperSocket.Server;
using SuperSocket.WebSocket.Server;

namespace WeChatApp.WebApp.WebSocket
{
    public class WsSession : WebSocketSession
    {
        public Guid UserId { get; set; }
    }
}