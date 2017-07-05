using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Reborn.Domain.Model.Utility
{
    //[Serializable]
    public abstract class BaseMongoEntity : IEntity<Guid>
    {
        /// <summary>
        /// _id
        /// </summary>
        [BsonId]
       // [BsonRepresentation(BsonType.ObjectId)]
        public virtual Guid Id { get; set; }
    }


    /// <summary>
    /// Generic BaseMongoEntity interface.
    /// </summary>
    /// <typeparam name="TKey">The type used for the entity's Id.</typeparam>
    public interface IEntity<TKey>
    {
        /// <summary>
        /// Gets or sets the Id of the BaseMongoEntity.
        /// </summary>
        /// <value>Id of the BaseMongoEntity.</value>
        [BsonId]
        TKey Id { get; set; }
    }

    /// <summary>
    /// "Default" BaseMongoEntity interface.
    /// </summary>
    /// <remarks>Entities are assumed to use strings for Id's.</remarks>
    public interface IEntity : IEntity<string>
    {

    }

}
