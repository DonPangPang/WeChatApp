using WeChatApp.Shared.RequestBody.WebApi;
using WeChatApp.Shared.FormBody;
using WeChatApp.AdminClient.Apis;
using WeChatApp.Shared.Extensions;

namespace WeChatApp.AdminClient.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IHttpFunc _HttpFunc;
        public DepartmentService(IHttpFunc HttpFunc)
        {
            _HttpFunc = HttpFunc;
        }

        public async Task<T> AddDepartmentAsync<T>(DepartmentDto dto)
        {
            var result = await _HttpFunc.Create()
                .Url(ApiBase.Get("AddDepartment"))
                .Body(dto)
                .PostAsync<T>();

            return result;
        }

        public async Task<T> DeleteDepartmentAsync<T>(DepartmentDto dto)
        {
            var result = await _HttpFunc.Create()
                .Url(ApiBase.Get("DeleteDepartment"))
                .Query(
                    new List<KeyValuePair<string, string>>{
                        new ("id", dto.Id.ToString())
                    }
                ).DeleteAsync<T>();

            return result;
        }

        public async Task<T> EditDepartmentAsync<T>(DepartmentDto dto)
        {
            var result = await _HttpFunc.Create()
                .Url(ApiBase.Get("EditDepartment"))
                .Body(dto)
                .PutAsync<T>();

            return result;
        }

        public async Task<T> GetDepartmentAsync<T>(Guid departmentId)
        {
            var result = await _HttpFunc.Create()
                .Url(ApiBase.Get("GetDepartment"))
                .Query(
                    new List<KeyValuePair<string, string>>{
                        new ("id", departmentId.ToString())
                    }
                ).GetAsync<T>();

            return result;
        }

        public async Task<T> GetDepartmentListAsync<T>(ParameterBase parameter)
        {
            var result = await _HttpFunc.Create()
                .Url(ApiBase.Get("GetDepartmentList") + parameter.GetQueryString())
                .GetAsync<T>();

            return result;
        }

        public async Task<T> GetDepartmentTreeAsync<T>()
        {
            var result = await _HttpFunc.Create()
                .Url(ApiBase.Get("GetDepartmentTree"))
                .GetAsync<T>();

            return result;
        }
    }
}