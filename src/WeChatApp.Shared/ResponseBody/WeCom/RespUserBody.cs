namespace WeChatApp.Shared.ResponseBody.WeCom;

/// <summary>
/// </summary>
public class RespUserBody : RespBodyBase
{
    /// <summary>
    /// </summary>
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public Guid DeviceId { get; set; }
}