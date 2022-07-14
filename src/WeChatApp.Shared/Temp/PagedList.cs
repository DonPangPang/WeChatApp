using Microsoft.EntityFrameworkCore;

namespace WeChatApp.Shared.Temp
{
    /// <summary>
    /// 分页
    /// </summary>
    /// <typeparam name="T"> </typeparam>
    public class PagedList<T> : List<T> where T : class
    {
        /// <summary>
        /// 当前页
        /// </summary>
        public int CurrentPage { get; private set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPages { get; private set; }

        /// <summary>
        /// 页大小
        /// </summary>
        public int PageSize { get; private set; }

        /// <summary>
        /// 总数
        /// </summary>
        public int TotalCount { get; private set; }

        /// <summary>
        /// 是否有上一页
        /// </summary>
        public bool HasPrevious => CurrentPage > 1;

        /// <summary>
        /// 是否有下一页
        /// </summary>
        public bool HasNext => CurrentPage < TotalPages;

        /// <summary>
        /// </summary>
        /// <param name="items">      </param>
        /// <param name="count">      </param>
        /// <param name="pageNumber"> </param>
        /// <param name="pageSize">   </param>
        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            AddRange(items);
        }

        /// <summary>
        /// </summary>
        public PagedList()
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="sourse">     </param>
        /// <param name="pageNumber"> </param>
        /// <param name="pageSize">   </param>
        /// <returns> </returns>
        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> sourse, int pageNumber, int pageSize)
        {
            var count = await sourse.CountAsync();
            var items = await sourse.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PagedList<T>(items, count, pageNumber, pageSize);
        }

        /// <summary>
        /// </summary>
        /// <param name="sourse">     </param>
        /// <param name="pageNumber"> </param>
        /// <param name="pageSize">   </param>
        /// <returns> </returns>
        public static PagedList<T> Create(IQueryable<T> sourse, int pageNumber, int pageSize)
        {
            var count = sourse.Count();
            var items = sourse.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
    }
}