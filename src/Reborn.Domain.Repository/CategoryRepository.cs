using Reborn.Domain.Infrastructure;
using Reborn.Domain.Model;

namespace Reborn.Domain.Repository
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICategoryRepository : IRepository<Category>
    {
        
    }

    /// <summary>
    /// 
    /// </summary>
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {

        }

      
    }
}
