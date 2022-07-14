using WeChatApp.Shared.RequestBody.WebApi;

namespace WeChatApp.Shared.Interfaces
{
    /// <summary>
    /// 排序
    /// </summary>
    public interface ISorting : IParameterBase
    {
        /// <summary>
        /// 排序
        /// </summary>
        public string? OrderBy { get; set; }
    }
}