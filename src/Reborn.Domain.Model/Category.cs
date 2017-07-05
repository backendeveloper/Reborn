using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using Reborn.Domain.Model.Utility;

namespace Reborn.Domain.Model
{
    [CollectionName("MongoDbCategory")]
    [BsonIgnoreExtraElements]
    public class Category : BaseMongoEntity
    {
        public string Title { get; set; }
        public string MetaTitle { get; set; }
        public string Description { get; set; }
        public string MetaDescription { get; set; }
        public string VideoMetaTitle { get; set; }
        public string VideoMetaDescription { get; set; }
        public string VideoDescription { get; set; }
        public string VideoSubDescription { get; set; }
        public Guid? ParentCategoryId { get; set; }
        public string Slug { get; set; }

        public int? Order { get; set; }
        public int Status { get; set; }
        public IEnumerable<int> Purpose { get; set; }
        public bool ShowOnMenu { get; set; }
        public string VideoGalleryGemiusId { get; set; }
        public string PhotoGalleryGemiusId { get; set; }
        public string StoryGemiusId { get; set; }
        public Guid? PopulerVideoCategory { get; set; }

        public bool? HideRecommendations { get; set; }
    }
} 
