using System;
using System.Linq.Expressions;
using Reborn.Domain.Infrastructure;
using Reborn.Domain.Model;

namespace Reborn.Domain.Repository
{
    public interface ICategoryRepository : IRepository<Category>
    {

    }

    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {

        }

       
    }
}
