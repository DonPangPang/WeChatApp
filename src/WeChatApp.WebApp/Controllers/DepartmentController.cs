using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pang.AutoMapperMiddleware;
using WeChatApp.Shared.Entity;
using WeChatApp.Shared.Extensions;
using WeChatApp.Shared.FormBody;
using WeChatApp.Shared.Temp;
using WeChatApp.WebApp.Extensions;
using WeChatApp.WebApp.Services;

namespace WeChatApp.WebApp.Controllers
{
    /// <summary>
    /// 部门
    /// </summary>
    public class DepartmentController : ApiController<Department, DepartmentDto>
    {
        private readonly IServiceGen _serviceGen;

        /// <summary>
        /// </summary>
        /// <param name="serviceGen"> </param>
        public DepartmentController(IServiceGen serviceGen) : base(serviceGen)
        {
            _serviceGen = serviceGen;
        }

        /// <summary>
        /// 获取子集部门
        /// </summary>
        /// <returns> </returns>
        [HttpGet]
        public async Task<ActionResult> GetTree()
        {
            try
            {
                var res = await _serviceGen.Query<Department>()
                    // .Where(x => x.ParentId == null || x.ParentId == Guid.Empty)
                    // .Include(x => x.Children)
                    .ToListAsync();

                var tree = res.ToTree();

                var returnDto = tree.MapTo<DepartmentDto>();


                return Success(returnDto);
            }
            catch (Exception e)
            {
                return Fail(e.Message);
            }
        }

        /// <summary>
        /// 获取子集部门(带Level)
        /// </summary>
        /// <returns> </returns>
        [Obsolete("不建议使用, Level有问题")]
        [HttpGet]
        public async Task<ActionResult> GetDeptTree()
        {
            try
            {
                var depts = await _serviceGen.Query<Department>()
                    // .Where(x => x.ParentId == null || x.ParentId == Guid.Empty)
                    // .Include(x => x.Children)
                    .Select(x => new TreeItem
                    {
                        Id = x.Id,
                        Name = x.DepartmentName,
                        ParentId = x.ParentId,
                        Type = TreeItemType.Department
                    }).ToListAsync();

                var tree = depts.ToTree();

                //var returnDto = tree.MapTo<DepartmentDto>();


                return Success(tree);
            }
            catch (Exception e)
            {
                return Fail(e.Message);
            }
        }

        /// <summary>
        /// 附带用户信息的部门列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> GetTreeWithUser()
        {
            var depts = await _serviceGen.Query<Department>()
                    // .Where(x => x.ParentId == null || x.ParentId == Guid.Empty)
                    // .Include(x => x.Children)
                    .Select(x => new TreeItem
                    {
                        Id = x.Id,
                        Name = x.DepartmentName,
                        ParentId = x.ParentId,
                        Type = TreeItemType.Department
                    }).ToListAsync();

            var users = await _serviceGen.Query<User>()
                .Select(x => new TreeItem
                {
                    Id = x.Id,
                    Name = x.Name,
                    ParentId = x.DepartmentId,
                    Type = TreeItemType.User
                }).ToListAsync();
            depts.AddRange(users);

            var tree = depts.ToTree();

            return Success(tree);
        }
    }
}