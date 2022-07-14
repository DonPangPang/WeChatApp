using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pang.AutoMapperMiddleware;
using WeChatApp.Shared;
using WeChatApp.Shared.FormBody;
using WeChatApp.Shared.GlobalVars;
using WeChatApp.Shared.Interfaces;
using WeChatApp.Shared.RequestBody.WebApi;
using WeChatApp.Shared.Temp;
using WeChatApp.WebApp.Extensions;
using WeChatApp.WebApp.Services;

namespace WeChatApp.WebApp.Controllers;

/// <summary>
/// 基础接口
/// </summary>
[ApiController]
[Route("api/[Controller]/[Action]")]
public class ApiController : ControllerBase
{
    #region Responses

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [NonAction]
    public ActionResult Success()
    {
        return Ok(new WcResponse
        {
        });
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [NonAction]
    public ActionResult Fail()
    {
        return Ok(new WcResponse
        {
            Code = WcStatus.Fail
        });
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="msg"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    [NonAction]
    public ActionResult Success(string msg, object? data = null)
    {
        return Ok(new WcResponse()
        {
            Message = msg,
            Data = data
        });
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    [NonAction]
    public ActionResult Success(object? data)
    {
        return Ok(new WcResponse()
        {
            Data = data
        });
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="msg"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    [NonAction]
    public ActionResult Fail(string msg, object? data = null)
    {
        return Ok(new WcResponse()
        {
            Code = WcStatus.Fail,
            Message = msg,
            Data = data
        });
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    [NonAction]
    public ActionResult Fail(object? data)
    {
        return Ok(new WcResponse()
        {
            Code = WcStatus.Fail,
            Data = data
        });
    }

    #endregion Responses
}

/// <summary>
/// 基础接口
/// </summary>
/// <typeparam name="TEntity"> </typeparam>
/// <typeparam name="TDto"> </typeparam>
[ApiController]
[Route("api/[Controller]/[Action]")]
[Authorize(GlobalVars.Permission)]
public abstract class ApiController<TEntity, TDto> : ControllerBase
    where TEntity : class, IEntity
    where TDto : class, IDtoBase
{
    private readonly IServiceGen _serviceGen;

    /// <summary>
    /// </summary>
    /// <param name="serviceGen"> </param>
    public ApiController(IServiceGen serviceGen)
    {
        _serviceGen = serviceGen;
    }

    /// <summary>
    /// 获取分页数据
    /// </summary>
    /// <param name="parameter"> </param>
    /// <returns> </returns>
    [HttpGet]
    public async Task<ActionResult> GetPagedListAsync([FromQuery] ParameterBase parameter)
    {
        var res = await _serviceGen.Query<TEntity>().QueryAsync(parameter);

        return Success("获取成功", res);
    }

    /// <summary>
    /// 获取一条数据
    /// </summary>
    /// <param name="id"> </param>
    /// <returns> </returns>
    [HttpGet]
    public async Task<ActionResult> GetEntityAsync(Guid id)
    {
        var res = await _serviceGen.Query<TEntity>().Where(x => x.Id.Equals(id)).FirstOrDefaultAsync();

        if (res is not null)
            return Success("获取成功", res);

        return Fail("获取失败");
    }

    /// <summary>
    /// 创建一条数据
    /// </summary>
    /// <param name="dto"> </param>
    /// <returns> </returns>
    [HttpPost]
    public async Task<ActionResult> CreateEntityAsync(TDto dto)
    {
        var entity = dto.MapTo<TEntity>();

        entity.Create();

        if (entity is ITree<TEntity> tree)
        {
            if (tree.ParentId.HasValue)
            {
                var parent = await _serviceGen.Query<TEntity>().Where(x => x.Id.Equals(tree.ParentId.Value)).FirstOrDefaultAsync();

                tree.TreeIds = (parent as ITree<TEntity>)!.TreeIds + "," + entity.Id;
            }
            else
            {
                tree.TreeIds = entity.Id.ToString();
            }
        }

        await _serviceGen.Db.AddAsync(entity);

        var res = await _serviceGen.SaveAsync();

        if (res)
            return Success("创建成功");

        return Fail("创建失败");
    }

    /// <summary>
    /// 更新一条数据
    /// </summary>
    /// <param name="dto"> </param>
    /// <returns> </returns>
    [HttpPut]
    public async Task<ActionResult> UpdateEntityAsync(TDto dto)
    {
        var elder = await _serviceGen.Query<TEntity>().Where(x => x.Id.Equals(dto.Id)).FirstOrDefaultAsync();

        if (elder is null)
        {
            var entity = dto.MapTo<TEntity>();
            await _serviceGen.Db.AddAsync(entity);

            var res = await _serviceGen.SaveAsync();

            if (res) return Success("原数据不存在, 创建成功.");
        }
        else
        {
            dto.Map(elder);

            _serviceGen.Db.Update(elder);

            var res = await _serviceGen.SaveAsync();

            if (res) return Success("更新成功");
        }

        return Fail("创建失败");
    }

    /// <summary>
    /// 删除一条数据
    /// </summary>
    /// <param name="id"> </param>
    /// <returns> </returns>
    [HttpDelete]
    public async Task<ActionResult> DeleteEntityAsync(Guid id)
    {
        var entity = await _serviceGen.Query<TEntity>().Where(x => x.Id.Equals(id)).FirstOrDefaultAsync();

        if (entity is not null)
        {
            _serviceGen.Db.Remove(entity);

            var res = await _serviceGen.SaveAsync();

            if (res) return Success("删除成功");
        }

        return Fail("删除失败");
    }

    /// <summary>
    /// 删除很多数据
    /// </summary>
    /// <param name="ids"> </param>
    /// <returns> </returns>
    [HttpDelete]
    public async Task<ActionResult> DeleteEntitiesAsync([FromQuery] IEnumerable<Guid> ids)
    {
        var entities = await _serviceGen.Query<TEntity>().Where(x => ids.Contains(x.Id)).ToListAsync();

        if (entities.Any())
        {
            _serviceGen.Db.RemoveRange(entities);

            var res = await _serviceGen.SaveAsync();

            if (res) return Success("删除成功");
        }

        return Fail("找不到数据");
    }

    #region Responses

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [NonAction]
    public ActionResult Success()
    {
        return Ok(new WcResponse
        {
        });
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [NonAction]
    public ActionResult Fail()
    {
        return Ok(new WcResponse
        {
            Code = WcStatus.Fail
        });
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="msg"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    [NonAction]
    public ActionResult Success(string msg, object? data = null)
    {
        return Ok(new WcResponse()
        {
            Message = msg,
            Data = data,
            TotalCount = data is PagedList<TEntity> list ? list.TotalCount : 0,
        });
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    [NonAction]
    public ActionResult Success(object? data)
    {
        return Ok(new WcResponse()
        {
            Data = data,
            TotalCount = data is PagedList<TEntity> list ? list.TotalCount : 0,
        });
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="msg"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    [NonAction]
    public ActionResult Fail(string msg, object? data = null)
    {
        return Ok(new WcResponse()
        {
            Code = WcStatus.Fail,
            Message = msg,

            Data = data,
            TotalCount = data is PagedList<TEntity> list ? list.TotalCount : 0,
        });
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    [NonAction]
    public ActionResult Fail(object? data)
    {
        return Ok(new WcResponse()
        {
            Code = WcStatus.Fail,
            Data = data,
            TotalCount = data is PagedList<TEntity> list ? list.TotalCount : 0,
        });
    }

    #endregion Responses
}