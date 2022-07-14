namespace WeChatApp.Shared.Interfaces
{
    /// <summary>
    /// 公开
    /// </summary>
    public interface IPublic
    {
        /// <summary>
        /// 公示开始时间
        /// </summary>
        public DateTime PublicStartTime { get; set; }

        /// <summary>
        /// 公示结束时间
        /// </summary>
        public DateTime PublicEndTime { get; set; }
    }
}