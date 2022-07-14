using WeChatApp.Shared.FormBody;
using WeChatApp.Shared.RequestBody.WebApi;

namespace WeChatApp.AdminClient.Services
{
    public interface IWorkTaskNodeService
    {
        Task<T> GetWorkTaskNodeListAsync<T>(ParameterBase parameter);

        Task<T> GetWorkTaskNodeAsync<T>(Guid workTaskNodeId);

        Task<T> AddWorkTaskNodeAsync<T>(WorkTaskNodeDto dto);

        Task<T> EditWorkTaskNodeAsync<T>(WorkTaskNodeDto dto);

        Task<T> DeleteWorkTaskNodeAsync<T>(WorkTaskNodeDto dto);
    }
}