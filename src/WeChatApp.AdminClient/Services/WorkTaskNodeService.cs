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
    public class WorkTaskNodeService : IWorkTaskNodeService
    {
        private readonly IHttpFunc _httpFunc;

        public WorkTaskNodeService(IHttpFunc httpFunc)
        {
            _httpFunc = httpFunc;
        }

        public async Task<T> AddWorkTaskNodeAsync<T>(WorkTaskNodeDto dto)
        {
            var result = await _httpFunc.Create()
                .Url(ApiBase.Get("AddWorkTaskNode"))
                .Body(dto)
                .PostAsync<T>();
            return result;
        }

        public async Task<T> DeleteWorkTaskNodeAsync<T>(WorkTaskNodeDto dto)
        {
            var result = await _httpFunc.Create()
                .Url(ApiBase.Get("DeleteWorkTaskNode"))
                .Query(
                    new List<KeyValuePair<string, string>>{
                        new ("id", dto.Id.ToString())
                    }
                ).DeleteAsync<T>();
            return result;
        }

        public async Task<T> EditWorkTaskNodeAsync<T>(WorkTaskNodeDto dto)
        {
            var result = await _httpFunc.Create()
                .Url(ApiBase.Get("EditWorkTaskNode"))
                .Body(dto)
                .PutAsync<T>();
            return result;
        }

        public async Task<T> GetWorkTaskNodeAsync<T>(Guid workTaskNodeId)
        {
            var result = await _httpFunc.Create()
                .Url(ApiBase.Get("GetWorkTaskNode"))
                .Query(
                    new List<KeyValuePair<string, string>>{
                        new ("id", workTaskNodeId.ToString())
                    }
                ).GetAsync<T>();

            return result;
        }

        public async Task<T> GetWorkTaskNodeListAsync<T>(ParameterBase parameter)
        {
            var result = await _httpFunc.Create()
                .Url(ApiBase.Get("GetWorkTaskNodeList") + parameter.GetQueryString())
                .GetAsync<T>();
            return result;
        }
    }
}