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
    public interface IBonusPointRecordService
    {
        Task<T> GetBonusPointRecordListAsync<T>(ParameterBase parameter);

        Task<T> GetBonusPointRecordAsync<T>(Guid bonusPointRecordId);

        Task<T> AddBonusPointRecordAsync<T>(BonusPointRecordDto dto);

        Task<T> EditBonusPointRecordAsync<T>(BonusPointRecordDto dto);

        Task<T> DeleteBonusPointRecordAsync<T>(BonusPointRecordDto dto);

        Task<T> GetRankingsAsync<T>(List<Guid> departmentIds);
    }

    public class BonusPointRecordService : IBonusPointRecordService
    {
        private readonly IHttpFunc _httpFunc;

        public BonusPointRecordService(IHttpFunc httpFunc)
        {
            _httpFunc = httpFunc;
        }

        public async Task<T> AddBonusPointRecordAsync<T>(BonusPointRecordDto dto)
        {
            var result = await _httpFunc.Create()
                .Url(ApiBase.Get("AddBonusPointRecord"))
                .Body(dto)
                .PostAsync<T>();
            return result;
        }

        public async Task<T> DeleteBonusPointRecordAsync<T>(BonusPointRecordDto dto)
        {
            var result = await _httpFunc.Create()
                .Url(ApiBase.Get("DeleteBonusPointRecord"))
                .Query(
                    new List<KeyValuePair<string, string>>{
                        new ("id", dto.Id.ToString())
                    }
                ).DeleteAsync<T>();
            return result;
        }

        public async Task<T> GetRankingsAsync<T>(List<Guid> departmentIds)
        {
            var result = await _httpFunc.Create()
                .Url(ApiBase.Get("GetRankings"))
                .Body(departmentIds)
                .PostAsync<T>();

            return result;
        }

        public async Task<T> EditBonusPointRecordAsync<T>(BonusPointRecordDto dto)
        {
            var result = await _httpFunc.Create()
                .Url(ApiBase.Get("EditBonusPointRecord"))
                .Body(dto)
                .PutAsync<T>();
            return result;
        }

        public async Task<T> GetBonusPointRecordAsync<T>(Guid bonusPointRecordId)
        {
            var result = await _httpFunc.Create()
                .Url(ApiBase.Get("GetBonusPointRecord"))
                .Query(
                    new List<KeyValuePair<string, string>>{
                        new ("id", bonusPointRecordId.ToString())
                    }
                ).GetAsync<T>();
            return result;
        }

        public async Task<T> GetBonusPointRecordListAsync<T>(ParameterBase parameter)
        {
            var result = await _httpFunc.Create()
                .Url(ApiBase.Get("GetBonusPointRecordList") + parameter.GetQueryString())
                .GetAsync<T>();

            return result;
        }
    }
}