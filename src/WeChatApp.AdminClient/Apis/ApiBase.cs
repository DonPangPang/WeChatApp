using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeChatApp.Shared.GlobalVars;

namespace WeChatApp.AdminClient.Apis
{
    public static class ApiBase
    {
        public static string ApiBaseUrl = ApiVars.ApiBase;

        private static Dictionary<string, string> ApiDict = new Dictionary<string, string>
        {
            { "Login", "/api/Authorization/Login" },
            { "GetUserInfo", "/api/User/GetUserInfo" },
            { "GetUser", "/api/User/GetEntity" },
            { "GetUserList", "/api/User/GetUserPagedList" },
            { "AddUser", "/api/User/CreateEntity" },
            { "EditUser", "/api/User/UpdateEntity" },
            { "DeleteUser", "/api/User/DeleteEntity" },

            { "GetDepartmentList", "/api/Department/GetPagedList" },
            { "GetDepartment", "/api/Department/GetEntity" },
            { "EditDepartment", "/api/Department/UpdateEntity" },
            { "DeleteDepartment", "/api/Department/DeleteEntity" },
            { "AddDepartment", "/api/Department/CreateEntity" },
            { "GetDepartmentTree", "/api/Department/GetTree" },
            { "GetDepartmentTreeWithUser", "/api/Department/GetTreeWithUser" },

            { "GetWorkTaskList", "/api/WorkTask/GetWorkTaskList" },
            { "GetWorkTask", "/api/WorkTask/GetEntity" },
            { "AddWorkTask", "/api/WorkTask/CreateEntity" },
            { "EditWorkTask", "/api/WorkTask/UpdateEntity" },
            { "DeleteWorkTask", "/api/WorkTask/DeleteEntity" },

            { "GetWorkTaskNodeList", "/api/WorkTaskNode/GetPagedList" },
            { "GetWorkTaskNode", "/api/WorkTaskNode/GetEntity" },
            { "AddWorkTaskNode", "/api/WorkTaskNode/CreateEntity" },
            { "EditWorkTaskNode", "/api/WorkTaskNode/UpdateEntity" },
            { "DeleteWorkTaskNode", "/api/WorkTaskNode/DeleteEntity" },

            {"GetBonusPointRecordList", "api/BonusPointRecord/GetPagedList"},
            {"GetBonusPointRecord", "api/BonusPointRecord/GetEntity"},
            {"AddBonusPointRecord", "api/BonusPointRecord/CreateEntity"},
            {"EditBonusPointRecord", "api/BonusPointRecord/UpdateEntity"},
            {"DeleteBonusPointRecord", "api/BonusPointRecord/DeleteEntity"},
        };

        public static string Get(string key)
        {
            return ApiDict.TryGetValue(key, out var value) ? value : throw new ArgumentNullException("null api");
        }
    }
}