using WeChatApp.Shared.Interfaces;
using System.Linq.Dynamic.Core;
using WeChatApp.Shared.RequestBody.WebApi;
using Microsoft.EntityFrameworkCore;
using WeChatApp.Shared.Temp;

namespace WeChatApp.WebApp.Extensions
{
    /// <summary>
    /// </summary>
    public static class IQueryableExtensions
    {
        /// <summary>
        /// 进行排序
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="source">  </param>
        /// <param name="orderBy"> </param>
        /// <returns> </returns>
        public static IQueryable<T> ApplySort<T>(
            this IQueryable<T> source, string orderBy) where T : class, IEntity
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (string.IsNullOrWhiteSpace(orderBy))
            {
                return source;
            }

            var orderByAfterSplit = orderBy.Split(",");

            foreach (var orderByClause in orderByAfterSplit.Reverse())
            {
                var trimedOrderByClause = orderByClause.Trim();

                var orderDescending = trimedOrderByClause.EndsWith(" desc");

                var indexOfFirstSpace = trimedOrderByClause.IndexOf(" ", StringComparison.Ordinal);

                var propertyName = indexOfFirstSpace == -1
                    ? trimedOrderByClause
                    : trimedOrderByClause.Remove(indexOfFirstSpace);

                source = source.OrderBy(propertyName
                                        + (orderDescending ? " descending" : " ascending"));
            }

            return source;
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="source"> </param>
        /// <param name="paging"> </param>
        /// <returns> </returns>
        public static async Task<PagedList<T>> ToPagedListAsync<T>(this IQueryable<T> source, IPaging paging) where T : class, IEntity
        {
            return await PagedList<T>.CreateAsync(source, paging.Page, paging.PageSize);
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="source"> </param>
        /// <param name="paging"> </param>
        /// <returns> </returns>
        public static PagedList<T> ToPagedList<T>(this IQueryable<T> source, IPaging paging) where T : class, IEntity
        {
            return PagedList<T>.Create(source, paging.Page, paging.PageSize);
        }

        /// <summary>
        /// 排序并分页
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="source">     </param>
        /// <param name="parameters"> </param>
        /// <returns> </returns>
        public static async Task<PagedList<T>> QueryAsync<T>(this IQueryable<T> source, IParameterBase parameters) where T : class, IEntity
        {
            if (parameters is ISorting sorting)
                source = source.ApplySort(sorting.OrderBy ?? "");

            if (parameters is IPaging paging && paging.PageSize > 0)
                return await source.ToPagedListAsync(paging);
            else
                return await PagedList<T>.CreateAsync(source, 1, 999);
        }

        /// <summary>
        /// 获取开启的数据
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="query"> </param>
        /// <returns> </returns>
        public static IQueryable<T> Enabled<T>(this IQueryable<T> query)
            where T : IEnabled, IEntity
        {
            return query.Where(x => x.IsEnabled);
        }

        /// <summary>
        /// 未被删除的数据
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="query"> </param>
        /// <returns> </returns>
        public static IQueryable<T> UnDeleted<T>(this IQueryable<T> query)
            where T : IDeleted, IEntity
        {
            return query.Where(x => !x.IsDeleted);
        }

        /// <summary>
        /// 获取开启的数据
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="query"> </param>
        /// <returns> </returns>
        public static IEnumerable<T> Enabled<T>(this IEnumerable<T> query)
            where T : IEnabled, IEntity
        {
            return query.Where(x => x.IsEnabled);
        }

        /// <summary>
        /// 未被删除的数据
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="query"> </param>
        /// <returns> </returns>
        public static IEnumerable<T> UnDeleted<T>(this IEnumerable<T> query)
            where T : IDeleted, IEntity
        {
            return query.Where(x => !x.IsDeleted);
        }

        /// <summary>
        /// 获取公开的
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="query"> </param>
        /// <returns> </returns>
        public static IQueryable<T> Published<T>(this IQueryable<T> query)
            where T : IPublic, IEntity
        {
            return query.Where(x => x.PublicStartTime <= DateTime.Now && x.PublicEndTime >= DateTime.Now);
        }

        /// <summary>
        /// 获取公开的
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="query"> </param>
        /// <returns> </returns>
        public static IEnumerable<T> Published<T>(this IEnumerable<T> query)
            where T : class, IEntity, IPublic
        {
            return query.Where(x => x.PublicStartTime <= DateTime.Now && x.PublicEndTime >= DateTime.Now);
        }

        /// <summary>
        /// 是否公开
        /// </summary>
        /// <typeparam name="T"> </typeparam>
        /// <param name="entity"> </param>
        /// <returns> </returns>
        public static bool IsPublic<T>(this T entity)
            where T : IEntity, IPublic
        {
            return (entity.PublicStartTime <= DateTime.Now && entity.PublicEndTime >= DateTime.Now);
        }

        /// <summary>
        /// 转为树形结构
        /// </summary>
        /// <param name="data"> </param>
        /// <typeparam name="T"> </typeparam>
        /// <returns> </returns>
        public static IEnumerable<T> ToTree<T>(this IEnumerable<T> data) where T : ITree<T>
        {
            var tree = new List<T>();
            foreach (var item in data)
            {
                if (item.ParentId == null || item.ParentId == Guid.Empty)
                {
                    tree.Add(item);
                }
                else
                {
                    var parent = tree.FirstOrDefault(x => x.Id == item.ParentId);
                    if (parent != null)
                    {
                        if (parent.Children == null)
                        {
                            parent.Children = new List<T>();
                        }
                        parent.Children.Append(item);
                    }
                }
            }
            return tree;
        }
    }
}