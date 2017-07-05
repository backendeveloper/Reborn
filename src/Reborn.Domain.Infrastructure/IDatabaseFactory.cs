namespace Reborn.Domain.Infrastructure
{
    public interface IDatabaseFactory
    {
        MongoContext Get();
    }
}