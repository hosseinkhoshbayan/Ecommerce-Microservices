using Catalog.Entites.CommonEntities;
using MongoDB.Bson.Serialization.Attributes;

namespace Catalog.Entites.ProductEntities
{
    public class ProductType:BaseEntity
    {
        [BsonElement("Name")]
        public string Name { get; set; }
    }
}
