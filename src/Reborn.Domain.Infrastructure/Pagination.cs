using System.Collections.Generic;
using System.Linq;

namespace Reborn.Domain.Infrastructure
{
    public class Pagination
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public Pagination()
        {
            PageNumber = 1;
            PageSize = 10;
        }

        public Pagination(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }

        public int Skip
        {
            get { return (PageNumber - 1) * PageSize; }
        }
    }

    public class PagedList<T>
    {
        public PagedList(IList<T> data, int totalCount)
        {
            Data = data;
            TotalCount = totalCount;
        }

        public IList<T> Data { get; set; }
        public int TotalCount { get; set; }        
    }

    public static class PagingExtensions
    {
        /// <summary>
        /// Extend IQueryable to simplify access to skip and take methods 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryable"></param>
        /// <param name="pagination"></param>
        /// <returns>IQueryable with Skip and Take having been performed</returns>
        public static IQueryable<T> GetPage<T>(this IQueryable<T> queryable, Pagination pagination)
        {
            return queryable.Skip(pagination.Skip).Take(pagination.PageSize);
        }
    }
}
