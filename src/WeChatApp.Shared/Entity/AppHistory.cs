using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeChatApp.Shared.Interfaces;

namespace WeChatApp.Shared.Entity
{
    /*

     {
        "msg": "操作成功",
        "code": 200,
        "data": {
            "createBy": "admin",
            "updateBy": "admin",
            "createTime": "2021-11-04 09:28:00",
            "updateTime": "2022-07-21 09:25:51",
            "remark": "APP更新json",
            "params": {},
            "configId": "6",
            "configName": "APP更新json",
            "configKey": "appjson",
            "configValue": "{\"code\": 0,\"version\": \"4.1\",\"downloadUrl\": \"http://1.192.213.158:83/apkupdate/hwry/xdhw-release-v4.1-1.apk\",\"describe\": \"●修改区域统计功能\\n●修复已知BUG\",\"updateType\": \"update\",\"htmlversion\": \"0\",\"downHtmlUrl\": \"\"}",
            "configType": "Y"
        }
     }

     */

    /// <summary>
    /// </summary>
    public class ConfigValue
    {
        public int Code = 0;
        public string? Version { get; set; }
        public string DownloadUrl { get; set; }
        public string Describe { get; set; }
    }

    /// <summary>
    /// App记录
    /// </summary>
    public class AppHistory : IEntity, ICreator, IModifyed
    {
        /// <summary>
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string? Remark { get; set; } = string.Empty;

        /// <summary>
        /// </summary>
        public string? Version { get; set; }

        /// <summary>
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// 路径
        /// </summary>
        public string? Source { get; set; }

        /// <summary>
        /// </summary>
        public string? CreateUserUid { get; set; }

        /// <summary>
        /// </summary>
        public Guid? CreateUserId { get; set; }

        /// <summary>
        /// </summary>
        public string? CreateUserName { get; set; }

        /// <summary>
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// </summary>
        public string? ModifyUserUid { get; set; }

        /// <summary>
        /// </summary>
        public Guid? ModifyUserId { get; set; }

        /// <summary>
        /// </summary>
        public string? ModifyUserName { get; set; }

        /// <summary>
        /// </summary>
        public DateTime ModifyTime { get; set; }
    }
}