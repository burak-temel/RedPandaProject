using MongoDB.Driver;
using Consumer.Domain.Models;
using Consumer.Infrastructure.Interfaces;
using Consumer.Infrastructure.Settings;
using Microsoft.Extensions.Options;

namespace Consumer.Infrastructure.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly IMongoCollection<ConsumerModel> _collection;

        public MessageRepository(IMongoDatabase database, IOptions<MongoDbSettings> settings)
        {
            _collection = database.GetCollection<ConsumerModel>(settings.Value.CollectionName);
        }

        public async Task SaveMessageAsync(ConsumerModel model)
        {
            await _collection.InsertOneAsync(model);
        }

        public async Task<ConsumerModel> GetMessageAsync(string id)
        {
            return await _collection.Find(m => m.Id == id).FirstOrDefaultAsync();
        }
    }
}
