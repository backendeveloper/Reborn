namespace Reborn.Domain.Infrastructure
{
    public class DatabaseFactory :  IDatabaseFactory
    {
        private MongoContext dataContext;
        public MongoContext Get()
        {
            return dataContext ?? (dataContext = new MongoContext("mongodb://localhost:21017/Pluto"));
        }
    }
}
