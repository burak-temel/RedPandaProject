using MongoDB.Driver;
using Consumer.Domain.Models;
using Consumer.Infrastructure.Interfaces;
using Consumer.Infrastructure.Settings;
using Microsoft.Extensions.Options;

namespace Consumer.Infrastructure.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly IMongoCollection<ConsumerModel> _abcCollection;

        public MessageRepository(IMongoDatabase database, IOptions<MongoDbSettings> settings)
        {
            _abcCollection = database.GetCollection<ConsumerModel>(settings.Value.CollectionName);
        }

        public async Task InsertAsync(ConsumerModel model)
        {
            await _abcCollection.InsertOneAsync(model);
        }
    }
}
