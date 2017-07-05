using System;
using System.Reflection;
using System.Text.RegularExpressions;
using MongoDB.Bson;
using MongoDB.Driver;
using Reborn.Domain.Model.Utility;

namespace Reborn.Domain.Infrastructure
{
    public abstract class BaseMongoContext
    {
        /// <summary>
        /// MongoDb database instance
        /// </summary>
        private readonly IMongoDatabase _mongoDatabase;

        public BaseMongoContext(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            var match = Regex.Match(connectionString, "^name=(.+)");
            if (match.Success)
            {

            }

            var mongoUrl = MongoUrl.Create(connectionString);
            _mongoDatabase = GetDatabaseFromUrl(mongoUrl);
        }


        /// <summary>
        /// MongoDB veritabanına bağlanmayı dener.
        /// </summary>
        /// <returns>Veritabanı bağlantı durumu.</returns>
        public bool TryConnect()
        {
            try
            {
                _mongoDatabase.RunCommandAsync((Command<BsonDocument>)"{ping:1}").Wait();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Verilen string ile collection nesnesini döner
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collectionName"></param>
        /// <returns></returns>
        public IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            if (collectionName == null)
            {
                throw new ArgumentNullException(nameof(collectionName));
            }

            return _mongoDatabase.GetCollection<T>(collectionName);
        }

        /// <summary>
        /// Generic tipteki nesnenin collectinName i ile collection nesnesi döner
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IMongoCollection<T> GetCollection<T>() where T : BaseMongoEntity
        {
            var collectionName = GetCollectionName<T>();

            return _mongoDatabase.GetCollection<T>(collectionName);
        }

        #region Privates 

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private static string GetCollectionName<T>() where T : BaseMongoEntity
        {
            var collectionName = GetCollectioNameFromInterface<T>();

            if (string.IsNullOrEmpty(collectionName))
            {
                throw new ArgumentException("Collection name cannot be empty for this entity");
            }
            return collectionName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private static string GetCollectioNameFromInterface<T>()
        {
            // Check to see if the object (inherited from BaseMongoEntity) has a CollectionName attribute
            var attr = typeof(T).GetTypeInfo().GetCustomAttribute(typeof(CollectionName));
            var collectionname = attr != null ? ((CollectionName)attr).Name
                : typeof(T).Name;


            return collectionname;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entitytype"></param>
        /// <returns></returns>
        private static string GetCollectionNameFromType(Type entitytype)
        {
            string collectionname;

            // Check to see if the object (inherited from BaseMongoEntity) has a CollectionName attribute
            var attr = typeof(Type).GetTypeInfo().GetCustomAttribute(typeof(CollectionName));
            if (attr != null)
            {
                // It does! Return the value specified by the CollectionName attribute
                collectionname = ((CollectionName)attr).Name;
            }
            else
            {
                if (typeof(BaseMongoEntity).IsAssignableFrom(entitytype))
                {
                    // No attribute found, get the basetype
                    while (!entitytype.GetTypeInfo().BaseType.Equals(typeof(BaseMongoEntity)))
                    {
                        entitytype = entitytype.GetTypeInfo().BaseType;
                    }
                }
                collectionname = entitytype.Name;
            }

            return collectionname;
        }

        /// <summary>
        /// Mongodb Database nesnesini interface olarak döner
        /// </summary>
        /// <param name="url">MongoUrl object</param>
        /// <returns>IMongoDatabase</returns>
        private static IMongoDatabase GetDatabaseFromUrl(MongoUrl url)
        {
            var settings = MongoClientSettings.FromUrl(url);
            settings.MaxConnectionIdleTime = TimeSpan.FromMinutes(1);
            settings.ClusterConfigurator = builder =>
            {
                builder.ConfigureCluster(s => s.With(
                    serverSelectionTimeout: TimeSpan.FromSeconds(10))
                );
            };

            var client = new MongoClient(settings);
            return client.GetDatabase(url.DatabaseName);
        }

        #endregion
    }


}