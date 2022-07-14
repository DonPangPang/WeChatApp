using WeChatApp.Shared.Extensions;
using WeChatApp.WebApp.WebSocket;

namespace WeChatApp.WebApp.Extensions
{
    public static class WsSessionExtensions
    {
        public static WsSession FormatParameter(this WsSession session)
        {
            var path = session.Path;
            var paras = path.Split("?")[1].Split("&");
            var paraDict = new Dictionary<string, string>();
            foreach (var para in paras)
            {
                var line = para.Split("=");
                paraDict.Add(line[0], line[1]);
            }
            session.UserId = paraDict["token"].ToGuid();
            //session.SIM = paraDict["sim"]; session.ChannelNo = Convert.ToInt32(paraDict["channel"]);

            return session;
        }
    }
}