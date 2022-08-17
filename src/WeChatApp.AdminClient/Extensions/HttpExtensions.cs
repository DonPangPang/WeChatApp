using WeChatApp.Shared;

namespace WeChatApp.AdminClient.Extensions;

public static class HttpExtensions
{
    public static bool IsSuccess<T>(this WcResponse<T> result)
    {
        if (result.Code == WcStatus.Success)
            return true;

        return false;
    }
}