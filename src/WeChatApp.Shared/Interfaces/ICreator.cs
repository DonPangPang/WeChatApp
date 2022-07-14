namespace WeChatApp.Shared.Interfaces
{
    /// <summary>
    /// 创建
    /// </summary>
    public interface ICreator
    {
        /// <summary>
        /// 创建人Id(Uid)
        /// </summary>
        public string? CreateUserUid { get; set; }

        /// <summary>
        /// 创建人Id
        /// </summary>
        /// <value></value>
        public Guid? CreateUserId { get; set; }

        /// <summary>
        /// 创建人姓名
        /// </summary>
        public string? CreateUserName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}