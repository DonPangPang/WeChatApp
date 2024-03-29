﻿namespace WeChatApp.Shared.GlobalVars;

/// <summary>
/// 常量配置
/// </summary>
public static class GlobalVars
{
    /// <summary>
    /// Token内存存储Key
    /// </summary>
    public const string AccessTokenKey = "access_token";

    /// <summary>
    /// 权限策略名称
    /// </summary>
    public const string Permission = "WeIdentify";

    /// <summary>
    /// 跨域策略名称
    /// </summary>
    public const string Cors = "WeCors";

    /// <summary>
    /// 客户端Token存储的名称
    /// </summary>
    public const string ClientTokenKey = "client_token";

    /// <summary>
    /// 用户信息
    /// </summary>
    public const string ClientUserInfo = "client_userInfo";

    /// <summary>
    /// 全局任务指定人员名称
    /// </summary>
    public const string GlobalTaskAssign = "指派人员";

    /// <summary>
    /// </summary>
    public const string DeptTaskAssign = "全部人员";

    /// <summary>
    /// 安装包上传路径
    /// </summary>
    public const string ApksPath = "uploads/apks/";

    /// <summary>
    /// 图片上传路径
    /// </summary>
    public const string ImagesPath = "uploads/images/";
}