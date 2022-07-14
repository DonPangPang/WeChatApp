using Microsoft.AspNetCore.Mvc.Filters;

namespace WeChatApp.WebApp.Filters
{
    /// <summary>
    /// </summary>
    public class AsyncAuthFilter : IAsyncAuthorizationFilter
    {
        /// <summary>
        /// </summary>
        /// <param name="context"> </param>
        /// <returns> </returns>
        /// <exception cref="NotImplementedException"> </exception>
        public Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            throw new NotImplementedException();
        }
    }
}