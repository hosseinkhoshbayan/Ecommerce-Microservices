using Catalog.Entites.CommonEntities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Catalog.Entites.ProductEntities
{
    public class ProductBrand:BaseEntity
    {
        [BsonElement("Name")]
        public string Name { get; set; }
    }
}
