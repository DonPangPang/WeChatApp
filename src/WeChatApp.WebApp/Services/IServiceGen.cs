using Microsoft.EntityFrameworkCore;
using WeChatApp.Shared.Interfaces;

namespace WeChatApp.WebApp.Services
{
    /// <summary>
    /// 通用服务
    /// </summary>
    public interface IServiceGen
    {
        /// <summary>
        /// Db
        /// </summary>
        DbContext Db { get; }

        /// <summary>
        /// 获取实体检索
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <returns> </returns>
        IQueryable<T> Query<T>() where T : class, IEntity;

        /// <summary>
        /// 开始事务
        /// </summary>
        /// <returns> </returns>
        Task BeginTrans();

        /// <summary>
        /// 提交事务
        /// </summary>
        /// <returns> </returns>
        Task CommitTrans();

        /// <summary>
        /// 回滚事务
        /// </summary>
        /// <returns> </returns>
        Task Rollback();

        /// <summary>
        /// 保存
        /// </summary>
        /// <returns> </returns>
        Task<bool> SaveAsync();

        /// <summary>
        /// 执行sql
        /// </summary>
        /// <param name="sql"> </param>
        /// <returns> </returns>
        Task<int> Execute(string sql);
    }
}