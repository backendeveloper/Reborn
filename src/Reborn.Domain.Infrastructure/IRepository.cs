using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Driver.Linq;

namespace Reborn.Domain.Infrastructure
{
    public interface IRepository<T>
    {
        T Create(T entity);
        Task<T> CreateAsync(T entity);

        void Update(T entity);
        Task UpdateAsync(T entity);

        void Delete(T entity);
        Task DeleteAsync(T entity);
        void DeleteById(string id);
        Task DeleteByIdAsync(string id);
        long DeleteMany(IList<string> ids);
        Task<long> DeleteManyAsync(IList<string> ids);

        T FirstOrDefault(Expression<Func<T, bool>> expression);
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> expression);
        T GetById(string id);
        Task<T> GetByIdAsync(string id);

        IMongoQueryable<T> Quarable { get; }

        long GetCount(Expression<Func<T, bool>> expression = null);

        bool Exist(Expression<Func<T, bool>> expression = null);

        PagedList<T> GetPage<TOrder>(Pagination pagination, Expression<Func<T, bool>> expression, Expression<Func<T, TOrder>> order, bool desc,
            bool totalCount);
    }

    public static class StringExtension
    {
        public static Guid ToGuid(this string id)
        {
            Guid.TryParse(id, out Guid result);

            if (result == null)
                throw new FormatException();

            return result;
        }
    }
}
