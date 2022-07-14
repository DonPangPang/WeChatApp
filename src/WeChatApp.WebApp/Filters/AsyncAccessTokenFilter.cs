using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using WeChatApp.Shared.GlobalVars;
using WeChatApp.WebApp.Services;

namespace WeChatApp.WebApp.Filters
{
    /// <summary>
    /// 会话过滤
    /// </summary>
    public class AsyncAccessTokenFilter : IAsyncActionFilter
    {
        private readonly Session _session;
        private readonly IWeComServices _weComServices;
        private readonly IMemoryCache _cache;

        /// <summary>
        /// </summary>
        /// <param name="session">       </param>
        /// <param name="weComServices"> </param>
        /// <param name="cache">         </param>
        public AsyncAccessTokenFilter(
            Session session,
            IWeComServices weComServices,
            IMemoryCache cache)
        {
            _session = session;
            _weComServices = weComServices;
            _cache = cache;
        }

        /// <summary>
        /// </summary>
        /// <param name="context"> </param>
        /// <param name="next">    </param>
        /// <returns> </returns>
        /// <exception cref="NotImplementedException"> </exception>
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            _cache.TryGetValue(GlobalVars.AccessTokenKey, out string access_token);

            if (string.IsNullOrEmpty(access_token))
            {
                var res = await _weComServices.GetAccessTokenAsync();
                //access_token = res.access_token;
            }

            await next();
        }
    }
}