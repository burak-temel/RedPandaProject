using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Producer.Domain.Models
{
    public class ProducerModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
    }
}
