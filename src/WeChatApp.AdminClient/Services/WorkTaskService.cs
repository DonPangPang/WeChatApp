using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeChatApp.AdminClient.Apis;
using WeChatApp.Shared.Extensions;
using WeChatApp.Shared.FormBody;
using WeChatApp.Shared.RequestBody.WebApi;

namespace WeChatApp.AdminClient.Services
{

    public class WorkTaskService : IWorkTaskService
    {
        private readonly IHttpFunc _httpFunc;

        public WorkTaskService(IHttpFunc httpFunc)
        {
            _httpFunc = httpFunc;
        }

        public async Task<T> AddWorkTaskAsync<T>(WorkTaskDto dto)
        {
            var result = await _httpFunc.Create()
                .Url(ApiBase.Get("AddWorkTask"))
                .Body(dto)
                .PostAsync<T>();

            return result;
        }

        public async Task<T> DeleteWorkTaskAsync<T>(WorkTaskDto dto)
        {
            var result = await _httpFunc.Create()
                .Url(ApiBase.Get("DeleteWorkTask"))
                .Query(
                    new List<KeyValuePair<string, string>>{
                        new ("id", dto.Id.ToString())
                    }
                ).DeleteAsync<T>();
            return result;
        }

        public async Task<T> GetWorkTaskIndex<T>(WorkTaskIndexParameters parameters)
        {
            var result = await _httpFunc.Create()
                .Url(ApiBase.Get("GetWorkTaskIndex") + parameters.GetQueryString())
                .GetAsync<T>();

            return result;
        }

        public async Task<T> EditWorkTaskAsync<T>(WorkTaskDto dto)
        {
            var result = await _httpFunc.Create()
                .Url(ApiBase.Get("EditWorkTask"))
                .Body(dto)
                .PutAsync<T>();

            return result;
        }

        public async Task<T> GetWorkTaskAsync<T>(Guid workTaskId)
        {
            var result = await _httpFunc.Create()
                .Url(ApiBase.Get("GetWorkTask"))
                .Query(
                    new List<KeyValuePair<string, string>>{
                        new ("id", workTaskId.ToString())
                    }
                ).GetAsync<T>();

            return result;
        }

        public async Task<T> GetWorkTaskListAsync<T>(ParameterBase parameter)
        {
            var result = await _httpFunc.Create()
                .Url(ApiBase.Get("GetWorkTaskList") + parameter.GetQueryString())
                .GetAsync<T>();
            return result;
        }
    }
}