using WeChatApp.Shared.RequestBody.WebApi;

namespace WeChatApp.Shared.Interfaces
{
    /// <summary>
    /// 分页
    /// </summary>
    public interface IPaging : IParameterBase
    {
        /// <summary>
        /// 页码
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// 每页大小
        /// </summary>
        public int PageSize { get; set; }
    }
}