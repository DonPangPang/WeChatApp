namespace WeChatApp.Shared;

/// <summary>
/// 统一返回
/// </summary>
public interface IResponse
{
}

/// <summary>
/// 统一返回
/// </summary>
public class WcResponse : IResponse
{
    /// <summary>
    /// 状态码
    /// </summary>
    public WcStatus Code { get; set; } = WcStatus.Success;

    /// <summary>
    /// 消息
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// 数据
    /// </summary>
    public object? Data { get; set; }

    /// <summary>
    /// 总数
    /// </summary>
    /// <value></value>
    public int TotalCount { get; set; } = 0;
}

/// <summary>
/// </summary>
/// <typeparam name="T"> </typeparam>
public class WcResponse<T> : IResponse
{
    /// <summary>
    /// 状态码
    /// </summary>
    public WcStatus Code { get; set; } = WcStatus.Success;

    /// <summary>
    /// 消息
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// 数据
    /// </summary>
    public T? Data { get; set; }

    /// <summary>
    /// 总数
    /// </summary>
    /// <value></value>
    public int TotalCount { get; set; } = 0;
}

/// <summary>
/// 状态码
/// </summary>
public enum WcStatus
{
    /// <summary>
    /// 成功
    /// </summary>
    Success = 200,

    /// <summary>
    /// 失败
    /// </summary>
    Fail = 400,

    /// <summary>
    /// Token失效
    /// </summary>
    TokenExpired = 401,

    /// <summary>
    /// 错误
    /// </summary>
    Error = 500,
}