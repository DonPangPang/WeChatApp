using AutoMapper;
using WeChatApp.Shared.FormBody;

namespace WeChatApp.Shared.ResponseBody.WeCom;

/// <summary>
/// </summary>
[AutoMap(typeof(UserDto), ReverseMap = true)]
public class RespUserInfoBody : RespBodyBase
{
    /// <summary>
    /// </summary>
    public string? UserId { get; set; }

    /// <summary>
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// </summary>
    public int[]? Department { get; set; }

    /// <summary>
    /// 职务
    /// </summary>
    public string? Position { get; set; }

    /// <summary>
    /// </summary>
    public string? Mobile { get; set; }

    /// <summary>
    /// </summary>
    public Gender Gender { get; set; }

    /// <summary>
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// 头像
    /// </summary>
    public string? Avatar { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    public int Status { get; set; }

    /// <summary>
    /// 是否是领导
    /// </summary>
    public bool IsLeader { get; set; }
}