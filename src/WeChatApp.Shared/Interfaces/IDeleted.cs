namespace WeChatApp.Shared.Interfaces
{
    /// <summary>
    /// 删除
    /// </summary>
    public interface IDeleted
    {
        /// <summary>
        /// 删除标记
        /// </summary>
        public bool IsDeleted { get; set; }
    }
}