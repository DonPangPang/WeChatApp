using Microsoft.AspNetCore.Components;
using System.Web;

namespace WeChatApp.Client.Extensions
{
    public static class NavExtension
    {
        public static string Get(this NavigationManager nav, string paramName)
        {
            var uri = nav.ToAbsoluteUri(nav.Uri);
            string paramValue = HttpUtility.ParseQueryString(uri.Query).Get(paramName) ?? "";
            return paramValue;
        }

        public static string Get(this NavigationManager nav)
        {
            return nav.Uri;
        }
    }
}
