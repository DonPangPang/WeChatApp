using WeChatApp.Shared.Entity;
using WeChatApp.Shared.ResponseBody.WeCom;

namespace WeChatApp.WebApp.Filters
{
    /// <summary>
    /// 用户会话
    /// </summary>
    public class Session
    {
        /// <summary>
        /// Uid
        /// </summary>
        public string? Uid { get; set; }

        /// <summary>
        /// UserId
        /// </summary>
        /// <value></value>
        public Guid UserId { get; set; } = Guid.Empty;

        /// <summary>
        /// 用户名
        /// </summary>
        /// <value></value>
        public string? UserName { get; set; } = string.Empty;

        /// <summary>
        /// 用户信息
        /// </summary>
        public RespUserInfoBody? WeComUserInfo { get; set; }

        /// <summary>
        /// User
        /// </summary>
        /// <value></value>
        public User? UserInfo { get; set; }
    }
}