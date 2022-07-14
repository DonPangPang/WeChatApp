using System.Net.Http.Json;
using WeChatApp.AdminClient.Apis;
using WeChatApp.EzHttpClient.Extensions;
using WeChatApp.Shared;
using WeChatApp.Shared.FormBody;
using WeChatApp.Shared.RequestBody.WebApi;
using WeChatApp.Shared.ResponseBody.WebApi;
using WeChatApp.Shared.Extensions;

namespace WeChatApp.AdminClient.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpFunc _HttpFunc;

        public UserService(IHttpFunc HttpFunc)
        {
            _HttpFunc = HttpFunc;
        }

        public async Task<T> AddUserAsync<T>(UserDto dto)
        {
            var result = await _HttpFunc.Create()
                .Url(ApiBase.Get("AddUser"))
                .Body(dto)
                .PostAsync<T>();

            return result;
        }

        public async Task<T> DeleteUserAsync<T>(UserDto dto)
        {
            var result = await _HttpFunc.Create()
                .Url(ApiBase.Get("DeleteUser"))
                .Query(
                    new List<KeyValuePair<string, string>>{
                        new ("id", dto.Id.ToString())
                    }
                )
                .DeleteAsync<T>();

            return result;
        }

        public async Task<T> EditUserAsync<T>(UserDto dto)
        {
            var res = await _HttpFunc.Create()
                .Url(ApiBase.Get("EditUser"))
                .Body(dto)
                .PutAsync<T>();

            return res;
        }

        public async Task<T> GetLoginUserAsync<T>()
        {
            var result = await _HttpFunc.Create()
                .Url(ApiBase.Get("GetUserInfo"))
                .GetAsync<T>();

            return result;
        }

        public async Task<T> GetUserAsync<T>(string userId)
        {
            var result = await _HttpFunc.Create()
                .Url(ApiBase.Get("GetUser"))
                .GetAsync<T>();

            return result;
        }

        public async Task<T> GetUserListAsync<T>(ParameterBase parameter)
        {
            var result = await _HttpFunc.Create()
                .Url(ApiBase.Get("GetUserList") + parameter.GetQueryString())
                .GetAsync<T>();

            return result;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="dto"> </param>
        /// <returns> </returns>
        public async Task<T> LoginAsync<T>(LoginDto dto)
        {
            var result = await _HttpFunc.Create()
                .Url(ApiBase.Get("Login"))
                .Body(dto)
                .PostAsync<T>();

            return result;
        }
    }
}