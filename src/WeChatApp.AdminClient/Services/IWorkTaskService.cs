using WeChatApp.Shared.FormBody;
using WeChatApp.Shared.RequestBody.WebApi;

namespace WeChatApp.AdminClient.Services
{
    public interface IWorkTaskService
    {
        Task<T> GetWorkTaskListAsync<T>(ParameterBase parameter);

        Task<T> GetWorkTaskAsync<T>(Guid workTaskId);

        Task<T> AddWorkTaskAsync<T>(WorkTaskDto dto);

        Task<T> EditWorkTaskAsync<T>(WorkTaskDto dto);

        Task<T> DeleteWorkTaskAsync<T>(WorkTaskDto dto);

        Task<T> GetWorkTaskIndex<T>(WorkTaskIndexParameters parameters);
    }
}