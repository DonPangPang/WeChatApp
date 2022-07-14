using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Pang.AutoMapperMiddleware;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WeChatApp.Shared.Entity;
using WeChatApp.Shared.FormBody;
using WeChatApp.Shared.RequestBody.WeCom;
using WeChatApp.Shared.ResponseBody.WebApi;
using WeChatApp.Shared.ResponseBody.WeCom;
using WeChatApp.WebApp.Auth;
using WeChatApp.WebApp.Extensions;
using WeChatApp.WebApp.Filters;
using WeChatApp.WebApp.Services;

namespace WeChatApp.WebApp.Controllers
{
    /// <summary>
    /// 注册用户信息
    /// </summary>
    public class AuthorizationController : ApiController
    {
        private readonly IWeComServices _weComServices;
        private readonly Session _session;
        private readonly IServiceGen _serviceGen;
        private PermissionRequirement _tokenParameter;

        /// <summary>
        /// </summary>
        /// <param name="weComServices"> </param>
        /// <param name="session">       </param>
        /// <param name="serviceGen">    </param>
        /// <param name="configuration"> </param>
        public AuthorizationController(
            IWeComServices weComServices,
            Session session,
            IServiceGen serviceGen,
            IConfiguration configuration)
        {
            _weComServices = weComServices;
            _session = session;
            _serviceGen = serviceGen;

            _tokenParameter = configuration.GetSection("TokenParameter").Get<PermissionRequirement>();
        }

        /// <summary>
        /// 认证
        /// </summary>
        /// <param name="parameters"> </param>
        /// <returns> </returns>
        [HttpGet]
        public async Task<ActionResult> WeComRegisterAsync([FromQuery] GetUserInfoRequestParameters parameters)
        {
            var res = await _weComServices.GetUserInfoAsync(parameters);

            _session.Uid = res.UserId;
            _session.WeComUserInfo = res;
            _session.UserName = res.Name;

            var exist = await _serviceGen.Query<User>().Where(x => x.Uid.Equals(res.UserId)).FirstOrDefaultAsync();

            var user = new User
            {
                Uid = res.Mobile!,
                Name = res.Name ?? "未命名",
                Email = res.Email,
                Tel = res.Mobile!,
                //Role = res.IsLeader ? Shared.Enums.Role.Admin : Shared.Enums.Role.General
                Role = res.IsLeader ? Shared.Enums.Role.高层管理员 : Shared.Enums.Role.普通成员
            };

            if (exist is null)
            {
                user.Create();

                _session.UserId = user.Id;

                await _serviceGen.Db.AddAsync(user);

                var save = await _serviceGen.SaveAsync();

                if (!save) return Fail("保存用户失败");
            }
            else
            {
                user.Id = exist.Id;

                _session.UserId = user.Id;

                user.Map(exist);

                _serviceGen.Db.Update(exist);

                var save = await _serviceGen.SaveAsync();

                if (!save) return Fail("更新用户信息失败");
            }

            return Success("认证成功");
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="request"> </param>
        /// <returns> </returns>
        [HttpPost]
        public async Task<ActionResult> Login([FromBody] LoginDto request)
        {
            if (string.IsNullOrEmpty(request.UserName) || string.IsNullOrEmpty(request.Password))
                return Fail("Invalid Request");

            if (!await _serviceGen.Query<User>().Where(x => x.Uid.Equals(request.UserName)).AnyAsync())
            {
                return Fail("账号不存在");
            }

            var user = await _serviceGen.Query<User>().Where(x => x.Uid.Equals(request.UserName)).FirstOrDefaultAsync();
            if (user!.Password != request.Password)
            {
                return Fail("密码错误");
            }

            if (request.ClientUA == Shared.Enums.ClientUA.PC && user.Role == Shared.Enums.Role.普通成员)
            {
                return Fail("普通成员无权访问后台");
            }
            _session.Uid = user.Uid;
            _session.UserId = user.Id;
            _session.UserName = user.Name;

            //生成Token和RefreshToken
            var token = GenUserToken(user.Id, request.UserName, user.Role.ToString());
            var refreshToken = "123456";

            //await _redisHelper.SetStringAsync(user.Id.ToString(), token);

            //LoginUserInfo.Set(user);

            return Success(new RespToken { Token = token, RefreshToken = refreshToken });
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="dto"> </param>
        /// <returns> </returns>
        [HttpPost]
        public async Task<ActionResult> Register([FromBody] UserDto dto)
        {
            if (dto is null) throw new ArgumentNullException(nameof(dto));

            var user = dto.MapTo<User>();

            await _serviceGen.Db.AddAsync<User>(user);

            var res = await _serviceGen.SaveAsync();

            if (res) Success("注册成功");

            return Fail("注册失败");
        }

        /// <summary>
        /// 生成Token
        /// </summary>
        /// <param name="id">       </param>
        /// <param name="username"> </param>
        /// <param name="role">     </param>
        /// <returns> </returns>
        private string GenUserToken(Guid id, string username, string role)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Sid, id.ToString()),
                new Claim(ClaimTypes.DateOfBirth,DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenParameter.Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var jwtToken = new JwtSecurityToken(_tokenParameter.Issuer,
                                                _tokenParameter.Audience,
                                                claims,
                                                expires: DateTime.UtcNow.AddMinutes(_tokenParameter.AccessExpiration),
                                                signingCredentials: credentials);

            var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            return token;
        }
    }
}