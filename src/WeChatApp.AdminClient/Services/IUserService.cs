using WeChatApp.Shared;
using WeChatApp.Shared.FormBody;
using WeChatApp.Shared.RequestBody.WebApi;
using WeChatApp.Shared.ResponseBody.WebApi;

namespace WeChatApp.AdminClient.Services
{
    public interface IUserService
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="dto"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<T> LoginAsync<T>(LoginDto dto);
        /// <summary>
        /// 获取一个用户的信息
        /// </summary>
        /// <param name="userId"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<T> GetUserAsync<T>(string userId);
        /// <summary>
        /// 获取当前登陆人的信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<T> GetLoginUserAsync<T>();
        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="parameter"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<T> GetUserListAsync<T>(ParameterBase parameter);
        /// <summary>
        /// 编辑用户信息
        /// </summary>
        /// <param name="dto"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<T> EditUserAsync<T>(UserDto dto);
        /// <summary>
        /// 删除用户信息
        /// </summary>
        /// <param name="dto"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<T> DeleteUserAsync<T>(UserDto dto);
        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="dto"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        Task<T> AddUserAsync<T>(UserDto dto);
    }
}