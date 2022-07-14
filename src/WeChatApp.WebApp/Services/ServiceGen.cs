using Microsoft.EntityFrameworkCore;
using WeChatApp.Shared.Interfaces;
using WeChatApp.WebApp.Data;

namespace WeChatApp.WebApp.Services
{
    /// <summary>
    /// </summary>
    public class ServiceGen : IServiceGen
    {
        private readonly WeComAppDbContext _dbContext;

        public DbContext Db => _dbContext;

        /// <summary>
        /// </summary>
        /// <param name="dbContext"> </param>
        public ServiceGen(WeComAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <returns> </returns>
        public IQueryable<T> Query<T>() where T : class, IEntity
        {
            return _dbContext.Set<T>() as IQueryable<T>;
        }

        /// <summary>
        /// </summary>
        /// <returns> </returns>
        public async Task BeginTrans()
        {
            await _dbContext.Database.BeginTransactionAsync();
        }

        /// <summary>
        /// </summary>
        /// <returns> </returns>
        public async Task CommitTrans()
        {
            await _dbContext.Database.CommitTransactionAsync();
        }

        /// <summary>
        /// </summary>
        /// <returns> </returns>
        public async Task Rollback()
        {
            await _dbContext.Database.RollbackTransactionAsync();
        }

        /// <summary>
        /// </summary>
        /// <returns> </returns>
        public async Task<bool> SaveAsync()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// </summary>
        /// <param name="sql"> </param>
        /// <returns> </returns>
        public async Task<int> Execute(string sql)
        {
            return await _dbContext.Database.ExecuteSqlRawAsync(sql);
        }
    }
}