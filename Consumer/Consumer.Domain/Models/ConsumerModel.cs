using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Consumer.Domain.Models
{
    public class ConsumerModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
    }
}
