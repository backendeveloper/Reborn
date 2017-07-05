namespace Reborn.Domain.Infrastructure
{
    public class MongoContext : BaseMongoContext
    {
        public MongoContext(string connectionString) : base(connectionString)
        {
             
        }
    }
}
