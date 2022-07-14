using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WeChatApp.Shared;

namespace WeChatApp.WebApp.Filters
{
    /// <summary>
    /// 全局错误筛选器
    /// </summary>
    public class AsyncExceptionFilter : IAsyncExceptionFilter
    {
        /// <summary>
        /// </summary>
        /// <param name="context"> </param>
        /// <returns> </returns>
        public Task OnExceptionAsync(ExceptionContext context)
        {
            OkObjectResult result = new OkObjectResult(new WcResponse
            {
                Code = WcStatus.Error,
                Message = "服务器异常.",
                Data = context.Exception.Message
            });

            context.Result = result;
            context.ExceptionHandled = true;

            return Task.FromResult(result);
        }
    }
}