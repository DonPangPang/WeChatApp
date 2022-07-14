using System.Linq;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using WeChatApp.Shared.Entity;
using WeChatApp.WebApp.Services;
using Microsoft.EntityFrameworkCore;
using WeChatApp.WebApp.Filters;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using WeChatApp.Shared;

namespace WeChatApp.WebApp.Auth
{
    /// <summary>
    /// 重写Permission
    /// </summary>
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        //private readonly RedisHelper _redisHelper;
        private readonly PermissionRequirement _tokenParameter;

        private readonly IServiceGen _service;

        private Session _session;

        /// <summary>
        /// </summary>
        /// <param name="config">              </param>
        /// <param name="httpContextAccessor"> </param>
        /// <param name="service">             </param>
        /// <param name="session">             </param>
        public PermissionHandler(
            IConfiguration config,
            IHttpContextAccessor httpContextAccessor,
            IServiceGen service,
            Session session)
        {
            _httpContextAccessor = httpContextAccessor;
            //_redisHelper = redisHelper;
            _tokenParameter = config.GetSection("TokenParameter").Get<PermissionRequirement>();
            _service = service;
            _session = session;
        }

        /// <summary>
        /// </summary>
        /// <param name="context">     </param>
        /// <param name="requirement"> </param>
        /// <returns> </returns>
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            // 校验 颁发和接收对象
            if (!context.User.HasClaim(c => c.Type == ClaimTypes.DateOfBirth &&
                                            c.Issuer == _tokenParameter.Issuer))
            {
                await Task.CompletedTask;
            }

            var dateOfBirth = Convert.ToDateTime(context.User.FindFirst(c => c.Type == ClaimTypes.DateOfBirth &&
                                                                             c.Issuer == _tokenParameter.Issuer)
                ?.Value);

            // var test =
            // TimeZone.CurrentTimeZone.ToLocalTime(Convert.ToDateTime(_tokenParameter.AccessExpiration)); 校验过期时间
            var accessExpiration = dateOfBirth.AddMinutes(_tokenParameter.AccessExpiration);
            var nowExpiration = DateTime.Now;
            if (accessExpiration < nowExpiration)
            {
                context.Fail();
                await Task.CompletedTask;
                return;
            }

            var id = Guid.Parse(context.User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Sid))!.Value);

            var user = await _service.Query<User>().Where(x => x.Id == id).FirstOrDefaultAsync();

            if (user is not null)
            {
                _session.Uid = user.Uid;
                _session.UserId = user.Id;
                _session.UserName = user.Name;
                _session.UserInfo = user;
            }
            else
            {
                context.Fail();
                await Task.CompletedTask;
                return;
            }

            //var token = await _redisHelper.GetStringAsync(id.ToString());

            //var elderToken = await _httpContextAccessor.HttpContext!.GetTokenAsync("Bearer", "access_token");

            //if (!token.Equals(elderToken))
            //{
            //    context.Fail();
            //    await Task.CompletedTask;
            //    return;
            //}

            // var questUrl = _httpContextAccessor.HttpContext!.Request.Path.ToString();

            // if (!user.Roles!.Any(x => x.Modules!.Any(t => questUrl.Contains(t.Name)))) {
            // context.Fail(); await Task.CompletedTask; return; }

            context.Succeed(requirement);
            await Task.CompletedTask;
        }
    }
}