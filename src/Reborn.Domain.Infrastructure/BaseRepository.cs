using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Reborn.Domain.Model.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Reborn.Domain.Infrastructure
{
    public abstract class BaseRepository<T> : IRepository<T> where T : BaseMongoEntity
    {
        private readonly MongoContext _context;
        private const string IdName = "_id";
        private IMongoCollection<T> Collection => _context.GetCollection<T>();

        protected BaseRepository(IDatabaseFactory databaseFactory)
        {
            _context = databaseFactory.Get();
        }

        #region IRepository Implementation

        public virtual T Create(T entity)
        {
            Collection.InsertOne(entity);
            return entity;
        }

        public virtual async Task<T> CreateAsync(T entity)
        {
            await Collection.InsertOneAsync(entity);
            return entity;
        }

        public virtual void Update(T entity)
        {
            var filter = Builders<T>.Filter.Eq(IdName, entity.Id);
            Collection.ReplaceOne(filter, entity);
        }

        public virtual async Task UpdateAsync(T entity)
        {
            var filter = Builders<T>.Filter.Eq(IdName, entity.Id);
            await Collection.ReplaceOneAsync(filter, entity);
        }

        public virtual void Delete(T entity)
        {
            DeleteById(entity.Id.ToString());
        }

        public virtual async Task DeleteAsync(T entity)
        {
            await DeleteByIdAsync(entity.Id.ToString());
        }

        public virtual void DeleteById(string id)
        {
            Collection.DeleteOne(x => x.Id == id.ToGuid());
        }

        public virtual async Task DeleteByIdAsync(string id)
        {
            await Collection.DeleteOneAsync(x => x.Id == id.ToGuid());
        }

        public virtual long DeleteMany(IList<string> ids)
        {
            var idsToGuid = ids.Select(Guid.Parse).ToList();
            var deleteResult = Collection.DeleteMany(x => idsToGuid.Contains(x.Id));

            return deleteResult.DeletedCount;
        }

        public virtual async Task<long> DeleteManyAsync(IList<string> ids)
        {
            var idsToGuid = ids.Select(Guid.Parse).ToList();
            var deleteResult = await Collection.DeleteManyAsync(x => idsToGuid.Contains(x.Id));

            return await Task.FromResult(deleteResult.DeletedCount);
        }

        public virtual T FirstOrDefault(Expression<Func<T, bool>> expression)
        {
            return Collection.AsQueryable().FirstOrDefault(expression);
        }

        public virtual async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> expression)
        {
            return await Quarable.FirstOrDefaultAsync(expression);
        }

        public virtual IQueryable<T> Get(Expression<Func<T, bool>> expression = null)
        {
            var result = Collection.AsQueryable();
            return expression == null ?
                result : Queryable.Where(result, expression);
        }

        public virtual IMongoQueryable<T> Quarable => Collection.AsQueryable();


        public virtual T GetById(string id)
        {
            return Quarable.FirstOrDefault(x => x.Id == id.ToGuid());
        }

        public virtual async Task<T> GetByIdAsync(string id)
        {
            return await Collection.AsQueryable().FirstOrDefaultAsync(x => x.Id == id.ToGuid());
        }

        public virtual long GetCount(Expression<Func<T, bool>> expression = null)
        {
            return Quarable.Where(expression).Count();
        }

        public virtual bool Exist(Expression<Func<T, bool>> expression = null)
        {
            return Quarable.Any(expression);
        }

        public virtual PagedList<T> GetPage<TOrder>(Pagination pagination, Expression<Func<T, bool>> expression, Expression<Func<T, TOrder>> order, bool desc, bool totalCount)
        {
            var resultsQuerable = Quarable;
            resultsQuerable = desc ? resultsQuerable.OrderByDescending(order)
                : resultsQuerable.OrderBy(order);

            var results = resultsQuerable.Where(expression).GetPage(pagination).ToList();
            var total = totalCount ? Quarable.Count(expression) : 0;

            return new PagedList<T>(results, total);
        }

        #endregion
    }
}
