using Microsoft.AspNetCore.Mvc;
using WeChatApp.Shared.Entity;
using WeChatApp.Shared.FormBody;
using WeChatApp.WebApp.Services;

namespace WeChatApp.WebApp.Controllers
{
    /// <summary>
    /// 工作任务节点
    /// </summary>
    public class WorkTaskNodeController : ApiController<WorkTaskNode, WorkTaskNodeDto>
    {
        private readonly IServiceGen _serviceGen;

        /// <summary>
        /// </summary>
        /// <param name="serviceGen"> </param>
        public WorkTaskNodeController(IServiceGen serviceGen) : base(serviceGen)
        {
            _serviceGen = serviceGen;
        }
    }
}