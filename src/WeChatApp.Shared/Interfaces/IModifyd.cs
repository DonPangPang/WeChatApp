namespace WeChatApp.Shared.Interfaces
{
    /// <summary>
    /// 修改
    /// </summary>
    public interface IModifyed
    {
        /// <summary>
        /// 修改人Id
        /// </summary>
        public string? ModifyUserUid { get; set; }

        /// <summary>
        /// 修改人Id
        /// </summary>
        /// <value></value>
        public Guid? ModifyUserId { get; set; }

        /// <summary>
        /// 修改人姓名
        /// </summary>
        public string? ModifyUserName { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime ModifyTime { get; set; }
    }
}