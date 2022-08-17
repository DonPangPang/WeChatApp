using Microsoft.AspNetCore.Components.Forms;
using WeChatApp.AdminClient.Apis;
using WeChatApp.Shared.Extensions;
using WeChatApp.Shared.FormBody;
using WeChatApp.Shared.RequestBody.WebApi;

namespace WeChatApp.AdminClient.Services
{
    public interface IApkHistoryService
    {
        Task<T> GetPageListAsync<T>(ParameterBase parameter);

        Task<T> GetNewestVersionAsync<T>();

        Task<T> AddAppVersionAsync<T>(AppHistoryUploadDto dto);

        Task<T> DeleteVersionAsync<T>(AppHistoryDto dto);

        Task<T> EditVersionAsync<T>(AppHistoryDto dto);

        Task<T> UploadAppVersion<T>(IBrowserFile file);
    }

    public class ApkHistoryService : IApkHistoryService
    {
        private readonly IHttpFunc _httpFunc;

        public ApkHistoryService(IHttpFunc httpFunc)
        {
            _httpFunc = httpFunc;
        }

        public async Task<T> AddAppVersionAsync<T>(AppHistoryUploadDto dto)
        {
            var result = await _httpFunc.Create()
                .Url(ApiBase.Get("AddAppVersion"))
                .Body(dto)
                .PostAsync<T>();

            return result;
        }

        public async Task<T> DeleteVersionAsync<T>(AppHistoryDto dto)
        {
            var result = await _httpFunc.Create()
                .Url(ApiBase.Get("DeleteAppVersion"))
                .Query(new List<KeyValuePair<string, string>>{
                    new("id", dto.Id.ToString())
                }).DeleteAsync<T>();

            return result;
        }

        public async Task<T> EditVersionAsync<T>(AppHistoryDto dto)
        {
            var result = await _httpFunc.Create()
                .Url(ApiBase.Get("EditAppVersion"))
                .Body(dto)
                .PostAsync<T>();

            return result;
        }

        public async Task<T> GetNewestVersionAsync<T>()
        {
            var result = await _httpFunc.Create()
                .Url(ApiBase.Get("GetNewestVersion"))
                .GetAsync<T>();

            return result;
        }

        public async Task<T> GetPageListAsync<T>(ParameterBase parameter)
        {
            var result = await _httpFunc.Create()
                .Url(ApiBase.Get("GetApkHistoryList") + parameter.GetQueryString())
                .GetAsync<T>();

            return result;
        }

        public async Task<T> UploadAppVersion<T>(IBrowserFile file)
        {
            var result = await _httpFunc.Create()
                .Url(ApiBase.Get("UploadAppVersion"))
                .File(file)
                .PostAsync<T>();

            return result;
        }
    }
}