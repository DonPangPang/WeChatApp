using Microsoft.EntityFrameworkCore;
using WeChatApp.Shared.Entity;
using WeChatApp.WebApp.Services;

namespace WeChatApp.WebApp.HangfireTasks
{
    public class WorkTaskJob : IHangFireJob
    {
        private readonly IServiceGen _serviceGen;

        public WorkTaskJob(IServiceGen serviceGen)
        {
            _serviceGen = serviceGen;
        }

        /// <summary>
        /// 执行
        /// </summary>
        public async Task Execute()
        {
            var tasks = await _serviceGen.Query<WorkTask>().Where(x => x.EndTime <= DateTime.Now.Date && x.EndTime > DateTime.MinValue)
                .ToListAsync();

            foreach (var item in tasks)
            {
                item.Status = Shared.Enums.WorkTaskStatus.End;
            }

            _serviceGen.Db.UpdateRange(tasks);
            await _serviceGen.Db.SaveChangesAsync();
        }
    }
}