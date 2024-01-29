using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace CatalogAPI.Models
{
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public required int Id { get; set; }

        [BsonElement("Name")]
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string? Summary { get; set; }
        public string? Description { get; set; }
        public string? ImageFile { get; set; }
        public decimal Price { get; set; }
    }
}
