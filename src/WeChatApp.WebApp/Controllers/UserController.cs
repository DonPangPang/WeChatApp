using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop.Infrastructure;
using Pang.AutoMapperMiddleware;
using WeChatApp.Shared.Entity;
using WeChatApp.Shared.Extensions;
using WeChatApp.Shared.FormBody;
using WeChatApp.Shared.RequestBody.WebApi;
using WeChatApp.Shared.RequestBody.WeCom;
using WeChatApp.WebApp.Extensions;
using WeChatApp.WebApp.Filters;
using WeChatApp.WebApp.Services;

namespace WeChatApp.WebApp.Controllers;

/// <summary>
/// 用户接口
/// </summary>
public class UserController : ApiController<User, UserDto>
{
    private readonly IServiceGen _serviceGen;
    private readonly IWeComServices _weComServices;
    private readonly Session _session;

    /// <summary>
    /// </summary>
    /// <param name="serviceGen">    </param>
    /// <param name="weComServices"> </param>
    /// <param name="session">       </param>
    public UserController(
        IServiceGen serviceGen,
        IWeComServices weComServices,
        Session session)
        : base(serviceGen)
    {
        _serviceGen = serviceGen;
        _weComServices = weComServices;
        _session = session;
    }

    ///// <summary>
    ///// 获取用户信息
    ///// </summary>
    ///// <param name="parameters"> </param>
    ///// <returns> </returns>
    //[HttpGet]
    //public async Task<ActionResult> GetUserInfoAsync([FromQuery] GetUserInfoRequestParameters parameters)
    //{
    //    var res = await _weComServices.GetUserInfoAsync(parameters);

    //    return Success(res);
    //}

    /// <summary>
    /// 获取当前登录的用户信息
    /// </summary>
    /// <returns> </returns>
    [HttpGet]
    public async Task<ActionResult> GetUserInfo()
    {
        var userId = _session.UserId;

        if (userId.IsEmpty())
        {
            return Fail("登录信息丢失");
        }

        //var user = await _serviceGen.Query<User>().Where(x => x.Id == userId).FirstOrDefaultAsync();
        var user = _session.UserInfo;

        var userInfo = user!.MapTo<UserDto>();

        var score = await _serviceGen.Query<BonusPointRecord>()
            .Where(x => x.PickUpUserId == user!.Id)
            .SumAsync(x => x.BonusPoints);

        var dept = await _serviceGen.Query<Department>()
            .Where(x => x.Id == user!.DepartmentId)
            .FirstOrDefaultAsync();

        var globalRank = (await _serviceGen.Query<User>()
                    .GroupJoin(_serviceGen.Query<BonusPointRecord>(), user => user.Id,
                        bpRecord => bpRecord.PickUpUserId, (user, grouping) => new { user, grouping })
                    .SelectMany(@t => @t.grouping.DefaultIfEmpty(), (@t, bpRecord) => new { @t.user, bpRecord })
                    .Where(x => x.user.Role == Shared.Enums.Role.普通成员)
                    .GroupBy(x => new { x.user.Id, x.user.Name })
                .Select(x => new { Id = x.Key.Id, Name = x.Key.Name, Score = x.Sum(t => t.bpRecord.BonusPoints) })
                .OrderByDescending(x => x.Score)
                .ToListAsync())
            .Select((x, row) => new { Row = row + 1, Id = x.Id })
            .FirstOrDefault(x => x.Id == user!.Id);

        var userIds = await _serviceGen.Query<User>()
            .Where(x => x.DepartmentId == user!.DepartmentId)
            .Select(x => x.Id).ToListAsync();

        var departmentRank = (await _serviceGen.Query<User>()
                    .GroupJoin(_serviceGen.Query<BonusPointRecord>(), user => user.Id,
                        bpRecord => bpRecord.PickUpUserId, (user, grouping) => new { user, grouping })
                    .SelectMany(@t => @t.grouping.DefaultIfEmpty(), (@t, bpRecord) => new { @t.user, bpRecord })
                .Where(x => x.user.Role == Shared.Enums.Role.普通成员)
                .Where(x => (dept.Id) == x.user.DepartmentId)
                .GroupBy(x => new { x.user.Id, x.user.Name })
                .Select(x => new { Id = x.Key.Id, Name = x.Key.Name, Score = x.Sum(t => t.bpRecord.BonusPoints) })
                .OrderByDescending(x => x.Score)
                .ToListAsync())
            .Select((x, row) => new { Row = row + 1, Id = x.Id })
            .FirstOrDefault(x => x.Id == user!.Id);

        userInfo.Score = score;
        userInfo.DepartmentName = (dept ?? new Department()).DepartmentName;
        userInfo.GlobalRank = globalRank is null ? 0 : globalRank.Row;
        userInfo.DepartmentRank = departmentRank is null ? 0 : departmentRank.Row;

        return Success("获取用户信息成功", userInfo);
    }

    /// <summary>
    /// 用户获取分页
    /// </summary>
    /// <param name="parameter"> </param>
    /// <returns> </returns>
    [HttpGet]
    public async Task<ActionResult> GetUserPagedListAsync([FromQuery] UserDtoParameters parameter)
    {
        var query = _serviceGen.Query<User>();

        if (!parameter.DepartmentId.IsEmpty())
        {
            var depts = string.Join(",", await _serviceGen.Query<Department>().Where(x => x.TreeIds!.Contains(parameter.DepartmentId.ToString())).Select(x => x.TreeIds).ToListAsync());
            query = query.Where(x => depts.Contains(x.DepartmentId.ToString() ?? ""));
        }

        if (!parameter.Q.IsEmpty())
        {
            query = query.Where(x => x.Name.Contains(parameter.Q ?? "") ||
                        x.Tel.Contains(parameter.Q ?? ""));
        }

        var res = await query.Where(x => !x.IsSuper).QueryAsync(parameter);

        return Success("获取成功", res);
    }

    /// <summary>
    /// 通过部门Id获取用户
    /// </summary>
    /// <param name="deptId"> </param>
    /// <returns> </returns>
    [HttpGet]
    public async Task<ActionResult> GetUsersByDeptId(Guid deptId)
    {
        if (deptId == Guid.Empty) throw new ArgumentNullException(nameof(deptId));

        var users = await _serviceGen.Query<User>().Where(x => x.DepartmentId == deptId).ToListAsync();

        return Success("获取成功", users);
    }

    /// <summary>
    /// 修改密码
    /// </summary>
    /// <param name="dto"> </param>
    /// <returns> </returns>
    [HttpPost]
    public async Task<ActionResult> ChangePassword([FromBody] UserChangePasswordDto dto)
    {
        if (dto is null) return Fail("参数错误");

        if (dto.OldPassword.IsEmpty()) return Fail("请填写旧密码");

        if (dto.NewPassword.IsEmpty()) return Fail("请填写新密码");

        var user = await _serviceGen.Query<User>().Where(x => x.Id == _session.UserId).FirstOrDefaultAsync();

        if (user is null) return Fail("用户不存在");

        if (dto.OldPassword == user.Password)
        {
            user.Password = dto.NewPassword;

            _serviceGen.Db.Update(user);

            var res = await _serviceGen.SaveAsync();

            if (res) return Success("修改成功");

            return Fail("修改失败");
        }

        return Fail("密码错误");
    }
}